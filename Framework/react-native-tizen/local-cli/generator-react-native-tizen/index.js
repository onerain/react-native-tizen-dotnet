/**
 * Copyright (c) 2017 Samsung Electronics Co., Ltd All Rights Reserved 
 * PROPRIETARY/CONFIDENTIAL
 *  
 * This software is the confidential and proprietary information of SAMSUNG 
 * ELECTRONICS ("Confidential Information"). You shall not disclose such 
 * Confidential Information and shall use it only in accordance with the terms of 
 * the license agreement you entered into with SAMSUNG ELECTRONICS. SAMSUNG make 
 * no representations or warranties about the suitability of the software, either 
 * express or implied, including but not limited to the implied warranties of 
 * merchantability, fitness for a particular purpose, or non-infringement. 
 * SAMSUNG shall not be liable for any damages suffered by licensee as a result 
 * of using, modifying or distributing this software or its derivatives.
 */
'use strict';

const Generator = require('yeoman-generator');
const fs = require('fs');

const pkg = require('../../../../package.json');

module.exports = class extends Generator {   

    writing() {
        
        /*pkg.scripts = 'test';
        fs.writeFile('../../../../package.json', JSON.stringify(pkg), function(err) {
            console.log(err);
        });*/

    	this.fs.copyTpl(
    		this.templatePath('index.tizen.js'),
    		this.destinationPath('index.tizen.js')
    		//{ title: 'Templating with fzhao generator'}
    	);

        this.fs.copy(
            this.templatePath('defaults.js'),
    		this.destinationPath('node_modules/react-native/packager/defaults.js')
        );

        this.fs.copy(
            this.templatePath('getPlatformExtension.js'),
    		this.destinationPath('node_modules/react-native/packager/src/node-haste/lib/getPlatformExtension.js')
        );
    }

};