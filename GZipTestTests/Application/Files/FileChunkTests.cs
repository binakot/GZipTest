using System;
using System.Text;
using GZipTest.Application.Files;
using GZipTest.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Application.Files
{
    [TestClass]
    public sealed class FileChunkTests
    {
        [TestMethod]
        public void TestFileChunksFromSomeByteArray()
        {
            var bytes = Encoding.UTF8.GetBytes("Hello, World!");
            var holder = new FileChunkHolder();
            holder.Add(0, bytes.RangeSubset(0, 5));
            holder.Add(1, bytes.RangeSubset(5, 2));
            holder.Add(2, bytes.RangeSubset(7, 5));
            holder.Add(3, bytes.RangeSubset(12, 1));

            Assert.AreEqual(4, holder.Count());
            Assert.AreEqual("Hello", Encoding.UTF8.GetString(holder.Get(0)));
            Assert.AreEqual(", ", Encoding.UTF8.GetString(holder.Get(1)));
            Assert.AreEqual("World", Encoding.UTF8.GetString(holder.Get(2)));
            Assert.AreEqual("!", Encoding.UTF8.GetString(holder.Get(3)));
            Assert.AreEqual(0, holder.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestAddChucksWithSameIndexes()
        {
            var holder = new FileChunkHolder();
            holder.Add(0, new byte[] {0x00});
            holder.Add(0, new byte[] {0xDE, 0xAD});
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestGetSameChuckSeveralTimes()
        {
            var holder = new FileChunkHolder();
            holder.Add(0, new byte[0]);
            holder.Get(0);
            holder.Get(0);
        }
    }
}