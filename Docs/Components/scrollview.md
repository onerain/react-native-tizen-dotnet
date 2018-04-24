---
id: scrollview
title: ScrollView
layout: docs
category: components
permalink: docs/scrollview.html
next: sectionlist
previous: refreshcontrol
---
Component that wraps platform ScrollView while providing
integration with touch locking "responder" system.

Keep in mind that ScrollViews must have a bounded height in order to work,
since they contain unbounded-height children into a bounded container (via
a scroll interaction). In order to bound the height of a ScrollView, either
set the height of the view directly (discouraged) or make sure all parent
views have bounded height. Forgetting to transfer `{flex: 1}` down the
view stack can lead to errors here, which the element inspector makes
easy to debug.

Doesn't yet support other contained responders from blocking this scroll
view from becoming the responder.


### Props

- [View props...](docs/view.html#props)
- [`onContentSizeChange`](docs/scrollview.html#oncontentsizechange)
- [`onScroll`](docs/scrollview.html#onscroll)
- [`scrollEnabled`](docs/scrollview.html#scrollenabled)
- [`showsHorizontalScrollIndicator`](docs/scrollview.html#showshorizontalscrollindicator)
- [`showsVerticalScrollIndicator`](docs/scrollview.html#showsverticalscrollindicator)
- [`stickyHeaderIndices`](docs/scrollview.html#stickyheaderindices)
- [`horizontal`](docs/scrollview.html#horizontal)


### Methods

- [`scrollTo`](docs/scrollview.html#scrollto)
- [`scrollToEnd`](docs/scrollview.html#scrolltoend)


---

# Reference

## Props




### `onContentSizeChange`

Called when scrollable content view of the ScrollView changes.

Handler function is passed the content width and content height as parameters:
`(contentWidth, contentHeight)`

It's implemented using onLayout handler attached to the content container
which this ScrollView renders.

| Type | Required |
| - | - |
| function | No |



---

### `onScroll`

Fires at most once per frame during scrolling. The frequency of the
events can be controlled using the `scrollEventThrottle` prop.

| Type | Required |
| - | - |
| function | No |




---

### `scrollEnabled`

When false, the view cannot be scrolled via touch interaction.
The default value is true.

Note that the view can always be scrolled by calling `scrollTo`.

| Type | Required |
| - | - |
| bool | No |




---

### `showsHorizontalScrollIndicator`

When true, shows a horizontal scroll indicator.
The default value is true.

| Type | Required |
| - | - |
| bool | No |




---

### `showsVerticalScrollIndicator`

When true, shows a vertical scroll indicator.
The default value is true.

| Type | Required |
| - | - |
| bool | No |





---

### `horizontal`

When true, the scroll view's children are arranged horizontally in a row
instead of vertically in a column. The default value is false.

| Type | Required |
| - | - |
| bool | No |



## Methods

### `scrollTo()`

```javascript
scrollTo([y]: number, object, [x]: number, [animated]: boolean)
```

Scrolls to a given x, y offset, either immediately or with a smooth animation.

Example:

`scrollTo({x: 0, y: 0, animated: true})`

Note: The weird function signature is due to the fact that, for historical reasons,
the function also accepts separate arguments as an alternative to the options object.
This is deprecated due to ambiguity (y before x), and SHOULD NOT BE USED.



---

### `scrollToEnd()`

```javascript
scrollToEnd([options]: object)
```

If this is a vertical ScrollView scrolls to the bottom.
If this is a horizontal ScrollView scrolls to the right.

Use `scrollToEnd({animated: true})` for smooth animated scrolling,
`scrollToEnd({animated: false})` for immediate scrolling.
If no options are passed, `animated` defaults to true.


