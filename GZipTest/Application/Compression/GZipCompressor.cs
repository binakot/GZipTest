using System.IO;
using System.IO.Compression;
using GZipTest.Application.Interfaces;
using GZipTest.Utils;

namespace GZipTest.Application.Compression
{
    public sealed class GZipCompressor : ICompressor
    {
        public byte[] Compress(byte[] originalBytes)
        {
            using (var output = new MemoryStream())
            {
                using (var compressStream = new GZipStream(output, CompressionMode.Compress))
                {
                    compressStream.Write(originalBytes, 0, originalBytes.Length);
                }

                return output.ToArray();
            }
        }

        public byte[] Decompress(byte[] compressedBytes)
        {
            using (var output = new MemoryStream())
            {
                using (var input = new MemoryStream(compressedBytes))
                {
                    using (var decompressStream = new GZipStream(input, CompressionMode.Decompress))
                    {
                        decompressStream.Copy(output);
                    }

                    return output.ToArray();
                }
            }
        }
    }
}