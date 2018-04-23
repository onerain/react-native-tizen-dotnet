using System;
using System.Collections.Generic;

namespace Websockets.Net
{
    /// <summary>
    /// A Websocket connection for 'full' .Net Core applications
    /// </summary>
    public class WebsocketConnection : IWebSocketConnection
    {
        public bool IsOpen { get; private set; }

        public event Action<object, EventArgs> OnClosed = delegate { };
        public event Action<object, EventArgs> OnOpened = delegate { };
        public event Action<IWebSocketConnection> OnDispose = delegate { };
        public event Action<object, string> OnError = delegate { };
        public event Action<object, MessageEventArgs> OnMessage = delegate { };
        public event Action<string> OnLog = delegate { };

        static WebsocketConnection()
        {
            //System.Net.ServicePointManager.ServerCertificateValidationCallback += (o, certificate, chain, errors) => true;
        }

        /// <summary>
        /// Factory Initializer
        /// </summary>
        public static void Link()
        {
            WebSocketFactory.Init(() => new WebsocketConnection());
        }

        private WebSocketWrapper _websocket = null;

        public void Open(string url, string protocol = null, string authToken = null)
        {
            var headers = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
            if (authToken != null)
            {
                headers.Add("Authorization", authToken);
            }

            Open(url, protocol, headers);
        }

        public async void Open(string url, string protocol, IDictionary<string, string> headers = null)
        {
            try
            {
                if (_websocket != null)
                    EndConnection();

                _websocket = new WebSocketWrapper();
                _websocket.Closed += _websocket_Closed;
                _websocket.Opened += _websocket_Opened;
                _websocket.Error += _websocket_Error;
                _websocket.MessageReceived += _websocket_MessageReceived;

                if (url.StartsWith("https"))
                    url = url.Replace("https://", "wss://");
                else if (url.StartsWith("http"))
                    url = url.Replace("http://", "ws://");
                
                await _websocket.Connect(url, protocol, headers);

            }
            catch (Exception ex)
            {
                OnError(this, ex.Message);
            }
        }

        public void Close()
        {
            EndConnection();
        }

        public async void Send(string message)
        {
            await _websocket.SendMessage(message);
        }

        public async void Send(byte[] message)
        {
            await _websocket.SendMessage(message);
        }

        public void Dispose()
        {
            Close();
            OnDispose(this);
        }

        //
        void EndConnection()
        {
            if (_websocket != null)
            {
                _websocket.Closed -= _websocket_Closed;
                _websocket.Opened -= _websocket_Opened;
                _websocket.Error -= _websocket_Error;
                _websocket.MessageReceived -= _websocket_MessageReceived;
                _websocket.Dispose();
                _websocket = null;

                IsOpen = false;
                OnClosed(this, new EventArgs());
            }
        }


        void _websocket_Error(Exception obj)
        {
            OnError(this, obj.Message);
        }

        void _websocket_Opened(WebSocketWrapper arg)
        {

            IsOpen = true;
            OnOpened(this, new EventArgs());
        }

        void _websocket_MessageReceived(MessageEventArgs m, WebSocketWrapper arg)
        {

            OnMessage(this, m);
        }

        void _websocket_Closed(WebSocketWrapper arg)
        {
            EndConnection();
        }
    }
}