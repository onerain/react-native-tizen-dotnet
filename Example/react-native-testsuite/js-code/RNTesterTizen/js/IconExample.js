/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule IconExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');
var {
  StyleSheet,
  Text,
  View,
  Image,
  TouchableWithoutFeedback,
  TouchableHighlight,
} = ReactNative;

import SimpleLineIcons from 'react-native-vector-icons/SimpleLineIcons'
import FontAwesome from 'react-native-vector-icons/FontAwesome'

var RNTesterBlock = require('./RNTesterBlock');
var RNTesterPage = require('./RNTesterPage');

class IconExample extends React.Component {
  static title = 'Icon';
  static description = 'Examples of using the Icon.';
  static displayName = 'IconExample';
  constructor(props) {
    super(props)
    this.state = {
      focusedItem: 'control-start',
    }
    this._focus = this._focus.bind(this)
  }

  render() {
    const SimpleLineIconsNames = ['menu','list','options-vertical','options','arrow-down','arrow-left','arrow-right','arrow-up',
    'arrow-up-circle','arrow-left-circle','arrow-right-circle','arrow-down-circle','check','clock','plus','globe-alt','globe','folder-alt',
    'folder','film','feed','drop','note','loop','home','grid','graph','microphone','music-tone-alt','music-tone','earphones-alt','earphones','equalizer',
    'like','dislike','control-start','control-rewind','control-play','control-pause','control-forward','control-end','volume-1','volume-2','volume-off']
    // more icons names referto: http://simplelineicons.com/

    const FontAwesomeNames = ['arrows-alt','backward','compress','eject','expand','fast-backward','fast-forward','forward','pause','pause-circle',
    'pause-circle-o','play','play-circle','play-circle-o','random','step-backward','step-forward','stop','stop-circle','stop-circle-o','youtube-play',
    'address-book','address-book-o','address-card','address-card-o','adjust','american-sign-language-interpreting','anchor','archive','area-chart',
    'arrows','arrows-h','arrows-v','assistive-listening-systems','asterisk','at','audio-description','barcode','bars','battery-empty','battery-full','battery-half',
    'battery-quarter','battery-three-quarters','bed','beer','bell','bell-o','bell-slash','bell-slash-o','bicycle','binoculars','birthday-cake','blind',
    'bluetooth','bluetooth-b','bolt','bomb','book','bookmark','bookmark-o','braille','briefcase','bug','building','building-o','bullhorn','bullseye','bus']
    // more FontAwesome icons names referto: http://fontawesome.io/icons/

    return (
      <RNTesterPage title={'Icon'}>
        <RNTesterBlock title="SimpleLineIcons" titleNotHightlightable={true}>
          <View style={styles.iconsContainer}>
          {
            SimpleLineIconsNames.map((v, k) => <SimpleLineIconsWithTouchable key={k} item={v} focusedItem={this.state.focusedItem} _focus={this._focus}/>)
          }
          </View>
        </RNTesterBlock>
        <RNTesterBlock title="FontAwesome" titleNotHightlightable={true}>
          <View style={styles.iconsContainer}>
          {
            FontAwesomeNames.map((v, k) => <FontAwesomeWithTouchable key={k} item={v} focusedItem={this.state.focusedItem} _focus={this._focus}/>)
          }
          </View>
        </RNTesterBlock>
      </RNTesterPage>
    );
  }
  _focus(item){
    item && this.setState({
      focusedItem: item,
    })
  }
}

export class SimpleLineIconsWithTouchable extends React.Component {
  render() {
    return (
      <TouchableHighlight style={styles.item} activeOpacity={.65} onPressIn={() => this.props._focus(this.props.item)}>
        <SimpleLineIcons size={30} color={this.props.focusedItem === this.props.item ? '#ffd700' : 'black'} name={this.props.item}></SimpleLineIcons>
      </TouchableHighlight>
    )
  }
}

export class FontAwesomeWithTouchable extends React.Component {
  render() {
    return (
      <TouchableHighlight style={styles.item} activeOpacity={.65} onPressIn={() => this.props._focus(this.props.item)}>
        <FontAwesome size={30} color={this.props.focusedItem === this.props.item ? '#ffd700' : 'black'} name={this.props.item}></FontAwesome>
      </TouchableHighlight>
    )
  }
}


var styles = StyleSheet.create({
  iconsContainer: {
    flex: 0,
    flexDirection: 'row',
    flexWrap: 'wrap',
  },
  item:{
    padding: 10,
  }
});

module.exports = IconExample;
