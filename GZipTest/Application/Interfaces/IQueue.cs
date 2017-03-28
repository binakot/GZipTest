namespace GZipTest.Application.Interfaces
{
    public interface IQueue<T>
    {
        void Enqueue(T item);
        T Dequeue();
        T Peek();
        int Count();
        void Clear();
    }
}