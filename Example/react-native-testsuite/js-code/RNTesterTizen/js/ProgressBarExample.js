/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 */
'use strict';

var ProgressBar = require('ProgressBarAndroid');
var React = require('React');
var RNTesterBlock = require('RNTesterBlock');
var RNTesterPage = require('RNTesterPage');

var TimerMixin = require('react-timer-mixin');

var MovingBar = React.createClass({
  mixins: [TimerMixin],

  getInitialState: function() {
    return {
      progress: 0
    };
  },

  componentDidMount: function() {
    this.setInterval(
      () => {
        var progress = (this.state.progress + 0.02) % 1;
        this.setState({progress: progress});
      }, 50
    );
  },

  render: function() {
    return <ProgressBar progress={this.state.progress} {...this.props} />;
  },
});

class ProgressBarExample extends React.Component {
  static title = 'ProgressBarExample';
  static description = 'Horizontal bar to show the progress of some operation.';

  render() {
    return (
      <RNTesterPage title="ProgressBar Examples">
        <RNTesterBlock title="Horizontal ProgressBar">
          <ProgressBar value={0.5} style={{width:1000, height:4}} horizontal={true} />
        </RNTesterBlock>

        <RNTesterBlock title="Colored ProgressBar(yellow)">
          <MovingBar horizontal={true} style={{width:1000, height:4}} />
        </RNTesterBlock>

        <RNTesterBlock title="IsPulseMode = TRUE, ProgressBar">
          <ProgressBar style={{width:1000, height:4}} value={0.3} horizontal={true} isPulseMode={true}/>
        </RNTesterBlock>

        <RNTesterBlock title="Valued ProgressBar">
          <ProgressBar style={{width:1000, height:4}} value={0.7} color="red" />
        </RNTesterBlock>
      </RNTesterPage>
    );
  }
}

module.exports = ProgressBarExample;
