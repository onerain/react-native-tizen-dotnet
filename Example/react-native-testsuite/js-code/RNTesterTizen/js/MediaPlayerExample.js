/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule MediaPlayerExample
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

import {MediaPlayer} from 'react-native-tizen'
import SimpleLineIcons from 'react-native-vector-icons/SimpleLineIcons'
import FontAwesome from 'react-native-vector-icons/FontAwesome'

var RNTesterBlock = require('./RNTesterBlock');
var RNTesterPage = require('./RNTesterPage');

const items = [{
  name: 'Dreaming of Home and Mother',
  author: 'Nobody',
  source: require('./MusicPlayer/DreamingofHomeandMother.mp3'),
}, {
  name: 'Cheng Du',
  author: 'Zhao Lei',
  source: require('./MusicPlayer/chengdu.mp3'),
}, {
  name: 'Almost Is Never Enough',
  author: 'Ariana Grande',
  source: require('./MusicPlayer/music.mp3')
}];

class MediaPlayerExample extends React.Component {
  static title = 'MediaPlayer';
  static description = 'Examples of using the MediaPlayer API.';
  static displayName = 'MediaPlayerExample';
 
  constructor(props) {
    super(props)
    this.state = {
      i: 0, // current playing index of PLAY_LIST
    }
  }

  render() {
    return (
      <RNTesterPage title={'Meida Player'}>
        <RNTesterBlock title="Audio">
          <View style={styles.audioContainer}>
            <MusicPlayer playingItem={items[this.state.i]} nextMethod={(preOrNext) => {
              let nextIndex = this.state.i + preOrNext;
              this.setState({
                  i: (nextIndex >= items.length || nextIndex < 0) ? 0 : nextIndex,
              })
            }}/>
          </View>
        </RNTesterBlock>
      </RNTesterPage>
    );
  }
}

export class MusicPlayer extends React.Component {
  constructor(props) {
    super(props)
    MediaPlayer.init(this.props.playingItem.source)
    MediaPlayer.addEventListener('prepared', (event) => {
      MediaPlayer.play()
    })
    this.state = {
      focusedItem: 'play',
      playing: true,
      playingBarX: -615,
      barWidth: 615,
    }

    this.next = this.next.bind(this)
    this._focus = this._focus.bind(this)
    this._toTimeString =  this._toTimeString.bind(this)
  }

  render() {
    return (
      <View>
        <View style={styles.operationContainer}>
          <View style={styles.operation}>
            <OperateIcon item='control-start' focusedItem={this.state.focusedItem} _focus={this._focus} _pressIcon={()=>{this.next(-1)}}/>
            <OperateIcon item='control-rewind' focusedItem={this.state.focusedItem} _focus={this._focus} _pressIcon={()=>{{this.seek(-10000)}}}/>
            <OperateIcon item={this.state.playing ? 'control-pause' : 'control-play'} focusedItem={this.state.focusedItem} _focus={this._focus} _pressIcon={()=>{this._togglePlay()}}/>
            <OperateIcon item='control-forward' focusedItem={this.state.focusedItem} _focus={this._focus} _pressIcon={()=>{this.seek(10000)}}/>
            <OperateIcon item='control-end' focusedItem={this.state.focusedItem} _focus={this._focus} _pressIcon={()=>{this.next(1)}}/>
          </View>
        </View>
        <View style={styles.information}>
          <View style={styles.audioInfo}>
            <Text style={styles.name}>{this.props.playingItem.name}</Text>
            <Text style={styles.author}>{this.props.playingItem.author}</Text>
          </View>
          <View style={styles.process}>
            <Text style={[styles.time]}>{this.state.currentTimeString || '00:00'}</Text>
            <View style={[styles.bar]}>
              <View style={[styles.bar, styles.playingBar, {left: this.state.playingBarX}]}></View>
            </View>
            <Text style={styles.time}>{this.state.durationString || '00:00'}</Text>
          </View>
        </View>
        <View style={styles.eventsInfoContainer}>
          <Text style={styles.eventsInfo}>{'Event: ' + this.state.eventsInfo}</Text>
          <Text style={styles.eventsInfo}>{'Event : ' + this.state.updateplayinfo}</Text>
        </View>
      </View>
    )
  }

  componentWillUnmount() {
    MediaPlayer.destroy()
  }

  componentDidMount() {
    MediaPlayer.addEventListener('updateplayinfo', (event) => {
      this.setState({
        durationString: this._toTimeString(event.duration),
        currentTimeString: this._toTimeString(event.position),
        currentTime: event.position,
        duration: event.duration,
        playingBarX: (event.position / event.duration - 1) * this.state.barWidth,
      })

    })
    MediaPlayer.addEventListener('playbackcomplete', (event) => {
      this.next(1)
    })

    //Events test
    const MEDIA_PLAYER_EVENTS = {
      idle: 'idle',
      preparing: 'preparing',
      prepared: 'prepared',
      started: 'started',
      paused: 'paused',
      updateplayinfo: 'updatePlayInfo',
      seeking: 'seeking',
      seeked: 'seeked',
      playbackcomplete: 'playbackComplete',
      playbackinterrupted: 'playbackInterrupted',
      erroroccurred: 'errorOccurred',
      exceptionhappened: 'exceptionHappened'
    }
    for(let key in MEDIA_PLAYER_EVENTS) {
      MediaPlayer.addEventListener(key, (event) => {
        console.log('Event ' + key + ' callback invoked.')
        this.setState(key === 'updateplayinfo' ? {
          updateplayinfo: 'updateplayinfo, Position: ' + event.position,
        } : {
          eventsInfo: key + ', Result: ' + event.EVENT_RESULT
        })
      })
    }
  }

  next(preOrNext) {
    this.props.nextMethod(preOrNext)
    MediaPlayer.init(this.props.playingItem.source)
    MediaPlayer.play()
    this.setState({
      playing: true
    })
  }

  seek(time: number) {
    console.log('currentTime:' + this.state.currentTime)
    console.log(time)
    let seekTo = this.state.currentTime + time
    if(this.state.duration && (seekTo > this.state.duration || seekTo === this.state.duration)){ 
      this.setState({
        currentTimeString: this._toTimeString(this.state.duration)
      })
      this.next(1)
    } else {
      MediaPlayer.seekTo(seekTo)
    }
  }

  _focus(item) {
    item && this.setState({
      focusedItem: item,
    })
  }

  _togglePlay() {
    this.state.playing ? MediaPlayer.pause() : MediaPlayer.play()
    this.setState({
      playing: this.state.playing ? false : true,
    })
  }

  _toTimeString(time){
    return getFullTime(new Date(time).getMinutes()) + ':' + getFullTime(new Date(time).getSeconds())
  }
}

function getFullTime(time) {
  return time < 10 ? '0' + time : time
}

export class OperateIcon extends React.Component {
  render() {
    return (
      <TouchableHighlight underlayColor='rgba(0, 0, 0, 0)' activeOpacity={.65} onPressIn={() => this.props._focus(this.props.item)} onPress={() => this.props._pressIcon()}>
        <SimpleLineIcons name={this.props.item} size={this.props.focusedItem === this.props.item ? 55 : 40}></SimpleLineIcons>
      </TouchableHighlight>
    )
  }
}

var styles = StyleSheet.create({
  audioContainer: {
    backgroundColor: '#aaccff',
    width: 1700,
    height: 400,
  },
  operationContainer: {
    position: 'absolute',
    left: 50,
    top: 100,
    width: 640,
    height: 225,
    flex: 0,
    alignItems: 'center',
    justifyContent: 'center',
  },
  operation:{
    width: 350,
    height: 225,
    flex: 0,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  information: {
    position: 'absolute',
    left: 700,
    top: 100,
    width: 790,
    height: 100,
  },
  audioInfo: {
    position: 'absolute',
  },
  name:{
    fontSize: 45,
    fontWeight: 'bold',
  },
  author:{
    fontSize: 25,
  },
  process:{
    position: 'absolute',
    width: 790,
    left: 0,
    top: 100,
    flex: 0,
    flexDirection: 'row',
    alignItems: 'center',
    justifyContent: 'space-between',
  },
  bar:{
    width: 615,
    height: 2,
    opacity: .4,
    backgroundColor:'white',
  },
  playingBar:{
    position: 'absolute',
    width: 615,
    top: 0,
    opacity: 1,
    //backgroundColor:'red',
  },
  time:{
    fontSize: 20,
    color: 'white',
    opacity: .6,
  },
  eventsInfoContainer: {
    position: 'absolute',
    left: 50,
    top: 300,
    width: 800,
    height: 50,
  },
  eventsInfo: {
    color: 'white',
    //backgroundColor: '#ffd700',
  },
});

module.exports = MediaPlayerExample;
