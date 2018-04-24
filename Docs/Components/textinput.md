---
id: textinput
title: TextInput
layout: docs
category: components
permalink: docs/textinput.html
next: toolbarandroid
previous: text
---
A foundational component for inputting text into the app via a
keyboard. Props provide configurability for several features, such as
auto-correction, auto-capitalization, placeholder text, and different keyboard
types, such as a numeric keypad.

The simplest use case is to plop down a `TextInput` and subscribe to the
`onChangeText` events to read the user input. There are also other events,
such as `onSubmitEditing` and `onFocus` that can be subscribed to. A simple
example:

```ReactNativeWebPlayer
import React, { Component } from 'react';
import { AppRegistry, TextInput } from 'react-native';

export default class UselessTextInput extends Component {
  constructor(props) {
    super(props);
    this.state = { text: 'Useless Placeholder' };
  }

  render() {
    return (
      <TextInput
        style={{height: 40, borderColor: 'gray', borderWidth: 1}}
        onChangeText={(text) => this.setState({text})}
        value={this.state.text}
      />
    );
  }
}

// skip this line if using Create React Native App
AppRegistry.registerComponent('AwesomeProject', () => UselessTextInput);
```

### Props

- [View props...](docs/view.html#props)
- [`placeholderTextColor`](docs/textinput.html#placeholdertextcolor)
- [`editable`](docs/textinput.html#editable)
- [`multiline`](docs/textinput.html#multiline)
- [`onBlur`](docs/textinput.html#onblur)
- [`onChange`](docs/textinput.html#onchange)
- [`onChangeText`](docs/textinput.html#onchangetext)
- [`onContentSizeChange`](docs/textinput.html#oncontentsizechange)
- [`onEndEditing`](docs/textinput.html#onendediting)
- [`onFocus`](docs/textinput.html#onfocus)
- [`placeholder`](docs/textinput.html#placeholder)


---

# Reference

## Props

### `placeholderTextColor`

The text color of the placeholder string.

| Type | Required |
| - | - |
| [color](docs/colors.html) | No |


---

### `editable`

If `false`, text is not editable. The default value is `true`.

| Type | Required |
| - | - |
| bool | No |


---

### `multiline`

If `true`, the text input can be multiple lines.
The default value is `false`.

| Type | Required |
| - | - |
| bool | No |




---

### `onBlur`

Callback that is called when the text input is blurred.

| Type | Required |
| - | - |
| function | No |




---

### `onChange`

Callback that is called when the text input's text changes.

| Type | Required |
| - | - |
| function | No |




---

### `onChangeText`

Callback that is called when the text input's text changes.
Changed text is passed as an argument to the callback handler.

| Type | Required |
| - | - |
| function | No |



---

### `onFocus`

Callback that is called when the text input is focused.

| Type | Required |
| - | - |
| function | No |




---

### `placeholder`

The string that will be rendered before text input has been entered.

| Type | Required |
| - | - |
| string | No |
