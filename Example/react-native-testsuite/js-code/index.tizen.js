/**
 * Sample React Native App
 * https://github.com/facebook/react-native
 */

import React, { Component } from 'react';
import {
  AppRegistry,
  StyleSheet,
  Button,
  View,
  Text,
  ListView,
  TouchableHighlight,
  ScrollView
} from 'react-native';

var ExamplesList = require('./RNTesterTizen/js/ExamplesList');
var RNTesterExampleContainer = require('./RNTesterTizen/js/RNTesterExampleContainer');

export default class hello_tizen extends Component {
  constructor(props) {
    super(props);

    this.state = {
      currentExample: 'Button',
      //isDetailShow: true
    };
    this.menu = this.menu.bind(this);
  }
  render() {
    const exampleModule = ExamplesList.Modules[this.state.currentExample];
    return (
      <View style={styles.container}>
        <View style={styles.left}>
          <View style={styles.tag}>
            <Text style={styles.logo}>React Native Tizen</Text>
          </View>
          <View style={styles.tag}>
            <Text style={styles.tagText}>Components</Text>
          </View>
          <View style={styles.menuList}>
            {ExamplesList.ComponentExamples.map((menuData) => this.menu(menuData))}
          </View>
          <View style={styles.tag}>
            <Text style={styles.tagText}>APIs</Text>
          </View>
          <View style={styles.menuList}>
            {ExamplesList.ApiExamples.map((menuData) => this.menu(menuData))}
          </View>
        </View>
        <View style={styles.detail}>
          {/*this.state.isDetailShow === true ? <RNTesterExampleContainer title={exampleModule.title} description={exampleModule.description} module={exampleModule} /> : null*/}
          <RNTesterExampleContainer title={exampleModule.title} description={exampleModule.description} module={exampleModule} />
        </View>
      </View>
    )
  }
  componentDidUpdate(){
    /*this.state.isDetailShow === false && this.setState({
      isDetailShow: true
    })*/
  }
  menu(menuData) {
    return (
      <View style={{paddingTop: 1}} key={menuData.key}>
        <TouchableHighlight underlayColor="#ffd700" style={styles.menu}  onPress={() => {
          this.setState({
            currentExample: menuData.key,
            //isDetailShow: false
          });
        } }>
          <Text style={[styles.menuSelected]}>{menuData.key}</Text>
        </TouchableHighlight>
      </View>)
  }
}

const styles = StyleSheet.create({
  container: {
    position: 'absolute',
    left:0,
    top:0,
    width: 1920,
    height: 1080,
  },
  left: {
    position: 'absolute',
    left:0,
    top: 0,
    width: 300,
    height: 1080,
    flex: 1,
    backgroundColor: 'black',
  },
  tag:{
    width: 300,
    height: 26,
    paddingLeft: 20,
  },
  tagText:{
    color: 'white',
    fontSize: 20,
  },
  logo:{
    color: 'white',
    fontWeight:'bold',
  },
  menuList: {
    width: 300,
    backgroundColor: 'black',
  },
  menu: {
    width: 300,
    height: 33,
    paddingLeft: 40,
  },
  menuSelected:{
    backgroundColor: '#ffd700',
    color:'black',
    fontSize: 26,
  },
  detail: {
    position: 'absolute',
    top:0,
    left:300,
    width: 1620,
    height: 1080,
  },
});

AppRegistry.registerComponent('react_native_testsuite.tizen', () => hello_tizen);
