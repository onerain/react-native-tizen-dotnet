---
id: touchablewithoutfeedback
title: TouchableWithoutFeedback
layout: docs
category: components
permalink: Docs/touchablewithoutfeedback.md
next: view
previous: touchableopacity
---
Do not use unless you have a very good reason. All elements that
respond to press should have a visual feedback when focused.

TouchableWithoutFeedback supports only one child.
If you wish to have several child components, wrap them in a View.

### Props

- [`delayPressIn`](#delaypressin)
- [`delayPressOut`](#delaypressout)
- [`disabled`](#disabled)
- [`onLayout`](#onlayout)
- [`onPress`](#onpress)
- [`onPressIn`](#onpressin)
- [`onPressOut`](#onpressout)



---

# Reference

## Props


### `delayPressIn`

Delay in ms, from the start of the touch, before onPressIn is called.

| Type | Required |
| - | - |
| number | No |




---

### `delayPressOut`

Delay in ms, from the release of the touch, before onPressOut is called.

| Type | Required |
| - | - |
| number | No |




---

### `disabled`

If true, disable all interactions for this component.

| Type | Required |
| - | - |
| bool | No |



---

### `onLayout`

Invoked on mount and layout changes with

  `{nativeEvent: {layout: {x, y, width, height}}}`

| Type | Required |
| - | - |
| function | No |




---

### `onPress`

Called when the touch is released, but not if cancelled (e.g. by a scroll
that steals the responder lock).

| Type | Required |
| - | - |
| function | No |




---

### `onPressIn`

Called as soon as the touchable element is pressed and invoked even before onPress.
This can be useful when making network requests.

| Type | Required |
| - | - |
| function | No |




---

### `onPressOut`

Called as soon as the touch is released even before onPress.

| Type | Required |
| - | - |
| function | No |





