using System.IO;
using System.Threading;
using GZipTest.Application.Files;

namespace GZipTest.Application.Workers
{
    public sealed class WriteWorker : BaseWorker
    {
        private readonly string _fileName;
        private readonly FileChunkQueue _fileChunkQueue;

        public WriteWorker(string fileName, FileChunkQueue fileChunkQueue)
        {
            _fileName = fileName;
            _fileChunkQueue = fileChunkQueue;
        }

        public override void Start()
        {
            using (var writer = new BinaryWriter(File.Open(_fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None)))
            {
                while (!IsStopped)
                {
                    if (_fileChunkQueue.Count() > 0)
                    {
                        var bytes = _fileChunkQueue.Dequeue();
                        writer.Write(bytes);
                        //writer.Flush();
                    }
                    else
                    {
                        if (_fileChunkQueue.IsLast)
                            break;

                        Thread.Sleep(100);
                    }
                }
            }
        }
    }
}