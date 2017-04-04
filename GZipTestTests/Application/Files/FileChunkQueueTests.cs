using GZipTest.Application.Files;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Files
{
    [TestClass]
    public sealed class FileChunkQueueTests
    {
        [TestMethod]
        public void TestFileChunkQueue()
        {
            var queue = new FileChunkQueue();
            Assert.AreEqual(0, queue.Count());

            queue.Enqueue(new byte[0]);
            CollectionAssert.AreEqual(new byte[0], queue.Peek());
            Assert.AreEqual(1, queue.Count());
            CollectionAssert.AreEqual(new byte[0], queue.Dequeue());
            Assert.AreEqual(0, queue.Count());

            queue.Enqueue(new byte[] { 0x00 });
            queue.Enqueue(new byte[] { 0x01 });
            queue.Enqueue(new byte[] { 0x02 });
            Assert.AreEqual(3, queue.Count());

            queue.Dequeue();
            Assert.AreEqual(2, queue.Count());

            queue.Clear();
            Assert.AreEqual(0, queue.Count());
        }
    }
}
