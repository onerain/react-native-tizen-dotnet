// @flow

import chalk from 'chalk';
import fse from 'fs-extra';
import path from 'path';
import pathExists from 'path-exists';
import spawn from 'cross-spawn';
import log from './util/log';

const DEFAULT_DEPENDENCIES = {
    react: '15.4.2',
    'react-native': '0.42.3',
    'react-native-tizen-dotnet': '>=0.1.1',
    //for debug
    //'react-native-tizen': 'file:/Users/admin/Documents/reactDotNative/Github/rn-tizen-netcore-app/react-native-tizen/',
};

const DEFAULT_DEV_DEPENDENCIES = {
    'babel-jest': '20.0.3',
    'babel-preset-react-native': '2.1.0',
    'jest': '20.0.4',
    'react-test-renderer': '15.4.2'
};

module.exports = async(appPath: string, appName: string, verbose: boolean, cwd: string = ' ') => {
        const ownPackageName: string = require('../package.json').name;
        const ownPath: string = path.join(appPath, 'node_modules', ownPackageName);
        const useYarn: boolean = await pathExists(path.join(appPath, 'yarn.lock'));

        if (!useYarn) {
            let npmVersion = spawn.sync('npm', ['--version']).stdout.toString().trim();

            if (npmVersion.startsWith('5')) {
                console.log(chalk.yellow(`
*******************************************************************************
ERROR: npm 5 is not supported yet
*******************************************************************************

It looks like you're using npm 5 which was recently released.

Create React Native App doesn't work with npm 5 yet, unfortunately. We
recommend using npm 4 or yarn until some bugs are resolved.

You can follow the known issues with npm 5 at:
https://github.com/npm/npm/issues/16991

*******************************************************************************
`));
                process.exit(1);
            }
        }

        const readmeExists: boolean = await pathExists(path.join(appPath, 'README.md'));
        if (readmeExists) {
            await fse.name(path.join(appPath, 'README.md'), path.join(appPath, 'README.md.bak'));
        }

        const appPackagePath: string = path.join(appPath, 'package.json');
        const appPackage = JSON.parse(await fse.readFile(appPackagePath));

        appPackage.main = 'index.js';
        appPackage.scripts = {
            server: 'node node_modules/react-native/local-cli/cli.js start',
            test: 'node node_modules/jest/bin/jest.js --watch',
            package: 'react-native-tizen package $npm_package_config_mode',
            bundle: 'react-native-tizen bundle',
            //dotnet: 'react-native-tizen dotnet',
            launch: 'react-native-tizen-dotnet launch $npm_package_config_tvip $npm_package_config_mode'
                //packageDebug:'yarn bundle -- dev; yarn dotnet -- debug; yarn package',
                //packageRelease: 'yarn bundle; yarn dotnet -- release; yarn package',
        };

        appPackage.jest = {};
        appPackage.config = {
            "tvip": "192.168.100.1",
            "mode": "Release"
        };

        if (!appPackage.dependencies) {
            appPackage.dependencies = {};
        }

        if (!appPackage.devDependencies) {
            appPackage.devDependencies = {};
        }

        Object.assign(appPackage.dependencies, DEFAULT_DEPENDENCIES);
        Object.assign(appPackage.devDependencies, DEFAULT_DEV_DEPENDENCIES);

        //npm install
        await fse.writeFile(appPackagePath, JSON.stringify(appPackage, null, 2));

        //template
        await fse.copy(path.join(ownPath, 'template'), appPath);

        //gitignore
        try {
            await fse.rename(path.join(appPath, 'gitignore'), path.join(appPath, '.gitignore'));
        } catch (err) {
            if (err.code === 'EEXIST') {
                const data = await fse.readFile(path.join(appPath, 'gitignore'));
                await fse.appendFile(path.join(appPath, '.gitignore'), data);
                await fse.unlink(path.join(appPath, 'gitignore'));
            } else {
                throw err;
            }
        }

        //run yarn or npm
        let command = '';
        let args = [];

        if (useYarn) {
            command = 'yarnpkg';
        } else {
            command = 'npm';
            args = ['install', '--save'];

            if (verbose) {
                args.push('--verbose');
            }
        }

        log(`Installing dependencies using ${command}...`);
        log();

        if (command === 'yarnpkg') {
            command = 'yarn';
        }

        const proc = spawn(command, args, { stdio: 'inherit' });
        proc.on('close', code => {
                    if (code !== 0) {
                        console.error(`\`${command} ${args.join(' ')}\` failed`);
                        return;
                    }

                    let cdpath;
                    if (path.resolve(cwd, appName) === appPath) {
                        cdpath = appName;
                    } else {
                        cdpath = appPath;
                    }

                    log(
                        `
            
Success! Created ${appName} at ${appPath}
            `
                    );

                    if (readmeExists) {
                        log(
                                `
                
${chalk.yellow('You had a `README.md` file, we renamed it to `README.old.md`')}`
            );
        }

        log('Start Prepare Tizen dotnet Project....');

        let tempArray = [
            appPath + '/Tizen/tizen-manifest.xml',
            appPath + '/index.tizen.js',
            appPath + '/Tizen/Program.cs'
        ];
        tempArray.forEach(file => {
            replaceName(file, /react_native_template/g, appName);
        });

        //Change template Name to App Name
        function replaceName(file, src, dst) {
            var data = fse.readFileSync(file, 'utf8');
            let result = data.replace(src, dst);
            //create hash code to tizen app id;
            let hashVal = result.replace(/hash_val/g, randomCode());
            fse.writeFileSync(file, hashVal, 'utf8');
        }
        
        //return 
        function randomCode(){  
            var str = "", 
                arr = ['0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'];  
           
            // generate 8 random numbers  
            for(var i=0; i<8; i++){  
                let pos = Math.round(Math.random() * (arr.length-1));  
                str += arr[pos];  
            }  
            return str;  
        } 

        //rename csproj file
        let csFile = appPath + '/Tizen/react_native_template.csproj';
        let newName = appPath + '/Tizen/' + appName + '.csproj';
        fse.rename(csFile, newName, err => {
            if (err) throw err;
        });

        log();
        log('Happy on Tizen dotnet!');

    });


}