namespace GZipTest.Application.Interfaces
{
    public interface IHolder
    {
        void Add(int index, byte[] bytes);
        byte[] Get(int index, bool remove = true);
        int Count();
        void Clear();
    }
}