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
            Tracer.Write("RN", "NETINFO DefaultNetworkInformation Start");
            ConnectionManager.ConnectionTypeChanged += OnNetworkAvailabilityChanged;
        }

        public void Stop()
        {
            Tracer.Write("RN", "NETINFO DefaultNetworkInformation Stop");
            ConnectionManager.ConnectionTypeChanged -= OnNetworkAvailabilityChanged;
        }

        public string GetInternetStatus()
        {
            Tracer.Write("RN", "NETINFO DefaultNetworkInformation GetInternetStatus");
            return NetworkInterface.GetIsNetworkAvailable() ? "InternetAccess" : "None";
        }

        private void OnNetworkAvailabilityChanged(object sender, EventArgs e)
        {
            Tracer.Write("RN", "NETINFO DefaultNetworkInformation OnNetworkAvailabilityChanged");
            NetworkAvailabilityChanged?.Invoke(sender, e);
        }
    }
}
