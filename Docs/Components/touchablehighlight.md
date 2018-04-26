---
id: touchablehighlight
title: TouchableHighlight
layout: docs
category: components
permalink: Docs/Components/touchablehighlight.md
next: touchablenativefeedback
previous: toolbarandroid
---
A wrapper for making views respond properly to focuable.
On press down, the opacity of the wrapped view is decreased, which allows
the underlay color to show through, darkening or tinting the view.

The underlay comes from wrapping the child in a new View, which can affect
layout, and sometimes cause unwanted visual artifacts if not used correctly,
for example if the backgroundColor of the wrapped view isn't explicitly set
to an opaque color.

TouchableHighlight must have one child (not zero or more than one).
If you wish to have several child components, wrap them in a View.

Example:

```
renderButton: function() {
  return (
    <TouchableHighlight onPress={this._onPressButton}>
      <Image
        style={styles.button}
        source={require('./myButton.png')}
      />
    </TouchableHighlight>
  );
},
```


### Example

```javascript
import React, { Component } from 'react'
import {
  AppRegistry,
  StyleSheet,
  TouchableHighlight,
  Text,
  View,
} from 'react-native'

class App extends Component {
  constructor(props) {
    super(props)
    this.state = { count: 0 }
  }

  onPress = () => {
    this.setState({
      count: this.state.count+1
    })
  }

 render() {
    return (
      <View style={styles.container}>
        <TouchableHighlight
         style={styles.button}
         onPress={this.onPress}
        >
         <Text> Touch Here </Text>
        </TouchableHighlight>
        <View style={[styles.countContainer]}>
          <Text style={[styles.countText]}>
            { this.state.count !== 0 ? this.state.count: null}
          </Text>
        </View>
      </View>
    )
  }
}

const styles = StyleSheet.create({
  container: {
    flex: 1,
    justifyContent: 'center',
    paddingHorizontal: 10
  },
  button: {
    alignItems: 'center',
    backgroundColor: '#DDDDDD',
    padding: 10
  },
  countContainer: {
    alignItems: 'center',
    padding: 10
  },
  countText: {
    color: '#FF00FF'
  }
})

AppRegistry.registerComponent('App', () => App)
```

### Props

* [TouchableWithoutFeedback props...](touchablewithoutfeedback.md#props)
- [`activeOpacity`](#activeopacity)
- [`underlayColor`](#underlaycolor)






---

# Reference

## Props

### `activeOpacity`

Determines what the opacity of the wrapped view should be when touch is
active.

| Type | Required |
| - | - |
| number | No |


---

### `underlayColor`

The color of the underlay that will show through when the touch is
active.

| Type | Required |
| - | - |
| [color](../colors.md) | No |
