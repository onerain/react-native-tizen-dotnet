using System;
using System.Threading;
using System.Threading.Tasks;

using ElmSharp;
using ReactNativeTizen.ElmSharp.Extension;

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

        public static void RunOnDispatcher(Action action)
        {
            //Async call
            EcoreLoopProxy.PostAndWakeUp(action);
            //await new Task(action);   　// TODO: bind to the ecore main loop
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
