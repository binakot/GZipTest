using System.IO;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Commands
{
    public sealed class WriteChunkCommand : ICommand
    {
        private static volatile int _nextWriteChunk; // TODO Replace with AutoResetEvent, EventWaitHandle, Interlocked or something else?

        // From
        private readonly int _chunkIndex;
        private readonly IHolder _inputHolder;
        // To
        private readonly string _fileName;

        public WriteChunkCommand(int chunkIndex, IHolder inputHolder, string fileName)
        {
            _chunkIndex = chunkIndex;
            _inputHolder = inputHolder;
            _fileName = fileName;

            if (_chunkIndex == 0)
                _nextWriteChunk = 0; // Reset on a new file processing.
        }

        public void Execute()
        {
            while (_chunkIndex != _nextWriteChunk) { } // The file must be written sequentially, because the size of the following blocks is unknown.

            using (var writer = new BinaryWriter(File.Open(_fileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None)))
            {
                writer.BaseStream.Seek(0, SeekOrigin.End);
                var bytes = _inputHolder.Get(_chunkIndex);
                writer.Write(bytes);
            }

            _nextWriteChunk++;
        }
    }
}