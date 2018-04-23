using System;
using System.Net.NetworkInformation;
using Tizen.Network.Connection;
using ReactNative.Tracing;

namespace ReactNative.Modules.NetInfo
{
    class DefaultNetworkInformation : INetworkInformation
    {
        public event EventHandler NetworkAvailabilityChanged;


        public void Start()
        {
            ConnectionManager.ConnectionTypeChanged += OnNetworkAvailabilityChanged;
        }

        public void Stop()
        {
            ConnectionManager.ConnectionTypeChanged -= OnNetworkAvailabilityChanged;
        }

        public string GetNetworkStatus()
        {
            string ret = "Disconnect";
            var connectionManager = ConnectionManager.CurrentConnection;
            var connectionType = connectionManager.Type;
            switch (connectionType)
            {
                case ConnectionType.Disconnected:
                    ret = "Disconnect";
                    break;
                case ConnectionType.WiFi:
                    ret = "WIFI";
                    break;
                case ConnectionType.Cellular:
                    ret = "Cellular";
                    break;
                case ConnectionType.Ethernet:
                    ret = "Ethernet";
                    break;
                case ConnectionType.NetProxy:
                    ret = "NetProxy";
                    break;
                case ConnectionType.Bluetooth:
                    ret = "Bluetooth";
                    break;
            }
            return ret;
        }

        private void OnNetworkAvailabilityChanged(object sender, EventArgs e)
        {
            NetworkAvailabilityChanged?.Invoke(sender, e);
        }
    }
}
