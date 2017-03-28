using System.Collections.Generic;
using System.IO;
using GZipTest.Application.Commands;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Commands
{
    [TestClass]
    public class ReadChunkCommandTests
    {
        [TestMethod]
        public void TestReadWholeFileToOneChunk()
        {
            var file = new FileInfo("input.txt");
            var chunkHolder = new FileChunkHolder();

            var command = new ReadChunkCommand(file.Name, 0, (int) file.Length, 0, chunkHolder);
            command.Execute();

            var chunk = chunkHolder.Get(0);
            Assert.AreEqual(file.Length, chunk.Length);
            CollectionAssert.AreEqual(File.ReadAllBytes(file.Name), chunk);
        }

        [TestMethod]
        public void TestReadSomeFileToSeveralChunks()
        {
            var file = new FileInfo("input.txt");
            var chunkHolder = new FileChunkHolder();
            var commands = new List<ICommand>(0);

            var bytesAvailable = file.Length;
            var readPosition = 0;
            var chunkIndex = 0;
            while (bytesAvailable > 0)
            {
                var readCount = bytesAvailable < Constants.DefaultByteBufferSize
                    ? (int) bytesAvailable
                    : Constants.DefaultByteBufferSize;

                commands.Add(new ReadChunkCommand(file.Name, readPosition, readCount, chunkIndex, chunkHolder));

                bytesAvailable -= readCount;
                readPosition += readCount;
                chunkIndex++;
            }

            foreach (var command in commands)
                command.Execute();

            var chunksCount = chunkHolder.Count();
            Assert.AreEqual(file.Length / Constants.DefaultByteBufferSize + 1, chunksCount);

            var chunkHolderBytes = new List<byte>(0);
            for (var i = 0; i < chunksCount; i++)
                chunkHolderBytes.AddRange(chunkHolder.Get(i));
            Assert.AreEqual(0, chunkHolder.Count());
            Assert.AreEqual(file.Length, chunkHolderBytes.Count);
            CollectionAssert.AreEqual(File.ReadAllBytes(file.Name), chunkHolderBytes);
        }
    }
}