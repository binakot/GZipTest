using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Workers
{
    [TestClass]
    public class ReadWorkerTests
    {
        [TestMethod]
        public void TestReadSomeFile()
        {
            var file = new FileInfo("input.txt");
            var fileLength = (int) file.Length;

            var fileChunks = new FileChunkQueue();
            const int chunksCount = 10;

            var readWorker = new ReadWorker(file.Name, fileChunks, OperationType.Compress, fileLength / (chunksCount - 1), fileLength);
            var readWorkerThread = new Thread(() => readWorker.Start())
            {
                Name = "FileReadWorker",
                Priority = ThreadPriority.AboveNormal
            };
            readWorkerThread.Start();
            readWorkerThread.Join(TimeSpan.FromSeconds(1));

            Assert.AreEqual(chunksCount, fileChunks.Count());
            
            var bytes = new List<byte>(fileLength);
            for (var i = 0; i < chunksCount; i++)
                bytes.AddRange(fileChunks.Dequeue());

            Assert.AreEqual(0, fileChunks.Count());
            Assert.AreEqual(fileLength, bytes.Count);
            CollectionAssert.AreEqual(File.ReadAllBytes(file.Name), bytes);
        }
    }
}
