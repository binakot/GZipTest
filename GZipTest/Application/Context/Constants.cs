namespace GZipTest.Application.Context
{
    public static class Constants
    {
        #region EXIT CODES
        public const int SuccessExitCode = 0;
        public const int ErrorExitCode = 1;
        #endregion

        #region COMMAND LINE ARGUMENTS
        public const int OperationTypeArgumentIndex = 0;
        public const int InputFilePathArgumentIndex = 1;
        public const int OutputFilePathArgumentIndex = 2;
        public const int CommandLineArgumentsCount = 3;
        #endregion 

        #region OPERATION TYPES STRINGS
        public const string CompressOperationString = "compress";
        public const string DecompressOperationString = "decompress";
        #endregion

        #region DATA PROCESSING CONSTANTS
        // TODO Use native GetSystemInfo() from kernel32.dll to get the actual information.
        public const int MemoryPageSize = 4096; // Default system value.
        public const int DefaultByteBufferSize = 81920; // The largest multiple of 4096 that is still smaller than the large object heap threshold (85K).
        public const int MinimalAvailableMemoryInPercentages = 25; // If available physical memory is less than 25 % of total RAM, then force a full garbage collection.
        public const int MegabyteInBytes = 1024 * 1024;
        #endregion

        #region GZIP
        /// <summary>
        /// GZip header structure.
        /// http://www.zlib.org/rfc-gzip.html
        /// +---+---+---+---+---+---+---+---+---+---+
        /// |ID1|ID2|CM |FLG|     MTIME     |XFL|OS | (more-->)
        /// +---+---+---+---+---+---+---+---+---+---+
        /// +=======================+
        /// |...compressed blocks...| (more-->)
        /// +=======================+
        /// +---+---+---+---+---+---+---+---+
        /// |     CRC32     |     ISIZE     | // TODO Add CRC and original size checks? More security, but less performance...
        /// +---+---+---+---+---+---+---+---+
        /// </summary>
        public static readonly byte[] GZipDefaultHeader = { 0x1f, 0x8b, 0x08, 0x00, 0x00, 0x00, 0x00, 0x00, 0x04, 0x00 };
        #endregion
    }
}