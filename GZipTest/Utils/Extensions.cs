using System;
using System.IO;
using GZipTest.Application.Context;

namespace GZipTest.Utils
{
    public static class Extensions
    {
        /// <summary>
        /// Copy data from this stream to the output one using buffer with certain size.
        /// </summary>
        /// <param name="input">Source stream</param>
        /// <param name="output">Destination stream</param>
        /// <param name="bufferSize">Buffer size for data copying</param>
        public static void Copy(this Stream input, Stream output, int bufferSize = Constants.DefaultByteBufferSize)
        {
            var buffer = new byte[bufferSize];
            int bytesRead;
            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
                output.Write(buffer, 0, bytesRead);
        }

        /// <summary>
        /// Read all available bytes from input stream to an array.
        /// </summary>
        /// <param name="input">Source stream</param>
        /// <param name="bufferSize">Buffer size for data copying</param>
        /// <returns>Array of bytes from the input stream</returns>
        public static byte[] ReadAllBytes(this Stream input, int bufferSize = Constants.DefaultByteBufferSize)
        {
            using (var output = new MemoryStream())
            {
                input.Copy(output, bufferSize);
                return output.ToArray();
            }
        }

        /// <summary>
        /// Return a subset from a range of source array.
        /// </summary>
        /// <typeparam name="T">Type of array elements</typeparam>
        /// <param name="array">Source array</param>
        /// <param name="startIndex">Start index to copy</param>
        /// <param name="length">Length of subset</param>
        /// <returns>Range subset array</returns>
        public static T[] RangeSubset<T>(this T[] array, int startIndex, int length)
        {
            var subset = new T[length];
            Array.Copy(array, startIndex, subset, 0, length);
            return subset;
        }
    }
}