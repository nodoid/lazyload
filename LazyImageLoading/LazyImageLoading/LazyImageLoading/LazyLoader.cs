// The source for lazy loading is from https://github.com/jquintus/ShoppingCartXF/tree/master/ShoppingCart/ShoppingCart/Async
// (c) Josh Quintus

using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Xamarin.Forms;

namespace LazyImageLoading
{
    public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
    {
        private readonly TResult defaultResult;

        public event PropertyChangedEventHandler PropertyChanged;

        public bool IsCanceled { get { return Task.IsCanceled; } }

        public bool IsCompleted { get { return Task.IsCompleted; } }

        public bool IsFaulted { get { return Task.IsFaulted; } }

        public bool IsNotCompleted { get { return !Task.IsCompleted; } }

        public bool IsSuccessfullyCompleted { get { return Task.Status == TaskStatus.RanToCompletion; } }

        public TaskStatus Status { get { return Task.Status; } }

        public Task<TResult> Task { get; private set; }

        public string ErrorMessage
        {
            get { return (InnerException == null) ? null : InnerException.Message; }
        }

        public AggregateException Exception { get { return Task.Exception; } }

        public Exception InnerException
        {
            get { return (Exception == null) ? null : Exception.InnerException; }
        }

        public NotifyTaskCompletion(Task<TResult> task, TResult defResult = default(TResult))
        {
            defaultResult = defResult;
            Task = task;
            if (!task.IsCompleted)
                WatchTaskAsync(task).GetAwaiter();
        }

        public TResult Result
        {
            get { return (Task.Status == TaskStatus.RanToCompletion) ? Task.Result : defaultResult; }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
            }

            var propertyChanged = PropertyChanged;
            if (propertyChanged != null)
            {
                propertyChanged(this, new PropertyChangedEventArgs("Status"));
                propertyChanged(this, new PropertyChangedEventArgs("IsCompleted"));
                propertyChanged(this, new PropertyChangedEventArgs("IsNotCompleted"));

                if (task.IsCanceled)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsCanceled"));
                }
                else if (task.IsFaulted)
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsFaulted"));
                    propertyChanged(this, new PropertyChangedEventArgs("Exception"));
                    propertyChanged(this, new PropertyChangedEventArgs("InnerException"));
                    propertyChanged(this, new PropertyChangedEventArgs("ErrorMessage"));
                }
                else
                {
                    propertyChanged(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
                    propertyChanged(this, new PropertyChangedEventArgs("Result"));
                }
            }
        }
    }

    public class AsyncLazy<T> : Lazy<Task<T>>
    {
        public AsyncLazy(Func<T> valueFactory)
            : base(() => Task.Factory.StartNew(valueFactory))
        {
        }

        public AsyncLazy(Func<Task<T>> taskFactory)
            : base(() => Task.Factory.StartNew(() => taskFactory()).Unwrap())
        {
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return base.Value.GetAwaiter();
        }
    }

    public static class AsyncImageSource
    {
        public static NotifyTaskCompletion<ImageSource> FromTask(Task<ImageSource> task, ImageSource defaultSource)
        {
            return new NotifyTaskCompletion<ImageSource>(task, defaultSource);
        }

        public static NotifyTaskCompletion<ImageSource> FromUriAndResource(string uri, string resource)
        {
            var u = new Uri(uri);
            return FromUriAndResource(u, resource);
        }

        public static NotifyTaskCompletion<ImageSource> FromUriAndResource(Uri uri, string resource)
        {
            var t = Task.Run(() => ImageSource.FromUri(uri));
            var defaultResouce = ImageSource.FromResource(resource);

            return FromTask(t, defaultResouce);
        }
    }
}

