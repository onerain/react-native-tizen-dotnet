using ElmSharp;
using ReactNative.Bridge;
using System;
using System.Threading;
using Tizen;

namespace ReactNativeTizen.ElmSharp.Extension
{
    public class EcoreLoopProxy : ILifecycleEventListener
    {

        private static readonly SemaphoreSlim _mutex = new SemaphoreSlim(1, 1);

        public void OnSuspend()
        {
        }

        public void OnResume()
        {
        }

        public void OnDestroy()
        {
            _mutex.Dispose();
        }

        public static async void Send(Action action)
        {
            await _mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                EcoreMainloop.Send(action);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public static bool IsMainThread
        {
            get
            {
                return EcoreMainloop.IsMainThread;
            }
        }

        public static async void Post(Action action)
        {
            await _mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                EcoreMainloop.Post(action);
            }
            finally
            {
                _mutex.Release();
            }
        }

        public static async void Quit()
        {
            await _mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                EcoreMainloop.Quit();
            }
            finally
            {
                _mutex.Release();
            }
        }

        public static async void PostAndWakeUp(Action action)
        {
            await _mutex.WaitAsync().ConfigureAwait(false);

            try
            {
                EcoreMainloop.PostAndWakeUp(action);
            }
            finally
            {
                _mutex.Release();
            }
        }
    }
}
