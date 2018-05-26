// @flow

import fse from 'fs-extra';
import path from 'path';
import minimist from 'minimist';
import { exec, execSync } from 'child_process';

import { preBuild } from './prebuild';

const argv = minimist(process.argv.slice(2));

const packager = async() => {

    let command = argv._[0];
    console.log(`command:${command}`);

    let app = await preBuild();
    const appPath = app.path;
    const packageDir = path.normalize(`${appPath}/Tizen/shared/res`);
    console.log(`[packager] appPath: ${appPath}`);

    //bundle: '        --dev false'
    const RN = path.normalize('/node_modules/react-native/packager/');
    replaceTizen(path.normalize(`${appPath}${RN}defaults.js`), /windows/g, 'tizen');
    replaceTizen(path.normalize(`${appPath}${RN}src/node-haste/lib/getPlatformExtension.js`), /web/g, 'tizen');

    replaceTizen(path.normalize(`${appPath}${RN}defaults.js`), /react-native-tizen/g, 'react-native-tizen-dotnet');

    function replaceTizen(file, reg, key) {
        let data = fse.readFileSync(file, 'utf8');
        let result = data.replace(reg, key);
        fse.writeFileSync(file, result, 'utf8');
    }

    function checkCommand(cmd) {
        if (!cmd) {
            return false;
        }
        if (cmd.toLowerCase() === 'dev') {
            return true;
        }
        return false;
    }

    //make bundle comand
    const SPACE = ' ';
    let arg1 = 'node' + SPACE + path.normalize(`${appPath}/node_modules/react-native/local-cli/cli.js`) + SPACE + 'bundle --entry-file index.tizen.js';
    let arg2 = ' --bundle-output' + SPACE + path.normalize(`${packageDir}/index.tizen.bundle`);
    let arg3 = ' --platform tizen --assets-dest' + SPACE + path.normalize(`${packageDir}/assets/`);
    let arg4 = ' --dev ' + checkCommand(command);

    execSync(arg1 + arg2 + arg3 + arg4, { stdio: [0, 1, 2] });

};
packager();

module.exports = packager;