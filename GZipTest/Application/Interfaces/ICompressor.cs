namespace GZipTest.Application.Interfaces
{
    public interface ICompressor
    {
        byte[] Compress(byte[] bytes);
        byte[] Decompress(byte[] bytes);
    }
}