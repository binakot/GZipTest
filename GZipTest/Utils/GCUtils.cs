using System;
using Microsoft.VisualBasic.Devices;

namespace GZipTest.Utils
{
    public static class GCUtils
    {
        /// <summary>
        /// Do available memory check and start garbage collection, if free memory less than limit parameter.
        /// </summary>
        /// <param name="minFreeMemPercentages">Limit of available memory before start garbage collection</param>
        public static void FreeMemoryOptimizer(ulong minFreeMemPercentages)
        {
            var info = new ComputerInfo();
            var availableMemoryPercents = info.AvailablePhysicalMemory / info.AvailablePhysicalMemory * 100;
            if (availableMemoryPercents < minFreeMemPercentages)
                ForceGC();
        }

        /// <summary>
        /// Force garbage collection of the all generations.
        /// </summary>
        /// <param name="message">Output to console the information about allocated memory before and after GC collection.</param>
        public static void ForceGC(bool message = false)
        {
            if (message)
            {
                Console.Out.WriteLine($"Memory before full GC: {GC.GetTotalMemory(false)} bytes.");
                Console.Out.WriteLine($"Memory after full GC: {GC.GetTotalMemory(true)} bytes.");
            }
            else
            {
                GC.GetTotalMemory(true);
            }
        }
        
    }
}