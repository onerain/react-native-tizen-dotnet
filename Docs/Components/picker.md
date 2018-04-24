---
id: picker
title: Picker
layout: docs
category: components
permalink: docs/picker.html
next: picker-item
previous: navigatorios
---

Renders the native picker component on Tizen. Example:

```javascript
  <Picker
    selectedValue={this.state.language}
    onValueChange={(itemValue, itemIndex) => this.setState({language: itemValue})}>
    <Picker.Item label="Java" value="java" />
    <Picker.Item label="JavaScript" value="js" />
  </Picker>
```

### Props

- [View props...](docs/view.html#props)
- [`onValueChange`](docs/picker.html#onvaluechange)
- [`enabled`](docs/picker.html#enabled)
- [`style`](docs/picker.html#style)


---

# Reference

## Props

### `onValueChange`

Callback for when an item is selected. This is called with the following parameters:

  - `itemValue`: the `value` prop of the item that was selected
  - `itemPosition`: the index of the selected item in this picker

| Type | Required |
| - | - |
| function | No |



---

### `enabled`

If set to false, the picker will be disabled, i.e. the user will not be able to make a
selection.


| Type | Required | Platform |
| - | - | - |
| bool | No | Android  |




---

### `style`



| Type | Required |
| - | - |
| [style](docs/picker-style-props.html) | No |


---

### `itemStyle`

Style to apply to each of the item labels.


| Type | Required | Platform |
| - | - | - |
| [style](docs/textstyleproptypes.html) | No | iOS  |



