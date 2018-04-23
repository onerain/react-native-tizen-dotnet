using System;
using System.Collections.Generic;
using NUnit.Framework;
using Tizen;

namespace Websockets.Net.Tests
{
    [TestFixture]
    public class WebsocketConnectionTests
    {
        [Test]
        [Description("TC WebsocketConnection Ctor")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.WebsocketConnection C")]
        static void WebsocketConnection_Return()
        {
            var websocketConnection = new WebsocketConnection();
        }

        [Test]
        [Description("TC WebsocketConnection Link")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Link M")]
        static void Link_Return()
        {
            WebsocketConnection.Link();
        }

        [Test]
        [Description("TC WebsocketConnection Open")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Open M")]
        [Property("COVPARAM", "string,string,string")]
        static void Open_ReturnA()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Open("ws://127.0.0.1:8081");
        }

        [Test]
        [Description("TC WebsocketConnection Open")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Open M")]
        [Property("COVPARAM", "string,string,System.Collections.Generic.IDictionary<string,string>")]
        static void Open_ReturnB()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Open("ws://127.0.0.1:8081", "none", new Dictionary<string, string>());
        }

        [Test]
        [Description("TC WebsocketConnection Send")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Send M")]
        [Property("COVPARAM", "string")]
        static void Send_ReturnA()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Open("ws://127.0.0.1:8081", "none", new Dictionary<string, string>());
            websocketConnection.Send("testsend");
        }

        [Test]
        [Description("TC WebsocketConnection Send")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Send M")]
        [Property("COVPARAM", "byte[]")]
        static void Send_ReturnB()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Open("ws://127.0.0.1:8081", "none", new Dictionary<string, string>());
            byte[] array = { 0x01, 0x02, 0x03 };
            websocketConnection.Send(array);
        }

        [Test]
        [Description("TC WebsocketConnection Close")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Close M")]
        static void Close_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Open("ws://127.0.0.1:8081", "none", new Dictionary<string, string>());
            websocketConnection.Close();
        }

        [Test]
        [Description("TC WebsocketConnection IsOpen")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.IsOpen A")]
        static void IsOpen_Return()
        {
            var websocketConnection = new WebsocketConnection();
            var bopen = websocketConnection.IsOpen;
        }

        [Test]
        [Description("TC WebsocketConnection OnClosed")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnClosed E")]
        static void OnClosed_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnClosed += (sender, args) =>
            {
                OnClosed(sender, args);
            };
        }

        private static void OnClosed(object webSocket, EventArgs args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnClosed =========");
        }

        [Test]
        [Description("TC WebsocketConnection OnDispose")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnDispose E")]
        static void OnDispose_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnDispose += (args) =>
            {
                OnDispose(args);
            };
        }

        private static void OnDispose(IWebSocketConnection args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnDispose =========");
        }

        [Test]
        [Description("TC WebsocketConnection OnOpened")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnOpened E")]
        static void OnOpened_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnOpened += (sender, args) =>
            {
                OnOpened(sender, args);
            };
        }

        private static void OnOpened(object webSocket, EventArgs args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnOpened =========");
        }


        [Test]
        [Description("TC WebsocketConnection OnError")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnError E")]
        static void OnError_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnError += (sender, args) =>
            {
                OnError(sender, args);
            };
        }

        private static void OnError(object webSocket, string args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnOpened =========");
        }

        [Test]
        [Description("TC WebsocketConnection OnLog")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnLog E")]
        static void OnLog_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnError += (sender, args) =>
            {
                OnLog(sender, args);
            };
        }

        private static void OnLog(object webSocket, string args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnOpened =========");
        }

        [Test]
        [Description("TC WebsocketConnection OnMessage")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.OnMessage E")]
        static void OnMessage_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.OnError += (sender, args) =>
            {
                OnMessage(sender, args);
            };
        }


        [Test]
        [Description("TC WebsocketConnection Dispose")]
        [Property("SPEC", "Websockets.Net.WebsocketConnection.Dispose E")]
        static void Dispose_Return()
        {
            var websocketConnection = new WebsocketConnection();
            websocketConnection.Dispose();
        }

        private static void OnMessage(object webSocket, string args)
        {
            Log.Info("RNUT", "=========== WebsocketConnection OnOpened =========");
        }
    }
}