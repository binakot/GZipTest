using System.Collections.Generic;
using System.IO;
using System.Text;
using GZipTest.Application.Commands;
using GZipTest.Application.Files;
using GZipTest.Application.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Commands
{
    [TestClass]
    public class WriteChunkCommandTests
    {
        [TestMethod]
        public void TestWriteWholeFileFromOneChunk()
        {
            var file = new FileInfo("output.txt");
            File.Delete(file.Name);

            var chunkHolder = new FileChunkHolder();
            var bytes = Encoding.UTF8.GetBytes("Hello, World!");
            chunkHolder.Add(0, bytes);

            var command = new WriteChunkCommand(0, chunkHolder, file.Name);
            command.Execute();

            Assert.AreEqual(0, chunkHolder.Count());
            CollectionAssert.AreEqual(File.ReadAllBytes(file.Name), bytes);
        }

        [TestMethod]
        public void TestWriteSomeFileFromSeveralChunks()
        {
            var file = new FileInfo("output.txt");
            File.Delete(file.Name);

            var chunkHolder = new FileChunkHolder();
            var chunks = new List<byte[]>
            {
                Encoding.UTF8.GetBytes("Hello, World!"),
                Encoding.UTF8.GetBytes("Hello, Veeam!"),
                Encoding.UTF8.GetBytes("Availability for the Always-On Enterprise")
            };
            chunkHolder.Add(0, chunks[0]);
            chunkHolder.Add(1, chunks[1]);
            chunkHolder.Add(2, chunks[2]);
            Assert.AreEqual(chunks.Count, chunkHolder.Count());

            var commands = new List<ICommand>(3)
            {
                new WriteChunkCommand(0, chunkHolder, file.Name),
                new WriteChunkCommand(1, chunkHolder, file.Name),
                new WriteChunkCommand(2, chunkHolder, file.Name)
            };

            foreach (var command in commands)
                command.Execute();

            var chunkHolderBytes = new List<byte>(0);
            foreach (var chunk in chunks)
                chunkHolderBytes.AddRange(chunk);
            Assert.AreEqual(0, chunkHolder.Count());
            CollectionAssert.AreEqual(chunkHolderBytes, File.ReadAllBytes(file.Name));
        }
    }
}