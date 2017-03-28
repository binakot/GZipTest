using System.Collections.Generic;
using System.Text;
using GZipTest.Application.Commands;
using GZipTest.Application.Compression;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Commands
{
    [TestClass]
    public class CompressChunkCommandTests
    {
        [TestMethod]
        public void TestCompressOneChunk()
        {
            var originalBytes = Encoding.UTF8.GetBytes("AVAILABILITY for the Always-On Enterprise");

            var compressor = new GZipCompressor();
            var inputHolder = new FileChunkHolder();
            var outputHolder = new FileChunkHolder();

            inputHolder.Add(0, originalBytes);
            Assert.AreEqual(1, inputHolder.Count());
            Assert.AreEqual(0, outputHolder.Count());

            var command = new CompressChunkCommand(0, inputHolder, outputHolder, compressor);
            command.Execute();

            Assert.AreEqual(0, inputHolder.Count());
            Assert.AreEqual(1, outputHolder.Count());
            CollectionAssert.AreEqual(compressor.Compress(originalBytes), outputHolder.Get(0));
        }

        [TestMethod]
        public void TestCompressSeveralChunks()
        {
            var originalByteChunks = new List<byte[]>(3)
            {
                Encoding.UTF8.GetBytes("High-Speed Recovery"),
                Encoding.UTF8.GetBytes("Verified Recoverability"),
                Encoding.UTF8.GetBytes("Complete Visibility")
            };

            var compressor = new GZipCompressor();
            var inputHolder = new FileChunkHolder();
            var outputHolder = new FileChunkHolder();

            inputHolder.Add(0, originalByteChunks[0]);
            inputHolder.Add(1, originalByteChunks[1]);
            inputHolder.Add(2, originalByteChunks[2]);
            Assert.AreEqual(originalByteChunks.Count, inputHolder.Count());
            Assert.AreEqual(0, outputHolder.Count());

            var commands = new List<ICommand>(3)
            {
                new CompressChunkCommand(0, inputHolder, outputHolder, compressor),
                new CompressChunkCommand(1, inputHolder, outputHolder, compressor),
                new CompressChunkCommand(2, inputHolder, outputHolder, compressor)
            };
            foreach (var command in commands)
                command.Execute();

            Assert.AreEqual(0, inputHolder.Count());
            Assert.AreEqual(originalByteChunks.Count, outputHolder.Count());
            CollectionAssert.AreEqual(compressor.Compress(originalByteChunks[0]), outputHolder.Get(0));
            CollectionAssert.AreEqual(compressor.Compress(originalByteChunks[1]), outputHolder.Get(1));
            CollectionAssert.AreEqual(compressor.Compress(originalByteChunks[2]), outputHolder.Get(2));
        }
    }
}