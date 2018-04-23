using System;
using System.Collections.Generic;

namespace Websockets.Net
{
    /// <summary>
    /// WebSocket contract.
    /// </summary>
    public interface IWebSocketConnection : IDisposable
    {
        bool IsOpen { get; }

        void Open(string url, string protocol = null, string authToken = null);

        void Open(string url, string protocol, IDictionary<string, string> headers);

        void Close();

        void Send(string message);

        void Send(byte[] message);


        event Action<object, EventArgs> OnOpened;

        event Action<object, EventArgs> OnClosed;

        event Action<IWebSocketConnection> OnDispose;

        event Action<object, string> OnError;

        event Action<object, MessageEventArgs> OnMessage;
        
        event Action<string> OnLog;
    }
}
