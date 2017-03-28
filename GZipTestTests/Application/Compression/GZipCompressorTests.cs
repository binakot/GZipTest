using System.Text;
using GZipTest.Application.Compression;
using GZipTest.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Compression
{
    [TestClass]
    public sealed class GZipCompressorTests
    {
        [TestMethod]
        public void TestCompressDecompressEmptyByteArray()
        {
            ICompressor compressor = new GZipCompressor();

            var originalBytes = new byte[0];
            var compressedBytes = compressor.Compress(originalBytes);
            var decompressedBytes = compressor.Decompress(compressedBytes);

            CollectionAssert.AreEqual(originalBytes, decompressedBytes);
        }

        [TestMethod]
        public void TestCompressDecompressSomeByteArray()
        {
            ICompressor compressor = new GZipCompressor();

            var originalBytes = Encoding.UTF8.GetBytes("Hello, World!");
            var compressedBytes = compressor.Compress(originalBytes);
            var decompressedBytes = compressor.Decompress(compressedBytes);

            CollectionAssert.AreEqual(originalBytes, decompressedBytes);
        }
    }
}