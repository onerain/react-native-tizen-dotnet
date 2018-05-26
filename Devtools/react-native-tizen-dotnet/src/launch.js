// @flow
import minimist from 'minimist';
import { launchTarget } from 'tv-dev-cli-sdk';
import path from 'path';

const argv = minimist(process.argv.slice(2));

function targetIP() {
    return argv._[0];
};

function tpkPath(flag) {
    let mode;
    if (!flag || flag.toLowerCase() === 'release') {
        mode = 'Release';
    } else {
        mode = 'Debug';
    }
    console.log(`path:${mode}`);
    return path.normalize(`/Tizen/bin/${mode}/netcoreapp2.0/`);
}
//console.log(`command:${command}`);

launchTarget.handleCommand(targetIP(), tpkPath(argv._[1]));