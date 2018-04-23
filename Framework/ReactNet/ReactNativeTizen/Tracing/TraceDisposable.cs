using System;
using System.Collections.Generic;
using System.Diagnostics;
using ReactNative.Common;
using Tizen;

namespace ReactNative.Tracing
{
    /// <summary>
    /// Disposable implementation for tracing operations.
    /// </summary>
    /// <remarks>
    /// This implementation is created as a struct to minimize the heap impact
    /// for tracing operations.
    /// </remarks>
    public struct /* do not make class */ TraceDisposable : IDisposable
    {
        private static readonly Stopwatch s_stopwatch = Stopwatch.StartNew();

        private readonly int _traceId;
        private readonly string _title;
        private long _timestamp;
        private Dictionary<string, object> _properties;

        private readonly string _file;
        private readonly string _func;
        private readonly int _line;

        /// <summary>
        /// Instantiates the <see cref="TraceDisposable"/>.
        /// </summary>
        /// <param name="traceId">The trace ID.</param>
        /// <param name="title">The event title.</param>
        public TraceDisposable(int traceId, string title, string file = "", string func = "", int line = 0)
        {
            _traceId = traceId;
            _title = title;
            _timestamp = 0;
            _properties = null;

            _file = file;
            _func = func;
            _line = line;
        }

        /// <summary>
        /// Add a property to the <see cref="TraceDisposable"/>.
        /// </summary>
        /// <param name="key">The property key.</param>
        /// <param name="value">The property value.</param>
        /// <returns>The disposable instance.</returns>
        public TraceDisposable With(string key, object value)
        {
            var properties = _properties;
            if (properties == null)
            {
                properties = new Dictionary<string, object>();
            }

            properties[key] = value;
            _properties = properties;
            return this;
        }

        /// <summary>
        /// Start Tracing, and return this instance.
        /// </summary>
        public TraceDisposable Start()
        {
            _timestamp = s_stopwatch.ElapsedMilliseconds;

            if (_properties == null)
            {
                Log.Fatal(ReactConstants.Tag, "START-> _traceId=[" + _traceId + "] _title=[" + _title + "] _timestamp=[" + _timestamp + "ms]", _file, _func, _line);
            }
            else
            {
                string temp = "";
                foreach (KeyValuePair<string, object> k in _properties)
                {
                    temp += " " + k.Key + ":" + k.Value.ToString() + " ";
                }

                Log.Fatal(ReactConstants.Tag, "START-> _traceId=[" + _traceId + "] _title=[" + _title + "]  properties=[" + temp + "] _timestamp=[" + _timestamp + "ms]", _file, _func, _line);
            }
            return this;
        }

        /// <summary>
        /// Disposed the instance, capturing the trace.
        /// </summary>
        public void Dispose()
        {
            long used = s_stopwatch.ElapsedMilliseconds - _timestamp;

            if (_properties == null)
            {
                Log.Fatal(ReactConstants.Tag, "END-> _traceId=[" + _traceId + "] _title=[" + _title + "] time_used=[" + used + "ms]", _file, _func, _line);
            }
            else
            {
                string temp = "";
                foreach (KeyValuePair<string, object> k in _properties)
                {
                    temp += " " + k.Key + ":" + k.Value.ToString() + " ";
                }

                Log.Fatal(ReactConstants.Tag, "END  -> _traceId=[" + _traceId + "] _title=[" + _title + "]  properties=[" + temp + "] time_used=[" + used + "ms]", _file, _func, _line);
            }
        }
    }
}
