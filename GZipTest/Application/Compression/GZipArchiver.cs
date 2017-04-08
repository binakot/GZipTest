using System.Collections.Generic;
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
        private List<IWorker> _processWorkers;

        public GZipArchiver(int cpuCount, int totalMemoryAmount, int bufferSize)
        {
            _cpuCount = cpuCount;
            _totalMemoryAmount = totalMemoryAmount;
            _bufferSize = bufferSize;
        }

        public void Process(string inputFilePath, string outputFilePath, OperationType operation)
        {
            #region INIT WORKERS

            _readWorker = new ReadWorker(inputFilePath, _inputFileChunkQueue, operation, _bufferSize, _totalMemoryAmount);

            var processWorkersCount = 1; // TODO Multiple processing workers amount are based on CPU count.
            _processWorkers = new List<IWorker>(processWorkersCount);
            for (var i = 0; i < processWorkersCount; i++)
                _processWorkers.Add(new ProcessWorker(_inputFileChunkQueue, _outputFileChunkQueue, operation));

            _writeWorker = new WriteWorker(outputFilePath, _outputFileChunkQueue);

            #endregion INIT WORKERS

            #region RUN WORKERS IN THREADS

            var readWorkerThread = new Thread(() => _readWorker.Start())
            {
                Name = "FileReadWorker",
                Priority = ThreadPriority.AboveNormal
            };
            readWorkerThread.Start();

            for (var i = 0; i < _processWorkers.Count; i++)
            {
                var worker = _processWorkers[i];
                var processWorkerThread = new Thread(() => worker.Start())
                {
                    Name = "DataProcessWorker" + i,
                    Priority = ThreadPriority.AboveNormal
                };
                processWorkerThread.Start();
            }

            var writeWorkerThread = new Thread(() => _writeWorker.Start())
            {
                Name = "FileWriteWorker",
                Priority = ThreadPriority.AboveNormal
            };
            writeWorkerThread.Start();

            writeWorkerThread.Join(); // Wait until the all processed file chunks will written.

            #endregion RUN WORKERS IN THREADS
        }

        public void Abort()
        {
            _readWorker.Stop();
            foreach (var worker in _processWorkers)
                worker.Stop();
            _writeWorker.Stop();

            _inputFileChunkQueue.Clear();
            _outputFileChunkQueue.Clear();
        }
    }
}