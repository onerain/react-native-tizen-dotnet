---
id: text
title: Text
layout: docs
category: components
permalink: Docs\Components\text.md
next: textinput
previous: tabbarios-item
---

A React component for displaying text.

`Text` supports nesting, styling.

In the following example, the nested title and body text will inherit the
`fontFamily` from `styles.baseText`, but the title provides its own
additional styles.  The title and body will stack on top of each other on
account of the literal newlines:

```javascript
import React, { Component } from 'react';
import { AppRegistry, Text, StyleSheet } from 'react-native';

export default class TextInANest extends Component {
  constructor(props) {
    super(props);
    this.state = {
      titleText: "Bird's Nest",
      bodyText: 'This is not really a bird nest.'
    };
  }

  render() {
    return (
      <Text style={styles.baseText}>
        <Text style={styles.titleText} onPress={this.onPressTitle}>
          {this.state.titleText}{'\n'}{'\n'}
        </Text>
        <Text numberOfLines={5}>
          {this.state.bodyText}
        </Text>
      </Text>
    );
  }
}

const styles = StyleSheet.create({
  baseText: {
    fontFamily: 'Cochin',
  },
  titleText: {
    fontSize: 20,
    fontWeight: 'bold',
  },
});

// skip this line if using Create React Native App
AppRegistry.registerComponent('TextInANest', () => TextInANest);
```

## Nested text

For React Native, we decided to use web paradigm for
this where you can nest text to achieve the same effect.


```javascript
import React, { Component } from 'react';
import { AppRegistry, Text } from 'react-native';

export default class BoldAndBeautiful extends Component {
  render() {
    return (
      <Text style={{fontWeight: 'bold'}}>
        I am bold
        <Text style={{color: 'red'}}>
          and red
        </Text>
      </Text>
    );
  }
}

// skip this line if using Create React Native App
AppRegistry.registerComponent('AwesomeProject', () => BoldAndBeautiful);
```

Behind the scenes, React Native converts this to a flat `NSAttributedString`
or `SpannableString` that contains the following information:

```javascript
"I am bold and red"
0-9: bold
9-17: bold, red
```


## Containers

The `<Text>` element is special relative to layout: everything inside is no
longer using the flexbox layout but using text layout. This means that
elements inside of a `<Text>` are no longer rectangles, but wrap when they
see the end of the line.

```javascript
<Text>
  <Text>First part and </Text>
  <Text>second part</Text>
</Text>
// Text container: all the text flows as if it was one
// |First part |
// |and second |
// |part       |

<View>
  <Text>First part and </Text>
  <Text>second part</Text>
</View>
// View container: each text is its own block
// |First part |
// |and        |
// |second part|
```

## Limited Style Inheritance

On the web, the usual way to set a font family and size for the entire
document is to take advantage of inherited CSS properties like so:

```css
html {
  font-family: 'lucida grande', tahoma, verdana, arial, sans-serif;
  font-size: 11px;
  color: #141823;
}
```

All elements in the document will inherit this font unless they or one of
their parents specifies a new rule.

In React Native, we are more strict about it: **you must wrap all the text
nodes inside of a `<Text>` component**. You cannot have a text node directly
under a `<View>`.


```javascript
// BAD: will raise exception, can't have a text node as child of a <View>
<View>
  Some text
</View>

// GOOD
<View>
  <Text>
    Some text
  </Text>
</View>
```

You also lose the ability to set up a default font for an entire subtree.
The recommended way to use consistent fonts and sizes across your
application is to create a component `MyAppText` that includes them and use
this component across your app. You can also use this component to make more
specific components like `MyAppHeaderText` for other kinds of text.

```javascript
<View>
  <MyAppText>Text styled with the default font for the entire application</MyAppText>
  <MyAppHeaderText>Text styled as a header</MyAppHeaderText>
</View>
```

Assuming that `MyAppText` is a component that simply renders out its
children into a `Text` component with styling, then `MyAppHeaderText` can be
defined as follows:

```javascript
class MyAppHeaderText extends Component {
  render() {
    <MyAppText>
      <Text style={{fontSize: 20}}>
        {this.props.children}
      </Text>
    </MyAppText>
  }
}
```

Composing `MyAppText` in this way ensures that we get the styles from a
top-level component, but leaves us the ability to add / override them in
specific use cases.

React Native still has the concept of style inheritance, but limited to text
subtrees. In this case, the second part will be both bold and red.

```javascript
<Text style={{fontWeight: 'bold'}}>
  I am bold
  <Text style={{color: 'red'}}>
    and red
  </Text>
</Text>
```

We believe that this more constrained way to style text will yield better
apps:

- (Developer) React components are designed with strong isolation in mind:
You should be able to drop a component anywhere in your application,
trusting that as long as the props are the same, it will look and behave the
same way. Text properties that could inherit from outside of the props would
break this isolation.

- (Implementor) The implementation of React Native is also simplified. We do
not need to have a `fontFamily` field on every single element, and we do not
need to potentially traverse the tree up to the root every time we display a
text node. The style inheritance is only encoded inside of the native Text
component and doesn't leak to other components or the system itself.

### Props

- [`nativeID`](#nativeid)
- [`style`](#style)
- [`testID`](#testid)






---

# Reference

## Props


### `nativeID`

Used to locate this view from native code.

| Type | Required |
| - | - |
| string | No |



---

### `style`



| Type | Required |
| - | - |
| style | No |


  - [View Style Props...](../view-style-props.md#style)


  - **`color`**: [color](../colors.md)

  - **`fontSize`**: number

  - **`fontStyle`**: enum('normal', 'italic')

  - **`fontWeight`**: enum('normal', 'bold', '100', '200', '300', '400', '500', '600', '700', '800', '900')

    Specifies font weight. The values 'normal' and 'bold' are supported for
    most fonts. Not all fonts have a variant for each of the numeric values,
    in that case the closest one is chosen.

  - **`lineHeight`**: number

  - **`textAlign`**: enum('auto', 'left', 'right', 'center', 'justify')

    Specifies text alignment. The value 'justify' is only supported on iOS and
    fallbacks to `left` on Android.

  - **`textDecorationLine`**: enum('none', 'underline', 'line-through', 'underline line-through')

  - **`textShadowColor`**: [color](../colors.md)

  - **`fontFamily`**: string
    

  - **`textAlignVertical`**: enum('auto', 'top', 'bottom', 'center')



---

### `testID`

Used to locate this view in end-to-end tests.

| Type | Required |
| - | - |
| string | No |