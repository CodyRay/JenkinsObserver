using System;
using System.Threading;
using System.Threading.Tasks;
using TPLTask = System.Threading.Tasks.Task;

namespace Contracts
{
    public class TaskAsService
    {
        private CancellationTokenSource _cancel = new CancellationTokenSource();
        private TPLTask _task = TPLTask.Delay(0);
        public string Name { get; set; }

        protected Func<CancellationToken, Task> CreateTask { get; set; }

        protected CancellationTokenSource Cancel
        {
            get { return _cancel; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Cannot Set Cancel to Null");
                _cancel = value;
            }
        }

        public TPLTask Task
        {
            get { return _task; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Cannot Set Task to Null");
                _task = value;
            }
        }

        internal TaskAsService(Func<CancellationToken, TPLTask> createTask, string name)
        {
            CreateTask = createTask;
            Name = name;
        }

        public void Start()
        {
            Cancel = new CancellationTokenSource();
            Task = CreateTask(Cancel.Token);
        }

        public void Start(CancellationToken token)
        {
            Cancel = CancellationTokenSource.CreateLinkedTokenSource(token);
            Task = CreateTask(Cancel.Token);
        }

        public TPLTask Stop()
        {
            Cancel.Cancel();
            return Task;
        }

        public TPLTask StopAfter(TimeSpan delay)
        {
            Cancel.CancelAfter(delay);
            return Task;
        }

        public bool IsRunning { get { return !(Task.IsCanceled || Task.IsFaulted || Task.IsCompleted); } }

        public static TaskAsService<T> Create<T>(Func<CancellationToken, Task<T>> createTask, string name = null)
        {
            return new TaskAsService<T>(createTask, name);
        }

        public static TaskAsService Create(Func<CancellationToken, TPLTask> createTask, string name = null)
        {
            return new TaskAsService(createTask, name);
        }
    }

    public class TaskAsService<TResult>
    {
        private CancellationTokenSource _cancel = new CancellationTokenSource();
        private Task<TResult> _task = TPLTask.FromResult(default(TResult));
        public string Name { get; set; }

        protected Func<CancellationToken, Task<TResult>> CreateTask { get; set; }

        protected CancellationTokenSource Cancel
        {
            get { return _cancel; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Cannot Set Cancel to Null");
                _cancel = value;
            }
        }

        public Task<TResult> Task
        {
            get { return _task; }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Cannot Set Task to Null");
                _task = value;
            }
        }

        internal TaskAsService(Func<CancellationToken, Task<TResult>> createTask, string name)
        {
            CreateTask = createTask;
            Name = name;
        }

        public void Start()
        {
            Cancel = new CancellationTokenSource();
            Task = CreateTask(Cancel.Token);
        }

        public void Start(CancellationToken token)
        {
            Cancel = CancellationTokenSource.CreateLinkedTokenSource(token);
            Task = CreateTask(Cancel.Token);
        }

        public Task<TResult> Stop()
        {
            Cancel.Cancel();
            return Task;
        }

        public Task<TResult> StopAfter(TimeSpan delay)
        {
            Cancel.CancelAfter(delay);
            return Task;
        }

        public bool IsRunning { get { return !(Task.IsCanceled || Task.IsFaulted || Task.IsCompleted); } }
    }
}