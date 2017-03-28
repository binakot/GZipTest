using GZipTest.Application.Commands;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Tasks
{
    public sealed class DecompressionTask : BaseTask
    {
        public DecompressionTask(
            int chunkIndex, IHolder inputHolder, IHolder outputHolder, ICompressor compressor,
            string inputFileName, long readStartPosition, int readLength,
            string outputFileName)
        {
            AddCommand(new ReadChunkCommand(inputFileName, readStartPosition, readLength, chunkIndex, inputHolder));
            AddCommand(new DecompressChunkCommand(chunkIndex, inputHolder, outputHolder, compressor));
            AddCommand(new WriteChunkCommand(chunkIndex, outputHolder, outputFileName));
        }
    }
}