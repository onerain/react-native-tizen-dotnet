/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule TextInputExample
 */
'use strict';

var React = require('react');
var ReactNative = require('react-native');
var {
    Image,
    StyleSheet,
    TextInput,
    Text,
    View,
    } = ReactNative;
var RNTesterBlock = require('./RNTesterBlock');
var RNTesterPage = require('./RNTesterPage');

class TextEventsExample extends React.Component {
  state = {
    curText: '<No Event>',
    prevText: '<No Event>',
    prev2Text: '<No Event>',
  };

  updateText = (text) => {
      this.setState((state) => {
      return {
        curText: text,
        prevText: state.curText,
        prev2Text: state.prevText,
      };
});
};

render() {
  return (
      <View>
      <TextInput  autoCapitalize="none" placeholder="Enter text to see events" autoCorrect={false}
                  onFocus={() => this.updateText('onFocus')}onBlur={() => this.updateText('onBlur')}
                  onChange={(event) => this.updateText(
                          'onChange text: ' + event.nativeEvent.text
                  )}
                  onContentSizeChange={(event) => this.updateText(
                      'onContentSizeChange size: ' + event.nativeEvent.contentSize
                  )}
                  onEndEditing={(event) => this.updateText(
                      'onEndEditing text: ' + event.nativeEvent.text
                  )}
                  onSubmitEditing={(event) => this.updateText(
                      'onSubmitEditing text: ' + event.nativeEvent.text
                  )}
                  style={styles.singleLine}
                  />
                  <Text style={styles.eventLabel}>
                  {this.state.curText}{'\n'}
                  (prev: {this.state.prevText}){'\n'}
                  (prev2: {this.state.prev2Text})
                  </Text>
                  </View>
                  );
                  }
}

class AutoExpandingTextInput extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      height: 0,
    };
  }
  render() {
    return (
        <TextInput
    {...this.props}
    multiline={true}
    onContentSizeChange={(event) => {
      this.setState({height: event.nativeEvent.contentSize.height});
    }}
  style={[styles.default, {height: Math.min(200, Math.max(35, this.state.height))}]}
/>
);
}
}
class RewriteExample extends React.Component {
  constructor(props) {
    super(props);
    this.state = {text: ''};
  }
  render() {
    var limit = 20;
    var remainder = limit - this.state.text.length;
    var remainderColor = remainder > 5 ? 'blue' : 'red';
    return (
        <View style={styles.rewriteContainer}>
<TextInput multiline={false} maxLength={limit} onChangeText={(text) => {
      text = text.replace(/ /g, '_');
      this.setState({text});
    }}
    style={styles.default}
    value={this.state.text}
    />
<Text style={[styles.remainder, {color: remainderColor}]}>
{remainder}
</Text>
</View>
);
}
}

class ToggleDefaultPaddingExample extends React.Component {
  constructor(props) {
    super(props);
    this.state = {hasPadding: false};
  }
  render() {
    return (
        <View>
        <TextInput style={this.state.hasPadding ? { padding: 0 } : null}/>
            <Text onPress={() => this.setState({hasPadding: !this.state.hasPadding})}>
            Toggle padding
            </Text>
            </View>
            );
            }
}
class TokenizedTextExample extends React.Component {
  constructor(props) {
    super(props);
    this.state = {text: 'Hello #World'};
  }
  render() {

    //define delimiter
    let delimiter = /\s+/;

    //split string
    let _text = this.state.text;
    let token, index, parts = [];
    while (_text) {
      delimiter.lastIndex = 0;
      token = delimiter.exec(_text);
      if (token === null) {
        break;
      }
      index = token.index;
      if (token[0].length === 0) {
        index = 1;
      }
      parts.push(_text.substr(0, index));
      parts.push(token[0]);
      index = index + token[0].length;
      _text = _text.slice(index);
    }
    parts.push(_text);

    //highlight hashtags
    parts = parts.map((text) => {
          if (/^#/.test(text)) {
      return <Text key={text} style={styles.hashtag}>{text}</Text>;
  } else {
      return text;
}
});

return (
    <View>
    <TextInput
      multiline={true}
      style={styles.multiline}
      onChangeText={(text) => {
        this.setState({text});
      }}/>
      <Text>{parts}</Text>
      </View>
);
}
}
type SelectionExampleState = {
  selection: {
    start: number;
end: number;
};
value: string;
};

class SelectionExample extends React.Component {
  state: SelectionExampleState;

  _textInput: any;

  constructor(props) {
    super(props);
    this.state = {
      selection: {start: 0, end: 0},
      value: props.value
    };
  }

  onSelectionChange({nativeEvent: {selection}}) {
    this.setState({selection});
  }

  getRandomPosition() {
    var length = this.state.value.length;
    return Math.round(Math.random() * length);
  }

  select(start, end) {
    this._textInput.focus();
    this.setState({selection: {start, end}});
  }

  selectRandom() {
    var positions = [this.getRandomPosition(), this.getRandomPosition()].sort();
    this.select(...positions);
  }

  placeAt(position) {
    this.select(position, position);
  }

  placeAtRandom() {
    this.placeAt(this.getRandomPosition());
  }

  render() {
    var length = this.state.value.length;

    return (
        <View>
        <TextInput
    multiline={this.props.multiline}
  onChangeText={(value) => this.setState({value})}
onSelectionChange={this.onSelectionChange.bind(this)}
ref={textInput => (this._textInput = textInput)}
selection={this.state.selection}
style={this.props.style}
value={this.state.value}
/>
<View>
<Text>
selection = {JSON.stringify(this.state.selection)}
</Text>
<Text onPress={this.placeAt.bind(this, 0)}>
Place at Start (0, 0)
</Text>
<Text onPress={this.placeAt.bind(this, length)}>
Place at End ({length}, {length})
</Text>
<Text onPress={this.placeAtRandom.bind(this)}>
Place at Random
</Text>
<Text onPress={this.select.bind(this, 0, length)}>
Select All
</Text>
<Text onPress={this.selectRandom.bind(this)}>
Select Random
</Text>
</View>
</View>
);
}
}

exports.title = '<TextInput>';
exports.description = 'Single and multi-line text inputs.';
exports.examples = [

  {
      title: 'Auto-focus',
        render: function() {
        return (
          <TextInput  autoFocus={true} style={styles.singleLine}
                      placehohder="I am the accessibility label for text input"

/>
        );
        }
  },
  {
    title: "Live Re-Write (<sp>  ->  '_')",
        render: function() {
          return <RewriteExample />;
  }
  },

{
  title: 'Auto-capitalize',
      render: function() {
  var autoCapitalizeTypes = [
    'none',
    'sentences',
    'words',
    'characters',
  ];
  var examples = autoCapitalizeTypes.map((type) => {
    return (
    <TextInput
        key={type}
          autoCapitalize={type}
          placeholder={'autoCapitalize: ' + type}
          style={styles.singleLine}
        />
        );
        });
        return <View>{examples}</View>;
        }
},

{
  title: 'Event handling',
      render: function(): React.Element { return <TextEventsExample />; },
},

{
  title: 'Colors and text inputs',
      render: function() {
        return (
      <View>
        <TextInput style={[styles.singleLine]} defaultValue="Default color text"/>
        <TextInput style={[styles.singleLine, {color: 'green'}]} defaultValue="Green Text" />
        <TextInput placeholder="Default placeholder text color" style={styles.singleLine}/>
        <TextInput placeholder="Red placeholder text color" placeholderTextColor="red" style={styles.singleLine}/>
        <TextInput placeholder="Default underline color" style={styles.singleLine}/>
        <TextInput placeholder="Blue underline color" style={styles.singleLine} underlineColorAndroid="blue" />
        <TextInput defaultValue="Same BackgroundColor as View "style={[styles.singleLine, {backgroundColor: 'rgba(100, 100, 100, 0.3)'}]}/>
        <Text style={{backgroundColor: 'rgba(100, 100, 100, 0.3)'}}>Darker backgroundColor
        </Text>

      <TextInput defaultValue="Highlight Color is red" selectionColor={'red'}style={styles.singleLine}>
      </TextInput>
  </View>
  );
}
},


{
  title: 'Text input, themes and heights',
    render: function() {
    return (
      <TextInput placeholder="If you set height, beware of padding set from themes"style={[styles.singleLineWithHeightTextInput]}
/>
);
}
},


{
  title: 'fontFamily, fontWeight and fontStyle',
    render: function() {
    return (
      <View>
      <TextInput style={[styles.singleLine, {fontFamily: 'sans-serif'}]} placeholder="Custom fonts like Sans-Serif are supported"/>
      <TextInput style={[styles.singleLine, {fontFamily: 'sans-serif', fontWeight: 'bold'}]}placeholder="Sans-Serif bold"/>
    <TextInput style={[styles.singleLine, {fontFamily: 'sans-serif', fontStyle: 'italic'}]}
placeholder="Sans-Serif italic"/>
    <TextInput style={[styles.singleLine, {fontFamily: 'serif'}]} placeholder="Serif" />
    </View>
);
}
},

{
  title: 'Passwords',
  render: function() {
  return (
      <View>
      <TextInput defaultValue="iloveturtles" secureTextEntry={true} style={styles.singleLine}/>
      <TextInput secureTextEntry={true} style={[styles.singleLine, {color: 'red'}]} placeholder="color is supported too" placeholderTextColor="red" />
    </View>
);
}
},

{
  title: 'Editable',
  render: function() {
  return (
      <TextInput
  defaultValue="Can't touch this! (>'-')> ^(' - ')^ <('-'<) (>'-')> ^(' - ')^"
  editable={false}
  style={styles.singleLine}
/>
);
}
},
{
  title: 'Multiline',
  render: function() {
  return (
      <View>
      <TextInput
          autoCorrect={true}
          placeholder="multiline, aligned top-left"
          placeholderTextColor="red"
          multiline={true}
          style={[styles.multiline, {textAlign: 'left', textAlignVertical: 'top'}]}
      />
      <TextInput
          autoCorrect={true}
          placeholder="multiline, aligned center"
          placeholderTextColor="green"
          multiline={true}
          style={[styles.multiline, {textAlign: 'center', textAlignVertical: 'center'}]}
      />
      <TextInput
          autoCorrect={true}
          multiline={true}
          style={[styles.multiline, {color: 'blue'}, {textAlign: 'right', textAlignVertical: 'bottom'}]}/>
      <Text style={styles.multiline}>multiline with children, aligned bottom-right</Text>

</View>
);
}
},


{
  title: 'Auto-expanding',
    render: function() {
  return (
      <View>
      <AutoExpandingTextInput
  placeholder="height increases with content"
  defaultValue="React Native enables you to build world-class application experiences on native platforms using a consistent developer experience based on JavaScript and React. The focus of React Native is on developer efficiency across all the platforms you care about — learn once, write anywhere. Facebook uses React Native in multiple production apps and will continue investing in React Native."
  enablesReturnKeyAutomatically={true}
  returnKeyType="done"
      />
      </View>
);
}
},

{
  title: 'Attributed text',
  render: function() {
    return <TokenizedTextExample />;
}
},

{
  title: 'Toggle Default Padding',
  render: function(): React.Element { return <ToggleDefaultPaddingExample />; },
},
{
  title: 'Text selection & cursor placement',
      render: function() {
  return (
      <View>
      <SelectionExample
          style={styles.default}
          value="text selection can be changed"
              />
      <SelectionExample
  multiline
  style={styles.multiline}
value={"multiline text selection\ncan also be changed"}
    />
    </View>
);
}
},


]


var styles = StyleSheet.create({
  multiline: {
    height: 60,
    fontSize: 16,
    padding: 4,

    marginBottom: 10,
  },
  eventLabel: {
    margin: 3,
    height: 60,
    color:'red',
  },
  singleLine: {
    fontSize: 16,
    padding: 4,
    height: 60,
    color: 'blue'
  },
  singleLineWithHeightTextInput: {
    height: 30,
  },
  hashtag: {
    color: 'blue',
    height: 60,
    fontWeight: 'bold',
  },
});