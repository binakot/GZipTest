using System;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GZipTestTests.Utils
{
    [TestClass]
    //[Ignore]
    public sealed class InfoTests
    {
        [TestMethod]
        public void EnvironmentInfoTest()
        {
            var info = new ComputerInfo();
            Console.WriteLine("Physical memory: available {0} / total {1} Mb", info.AvailablePhysicalMemory / 1024 / 1024, info.TotalPhysicalMemory / 1024 / 1024);
            Console.WriteLine("Virtual memory: available {0} / total {1} Mb", info.AvailableVirtualMemory / 1024 / 1024, info.TotalVirtualMemory / 1024 / 1024);
            Console.WriteLine("CPU count is {0}", Environment.ProcessorCount); // Available since .NET 4.0
            Console.WriteLine("Memory page size is {0} bytes", Environment.SystemPageSize); // Available since .NET 4.0
        }
    }
}