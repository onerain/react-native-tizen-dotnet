using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PCLStorage;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Threading.Tasks;
using ReactNative.Bridge;
using static System.FormattableString;

using Tizen;
using ReactNative.Common;

namespace ReactNative.JavaScriptCore.Executor
{
    /// <summary>
    /// JavaScript runtime wrapper.
    /// </summary>
	public class JSCoreJavaScriptExecutor : IJavaScriptExecutor
    {
		private const string FBBatchedBridgeVariableName = "__fbBatchedBridge";

		private JSContext _context;

        private JSFunction _nativeLoggingHook;
        //private JSFunction _nativeInjectHMRUpdate;

		private JSObject _globalObject;

        private JSObject _callFunctionAndReturnFlushedQueueFunction;
        private JSObject _invokeCallbackAndReturnFlushedQueueFunction;
        private JSObject _flushedQueueFunction;

        /// <summary>
        /// Instantiates the <see cref="JSCoreJavaScriptExecutor"/>.
        /// </summary>
        public JSCoreJavaScriptExecutor()
        {
            InitializeJSCore();
        }
		
        /// <summary>
        /// Call the JavaScript method from the given module.
        /// </summary>
		/// <param name="moduleName">The module name.</param>
        /// <param name="methodName">The method name.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The flushed queue of native operations.</returns>
		public JToken CallFunctionReturnFlushedQueue(string moduleName, string methodName, JArray arguments){
		
			if (moduleName == null)
				throw new ArgumentNullException(nameof(moduleName));
			if (methodName == null)
				throw new ArgumentNullException(nameof(methodName));
			if (arguments == null)
				throw new ArgumentNullException(nameof(arguments));

			var moduleNameValue = new JSValue(_context, moduleName);
			moduleNameValue.Protect();
			var methodNameValue = new JSValue(_context, methodName);
			methodNameValue.Protect();
			var argumentsValue = ConvertJson(arguments);
			argumentsValue.Protect();

			//TODO: judge which one is right, due to microsoft's confusion code
			//var globalObject = EnsureGlobalObject();// as the first arguments???
			var callArguments = new JSValue[3];
			callArguments[0] = moduleNameValue;
            callArguments[1] = methodNameValue;
            callArguments[2] = argumentsValue;
			var batchedBridge = EnsureBatchedBridge();
			var method = EnsureCallFunction();
            //var func = "__fbBatchedBridge" + "." + "callFunctionReturnFlushedQueue" + ".apply(null, " + arguments.ToString() + ")";
            //var ret = _context.EvaluateScript(func);

            var ret = method.CallAsFunction(batchedBridge, callArguments);
            var flushedQueue = ConvertJson(ret);

            //Log.Info(Common.ReactConstants.Tag, Invariant($"{moduleName}.{methodName}({arguments.ToString()}) = {ret.ToString()}"));


            moduleNameValue.Unprotect();
			methodNameValue.Unprotect();
			argumentsValue.Unprotect();

            return flushedQueue;
		}

        /// <summary>
        /// Flush the queue.
        /// </summary>
        /// <returns>The flushed queue of native operations.</returns>
        public JToken FlushedQueue()
        {
            var batchedBridge = EnsureBatchedBridge();
            var method = EnsureFlushedQueueFunction();
            var callArguments = new JSValue[0]; //a empty JSValue must be initialized
            return ConvertJson(method.CallAsFunction(batchedBridge, callArguments));
        }

        /// <summary>
        /// Invoke the JavaScript callback.
        /// </summary>
        /// <param name="callbackId">The callback identifier.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>The flushed queue of native operations.</returns>
        public JToken InvokeCallbackAndReturnFlushedQueue(int callbackId, JArray arguments)
        {
            if (arguments == null)
                throw new ArgumentNullException(nameof(arguments));

            var callbackIdValue = new JSValue(_context, callbackId);
            callbackIdValue.Protect();
            var argumentsValue = ConvertJson(arguments);
            argumentsValue.Protect();

            var callArguments = new JSValue[2];
            callArguments[0] = callbackIdValue;
            callArguments[1] = argumentsValue;
			var batchedBridge = EnsureBatchedBridge();
            var method = EnsureInvokeFunction();
            var flushedQueue = ConvertJson(method.CallAsFunction(batchedBridge, callArguments));

            //Log.Info(Common.ReactConstants.Tag, Invariant($"{callbackId}({arguments.ToString()}) = {flushedQueue.ToString()}"));

            argumentsValue.Unprotect();
            callbackIdValue.Unprotect();

            return flushedQueue;
        }

        /// <summary>
        /// Runs the given script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="sourceUrl">The source URL.</param>
        public void RunScript(string script, string sourceUrl)
        {
            if (script == null)
                throw new ArgumentNullException(nameof(script));

            //if (sourceUrl == null)
            //    throw new ArgumentNullException(nameof(sourceUrl));

            string source = LoadScriptAsync(script).Result;

            try
            {
                _context.EvaluateScript(source, null, sourceUrl, 0);
            }
            catch (JSErrorException ex)
            {
                Log.Error(ReactConstants.Tag, "## Enter LoadScriptAsync ## JSErrorException:" + ex.ToString());
                var message = ex.Error.ToJsonString();
                throw new Modules.Core.JavaScriptException(message, "no stack info", ex);
            }
            catch (JSException ex)
            {
                Log.Error(ReactConstants.Tag, "## Enter LoadScriptAsync ## JSException:" + ex.ToString());
                var jsonError = JSValueToJTokenConverter.Convert(ex.Error);
                var message = "line: " + jsonError.Value<string>("line") + ", column: " + jsonError.Value<string>("column")
                    + ", sourceURL: " +  jsonError.Value<string>("sourceURL");
                var stackTrace = jsonError.Value<string>("stack");
                if (stackTrace == null)
                {
                    stackTrace = "no stack info";
                }
                throw new Modules.Core.JavaScriptException(message, stackTrace, ex);
            }
        }

        private static async Task<string> LoadScriptAsync(string fileName)
        {
            Log.Info(ReactConstants.Tag, "## Enter LoadScriptAsync ## bundle file:" + fileName.ToString());

            try
            {
                var storageFile = await FileSystem.Current.GetFileFromPathAsync(fileName).ConfigureAwait(false);
                return await storageFile.ReadAllTextAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                var exceptionMessage = Invariant($"File read exception for asset '{fileName}'.");
                throw new InvalidOperationException(exceptionMessage, ex);
            }
        }

		/// <summary>
		/// Sets a global variable in the JavaScript runtime.
		/// </summary>
		/// <param name="propertyName">The global variable name.</param>
		/// <param name="value">The value.</param>
        public void SetGlobalVariable(string propertyName, JToken value)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            var jSValue = ConvertJson(value);

            //Use more than 1second to print, opened only when you need it.
            //System.Console.Out.WriteLine(">>>>>>>>>>>>>>>: calling SetGlobalVariable(propertyName=" + propertyName + ", value=" + jSValue.ToJsonString() + ")");

            EnsureGlobalObject().SetProperty(propertyName, jSValue);
        }

        /// <summary>
        /// Gets a global variable from the JavaScript runtime.
        /// </summary>
        /// <param name="propertyName">The global variable name.</param>
        /// <returns>The value.</returns>
        public JToken GetGlobalVariable(string propertyName)
        {
            if (propertyName == null)
                throw new ArgumentNullException(nameof(propertyName));

            return ConvertJson(EnsureGlobalObject().GetProperty(propertyName));
        }

        /// <summary>
        /// Disposes the <see cref="JSCoreJavaScriptExecutor"/> instance.
        /// </summary>
		public void Dispose()
		{
			_context.GarbageCollect();
		}

		private void InitializeJSCore()
		{
			_context = new JSContext();

			_context.BindManagedRuntime();
			//we do not need this any more eval() is support now
            //_nativeInjectHMRUpdate = new JSFunction(_context, "nativeInjectHMRUpdate", nativeInjectHMRUpdate);
            //_context.GlobalObject.SetProperty("nativeInjectHMRUpdate", _nativeInjectHMRUpdate);

            _nativeLoggingHook = new JSFunction(_context, "nativeLoggingHook", nativeLoggingHook);
            _context.GlobalObject.SetProperty("nativeLoggingHook", _nativeLoggingHook);
        }

        private JSValue nativeInjectHMRUpdate(JSObject function, JSObject thisObject, JSValue[] args)
        {
            string execJSString = args[0].ToString();
            string jsURL = args[1].ToString();
            
            RunScript(execJSString, jsURL);

            return JSValue.NewUndefined(function.Context);
        }

        private JSValue nativeLoggingHook(JSObject function, JSObject thisObject, JSValue[] args)
        {
            /*
                Log level in js :
                const LOG_LEVELS = {
                  trace: 0,
                  info: 1,
                  warn: 2,
                  error: 3
                };
                global.console = {
                    error: getNativeLogFunction(LOG_LEVELS.error),
                    info: getNativeLogFunction(LOG_LEVELS.info),
                    log: getNativeLogFunction(LOG_LEVELS.info),
                    warn: getNativeLogFunction(LOG_LEVELS.warn),
                    trace: getNativeLogFunction(LOG_LEVELS.trace),
                    debug: getNativeLogFunction(LOG_LEVELS.trace),
                    table: consoleTablePolyfill
                };
            */
            string level = "0";
            if(args.Length > 1)
            {
                level = args[1].ToString();
            }
            if(args.Length > 0)
            {
                int lev = int.Parse(level);
                switch(lev)
                {
                    case 0:
                        Log.Debug(ReactConstants.Tag_JS, args[0].ToString(), "", "", 0);
                        break;
                    case 1:
                        Log.Info(ReactConstants.Tag_JS, args[0].ToString(), "", "", 0);
                        break;
                    case 2:
                        Log.Warn(ReactConstants.Tag_JS, args[0].ToString(), "", "", 0);
                        break;
                    case 3:
                        Log.Error(ReactConstants.Tag_JS, args[0].ToString(), "", "", 0);
                        break;
                }
            }

            return JSValue.NewUndefined(function.Context);
        }

		private JSValue ConvertJson(JToken token)
		{
			var json = token.ToString(Formatting.None);
			return JSValue.FromJson(_context, json);
		}

		private JToken ConvertJson(JSValue value)
		{
			return JToken.Parse(value.ToString());
		}

		#region Global Helpers

		private JSObject EnsureGlobalObject()
		{
			if (null == _globalObject)
			{
				_globalObject = _context.GlobalObject;
			}
			return _globalObject;
		}

		private JSObject EnsureBatchedBridge()
        {
            var globalObject = EnsureGlobalObject();
            var batchBridge = globalObject.GetProperty(FBBatchedBridgeVariableName);
            if (null == batchBridge)
            {
                throw new InvalidOperationException(
                    Invariant($"Could not fetch '{FBBatchedBridgeVariableName}' object.  Check the JavaScript bundle to ensure it is generated correctly."));
            }
            var fbBatchedBridge = batchBridge.ObjectValue;
            if (null == fbBatchedBridge)
            {
                throw new InvalidOperationException(
                    Invariant($"Could not resolve '{FBBatchedBridgeVariableName}' object.  Check the JavaScript bundle to ensure it is generated correctly."));
            }

            return fbBatchedBridge;
        }

		private JSObject EnsureCallFunction()
        {
            if (null == _callFunctionAndReturnFlushedQueueFunction)
            {
                var fbBatchedBridge = EnsureBatchedBridge();
                _callFunctionAndReturnFlushedQueueFunction = (fbBatchedBridge.GetProperty("callFunctionReturnFlushedQueue")).ObjectValue;
            }

            return _callFunctionAndReturnFlushedQueueFunction;
        }


        private JSObject EnsureInvokeFunction()
        {
            if (null == _invokeCallbackAndReturnFlushedQueueFunction)
            {
                var fbBatchedBridge = EnsureBatchedBridge();
                _invokeCallbackAndReturnFlushedQueueFunction = (fbBatchedBridge.GetProperty("invokeCallbackAndReturnFlushedQueue")).ObjectValue;
            }

            return _invokeCallbackAndReturnFlushedQueueFunction;
        }

        private JSObject EnsureFlushedQueueFunction()
        {
            if (null == _flushedQueueFunction)
            {
                var fbBatchedBridge = EnsureBatchedBridge();
                _flushedQueueFunction = (fbBatchedBridge.GetProperty("flushedQueue")).ObjectValue;
            }

            return _flushedQueueFunction;
        }

		#endregion
    }
}
