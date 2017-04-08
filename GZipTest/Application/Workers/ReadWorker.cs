using System;
using System.IO;
using System.Threading;
using GZipTest.Application.Context;
using GZipTest.Application.Files;

namespace GZipTest.Application.Workers
{
    public sealed class ReadWorker : BaseWorker
    {
        private readonly string _fileName;
        private readonly FileChunkQueue _fileChunkQueue;
        private readonly OperationType _operationType;
        private readonly int _fileChunkSize;
        private readonly int _totalBufferSize;

        public ReadWorker(string fileName, FileChunkQueue fileChunkQueue, OperationType operationType, int fileChunkSize, int totalBufferSize)
        {
            _fileName = fileName;
            _fileChunkQueue = fileChunkQueue;
            _operationType = operationType;
            _fileChunkSize = fileChunkSize;
            _totalBufferSize = totalBufferSize;
        }

        public override void Start()
        {
            var availableBytes = new FileInfo(_fileName).Length;
            using (var reader = new BinaryReader(File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                while (!IsStopped && !_fileChunkQueue.IsLast)
                {
                    switch (_operationType)
                    {
                        case OperationType.Compress:
                            var readCount = availableBytes < _fileChunkSize
                                ? (int)availableBytes
                                : _fileChunkSize;

                            var bytes = reader.ReadBytes(readCount);
                            _fileChunkQueue.Enqueue(bytes);

                            availableBytes -= bytes.Length;
                            if (availableBytes == 0)
                                _fileChunkQueue.IsLast = true;

                            break;
                        
                        case OperationType.Decompress:
                            throw new NotImplementedException(); // TODO Read compressed file chunks by gzip headers.
                    }

                    while (_fileChunkSize * _fileChunkQueue.Count() > _totalBufferSize)
                        Thread.Sleep(100);
                }
            }
        }
    }
}