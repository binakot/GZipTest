using System.IO;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Commands
{
    public sealed class ReadChunkCommand : ICommand
    {
        // From
        private readonly string _fileName;
        private readonly long _startPosition;
        private readonly int _bytesCount;
        // To
        private readonly int _chunkIndex;
        private readonly IHolder _outputHolder;

        public ReadChunkCommand(string fileName, long startPosition, int bytesCount, int chunkIndex, IHolder outputHolder)
        {
            _fileName = fileName;
            _startPosition = startPosition;
            _bytesCount = bytesCount;
            _chunkIndex = chunkIndex;
            _outputHolder = outputHolder;
        }

        public void Execute()
        {
            using (var reader = new BinaryReader(File.Open(_fileName, FileMode.Open, FileAccess.Read, FileShare.Read)))
            {
                reader.BaseStream.Seek(_startPosition, SeekOrigin.Begin);
                var bytes = reader.ReadBytes(_bytesCount);
                _outputHolder.Add(_chunkIndex, bytes);
            }
        }
    }
}