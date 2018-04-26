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

export type Example = {
  key: string,
  module: Object,
};

// Component
const ComponentExamples: Array<Example> = [
  {
    key: 'Button',
    module: require('./ButtonExample'),
  },
  {
    key: 'TextInput',
    module: require('./TextInputExample'),
  },
  {
    key: 'Time',
    module: require('./TimerExample'),
  },
  {
    key: 'Picker',
    module: require('./PickerExample'),
  },
  {
    key: 'Transforms',
    module: require('./TransformExample'),
  },
  {
  key: 'ListView',
  module: require('./ListViewExample'),
  },
  {
    key: 'ListViewPaging',
    module: require('./ListViewPagingExample'),
  },
  {
    key: 'ListViewGridLayout',
    module: require('./ListViewGridLayoutExample'),
  },
  {
    key: 'ScrollView',
    module: require('./ScrollViewExample'),
  },
  {
    key: 'NativeAnimations',
    module: require('./NativeAnimationsExample'),
  },
  {
    key: 'Image',
    module: require('./ImageExample'),
  },
  {
    key: 'View',
    module: require('./ViewExample'),
  },
  {
    key: 'Text',
    module: require('./TextExample'),
  },
  {
    key: 'Touchable',
    module: require('./TouchableExample'),
  },
  {
    key: 'Layout',
    module: require('./LayoutExample'),
  },
  {
    key: 'LayoutEvents',
    module: require('./LayoutEventsExample'),
  },
  {
    key: 'ActivityIndicator',
    module: require('./ActivityIndicatorExample'),
  },
  {
    key: 'ProgressBar',
    module: require('./ProgressBarExample'),
  },
  {
    key: 'Switch',
    module: require('./SwitchExample'),
  },
  {
    key: 'Icon',
    module: require('./IconExample'),
  },
];

// API
const ApiExamples: Array<Example> = [
 {
    key: 'AppState',
    module: require('./AppStateExample'),
  },
  {
    key: 'Alert',
    module: require('./AlertExample'),
  },
  {
    key: 'WebSocket',
    module: require('./WebSocketExample'),
  },
  {
    key: 'XHR',
    module: require('./XHRExample'),
  },
  {
    key: 'AsyncStorage',
    module: require('./AsyncStorageExample'),
  },
  {
    key: 'NetInfo',
    module: require('./NetInfoExample'),
  },
  {
    key: 'Animated',
    module: require('./AnimatedExample'),
  },
  {
    key: 'MediaPlayer',
    module: require('./MediaPlayerExample'),
  },
];

const Modules = {};

ComponentExamples.forEach(Example => {
  Modules[Example.key] = Example.module;
});

ApiExamples.forEach(Example => {
  Modules[Example.key] = Example.module;
});


const ExamplesList = {
  ComponentExamples,
  ApiExamples,
  Modules,
};

module.exports = ExamplesList;
