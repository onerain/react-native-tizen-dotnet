using System;
using System.Net.NetworkInformation;
using Tizen.Network.Connection;

namespace ReactNative.Modules.NetInfo
{
    /// <summary>
    /// An interface for network information status and updates.
    /// </summary>
    public interface INetworkInformation
    {
        event EventHandler NetworkAvailabilityChanged;
        /// <summary>
        /// Gets the internet status
        /// </summary>
        /// <returns>
        /// The React Native friendly internet status
        /// </returns>
        string GetNetworkStatus();

        /// <summary>
        /// Starts observing network status changes.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops observing network status changes.
        /// </summary>
        void Stop();
    }
}
