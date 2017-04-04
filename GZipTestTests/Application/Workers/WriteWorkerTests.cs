using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using GZipTest.Application.Files;
using GZipTest.Application.Workers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Workers
{
    [TestClass]
    public class WriteWorkerTests
    {
        [TestMethod]
        public void TestWriteSomeFile()
        {
            var file = new FileInfo("output.txt");
            var chunks = new List<byte[]>(3)
            {
                Encoding.UTF8.GetBytes("Hello, World!"),
                Encoding.UTF8.GetBytes("Hello, Veeam!"),
                Encoding.UTF8.GetBytes("Hello, .NET!")
            };

            var fileChunks = new FileChunkQueue();
            fileChunks.Enqueue(chunks[0]);
            fileChunks.Enqueue(chunks[1]);
            fileChunks.Enqueue(chunks[2]);

            var writeWorker = new WriteWorker(file.Name, fileChunks);
            var writeWorkerThread = new Thread(() => writeWorker.Start())
            {
                Name = "FileWriteWorker",
                Priority = ThreadPriority.AboveNormal
            };
            writeWorkerThread.Start();
            Thread.Sleep(TimeSpan.FromSeconds(1));
            fileChunks.IsLast = true;
            writeWorkerThread.Join(TimeSpan.FromSeconds(1));

            var excpectedBytes = new List<byte>(0);
            foreach (var chunk in chunks)
                excpectedBytes.AddRange(chunk);

            Assert.AreEqual(0, fileChunks.Count());
            Assert.AreEqual(excpectedBytes.Count, file.Length);
            CollectionAssert.AreEqual(excpectedBytes, File.ReadAllBytes(file.Name));
        }
    }
}
