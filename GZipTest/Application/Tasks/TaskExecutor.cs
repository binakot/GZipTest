using System.Collections.Generic;
using System.Threading;
using GZipTest.Application.Context;
using GZipTest.Application.Interfaces;
using GZipTest.Utils;

namespace GZipTest.Application.Tasks
{
    public sealed class TaskExecutor : IExecutor<BaseTask>
    {
        private readonly IQueue<BaseTask> _queue = new TaskQueue();
        private readonly int _maxThreadsCount;
        private readonly List<Thread> _threadPool; // TODO Use native SetThreadAffinityMask() from kernel32.dll to assign threads to different CPUs manually?
        private readonly object _lock = new object();

        private volatile bool _isExecuting;

        private int _totalTasks;

        public TaskExecutor(int maxThreadsCount)
        {
            _maxThreadsCount = maxThreadsCount;
            _threadPool = new List<Thread>(_maxThreadsCount);
        }

        public void AddTask(BaseTask task)
        {
            _queue.Enqueue(task);
        }

        public void AddTasks(IEnumerable<BaseTask> tasks)
        {
            foreach (var task in tasks)
                _queue.Enqueue(task);
        }

        public void Start()
        {
            _isExecuting = true;

            while (true)
            {
                GCUtils.FreeMemoryOptimizer(Constants.MinimalAvailableMemoryInPercentages);

                if (!_isExecuting || (_queue.Count() == 0 && _threadPool.Count == 0)) // Stop execution or all tasks done.
                    break;

                if (_threadPool.Count == _maxThreadsCount) // The whole pool is busy with threads.
                    continue;
                    
                var task = _queue.Dequeue();
                if (task == null) // All tasks from the queue are pulled out.
                    continue;

                task.TaskDone += (sender, args) => { lock (_lock) _threadPool.Remove(args.Thread); };
                _totalTasks++;

                var thread = new Thread(() => task.Start()) { Name = "GZipTask" + _totalTasks, Priority = ThreadPriority.AboveNormal};
                lock (_lock) _threadPool.Add(thread);
                thread.Start();
            }

            _isExecuting = false;
            _totalTasks = 0;
        }

        public void Stop()
        {
            _isExecuting = false;
            lock (_lock)
            {
                foreach (var thread in _threadPool)
                    thread.Abort();
                _threadPool.Clear();

                _queue.Clear();
            }
        }
    }
}