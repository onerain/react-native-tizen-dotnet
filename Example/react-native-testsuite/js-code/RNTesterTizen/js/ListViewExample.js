
/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule ListViewExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');
var {
    Image,
  ListView,
  TouchableHighlight,
  StyleSheet, Button,
  Text,
  View,
    } = ReactNative;

var RNTesterPage = require('./RNTesterPage');

var ListViewSimpleExample = React.createClass({
  displayName: 'ListViewSimpleExample',
  statics: {
    title: '<ListView>',
    description: 'Performant, scrollable list of data.'
  },

  getInitialState: function () {
    var ds = new ListView.DataSource({ rowHasChanged: (r1, r2) => r1 !== r2 });
    return {
      dataSource: ds.cloneWithRows(this._genRows({})),
    };
  },

  _pressData: ({}: {[key: number]: boolean }),

  componentWillMount: function() {
    this._pressData = {};

  },

  render: function() {
    return (
      <RNTesterPage
        title={this.props.navigator ? null : '<ListView>'}>
        <ListView
          ref={(scroll) => this._scroll = scroll}
          style={styles.listV}
          onEndReached={this._onEndReached}
          onChangeVisibleRows={this._onChangeVisibleRows}
          onEndReachedThreshold={100}
          initialListSize={5}
          renderHeader={this._renderHeader}
          renderFooter={this._renderFooter}
          pageSize={100}
          scrollRenderAheadDistance ={300}
          dataSource={this.state.dataSource}
          renderRow={this._renderRow}
          scrollRenderAheadDistance={200}
          renderSectionHeader={this._renderSectionHeader}
          renderSeparator={this._renderSeparator}
        />
      </RNTesterPage>
    );
  },
  _onChangeVisibleRows: function(visibleRows, changedRows) {
    alert(visibleRows);
  },

  
  _renderHeader: function() {
    return (
      <View style={{ height: 50, }}><Text style={styles.text}>{'ListView-Header'}</Text></View>
    )
  },
  _renderFooter: function() {
    return (
      <View style={{ height: 50, }}><Text style={styles.text}>{'ListView-Foot'}</Text></View>
    )
  },


  _onEndReached:function() {
    alert("onEndReached Called");
  },
  _renderRow: function(rowData: string, sectionID: number, rowID: number, highlightRow: (sectionID: number, rowID: number) => void) {
    var rowHash = Math.abs(hashCode(rowData));
    var imgSource = THUMB_URLS[rowHash % THUMB_URLS.length];
    return (
      <TouchableHighlight onPress={() => {
        this._pressRow(rowID);
        highlightRow(sectionID, rowID);
      }}>
        <View>
          <View style={styles.row}>
            <Image style={styles.thumb} source={imgSource} />
            <Text style={styles.text}>
              {rowData + ' - ' + LOREM_IPSUM.substr(0, rowHash % 301 + 10)}
            </Text>
          </View>
        </View>
      </TouchableHighlight>
    );
  },
  _genRows: function(pressData: { [key: number]: boolean }): Array<string> {
    var dataBlob = [];
    for (var ii = 0; ii < 10; ii++) {
      var pressedText = pressData[ii] ? ' (pressed)' : '';
      dataBlob.push('Row ' + ii + pressedText);
    }
    return dataBlob;
  },

  _pressRow: function(rowID: number) {
    this._pressData[rowID] = !this._pressData[rowID];
    this.setState({
      dataSource: this.state.dataSource.cloneWithRows(
        this._genRows(this._pressData)
      )
    });
  },
  _renderSectionHeader:function(sectionID: number, rowID: number) {
    return (
      <View style={{ flexDirection: 'row', justifyContent: 'space-between', height: 50, backgroundColor: '#CCCCCC' }}
        key={`${sectionID}-${rowID}`}>
        <Button style={{ width: 400, height: 50 }} title="Scroll to END" onPress={() => {
          this._scroll.scrollToEnd()
        }}>
        </Button>
        <Button style={{ width: 400, height: 50 }} title="Scroll to Y = 1300" onPress={() => {
          this._scroll.scrollTo({ y: 1300})
        }}>
        </Button>
        <Button style={{ width: 400, height: 50 }} title="getMetrics" onPress={() => {
          alert('contentLength :'+this._scroll.getMetrics().contentLength+'-totalRows:'+this._scroll.getMetrics().totalRows+'-renderedRows:'+this._scroll.getMetrics().renderedRows+'-visibleRows:'+this._scroll.getMetrics().visibleRows)
        }}>
        </Button>
      </View>
    );
  },

  _renderSeparator: function(sectionID: number, rowID: number, adjacentRowHighlighted: bool) {
    return (
      <View
        key={`${sectionID}-${rowID}`}
        style={{
          height: adjacentRowHighlighted ? 4 : 1,
          backgroundColor: adjacentRowHighlighted ? '#3B5998' : '#CCCCCC',
        }}
      />
    );
  }
});

var THUMB_URLS = [
  require('./Thumbnails/like.png'),
  require('./Thumbnails/dislike.png'),
  require('./Thumbnails/call.png'),
  require('./Thumbnails/fist.png'),

  require('./Thumbnails/poke.png'),
  require('./Thumbnails/superlike.png'),
  require('./Thumbnails/victory.png')
];
var LOREM_IPSUM = 'Lorem ipsum dolor sit amet, ius ad pertinax oportere accommodare, an vix civibus corrumpit referrentur. Te nam case ludus inciderint, te mea facilisi adipiscing. Sea id integre luptatum. In tota sale consequuntur nec. Erat ocurreret mei ei. Eu paulo sapientem vulputate est, vel an accusam intellegam interesset. Nam eu stet pericula reprimique, ea vim illud modus, putant invidunt reprehendunt ne qui.';

/* eslint no-bitwise: 0 */
var hashCode = function (str) {
  var hash = 15;
  for (var ii = str.length - 1; ii >= 0; ii--) {
    hash = ((hash << 5) - hash) + str.charCodeAt(ii);
  }
  return hash;
};

var styles = StyleSheet.create({
  row: {
    flexDirection: 'row',
    justifyContent: 'center',
    padding: 10,
    backgroundColor: '#F6F6F6',
  },
  thumb: {
    width: 50,
    height: 50,
  },
  listV: {
    height: 1000
  },
  text: {
    color: 'black',
    fontSize: 20,
    fontFamily: 'Times',
    textAlign: 'auto',
    flex: 1,
  },
});

module.exports = ListViewSimpleExample;
