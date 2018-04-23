using NUnit.Framework;
using ReactNative.Bridge.Queue;
using ReactNative.JavaScriptCore.Executor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
#if WINDOWS_UWP
using Windows.Storage;
#else
using System.IO;
using System.Reflection;
#endif
using Tizen.Applications;

namespace ReactNative.Tests
{
    static class JavaScriptHelpers
    {
        public static Task Run(Action<JSCoreJavaScriptExecutor, IMessageQueueThread> action)
        {
            return Run((executor, jsQueueThread) =>
            {
                action(executor, jsQueueThread);
                return Task.CompletedTask;
            });
        }

        public static async Task Run(Func<JSCoreJavaScriptExecutor, IMessageQueueThread, Task> action)
        {
            using (var jsQueueThread = CreateJavaScriptThread())
            {
                var executor = await jsQueueThread.CallOnQueue(() => new JSCoreJavaScriptExecutor());
                try
                {
                    await Initialize(executor, jsQueueThread);
                    await action(executor, jsQueueThread);
                }
                finally
                {
                    await jsQueueThread.CallOnQueue(() =>
                    {
                        executor.Dispose();
                        return true;
                    });
                }
            }
        }

        public static async Task Initialize(JSCoreJavaScriptExecutor executor, IMessageQueueThread jsQueueThread)
        {
            var scriptUris = new[]
            {
                @"Resources/test.js",
            };

            var scripts = new KeyValuePair<string, string>[scriptUris.Length];
            for (var i = 0; i < scriptUris.Length; ++i)
            {
                var uri = scriptUris[i];
#if WINDOWS_UWP
                var storageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx://" + "/" + uri)); 
                var filePath = storageFile.Path; 
#else
                var pathToAssemblyResource = Application.Current.DirectoryInfo.Resource;

                var u = new Uri(pathToAssemblyResource);
                var filePath = u.LocalPath;
#endif

                scripts[i] = new KeyValuePair<string, string>(uri, filePath);
            }

            await jsQueueThread.CallOnQueue(() =>
            {
                foreach (var script in scripts)
                {
                    executor.RunScript(script.Value, script.Key);
                }

                return true;
            });
        }

        private static MessageQueueThread CreateJavaScriptThread()
        {
            return MessageQueueThread.Create(MessageQueueThreadSpec.Create("js", MessageQueueThreadKind.BackgroundSingleThread), ex => { Assert.Fail(); });
        }
    }
}
