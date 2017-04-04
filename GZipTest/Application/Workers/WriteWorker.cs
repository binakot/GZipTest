using System.IO;
using System.Threading;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Workers
{
    public sealed class WriteWorker : IWorker
    {
        private readonly string _fileName;
        private readonly FileChunkQueue _fileChunkQueue;

        private volatile bool _isStopped;

        public WriteWorker(string fileName, FileChunkQueue fileChunkQueue)
        {
            _fileName = fileName;
            _fileChunkQueue = fileChunkQueue;
        }

        public void Start()
        {
            using (var writer = new BinaryWriter(File.Open(_fileName, FileMode.Create, FileAccess.Write)))
            {
                while (!_isStopped)
                {
                    if (_fileChunkQueue.Count() > 0)
                    {
                        var bytes = _fileChunkQueue.Dequeue();
                        writer.Write(bytes);
                    }
                    else
                    {
                        if (_fileChunkQueue.IsLast)
                            return;

                        Thread.Sleep(100);
                    }
                }
            }
        }

        public void Stop()
        {
            _isStopped = true;
        }
    }
}