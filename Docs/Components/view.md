---
id: view
title: View
layout: docs
category: components
permalink: Docs/Components/view.md
next: viewpagerandroid
previous: touchablewithoutfeedback
---

The most fundamental component for building a UI, `View` is a container that supports layout with , style. `View` maps directly to the native view equivalent on whatever platform React Native is running on, whether that is a `UIView`, `<div>`, etc.

`View` is designed to be nested inside other views and can have 0 to many children of any type.

This example creates a `View` that wraps two colored boxes and a text component in a row with padding.

```javascript
class ViewColoredBoxesWithText extends Component {
  render() {
    return (
      <View style={{flexDirection: 'row', height: 100, padding: 20}}>
        <View style={{backgroundColor: 'blue', flex: 0.3}} />
        <View style={{backgroundColor: 'red', flex: 0.5}} />
        <Text>Hello World!</Text>
      </View>
    );
  }
}
```


### Props

- [`onLayout`](#onlayout)
- [`style`](#style)



# Reference

## Props


### `onLayout`

Invoked on mount and layout changes with:

`{nativeEvent: { layout: {x, y, width, height}}}`

This event is fired immediately once the layout has been calculated, but
the new layout may not yet be reflected on the screen at the time the
event is received, especially if a layout animation is in progress.

| Type | Required |
| - | - |
| function | No |




### `style`



| Type | Required |
| - | - |
| [style](../style.md) | No |

