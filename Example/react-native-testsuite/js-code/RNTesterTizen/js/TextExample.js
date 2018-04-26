/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule TextExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');
var {
  Image,
  StyleSheet,
  Text,
  View,
} = ReactNative;
var RNTesterBlock = require('./RNTesterBlock');
var RNTesterPage = require('./RNTesterPage');

class Entity extends React.Component {
  render() {
    return (
      <Text style={{fontWeight: 'bold', color: '#527fe4'}}>
        {this.props.children}
      </Text>
    );
  }
}

class NotSupproted extends React.Component {
  render() {
    return <Text style={{fontSize:15, color:'red'}}>TODO, have not supproted</Text>
  }
}

class AttributeToggler extends React.Component {
  state = {fontWeight: 'bold', fontSize: 15};

  toggleWeight = () => {
    this.setState({
      fontWeight: this.state.fontWeight === 'bold' ? 'normal' : 'bold'
    });
  };

  increaseSize = () => {
    this.setState({
      fontSize: this.state.fontSize + 1
    });
  };

  render() {
    var curStyle = {fontWeight: this.state.fontWeight, fontSize: this.state.fontSize,color:'black'};
    return (
      <View>
        <Text style={curStyle}>
          Tap the controls below to change attributes.
        </Text>
        <Text>
          <Text style={{color:'black'}}>See how it will even work on <Text style={curStyle}>this nested text</Text></Text>
        </Text>
        <Text>
          <Text onPress={this.toggleWeight} style={{color:'black'}}>Toggle Weight</Text>
          {' (with highlight onPress)'}
        </Text>
        <Text onPress={this.increaseSize} suppressHighlighting={true} style={{color:'black'}}>
          Increase Size (suppressHighlighting true)
        </Text>
      </View>
    );
  }
}

class TextExample extends React.Component {
  static title = '<Text>';
  static description = 'Base component for rendering styled text.';

  render() {
    return (
      <RNTesterPage title="Text">
        <RNTesterBlock title="Wrap">
          <Text style={{color:'black'}}>
            The text should wrap if it goes on multiple lines.
            See, this is going to the next line.The text should wrap if it goes on multiple lines.
            See, this is going to the next line.The text should wrap if it goes on multiple lines.
            See, this is going to the next line.The text should wrap if it goes on multiple lines.
            See, this is going to the next line.
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Padding">
          <NotSupproted/>
          {false && <Text style={{padding: 10,color:'black'}}>
            This text is indented by 10px padding on all sides.
          </Text>}
        </RNTesterBlock>
        <RNTesterBlock title="Font Family">
          <Text style={{fontFamily: 'micross',color:'black'}}>
            Sans-Serif
          </Text>
          <Text style={{fontFamily: 'micross', fontWeight: 'bold',color:'black'}}>
            Sans-Serif Bold
          </Text>
          {false && <Text style={{fontFamily: 'serif', color:'black'}}>
            Serif
          </Text>}
          {false && <Text style={{fontFamily: 'serif', fontWeight: 'bold',color:'black'}}>
            Serif Bold
          </Text>}
          {false && <Text style={{fontFamily: 'monospace',color:'black'}}>
            Monospace
          </Text>}
          {false && <Text style={{fontFamily: 'monospace', fontWeight: 'bold',color:'black'}}>
            Monospace Bold (After 5.0)
          </Text>}
        </RNTesterBlock>
        
        <RNTesterBlock title="Custom Fonts">
          <View style={{flexDirection: 'row', alignItems: 'flex-start'}}>
            <View style={{flex: 1}}>
              <Text style={{fontFamily: 'COOPBL',color:'black'}}>
                COOPBL Regular
              </Text>
              <Text style={{fontFamily: 'COOPBL', fontStyle: 'italic', fontWeight: 'bold', color:'black'}}>
                COOPBL Bold Italic
              </Text>
              <Text style={{fontFamily: 'COOPBL', fontStyle: 'italic',color:'black'}}>
                COOPBL Italic (Missing Font file)
              </Text>
            </View>
          </View>
        </RNTesterBlock>

        <RNTesterBlock title="Font Size">
          <Text style={{fontSize: 30,color:'black'}}>
            Size 30
          </Text>
          <Text style={{fontSize: 8,color:'black'}}>
            Size 8
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Color">
          <Text style={{color: 'red'}}>
            Red color
          </Text>
          <Text style={{color: 'blue'}}>
            Blue color
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Font Weight">
          <Text style={{fontWeight: 'bold',color:'black'}}>
            Move fast and be bold
          </Text>
          <Text style={{fontWeight: 'normal',color:'black'}}>
            Move fast and be bold
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Font Style">
          <Text style={{fontStyle: 'italic',color:'black'}}>
            Move fast and be bold
          </Text>
          <Text style={{fontStyle: 'normal',color:'black'}}>
            Move fast and be bold
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Font Style and Weight">
          <Text style={{fontStyle: 'italic', fontWeight: 'bold',color:'black'}}>
            Move fast and be bold
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Text Decoration">
          <Text style={{textDecorationLine: 'underline',color:'black', textDecorationColor: 'green'}}>
            underline
          </Text>
          <Text style={{textDecorationLine: 'none',color:'black'}}>
            None textDecoration
          </Text>
          <Text style={{textDecorationLine: 'line-through',color:'black'}}>
            line-through (textDecorationLine: 'line-through',color:'black')
          </Text>
          <Text style={{textDecorationLine: 'underline line-through',color:'black', textDecorationColor: 'red'}}>
            Both underline and line-through
          </Text>
          <Text style={{color:'black'}}>
            Mixed text with <Text style={{textDecorationLine: 'underline',color:'black',textDecorationColor: '#6897bb'}}>underline</Text> and <Text style={{textDecorationLine: 'line-through',color:'black',textDecorationColor: 'yellow'}}>line-through</Text> text nodes
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Nested">
          <Text onPress={() => console.log('1st')} style={{color:'black'}}>
            (Normal text,
            <Text style={{fontWeight: 'bold'}} onPress={() => console.log('2nd')}>
              (and bold
              <Text style={{fontStyle: 'italic', fontSize: 11, color: '#527fe4'}} onPress={() => console.log('3rd')}>
                (and tiny bold italic blue
                <Text style={{fontWeight: 'normal', fontStyle: 'normal'}} onPress={() => console.log('4th')}>
                  (and tiny normal blue)
                </Text>
                )
              </Text>
              )
            </Text>
            )
          </Text>
          <Text style={{fontFamily: 'COOPBL',color:'black'}} onPress={() => console.log('1st')}>
            (COOPBL
            <Text style={{fontStyle: 'italic', fontWeight: 'bold'}} onPress={() => console.log('2nd')}>
              (Serif Bold Italic
              <Text
                style={{fontFamily: 'calibri', fontStyle: 'normal', fontWeight: 'normal'}}
                onPress={() => console.log('3rd')}>
                (Calibri Normal
                <Text
                  style={{fontFamily: 'micross', fontWeight: 'bold'}}
                  onPress={() => console.log('4th')}>
                  (Sans-Serif Bold
                  <Text style={{fontWeight: 'normal'}} onPress={() => console.log('5th')}>
                    (and Sans-Serif Normal)
                  </Text>
                  )
                </Text>
                )
              </Text>
              )
            </Text>
            )
          </Text>
          <Text style={{fontSize: 12,color:'black'}}>
            <Entity>Entity Name</Entity>
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Text Align">
          <Text style={{color:'black'}}>
            auto (default) - english LTR
          </Text>
          <Text style={{color:'black'}}>
            أحب اللغة العربية auto (default) - arabic RTL
          </Text>
          <Text style={{textAlign: 'left',color:'black'}}>
            left left left left left left left left left left left left left left left
          </Text>
          <Text style={{textAlign: 'center',color:'black'}} >
            center center center center center center center center center center center
          </Text>
          <Text style={{textAlign: 'right',color:'black'}} >
            right right right right right right right right right right right right right
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Unicode">
          <View>
            <View style={{flexDirection: 'row'}}>
              <Text style={{backgroundColor: 'red'}}>
                星际争霸是世界上最好的游戏。
              </Text>
            </View>
            <View>
              <Text style={{backgroundColor: 'red'}}>
                星际争霸是世界上最好的游戏。
              </Text>
            </View>
            <View style={{alignItems: 'center'}}>
              <Text style={{backgroundColor: 'red'}}>
                广告位招商TEL:911。星际争霸是世界上最好的游戏。
              </Text>
            </View>
            <View>
              <Text style={{backgroundColor: 'red'}}>
                星际争霸是世界上最好的游戏。星际争霸是世界上最好的游戏。星际争霸是世界上最好的游戏。星际争霸是世界上最好的游戏。
              </Text>
            </View>
          </View>
        </RNTesterBlock>
        <RNTesterBlock title="Spaces">
          <Text style={{color:'black'}}>
            A {'generated'} {' '} {'string'} and    some &nbsp;&nbsp;&nbsp; spaces
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Line Height">
          <Text style={{lineHeight: 35,color:'black'}}>
            Holisticly formulate inexpensive ideas<Text style={{fontSize: 20}}>ideas Continually before</Text> before best-of-breed benefits. Holisticly formulate inexpensive ideas before best-of-breed benefits.Holisticly formulate inexpensive ideas before best-of-breed benefits. expedite magnetic potentialities rather than client-focused interfaces. <Text>Continually</Text> expedite magnetic potentialities rather than client-focused interfaces.
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Empty Text">
          <Text />
        </RNTesterBlock>
        <RNTesterBlock title="Toggling Attributes">
          <AttributeToggler />
        </RNTesterBlock>
        <RNTesterBlock title="backgroundColor attribute">
          <Text style={{backgroundColor: '#ffaaaa'}}>
            Red background,
            <Text style={{backgroundColor: '#aaaaff'}}>
              {' '}blue background,
              <Text>
                {' '}inherited blue background,
                <Text style={{backgroundColor: '#aaffaa'}}>
                  {' '}nested green background.
                </Text>
              </Text>
            </Text>
          </Text>
          <Text style={{backgroundColor: 'rgba(100, 100, 100, 0.3)'}}>
            Same alpha as background,
            <Text>
              Inherited alpha from background,
              <Text style={{backgroundColor: 'rgba(100, 100, 100, 0.3)'}}>
                Reapply alpha
              </Text>
            </Text>
          </Text>
        </RNTesterBlock>
        
        <RNTesterBlock title="containerBackgroundColor attribute">
          <View style={{flexDirection: 'row', height: 85}}>
            <View style={{backgroundColor: '#ffaaaa', width: 150}} />
            <View style={{backgroundColor: '#aaaaff', width: 150}} />
          </View>
          <Text style={[styles.backgroundColorText, {top: -80}]}>
            Default containerBackgroundColor (inherited) + backgroundColor wash
          </Text>
          <Text style={[styles.backgroundColorText, {top: -70, backgroundColor: 'transparent'}]}>
            {"containerBackgroundColor: 'transparent' + backgroundColor wash"}
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="numberOfLines attribute">
          <Text numberOfLines={1} style={{color:'black'}}>
            Maximum of one line no matter now much I write here. If I keep writing it{"'"}ll just truncate after one line
          </Text>
          <Text numberOfLines={2} style={{marginTop: 20,color:'black'}}>
            Maximum of two lines no matter now much I write here. If I keep writing it{"'"}ll just truncate after two lines
          </Text>
          <Text style={{marginTop: 20,color:'black'}}>
            No maximum lines specified no matter now much I write here. If I keep writing it{"'"}ll just keep going and going
          </Text>
        </RNTesterBlock>
        
        <RNTesterBlock title="Text shadow (partial supported)">
          <Text style={{color:'black',fontSize: 35, textShadowOffset: {width: 2, height: 2}, textShadowRadius: 1, textShadowColor: '#00cccc'}}>
            Demo text shadow
          </Text>
          <Text style={{fontSize: 35, color:'black'}}>
            Demo text shadow
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="Ellipsize mode">
          <Text style={{height:35, width: 1000, color:'black'}} >
            This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.
          </Text>
          <Text style={{height:35, width: 1000, color:'black'}} ellipsizeMode="middle" >
            This very long text should be truncated with dots in the middle.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.
          </Text>
          <Text ellipsizeMode="head" style={{height:35, width: 1000, color:'black'}} >
            This very long text should be truncated with dots in the beginning.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.This very long text should be truncated with dots in the end.
          </Text>
        </RNTesterBlock>
        <RNTesterBlock title="textAlignVertical">
          <View style={{backgroundColor: 'green'}}>
            <Text style={{fontSize: 20, height:70, textAlignVertical: 'center'}}>
              This text's style is fontSize: 20, height:70, textAlignVertical: 'center'
            </Text>
          </View>
          <View style={{backgroundColor: 'red'}}>
            <Text style={{fontSize: 20, height:70, textAlignVertical: 'top'}}>
              This text's style is fontSize: 20, height:70, textAlignVertical: 'top'
            </Text>
          </View>
          <View style={{backgroundColor: 'yellow'}}>
            <Text style={{fontSize: 20, height:70, textAlignVertical: 'bottom'}}>
              This text's style is fontSize: 20, height:70, textAlignVertical: 'bottom'
            </Text>
          </View>
        </RNTesterBlock>
        
      </RNTesterPage>
    );
  }
}

var styles = StyleSheet.create({
  backgroundColorText: {
    left: 5,
    backgroundColor: 'rgba(100, 100, 100, 0.3)'
  },
  includeFontPaddingText: {
    fontSize: 120,
    fontFamily: 'micross',
    backgroundColor: '#EEEEEE',
    color: '#000000',
    textAlignVertical: 'center',
    alignSelf: 'center',
  }
});

module.exports = TextExample;
