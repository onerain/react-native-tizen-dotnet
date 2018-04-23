---
id: netinfo
title: NetInfo
layout: docs
category: APIs
permalink: docs/netinfo.html
next: panresponder
previous: linking
---

NetInfo exposes info about online/offline status.

```javascript
NetInfo.fetch().done((reach) => {
  console.log('Initial: ' + reach);
});
function handleFirstConnectivityChange(reach) {
  console.log('First change: ' + reach);
  NetInfo.removeEventListener(
    'change',
    handleFirstConnectivityChange
  );
}
NetInfo.addEventListener(
  'change',
  handleFirstConnectivityChange
);
```

### ConnectionType enum

`ConnectionType` describes the type of connection the device is using to communicate with the network.

- `Disconnected` - device is offline
- `WiFi` - device is online and connected via wifi
- `Cellular` - device is connected via Edge, 3G, WiMax, or LTE
- `Ethernet` - device is online and connected via cable
- `Bluetooth` - device is online and connected via bluetooth
- `NetProxy` - device is online and connected via proxy



### Methods

- [`addEventListener`](docs/netinfo.html#addeventlistener)
- [`removeEventListener`](docs/netinfo.html#removeeventlistener)


### Properties

- [`isConnected`](docs/netinfo.html#isconnected)




---

# Reference

## Methods

### `addEventListener()`

```javascript
NetInfo.addEventListener(eventName, handler)
```


Adds an event handler. Supported events:

- `connectionChange`: Fires when the network status changes. The argument to the event
  handler is an object with keys:
  - `type`: A `ConnectionType` (listed above)
  - `effectiveType`: An `EffectiveConnectionType` (listed above)
- `change`: This event is deprecated. Listen to `connectionChange` instead. Fires when
  the network status changes. The argument to the event handler is one of the deprecated
  connectivity types listed above.




---

### `removeEventListener()`

```javascript
NetInfo.removeEventListener(eventName, handler)
```


Removes the listener for network status changes.






## Properties

### `isConnected`

Available on all platforms. Asynchronously fetch a boolean to determine internet connectivity.

```
NetInfo.isConnected.fetch().then(isConnected => {
  console.log('First, is ' + (isConnected ? 'online' : 'offline'));
});
function handleFirstConnectivityChange(isConnected) {
  console.log('Then, is ' + (isConnected ? 'online' : 'offline'));
  NetInfo.isConnected.removeEventListener(
    'connectionChange',
    handleFirstConnectivityChange
  );
}
NetInfo.isConnected.addEventListener(
  'connectionChange',
  handleFirstConnectivityChange
);
```
