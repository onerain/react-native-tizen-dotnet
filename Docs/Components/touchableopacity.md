---
id: touchableopacity
title: TouchableOpacity
layout: docs
category: components
permalink: Docs/Components/touchableopacity.md
next: touchablewithoutfeedback
previous: touchablenativefeedback
---
A wrapper for making views respond properly to focusable.
On press down, the opacity of the wrapped view is decreased, dimming it.

Opacity is controlled by wrapping the children in an Animated.View, which is
added to the view hiearchy.  Be aware that this can affect layout.

Example:

```javascript
renderButton: function() {
  return (
    <TouchableOpacity onPress={this._onPressButton}>
      <Image
        style={styles.button}
        source={require('./myButton.png')}
      />
    </TouchableOpacity>
  );
},
```
### Example

```javascript
import React, { Component } from 'react'
import {
  AppRegistry,
  StyleSheet,
  TouchableOpacity,
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
       <TouchableOpacity
         style={styles.button}
         onPress={this.onPress}
       >
         <Text>  Here </Text>
       </TouchableOpacity>
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



---

# Reference

## Props

### `activeOpacity`

Determines what the opacity of the wrapped view should be when touch is
active. Defaults to 0.2.

| Type | Required |
| - | - |
| number | No |

