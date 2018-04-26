/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule AsyncStorageExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');
const StyleSheet = require('StyleSheet');
var {
  AsyncStorage,
  Picker,
  TouchableHighlight,
  Text,
  Button,
  View
} = ReactNative;
var PickerItem = Picker.Item;

var STORAGE_KEY = '@AsyncStorageExample:key';
var STORAGE_KEY_SAMSUNG = '@SAMSUNG:key';
var COLORS = ['orange', 'red', 'green', 'blue'];

class BasicStorageExample extends React.Component {
  state = {
    selectedValue: COLORS[0],
    messages: [],
    data: '\n',
    dataMuti: '\n',
  };
  componentDidMount() {
    this._loadInitialState().done();
  }

  _loadInitialState = async () => {
    try {
      var value = await AsyncStorage.getItem(STORAGE_KEY);
      if (value !== null) {
        this.setState({ selectedValue: value });
        this._appendMessage('getItem from disk: ' + value);
      } else {
        this._appendMessage('Initialized with no selection on disk.');
      }
    } catch (error) {
      this._appendMessage('AsyncStorage error: ' + error.message);
    }
  };

  render() {
    var color = this.state.selectedValue;
    return (
      <View>

        <Picker
          style={styles.picker}
          selectedValue={color}
          onValueChange={this._onValueChange}>
          {COLORS.map((value) => (
            <PickerItem
              key={value}
              value={value}
              label={value}
            />
          ))}
        </Picker>
        <Text style={{ color: color }}>
          {'Selected: '}
          <Text style={{ color }}>
            {this.state.selectedValue}
          </Text>
        </Text>

        <Button style={styles.picker} onPress={this._removeStorage} title='Remove Item.' />
        <Text style={{ color: color }}>Messages:</Text>
        {this.state.messages.map((m) => <Text style={{ color: color }} key={m}>{m}</Text>)}

        <Button style={styles.picker} onPress={this._getAll} title='GetAllKey' />
        <Text style={styles.text}>keys:</Text>
        <Text style={{ color }}>{this.state.data}</Text>

        <Button style={styles.picker} onPress={this._clearStorage} title='Clear' />
        <Text style={styles.text}>{"keys:" + this.state.data}</Text>


        <Button style={styles.picker} onPress={this._mergeItem} title='Merge' />
        <Text style={styles.text}>{"keys:" + this.state.data}</Text>

        <Button style={styles.picker} onPress={this._multiGet} title='MultiGet' />
        <Text style={styles.text}>{"keys:" + this.state.dataMuti}</Text>

        <Button style={styles.picker} onPress={this._multiSet} title='MultiSet' />
        <Text style={styles.text}>{"keys:" + this.state.data}</Text>

        <Button style={styles.picker} onPress={this._multiRemove} title='MultiRemove' />
      </View>
    );
  };

  _onValueChange = async (selectedValue) => {
    this.setState({ selectedValue });
    try {
      await AsyncStorage.setItem(STORAGE_KEY, selectedValue);
      await AsyncStorage.setItem(STORAGE_KEY_SAMSUNG, selectedValue);
      
      this._appendMessage('setItem to disk: ' + selectedValue);
    } catch (error) {
      this._appendMessage('AsyncStorage error: ' + error.message);
    }
  };
  _getAll = () => {
    this.setState({
      data: '\n',

    })
    AsyncStorage.getAllKeys((err, keys) => {
      if (keys && keys.length > 0) {
        keys.map((key, index) => {
          AsyncStorage.getItem(key, (err, result) => {
            var msg = this.state.data + key + ': ' + result ;
;
            this.setState({ data: msg });
          })
        });
      }
    })
  };
  _removeStorage = async () => {
    try {
      await AsyncStorage.removeItem(STORAGE_KEY);
      this._appendMessage('removeItem from disk.');
    } catch (error) {
      this._appendMessage('AsyncStorage error: ' + error.message);
    }
  };
  _clearStorage = async () => {
    var _that = this;
    AsyncStorage.clear(function (err) {
      if (!err) {
        _that.setState({
          data: [],
        });
      }
      alert("clear succeed");
    });
  };
  _mergeItem = async () => {
    var _that = this;
    AsyncStorage.mergeItem(STORAGE_KEY, '_Samsung', function (err) {
      if (!err) {
        let msg = _that.state.data + '_Samsung' + '\n';
        _that.setState({
          data:msg,
        });
      }
    });
  }
  _multiGet = async () => {
    var _that = this;
    AsyncStorage.multiGet([STORAGE_KEY, STORAGE_KEY_SAMSUNG], function (err, result) {
      if (!err) {
        let msg = _that.state.data + result + '\n';
        _that.setState({
          dataMuti: msg,
        });
      }

    })

  };

  _multiSet = async () => {
    var _that = this;
    AsyncStorage.multiSet([[STORAGE_KEY, 'Hello'], [STORAGE_KEY_SAMSUNG, 'Samsung']]);

  };

  _multiRemove = async () => {
    var _that = this;
    AsyncStorage.multiRemove([STORAGE_KEY, STORAGE_KEY_SAMSUNG]);

  };


  _appendMessage = (message) => {
    this.setState({ messages: this.state.messages.concat(message) });
  };
};




var styles = StyleSheet.create({
  picker: {
    width: 300,
    backgroundColor: '#A3A3A3'
  },
  text: {
    color: 'black',
  },
});

exports.title = 'AsyncStorage';
exports.description = 'Asynchronous local disk storage.';
exports.examples = [
  {
    title: 'Basics - getItem, setItem, removeItem',
    render(): React.Element<any> { return <BasicStorageExample />; }
  },
];
