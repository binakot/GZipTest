using System.Collections.Generic;
using GZipTest.Application.Interfaces;

namespace GZipTest.Application.Tasks
{
    public sealed class TaskQueue : IQueue<BaseTask>
    {
        private readonly Queue<BaseTask> _queue;

        public TaskQueue()
        {
            _queue = new Queue<BaseTask>(0);
        }

        public void Enqueue(BaseTask item)
        {
            _queue.Enqueue(item);
        }

        public BaseTask Dequeue()
        {
            return _queue.Count > 0 ? _queue.Dequeue() : null;
        }

        public BaseTask Peek()
        {
            return _queue.Peek();
        }

        public int Count()
        {
            return _queue.Count;
        }

        public void Clear()
        {
            _queue.Clear();
        }
    }
}