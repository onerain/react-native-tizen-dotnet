using System;
using System.Runtime.CompilerServices;
//using System.ComponentModel;

using Tizen;


namespace ReactNative.Tracing
{
    /// <summary>
    /// Temporary NullTracing helpers for the application.
    /// </summary>
    public static class RNTracer
    {
        /// <summary>
        /// Trace ID for bridge events.
        /// </summary>
        public const int TRACE_TAG_REACT_BRIDGE = 0;

        /// <summary>
        /// Trace ID for application events.
        /// </summary>
        public const int TRACE_TAG_REACT_APPS = 1;

        /// <summary>
        /// Trace ID for view events.
        /// </summary>
        public const int TRACE_TAG_REACT_VIEW = 2;

        /// <summary>
        /// Create a TraceDisposable.
        /// </summary>
        /// <param name="tag">The trace tag.</param>
        /// <param name="name">The event name.</param>
        /// <returns>The TraceDisposable with a Start method.</returns>
        public static TraceDisposable Trace(int tag, string name, [CallerFilePath] string file = "", [CallerMemberName] string func = "", [CallerLineNumber] int line = 0)
        {
            return new TraceDisposable(tag, name, file, func, line);
        }

        /// <summary>
        /// Write an event.
        /// </summary>
        /// <param name="tag">The trace tag.</param>
        /// <param name="eventName">The event name.</param>
        public static void Write(string tag, string eventName, [CallerFilePath] string file = "", [CallerMemberName] string func = "", [CallerLineNumber] int line = 0)
        {
            Log.Fatal(tag, eventName, file, func, line);
        }

        /// <summary>
        /// Write an error event.
        /// </summary>
        /// <param name="tag">The trace tag.</param>
        /// <param name="eventName">The event name.</param>
        public static void Error(string tag, string eventName, [CallerFilePath] string file = "", [CallerMemberName] string func = "", [CallerLineNumber] int line = 0)
        {
            Log.Error(tag, "RNTracer::Error eventName=[" + eventName + "]", file, func, line);
        }

        /// <summary>
        /// Write an error event.
        /// </summary>
        /// <param name="tag">The trace tag.</param>
        /// <param name="eventName">The event name.</param>
        /// <param name="ex">The exception.</param>
        public static void Error(string tag, string eventName, Exception ex, [CallerFilePath] string file = "", [CallerMemberName] string func = "", [CallerLineNumber] int line = 0)
        {
            Log.Error(tag, "RNTracer::Error eventName=[" + eventName + "]", file, func, line);
            Log.Error(tag, "RNTracer::Error exception=[" + ex.ToString() + "]", file, func, line);
        }
    }
}