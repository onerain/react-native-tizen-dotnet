using Newtonsoft.Json.Linq;
using ReactNative.Bridge;
using ReactNative.Collections;
using ReactNative.Common;
using ReactNative.Modules.Core;
using ReactNative.Tracing;
using System;
using System.Collections.Generic;
using System.Text;
using static System.FormattableString;

namespace ReactNative.Modules.WebSocket
{
    class WebSocketModule : ReactContextNativeModuleBase
    {
        private readonly IDictionary<int, Websockets.Net.IWebSocketConnection> _webSocketConnections = new Dictionary<int, Websockets.Net.IWebSocketConnection>();

        #region Constructor(s)

        public WebSocketModule(ReactContext reactContext)
            : base(reactContext)
        {
            Websockets.Net.WebsocketConnection.Link();
        }

        #endregion

        #region NativeModuleBase Overrides

        public override string Name
        {
            get
            {
                return "WebSocketModule";
            }
        }

        #endregion

        #region Public Methods

        [ReactMethod]
        public void connect(string url, string[] protocols, JObject options, int id)
        {
            if (options != null && options.ContainsKey("origin"))
            {
                throw new NotImplementedException(/* TODO: (#253) */);
            }

            var webSocket = Websockets.Net.WebSocketFactory.Create();
            //var webSocket = new WebSocketSharp.WebSocket(url);

            webSocket.OnMessage += (sender, args) =>
            {
                OnMessageReceived(id, sender, args);
            };

            webSocket.OnOpened += (sender, args) =>
            {
                OnOpen(id, sender, args);
            };

            webSocket.OnError += (sender, args) =>
            {
                OnError(id, args);
            };

            webSocket.OnClosed += (sender, args) =>
            {
                OnClosed(id, sender, args);
            };

            InitializeInBackground(id, url, webSocket);
        }

        [ReactMethod]
        public void close(ushort code, string reason, int id)
        {
            Websockets.Net.IWebSocketConnection webSocket;

            if (!_webSocketConnections.TryGetValue(id, out webSocket))
            {
                RNTracer.Write(
                    ReactConstants.Tag,
                    Invariant($"Cannot close WebSocket. Unknown WebSocket id {id}."));

                return;
            }

            try
            {
                webSocket.Close();
            }
            catch (Exception ex)
            {
                if (_webSocketConnections.ContainsKey(id))
                {
                    _webSocketConnections.Remove(id);
                }

                RNTracer.Error(
                    ReactConstants.Tag,
                    Invariant($"Could not close WebSocket connection for id '{id}'."),
                    ex);
            }
        }

        [ReactMethod]
        public void send(string message, int id)
        {
            SendMessageInBackground(id, message);
        }

        [ReactMethod]
        public void sendBinary(string message, int id)
        {
            SendMessageInBackground(id, Convert.FromBase64String(message));
        }

        #endregion

        #region Event Handlers

        private void OnOpen(int id, object webSocket, EventArgs args)
        {
            if (webSocket != null)
            {
                _webSocketConnections.Add(id, (Websockets.Net.IWebSocketConnection)webSocket);

                SendEvent("websocketOpen", new JObject
                {
                    {"id", id},
                });
            }
        }

        private void OnClosed(int id, object webSocket, EventArgs args)
        {
            if (_webSocketConnections.ContainsKey(id))
            {
                _webSocketConnections.Remove(id);

                SendEvent("websocketClosed", new JObject
                {
                    {"id", id},
                    {"code", "null"},
                    {"reason", "null"},
                });
            }
            else
            {
                SendEvent("websocketFailed", new JObject
                {
                    {"id", id },
                    {"code", "null"},
                    {"message", "null" },
                });
            }
        }

        private void OnError(int id, string args)
        {
            if (_webSocketConnections.ContainsKey(id))
            {
                _webSocketConnections.Remove(id);
            }

            SendEvent("websocketFailed", new JObject
            {
                { "id", id },
                { "message", args },
            });
        }

        private void OnMessageReceived(int id, object sender, Websockets.Net.MessageEventArgs args)
        {
            var message = default(string);
            if (args.IsText)
            {
                message = args.Data;
            }
            else
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(args.Data);
                message = Convert.ToBase64String(byteArray);
            }
            SendEvent("websocketMessage", new JObject
                {
                    { "id", id },
                    { "data", message },
                    { "type", args.IsText ? "text":"binary"},
                });
        }

        #endregion

        #region Private Methods

        private void InitializeInBackground(int id, string url, Websockets.Net.IWebSocketConnection webSocket)
        {
            webSocket?.Open(url);
        }

        private void SendMessageInBackground(int id, string message)
        {
            _webSocketConnections[id].Send(message);
        }

        private void SendMessageInBackground(int id, byte[] message)
        {
            _webSocketConnections[id].Send(message);
        }

        private void SendEvent(string eventName, JObject parameters)
        {
            Context.GetJavaScriptModule<RCTDeviceEventEmitter>()
                .emit(eventName, parameters);
        }

        #endregion
    }
}
