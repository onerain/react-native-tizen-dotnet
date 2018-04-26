/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule XHRExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');

var XHRExampleDownload = require('./XHRExampleDownload');
var XHRExampleBinaryUpload = require('./XHRExampleBinaryUpload');
var XHRExampleFormData = require('./XHRExampleFormData');
var XHRExampleHeaders = require('./XHRExampleHeaders');
var XHRExampleFetch = require('./XHRExampleFetch');
var XHRExampleOnTimeOut = require('./XHRExampleOnTimeOut');
//var XHRExampleCookies = require('./XHRExampleCookies');

const {
  Text,
  View,
} = ReactNative;

class NotSupproted extends React.Component {
  render() {
    return <Text style={{fontSize:15, color:'red'}}>TODO, have not supproted</Text>
  }
}

exports.framework = 'React';
exports.title = 'XMLHttpRequest';
exports.description = 'Example that demonstrates upload and download ' +
  'requests using XMLHttpRequest.';
exports.examples = [{
  title: 'File Download',
  titleNotHightlightable: true,
  render() {
    return <XHRExampleDownload/>;
  }
}, {
  title: 'multipart/form-data Upload',
  titleNotHightlightable: true,
  render() {
    return <XHRExampleBinaryUpload/>;
  }
}, /*{
  title: 'multipart/form-data Upload ',
  titleNotHightlightable: true,
  render() {
    return <View>{false ? <XHRExampleFormData/> : <Text style={{color: 'black'}}>[todo, there is error of CameraRoll.getPhotos on TIZEN]</Text>}</View>;
  }
}, */{
  title: 'Fetch Test',
  titleNotHightlightable: true,
  render() {
    return <XHRExampleFetch/>;
  }
}, {
  title: 'Headers',
  titleNotHightlightable: true,
  render() {
    return <XHRExampleHeaders/>;
  }
}, {
  title: 'Time Out Test',
  titleNotHightlightable: true,
  render() {
    return <XHRExampleOnTimeOut/>;
  }
}, {
  title: 'Cookies',
  titleNotHightlightable: true,
  render() {
    return <NotSupproted/>//<XHRExampleCookies/>;
  }
}];
