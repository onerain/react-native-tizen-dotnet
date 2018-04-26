---
id: image
title: Image
layout: docs
category: components
permalink: docs/image.html
next: keyboardavoidingview
previous: flatlist
---
A React component for displaying different types of images,
including network images, static resources, temporary local images, and
images from local disk, such as the camera roll.

This example shows fetching and displaying an image from local storage
as well as one from network and even from data provided in the `'data:'` uri scheme.

> Note that for network and data images, you will need to manually specify the dimensions of your image!

```ReactNativeWebPlayer
import React, { Component } from 'react';
import { AppRegistry, View, Image } from 'react-native';

export default class DisplayAnImage extends Component {
  render() {
    return (
      <View>
        <Image
          source={require('./img/favicon.png')}
        />
        <Image
          style={{width: 50, height: 50}}
          source={{uri: 'https://facebook.github.io/react-native/img/favicon.png'}}
        />
        <Image
          style={{width: 66, height: 58}}
          source={{uri: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAADMAAAAzCAYAAAA6oTAqAAAAEXRFWHRTb2Z0d2FyZQBwbmdjcnVzaEB1SfMAAABQSURBVGje7dSxCQBACARB+2/ab8BEeQNhFi6WSYzYLYudDQYGBgYGBgYGBgYGBgYGBgZmcvDqYGBgmhivGQYGBgYGBgYGBgYGBgYGBgbmQw+P/eMrC5UTVAAAAABJRU5ErkJggg=='}}
        />
      </View>
    );
  }
}

// skip this line if using Create React Native App
AppRegistry.registerComponent('DisplayAnImage', () => DisplayAnImage);
```

You can also add `style` to an image:

```ReactNativeWebPlayer
import React, { Component } from 'react';
import { AppRegistry, View, Image, StyleSheet } from 'react-native';

const styles = StyleSheet.create({
  stretch: {
    width: 50,
    height: 200
  }
});

export default class DisplayAnImageWithStyle extends Component {
  render() {
    return (
      <View>
        <Image
          style={styles.stretch}
          source={require('./img/favicon.png')}
        />
      </View>
    );
  }
}

// skip these lines if using Create React Native App
AppRegistry.registerComponent(
  'DisplayAnImageWithStyle',
  () => DisplayAnImageWithStyle
);
```


### Props

- [`onLayout`](docs/image.html#onlayout)
- [`onLoad`](docs/image.html#onload)
- [`onLoadEnd`](docs/image.html#onloadend)
- [`onLoadStart`](docs/image.html#onloadstart)
- [`resizeMode`](docs/image.html#resizemode)
- [`source`](docs/image.html#source)
- [`testID`](docs/image.html#testid)
- [`style`](docs/image.html#style)




---

### `onLayout`

Invoked on mount and layout changes with
`{nativeEvent: {layout: {x, y, width, height}}}`.

| Type | Required |
| - | - |
| function | No |




---

### `onLoad`

Invoked when load completes successfully.

| Type | Required |
| - | - |
| function | No |




---

### `onLoadEnd`

Invoked when load either succeeds or fails.

| Type | Required |
| - | - |
| function | No |




---

### `onLoadStart`

Invoked on load start.

e.g., `onLoadStart={(e) => this.setState({loading: true})}`

| Type | Required |
| - | - |
| function | No |




---

### `resizeMode`

Determines how to resize the image when the frame doesn't match the raw
image dimensions.

- `cover`: Scale the image uniformly (maintain the image's aspect ratio)
so that both dimensions (width and height) of the image will be equal
to or larger than the corresponding dimension of the view (minus padding).

- `contain`: Scale the image uniformly (maintain the image's aspect ratio)
so that both dimensions (width and height) of the image will be equal to
or less than the corresponding dimension of the view (minus padding).

- `stretch`: Scale width and height independently, This may change the
aspect ratio of the src.

| Type | Required |
| - | - |
| enum('cover', 'contain', 'stretch') | No |




---

### `source`

The image source (either a remote URL or a local file resource).

This prop can also contain several remote URLs, specified together with
their width and height and potentially with scale/other URI arguments.
The native side will then choose the best `uri` to display based on the
measured size of the image container. A `cache` property can be added to
control how networked request interacts with the local cache.

The currently supported formats are `png`, `jpg`, `jpeg`, `bmp`, `gif`,
`webp` (Android only), `psd` (iOS only).

| Type | Required |
| - | - |
| ImageSourcePropType | No |




---

### `testID`

A unique identifier for this element to be used in UI Automation
testing scripts.

| Type | Required |
| - | - |
| string | No |


---

### `style`

| Type | Required |
| - | - |
| [style](docs/imagestyleproptypes.html) | No |

## Methods

### `getSize()`

```javascript
static getSize(uri: string, success: function, [failure]: function): void
```

Retrieve the width and height (in pixels) of an image prior to displaying it. This method can fail if the image cannot be found, or fails to download.

In order to retrieve the image dimensions, the image may first need to be loaded or downloaded, after which it will be cached. This means that in principle you could use this method to preload images, however it is not optimized for that purpose, and may in future be implemented in a way that does not fully load/download the image data. A proper, supported way to preload images will be provided as a separate API.

Does not work for static image resources.

**Parameters:**

| Name | Type | Required | Description |
| - | - | - | - |
| uri | string | Yes | The location of the image. |
| success | function | Yes | The function that will be called if the image was successfully found and widthand height retrieved. |
| failure | function | No | The function that will be called if there was an error, such as failing toto retrieve the image. |







