using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using GZipTest.Application.Context;
using GZipTest.Application.Files;
using GZipTest.Application.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Workers
{
    [TestClass]
    public class ProcessWorkerTests
    {
        [TestMethod]
        public void TestProcessWorker()
        {
            var chunks = new List<byte[]>(3)
            {
                Encoding.UTF8.GetBytes("Hello, World!"),
                Encoding.UTF8.GetBytes("Hello, Veeam!"),
                Encoding.UTF8.GetBytes("Hello, .NET!")
            };

            var inputChunks = new FileChunkQueue();
            inputChunks.Enqueue(chunks[0]);
            inputChunks.Enqueue(chunks[1]);
            inputChunks.Enqueue(chunks[2]);
            var outputChunks = new FileChunkQueue();

            var compressWorker = new ProcessWorker(inputChunks, outputChunks, OperationType.Compress);
            var compressWorkerThread = new Thread(() => compressWorker.Start())
            {
                Name = "CompressWorker"
            };
            compressWorkerThread.Start();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            inputChunks.IsLast = true;
            compressWorkerThread.Join(TimeSpan.FromSeconds(1));

            Assert.AreEqual(0, inputChunks.Count());
            Assert.AreEqual(3, outputChunks.Count());

            var decompressWorker = new ProcessWorker(outputChunks, inputChunks, OperationType.Decompress);
            var decompressWorkerThread = new Thread(() => decompressWorker.Start())
            {
                Name = "DecompressWorker"
            };
            decompressWorkerThread.Start();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            outputChunks.IsLast = true;
            decompressWorkerThread.Join(TimeSpan.FromSeconds(1));

            Assert.AreEqual(3, inputChunks.Count());
            Assert.AreEqual(0, outputChunks.Count());

            CollectionAssert.AreEqual(chunks[0], inputChunks.Dequeue());
            CollectionAssert.AreEqual(chunks[1], inputChunks.Dequeue());
            CollectionAssert.AreEqual(chunks[2], inputChunks.Dequeue());
        }
    }
}
