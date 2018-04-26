/**
 * Copyright (c) 2013-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * The examples provided by Facebook are for non-commercial testing and
 * evaluation purposes only.
 *
 * Facebook reserves all rights not expressly granted.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NON INFRINGEMENT. IN NO EVENT SHALL
 * FACEBOOK BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN
 * AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
 * CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 *
 * @flow
 */
'use strict';

import React, { Component } from 'react';
import {ActivityIndicator,
        Platform,
        StyleSheet,
        TouchableHighlight,
        View,
        Image,
        Text } from 'react-native';
var ImageCapInsetsExample = require('./ImageCapInsetsExample');


class ImageSizeExample extends Component{
    constructor(props) {
        super(props);
        this.state = {
            width: 0,
            height: 0,
    }
    }
    componentDidMount(){
        this.setState({width:100, height:100});
    }

    render() {
        return (
            <View style={{flexDirection: 'row'}}>
            <Image
                style={{width: 60,height: 60,backgroundColor: 'transparent',marginRight: 10,}}
            source={this.props.source}/>
            <Text>
                Actual dimensions:{'\n'}
                Width: {this.state.width}, Height: {this.state.height}
            </Text>
            </View>
            );
            }
};


const IMAGE_PREFETCH_URL = 'http://origami.design/public/images/bird-logo.png?r=1&t=' + Date.now();
//var prefetchTask = Image.prefetch(IMAGE_PREFETCH_URL);
var MultipleSourcesExample = React.createClass({
    getInitialState: function() {
    return {
        width: 30,
        height: 30,
    };
},
render: function() {
    return (
        <View>
        <View style={{flexDirection: 'row', justifyContent: 'space-between'}}>
            <TouchableHighlight onPress={this.decreaseImageSize}>

                <Text style={styles.touchableText}>
                Decrease image size
                </Text>

            </TouchableHighlight>
            <TouchableHighlight onPress={this.increaseImageSize}>

            <Text style={styles.touchableText}>
                 Increase image size
            </Text>
            </TouchableHighlight>
        </View>
        <Text>Container image size: {this.state.width}x{this.state.height} </Text>
        <View
        style={{height: this.state.height, width: this.state.width}} >
        <Image
        style={{flex: 1}}
        source={[
            {uri: 'http://facebook.github.io/react/img/logo_small.png', width: 38, height: 38},
            {uri: 'http://facebook.github.io/react/img/logo_small_2x.png', width: 76, height: 76},
            {uri: 'http://facebook.github.io/react/img/logo_og.png', width: 400, height: 400}
            ]}
        />
        </View>
        </View>
);
},
increaseImageSize: function() {
    if (this.state.width >= 100) {
        return;
    }
    this.setState({
        width: this.state.width + 10,
        height: this.state.height + 10,
    });
},
decreaseImageSize: function() {
    if (this.state.width <= 10) {
        return;
    }
    this.setState({
        width: this.state.width - 10,
        height: this.state.height - 10,
    });
},
});
class NetworkImageCallbackExample extends Component{

    constructor(props) {
        super(props);
        this.state = {
            events: [],
            startLoadPrefetched: false,
            mountTime: new Date(),};
    }

    _loadEventFired(event) {
        console.log("_loadEventFired.......");
        this.setState((state) => {
            return state.events = [...state.events, event];
    });
    }
    componentWillMount(){
        this.setState({mountTime: new Date()});
    }

    render() {
        var { mountTime } = this.state;
        return (
            <View>
                <Image
                    source={this.props.source}
                    style={[styles.base, {overflow: 'visible'}]}
                    onLoadStart={() => this._loadEventFired(`✔ onLoadStart (+${new Date() - mountTime}ms)`)}
                    onLoad={(event) => {
                        // Currently this image source feature is only available on iOS.
                        if (event.nativeEvent.source) {
                            const url = event.nativeEvent.source.url;
                            this._loadEventFired(`✔ onLoad (+${new Date() - mountTime}ms) for URL ${url}`);
                        } else {this._loadEventFired(`✔ onLoad (+${new Date() - mountTime}ms)`);
                        }
                    }}
                    onLoadEnd={() => {
                        this._loadEventFired(`✔ onLoadEnd (+${new Date() - mountTime}ms)`);
                        this.setState({startLoadPrefetched: true}, () => {
                            fetch(IMAGE_PREFETCH_URL).then(() => {
                            this._loadEventFired(`✔ Prefetch OK (+${new Date() - mountTime}ms)`);
                    }, error => {
                        this._loadEventFired(`✘ Prefetch failed (+${new Date() - mountTime}ms)`);
                    });
                    });
                    }}
                />
                {this.state.startLoadPrefetched ?
                <Image
                    source={this.props.prefetchedSource}
                style={[styles.base, {overflow: 'visible'}]}
                onLoadStart={() => this._loadEventFired(`✔ (prefetched) onLoadStart (+${new Date() - mountTime}ms)`)}
                onLoad={(event) => {
                    // Currently this image source feature is only available on iOS.
                    if (event.nativeEvent.source) {
                        const url = event.nativeEvent.source.url;
                        this._loadEventFired(`✔ (prefetched) onLoad (+${new Date() - mountTime}ms) for URL ${url}`);
                    } else {
                        this._loadEventFired(`✔ (prefetched) onLoad (+${new Date() - mountTime}ms)`);
                    }
                }}
                onLoadEnd={() => this._loadEventFired(`✔ (prefetched) onLoadEnd (+${new Date() - mountTime}ms)`)}
                />
                : null}

            <Text style={{marginTop: 20,color:'black'}}>
                {this.state.events.join('\n')}
            </Text>

            </View>
        );
    }




};

var fullImage = {uri: 'https://facebook.github.io/react/img/logo_og.png'};
var smallImage = {uri: 'https://facebook.github.io/react/img/logo_small_2x.png'};
exports.title = 'Image';
exports.description = 'Image Test';
exports.displayName = 'ImageExample'
exports.examples = [
    {

        title: 'Plain Network Image',
        description: 'If the `source` prop `uri` property is prefixed with ' +
        '"http", then it will be downloaded from the network.',
    render: function() {
        return (
            <View style= {{flexDirection:'row',justifyContent:'space-between'}}>
            <Image
                source={{uri: 'https://facebook.github.io/react/img/logo_og.png'}}
                style={styles.base}
            />
            <Image
                source={{uri: 'https://facebook.github.io/react/img/logo_og.png'}}
                style={styles.base}
            />
            <Image
                source={{uri: 'https://facebook.github.io/react/img/logo_og.png'}}
                style={styles.base}
            />
            </View>
);
},
},
{
        title: 'Plain Static Image',
         description: 'Static assets should be placed in the source code tree, and ' +
        'required in the same way as JavaScript modules.',
        render: function() {
            return (
            <View style={styles.horizontal}>
                <Image style={styles.base} source={require('./Thumbnails/uie_thumb_normal.png')} />
                <Image style={styles.base} source={require('./Thumbnails/uie_thumb_selected.png')}  />
                <Image style={styles.base} source={require('./Thumbnails/uie_comment_normal.png')}  />
                <Image style={styles.base} source={require('./Thumbnails/uie_comment_highlighted.png')} />
            </View>
        );
        },
        },

       {
            title: 'Image Loading Events',
                render: function() {
            return (
                <NetworkImageCallbackExample source={{uri: 'https://facebook.github.io/react/img/logo_og.png'}}
                prefetchedSource={{uri: IMAGE_PREFETCH_URL}}/>
        );
        },
        },


        {
            title: 'Background Color',
                render: function() {
            return (
                <View style={styles.horizontal}>
                    <Image source={smallImage} style={styles.base} />
                    <Image style={[styles.base,styles.leftMargin,{backgroundColor: 'rgba(0, 0, 100, 0.25)'}]}
                            source={smallImage}/>
                    <Image style={[styles.base, styles.leftMargin, {backgroundColor: 'red'}]}
                            source={smallImage}/>
                    <Image style={[styles.base, styles.leftMargin, {backgroundColor: 'black'}]}
                            source={smallImage}/>
                </View>
        );
        },
        },


        {
            title: 'Opacity',
            render: function() {
            return (
                <View style={styles.horizontal}>
                    <Image style={[styles.base, {opacity: 1}]}source={fullImage}/>
                    <Image style={[styles.base, styles.leftMargin, {opacity: 0.8}]} source={fullImage}/>
                    <Image style={[styles.base, styles.leftMargin, {opacity: 0.6}]} source={fullImage}/>
                    <Image style={[styles.base, styles.leftMargin, {opacity: 0.4}]} source={fullImage}/>
                    <Image style={[styles.base, styles.leftMargin, {opacity: 0.2}]} source={fullImage}/>
                    <Image style={[styles.base, styles.leftMargin, {opacity: 0}]} source={fullImage}/>
                </View>
        );
        },
        },



{
    title: 'Resize Mode',
    description: 'The `resizeMode` style prop controls how the image is rendered within the frame.',
    render: function() {
    return (
    <View>
    {[smallImage, fullImage].map((image, index) => {
        return (
        <View key={index}>
            <View style={styles.horizontal}>

                <Text style={[styles.resizeModeText]}>Contain</Text>
                <Image style={styles.resizeMode} resizeMode={Image.resizeMode.contain}source={image}/>

                <Text style={[styles.resizeModeText]}>Cover</Text>
                <Image style={styles.resizeMode} resizeMode={Image.resizeMode.cover} source={image}/>

                <Text style={[styles.resizeModeText]}>Stretch</Text>
                <Image style={styles.resizeMode} resizeMode={Image.resizeMode.stretch}source={image}/>


            </View>
        </View>
        );
        })}
        </View>
        );
        },
        },

        {
            title: 'Image Size',
                render: function() {
                    return(<ImageSizeExample source={fullImage} />);
        },
        },

        {
            title: 'MultipleSourcesExample',
                description:
            'The `source` prop allows passing in an array of uris, so that native to choose which image ' +
            'to diplay based on the size of the of the target image',
                render: function() {
            return <MultipleSourcesExample />;
        },
        },


        {
            title: 'Legacy local image',
            description:
            'Images shipped with the native bundle, but not managed by the JS packager',
            render: function() {
                return (
                    <Image
                        source={{uri: 'legacy_image', width: 120, height: 120}}

        />
        );
        },
        },
]
var styles = StyleSheet.create({
  base: {
    width: 100,
    height: 100,
  },
  progress: {
    flex: 1,
    alignItems: 'center',
    flexDirection: 'row',
    width: 100
  },
  leftMargin: {
    marginLeft: 10,
  },
  background: {
    backgroundColor: '#222222'
  },
  sectionText: {
    marginVertical: 6,
  },
  nestedText: {
    marginLeft: 12,
    marginTop: 20,
    backgroundColor: 'transparent',
    color: 'white'
  },
  resizeMode: {
    width: 90,
    height: 60,
    borderWidth: 0.5,
    borderColor: 'black'
  },
  resizeModeText: {
    fontSize: 11,
    marginBottom: 3,
  },
  icon: {
    width: 15,
    height: 15,
  },
  horizontal: {
    flexDirection: 'row',
  },
  gif: {
    flex: 1,
    height: 200,
  },
  base64: {
    flex: 1,
    height: 50,
    resizeMode: 'contain',
  },
  touchableText: {
    fontWeight: '500',
    color: 'blue',
  },
});