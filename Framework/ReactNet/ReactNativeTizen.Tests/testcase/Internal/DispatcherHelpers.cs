using System;
using System.Threading;
using System.Threading.Tasks;

using ElmSharp;

namespace ReactNative.Bridge
{
    static class DispatcherHelpers
    {
        public static void AssertOnDispatcher()
        {
            if (!IsOnDispatcher())
            {
                throw new InvalidOperationException("Thread does not have dispatcher access.");
            }
        }

        public static bool IsOnDispatcher()
        {
            return EcoreMainloop.IsMainThread;
        }

        public static /*async*/ void RunOnDispatcher(Action action)
        {
            //Async call
            EcoreMainloop.Post(action);
        }

        public static Task<T> CallOnDispatcher<T>(Func<T> func)
        {
            var taskCompletionSource = new TaskCompletionSource<T>();

            RunOnDispatcher(() =>
            {
                var result = func();

                // TaskCompletionSource<T>.SetResult can call continuations
                // on the awaiter of the task completion source.
                Task.Run(() => taskCompletionSource.SetResult(result));
            });

            return taskCompletionSource.Task;
        }
    }
}
