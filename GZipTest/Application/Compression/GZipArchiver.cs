using System.Threading;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using GZipTest.Application.Workers;

namespace GZipTest.Application.Compression
{
    public sealed class GZipArchiver : IArchiver
    {
        private readonly FileChunkQueue _inputFileChunkQueue = new FileChunkQueue();
        private readonly FileChunkQueue _outputFileChunkQueue = new FileChunkQueue();

        private readonly int _cpuCount;
        private readonly int _totalMemoryAmount;
        private readonly int _bufferSize;

        private IWorker _readWorker;
        private IWorker _writeWorker;

        public GZipArchiver(int cpuCount, int totalMemoryAmount, int bufferSize)
        {
            _cpuCount = cpuCount;
            _totalMemoryAmount = totalMemoryAmount;
            _bufferSize = bufferSize;
        }

        public void Process(string inputFilePath, string outputFilePath, OperationType operation)
        {
            _readWorker = new ReadWorker(inputFilePath, _inputFileChunkQueue, operation, _bufferSize, _totalMemoryAmount);
            var readWorkerThread = new Thread(() => _readWorker.Start())
            {
                Name = "FileReadWorker",
                Priority = ThreadPriority.AboveNormal
            };

            _writeWorker = new WriteWorker(outputFilePath, _inputFileChunkQueue); // TODO TEMP Just write bytes to output file from the input one without any processing.
            var writeWorkerThread = new Thread(() => _writeWorker.Start())
            {
                Name = "FileWriteWorker",
                Priority = ThreadPriority.AboveNormal
            };

            // TODO Add processing workers.

            readWorkerThread.Start();
            writeWorkerThread.Start();
        }

        public void Abort()
        {
            _readWorker.Stop();
            _writeWorker.Stop();

            _inputFileChunkQueue.Clear();
            _outputFileChunkQueue.Clear();
        }
    }
}