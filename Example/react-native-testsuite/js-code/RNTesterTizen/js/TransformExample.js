/**
 * Created by ehj on 2017/6/26.
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule TransformExample
 */

'use strict';

var React = require('react');
var ReactNative = require('react-native');
var {
    Animated,
    StyleSheet,
    Text,
    View,
    } = ReactNative;


exports.title = 'Transforms';
exports.description = 'View transforms';

class Flip extends React.Component {
    state = {
        theta: new Animated.Value(45),
        loading:false
    };
    
    componentWillUnmount(){
     this.setState({
         loading: false,
     })
    }

    componentDidMount() {
        if(this.state.loading==true){
             
            this._animate();
            this.setState({
                loading: false,
        })
        }
       
    }
   
    componentWillMount(){
        this.setState(
            {
                loading : true
            }
        )
        
    }
 
    _animate = () => {
        this.state.theta.setValue(0);
        Animated.timing(this.state.theta, {
            toValue: 360,
            duration: 5000,
        }).start(this.state.loading && this._animate);
};
render() {
   
    return (
        <View style={styles.flipCardContainer}>
            <Animated.View style={[styles.flipCard,
            {transform: [
                {perspective: 850},
                {rotateX: this.state.theta.interpolate({
                    inputRange: [0, 180],
                    outputRange: ['0deg', '180deg']
                })},
            ]}]}>
            <Text style={styles.flipText}>
            This text is flipping great.
            </Text>
        </Animated.View>
        <Animated.View style={[styles.flipCard, {
            position: 'absolute',
            top: 0,
            backgroundColor: 'red',
            transform: [
                {perspective: 850},
                {rotateX: this.state.theta.interpolate({
                    inputRange: [0, 180],
                    outputRange: ['180deg', '360deg']
                })},
            ]}]}>
            <Text style={styles.flipText}>
            On the flip side...
            </Text>
        </Animated.View>
</View>
);
}
}

var styles = StyleSheet.create({
    container: {
        height: 500,
    },
    box1: {

        backgroundColor: 'green',
        height: 50,
        left: 0,
        top: 0,
        transform: [
            {translateX: 100},
            {translateY: 50},
            {rotate: '30deg'},
            {scaleX: 2},
            {scaleY: 2},
        ],
        width: 50,
    },
    box2: {
        left: 0,
        backgroundColor: 'purple',
        height: 50,
        position: 'absolute',
        top: 0,
        transform: [
            {scaleX: 2},
            {scaleY: 2},
            {translateX: 100},
            {translateY: 50},
            {rotate: '30deg'},
        ],
        width: 50,
    },
    box3step1: {
        left: 0,
        backgroundColor: 'lightpink',
        height: 50,
        position: 'absolute',
        top: 0,
        transform: [
            {rotate: '30deg'},
        ],
        width: 50,
    },
    box3step2: {
        left: 0,
        backgroundColor: 'hotpink',
        height: 50,
        opacity: 0.5,
        position: 'absolute',
        top: 0,
        transform: [
            {rotate: '30deg'},
            {scaleX: 2},
            {scaleY: 2},
        ],
        width: 50,
    },
    box3step3: {
        left: 0,
        backgroundColor: 'deeppink',
        height: 50,
        opacity: 0.5,
        position: 'absolute',
        top: 0,
        transform: [
            {rotate: '30deg'},
            {scaleX: 2},
            {scaleY: 2},
            {translateX: 100},
            {translateY: 50},
        ],
        width: 50,
    },
    box4: {
        left: 0,
        backgroundColor: 'darkorange',
        height: 50,
        position: 'absolute',
        top: 0,
        transform: [
            {translate: [200, 350]},
            {scale: 2.5},
            {rotate: '-0.2rad'},
        ],
        width: 100,
    },
    box5: {
        backgroundColor: 'maroon',
        height: 50,
        position: 'absolute',
        right: 0,
        top: 0,
        width: 50,
    },
    box5Transform: {
        transform: [
            {translate: [-50, 35]},
            {rotate: '50deg'},
            {scale: 2},
        ],
    },
    flipCardContainer: {
        marginVertical: 40,
        flex: 1,
        alignSelf: 'center',
    },
    flipCard: {
        width: 400,
        height: 200,
        alignItems: 'center',
        justifyContent: 'center',
        backgroundColor: 'blue',
        backfaceVisibility: 'hidden',
    },
    flipText: {
        width: 200,
        fontSize: 20,
        color: 'white',
        fontWeight: 'bold',
    }
});
exports.examples = [

    {
    title: 'Perspective, Rotate, Animation',
        description: 'perspective: 850, rotateX: Animated.timing(0 -> 360)',
        render(): React.Element<any> { return <Flip />; }
    },
    {
        title: 'Translate, Rotate, Scale',
            description: "translateX: 100, translateY: 50, rotate: '30deg', scaleX: 2, scaleY: 2",
        render() {
            return (
                <View style={styles.container}>
                    <Animated.View style={styles.box1} />
                </View>
                );
                }
    },
];