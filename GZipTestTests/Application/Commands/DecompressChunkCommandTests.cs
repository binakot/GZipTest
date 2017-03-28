using System.Collections.Generic;
using System.Linq;
using GZipTest.Application.Commands;
using GZipTest.Application.Compression;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Commands
{
    [TestClass]
    public class DecompressChunkCommandTests
    {
        [TestMethod]
        public void TestDecompressOneChunk()
        {
            var originalBytes = new List<byte>()
                .Concat(Constants.GZipDefaultHeader)
                .Concat(new byte[] {0x00, 0x01, 0x02}).ToArray();

            var compressor = new GZipCompressor();
            var inputHolder = new FileChunkHolder();
            var outputHolder = new FileChunkHolder();

            inputHolder.Add(0, originalBytes);
            Assert.AreEqual(1, inputHolder.Count());
            Assert.AreEqual(0, outputHolder.Count());

            var command = new DecompressChunkCommand(0, inputHolder, outputHolder, compressor);
            command.Execute();

            Assert.AreEqual(0, inputHolder.Count());
            Assert.AreEqual(1, outputHolder.Count());
            CollectionAssert.AreEqual(new byte[0], outputHolder.Get(0));
        }

        [TestMethod]
        public void TestDecompressSeveralChunks()
        {
            var originalByteChunks = new List<byte[]>(3)
            {
                new List<byte>().Concat(Constants.GZipDefaultHeader).Concat(new byte[] {0x00, 0x01, 0x02}).ToArray(),
                new List<byte>().Concat(Constants.GZipDefaultHeader).Concat(new byte[] {0x05, 0x06, 0x07}).ToArray(),
                new List<byte>().Concat(Constants.GZipDefaultHeader).Concat(new byte[] {0x0A, 0x0B, 0x0C}).ToArray()
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
                new DecompressChunkCommand(0, inputHolder, outputHolder, compressor),
                new DecompressChunkCommand(1, inputHolder, outputHolder, compressor),
                new DecompressChunkCommand(2, inputHolder, outputHolder, compressor)
            };
            foreach (var command in commands)
                command.Execute();

            Assert.AreEqual(0, inputHolder.Count());
            Assert.AreEqual(originalByteChunks.Count, outputHolder.Count());
            CollectionAssert.AreEqual(new byte[0], outputHolder.Get(0));
            CollectionAssert.AreEqual(new byte[0], outputHolder.Get(1));
            CollectionAssert.AreEqual(new byte[0], outputHolder.Get(2));
        }
    }
}