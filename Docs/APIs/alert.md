---
id: alert
title: Alert
layout: docs
category: APIs
permalink: Docs/APIs/alert.md
next: alertios
previous: actionsheetios
---

Use `Alert` to display an alert dialog.

This is an API can show static alerts. To show an alert that prompts the user to enter some information.

Optionally provide a list of buttons. Tapping any button will fire the respective `onPress` callback, and dismiss the alert. If no buttons are provided, a single 'OK' button will be displayed by default. 

Example usage:

```javascript
Alert.alert(
  'Alert Title',
  'My Alert Msg',
  [
    {text: 'Ask me later', onPress: () => console.log('Ask me later pressed')},
    {text: 'Cancel', onPress: () => console.log('Cancel Pressed'), style: 'cancel'},
    {text: 'OK', onPress: () => console.log('OK Pressed')},
  ]
)
```

### Methods

- [`alert`](#alert)

---

# Reference

## Methods

### `alert()`

```javascript
Alert.alert(title, [message], [buttons])
```

Launches an alert dialog with the specified title, and optionally a message.

| Name | Type | Required | Description |
| - | - | - | - |
| title | string | Yes | Alert title |
| message | string | No | Alert message |
| buttons | array | No | Array of buttons |

The optional `buttons` array should be composed of objects with any of the following:

- `text` (string) - text to display for this button
- `onPress` (function) - callback to be fired when button is tapped
