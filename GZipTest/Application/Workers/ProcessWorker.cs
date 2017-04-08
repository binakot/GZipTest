using System.Threading;
using GZipTest.Application.Compression;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Workers
{
    public sealed class ProcessWorker : BaseWorker
    {
        private static readonly ICompressor Compressor = new GZipCompressor();

        private readonly FileChunkQueue _inputChunkQueue;
        private readonly FileChunkQueue _outputChunkQueue;
        private readonly OperationType _operationType;

        public ProcessWorker(FileChunkQueue inputChunkQueue, FileChunkQueue outputChunkQueue, OperationType operationType)
        {
            _inputChunkQueue = inputChunkQueue;
            _outputChunkQueue = outputChunkQueue;
            _operationType = operationType;
        }

        public override void Start()
        {
            while (!IsStopped)
            {
                if (_inputChunkQueue.Count() > 0)
                {
                    switch (_operationType)
                    {
                        case OperationType.Compress:
                            _outputChunkQueue.Enqueue(Compressor.Compress(_inputChunkQueue.Dequeue()));
                            continue;

                        case OperationType.Decompress:
                            _outputChunkQueue.Enqueue(Compressor.Decompress(_inputChunkQueue.Dequeue()));
                            continue;
                    }
                }
                else
                {
                    if (_inputChunkQueue.IsLast)
                    {
                        _outputChunkQueue.IsLast = true;
                        break;
                    }

                    Thread.Sleep(100);
                }
            }
        }
    }
}