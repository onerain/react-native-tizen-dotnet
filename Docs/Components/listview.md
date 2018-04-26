---
id: listview
title: ListView
layout: docs
category: components
permalink: docs/listview.html
next: maskedviewios
previous: keyboardavoidingview
---

ListView - A core component designed for efficient display of vertically
scrolling lists of changing data. The minimal API is to create a
[`ListView.DataSource`](docs/listviewdatasource.html), populate it with a simple
array of data blobs, and instantiate a `ListView` component with that data
source and a `renderRow` callback which takes a blob from the data array and
returns a renderable component.

Minimal example:

```javascript
class MyComponent extends Component {
  constructor() {
    super();
    const ds = new ListView.DataSource({rowHasChanged: (r1, r2) => r1 !== r2});
    this.state = {
      dataSource: ds.cloneWithRows(['row 1', 'row 2']),
    };
  }

  render() {
    return (
      <ListView
        dataSource={this.state.dataSource}
        renderRow={(rowData) => <Text>{rowData}</Text>}
      />
    );
  }
}
```

ListView also supports more advanced features, including sections with sticky
section headers, header and footer support, callbacks on reaching the end of
the available data (`onEndReached`) and on the set of rows that are visible
in the device viewport change (`onChangeVisibleRows`), and several
performance optimizations.

There are a few performance operations designed to make ListView scroll
smoothly while dynamically loading potentially very large (or conceptually
infinite) data sets:

 * Only re-render changed rows - the rowHasChanged function provided to the
   data source tells the ListView if it needs to re-render a row because the
   source data has changed - see ListViewDataSource for more details.

 * Rate-limited row rendering - By default, only one row is rendered per
   event-loop (customizable with the `pageSize` prop). This breaks up the
   work into smaller chunks to reduce the chance of dropping frames while
   rendering rows.

### Props

- [`dataSource`](docs/listview.html#datasource)
- [`initialListSize`](docs/listview.html#initiallistsize)
- [`onEndReached`](docs/listview.html#onendreached)
- [`onEndReachedThreshold`](docs/listview.html#onendreachedthreshold)
- [`pageSize`](docs/listview.html#pagesize)
- [`renderFooter`](docs/listview.html#renderfooter)
- [`renderHeader`](docs/listview.html#renderheader)
- [`renderRow`](docs/listview.html#renderrow)
- [`renderSectionHeader`](docs/listview.html#rendersectionheader)
- [`renderSeparator`](docs/listview.html#renderseparator)
- [`scrollRenderAheadDistance`](docs/listview.html#scrollrenderaheaddistance)


### Methods

- [`getMetrics`](docs/listview.html#getmetrics)
- [`scrollTo`](docs/listview.html#scrollto)
- [`scrollToEnd`](docs/listview.html#scrolltoend)




---

# Reference

## Props

### `dataSource`

An instance of [ListView.DataSource](docs/listviewdatasource.html) to use

| Type | Required |
| - | - |
| ListViewDataSource | Yes |




---

### `initialListSize`

How many rows to render on initial component mount. Use this to make
it so that the first screen worth of data appears at one time instead of
over the course of multiple frames.

| Type | Required |
| - | - |
| number | Yes |




---

### `onEndReachedThreshold`

Threshold in pixels (virtual, not physical) for calling onEndReached.

| Type | Required |
| - | - |
| number | Yes |




---

### `pageSize`

Number of rows to render per event loop. Note: if your 'rows' are actually
cells, i.e. they don't span the full width of your view (as in the
ListViewGridLayoutExample), you should set the pageSize to be a multiple
of the number of cells per row, otherwise you're likely to see gaps at
the edge of the ListView as new pages are loaded.

| Type | Required |
| - | - |
| number | Yes |




---

### `renderRow`

(rowData, sectionID, rowID, highlightRow) => renderable

Takes a data entry from the data source and its ids and should return
a renderable component to be rendered as the row. By default the data
is exactly what was put into the data source, but it's also possible to
provide custom extractors. ListView can be notified when a row is
being highlighted by calling `highlightRow(sectionID, rowID)`. This
sets a boolean value of adjacentRowHighlighted in renderSeparator, allowing you
to control the separators above and below the highlighted row. The highlighted
state of a row can be reset by calling highlightRow(null).

| Type | Required |
| - | - |
| function | Yes |





---

### `scrollRenderAheadDistance`

How early to start rendering rows before they come on screen, in
pixels.

| Type | Required |
| - | - |
| number | Yes |




---

### `renderHeader`



| Type | Required |
| - | - |
| function | No |




---

### `onEndReached`

Called when all rows have been rendered and the list has been scrolled
to within onEndReachedThreshold of the bottom. The native scroll
event is provided.

| Type | Required |
| - | - |
| function | No |




---

### `renderSectionHeader`

(sectionData, sectionID) => renderable

If provided, a header is rendered for this section.

| Type | Required |
| - | - |
| function | No |




---

### `renderSeparator`

(sectionID, rowID, adjacentRowHighlighted) => renderable

If provided, a renderable component to be rendered as the separator
below each row but not the last row if there is a section header below.
Take a sectionID and rowID of the row above and whether its adjacent row
is highlighted.

| Type | Required |
| - | - |
| function | No |




---

### `renderFooter`

() => renderable

The header and footer are always rendered (if these props are provided)
on every render pass. If they are expensive to re-render, wrap them
in StaticContainer or other mechanism as appropriate. Footer is always
at the bottom of the list, and header at the top, on every render pass.
In a horizontal ListView, the header is rendered on the left and the
footer on the right.

| Type | Required |
| - | - |
| function | No |






## Methods

### `getMetrics()`

```javascript
getMetrics()
```

Exports some data, e.g. for perf investigations or analytics.



---

### `scrollTo()`

```javascript
scrollTo(...args: Array)
```

Scrolls to a given x, y offset, either immediately or with a smooth animation.

See `ScrollView#scrollTo`.



---

### `scrollToEnd()`

```javascript
scrollToEnd([options]: object)
```

If this is a vertical ListView scrolls to the bottom.
If this is a horizontal ListView scrolls to the right.

Use `scrollToEnd({animated: true})` for smooth animated scrolling,
`scrollToEnd({animated: false})` for immediate scrolling.
If no options are passed, `animated` defaults to true.

See `ScrollView#scrollToEnd`.
