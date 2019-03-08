// @flow
import fse from 'fs-extra';
import spawn from 'cross-spawn';
import crypto from 'crypto';
import minimist from 'minimist';
import path from 'path';
import { exec, execSync } from 'child_process';

import { preBuild } from './prebuild';

const argv = minimist(process.argv.slice(2));

async function packagerBuild() {

    let command = argv._[0];
    console.log(`command:${command}`);

    let app = await preBuild();
    const appPath = app.path;
    const appName = app.name;
    console.log(`[packagerBuild] appPath: ${appPath}`);

    //check tizen.dll hash
    checkHash(fse.readFileSync(path.normalize(`${appPath}/Tizen/ReactNativeTizen.dll`)));

    function checkHash(data) {
        let sha1 = crypto.createHash('sha1');
        sha1.update(data);
        let hash = sha1.digest('hex');
        console.log(`local ReactNativeTizen.dll sha1 hash check: ${hash}`);
        console.log(`The latest version of ReactNativeTizen.dll is e295ff62c9aa2bd0cc8aca188e206e3b2a82f985`);
    }

    function checkCommand(cmd) {
        if (!cmd) {
            return false;
        }
        if (cmd.toLowerCase() === 'release') {
            return true;
        }
        return false;
    }

    let mode = checkCommand(command) ? 'Release' : 'Debug';

    //dotnet build
    const SPACE = ' ';
    execSync('dotnet restore ' + SPACE + path.normalize(`${appPath}/Tizen/`), { stdio: [0, 1, 2] });

    execSync('dotnet build -c ' + mode + SPACE + path.normalize(`${appPath}/Tizen/`), { stdio: [0, 1, 2] });

};
packagerBuild();