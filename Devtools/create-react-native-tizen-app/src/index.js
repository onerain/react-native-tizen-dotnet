#!/usr/bin/env node

// @flow

import fse from 'fs-extra';
import chalk from 'chalk';
import minimist from 'minimist';
import path from 'path';
import pathExists from 'path-exists';
import semver from 'semver';
import spawn from 'cross-spawn';

const argv = minimist(process.argv.slice(2));

/**
 * Arguments:
 *   --version - to print current version
 *   --verbose - to print npm logs during init
 *   --scripts -version <alternative package>
 */
const commands = argv._;
const cwd = process.cwd();


if (commands.length === 0) {
    if (argv.version) {
        const version = require('../package.json').version;
        console.log(`create-react-native-tizen-app version: ${version}`);
        process.exit();
    }
    console.error('Usage: create-react-native-tizen-app <project-directory> [--verbose]');
    process.exit(1);
}

createApp(commands[0], !!argv.verbose, argv['scripts-version']).then(() => {});

// use yarn if it's available, otherwise use npm
function shouldUseYarn() {
    try {
        const result = spawn.sync('yarnpkg', ['--version'], { stdio: 'ignore' });
        if (result.error || result.status !== 0) {
            return false;
        }
        return true;
    } catch (e) {
        return false;
    }
}

async function createApp(name: string, verbose: boolean, version: ? string): Promise < void > {
    const root = path.resolve(name);
    const appName = path.basename(root);

    const packageToInstall = getInstallPackage(version);
    const packageName = getPackageName(packageToInstall);
    checkAppName(appName, packageName);

    if (!await pathExists(name)) {
        await fse.mkdir(root);
    } else if (!await isSafeToCreateProjectIn(root)) {
        console.log(`The directory \`${name}\` conatins file(s) that could conflict. Aborting. `);
        process.exit(1);
    }

    console.log(`Creating a new React Native Tizen dotnet App in ${root}. `);
    console.log();

    const packageJson = {
        name: appName,
        version: '0.0.1',
        private: true,
    };
    await fse.writeFile(path.join(root, 'package.json'), JSON.stringify(packageJson, null, 2));
    process.chdir(root);

    console.log('Installing packages. This might take a couple minutes.');
    console.log('Installing react-native-tizen-scripts...');
    console.log();

    await run(root, appName, version, verbose, packageToInstall, packageName);
}

function install(
    packageToInstall: string,
    verbose: boolean,
    callback: (code: number, command: string, args: Array < string > ) => Promise < void >
): void {
    const useYarn = shouldUseYarn();
    let args, cmd, result;
    //for local test
    //packageToInstall = 'file:/Users/admin/Documents/reactDotNative/Github/rn-tv-netcore-app/tv-react-native-scripts/';

    if (useYarn) {
        cmd = 'yarnpkg';
        args = ['add'];

        if (verbose) {
            args.push('--verbose');
        }

        args = args.concat(['--dev', '--exact', '--ignore-optional', packageToInstall]);
        result = spawn.sync(cmd, args, { stdio: 'inherit' });

    } else {
        args = ['install'];

        if (verbose) {
            args.push('--verbose');
        }
        cmd = 'npm';
        args = args.concat(['--save-dev', '--save-exact', packageToInstall]);
        result = spawn.sync(cmd, args, { stdio: 'inherit' });
    }

    callback(result.status, cmd, args).then(
        () => {},
        e => {
            throw e;
        }
    );
}

async function run(
    root: string,
    appName: string,
    version: ? string,
    verbose : boolean,
    packageToInstall: string,
    packageName: string
): Promise < void > {
    install(packageToInstall, verbose, async(code: number, command: string, args: Array < string > ) => {
        if (code !== 0) {
            console.error(`\`${command} ${args.join(' ')}\` failed`);
            return;
        }

        await checkNodeVersion(packageName);

        const scriptsPath = path.resolve(
            process.cwd(),
            'node_modules',
            packageName,
            'build',
            'init.js'
        );

        const init = require(scriptsPath);
        await init(root, appName, verbose, cwd);
    });
}

function getInstallPackage(version: ? string): string {
    let packageToInstall = 'react-native-tizen-scripts';

    const validSemver = semver.valid(version);
    if (validSemver) {
        packageToInstall += '@' + validSemver;
    } else if (version) {
        packageToInstall = version;
    }
    return packageToInstall;
}

function getPackageName(installPackage: ? string): string {
    if (installPackage.indexOf('.taz') > -1) {
        //e.g. react-scripts-0.2.0-alpha.1.tgz
        const matches = installPackage.match(/^.+[\/\\](.+?)(?:-d+.+)?\.tgz$/);
        if (matches && matches.length >= 2) {
            return matches[1];
        } else {
            throw new Error(
                `Provided scripts package (${installPackage}) doesn't have a valid filename.`
            );
        }
    } else if (installPackage.indexOf('@') > 0) {
        return installPackage.charAt(0) + installPackage.substr(1).split('@')[0];
    }
    return installPackage;
}

async function checkNodeVersion(packageName: string): Promise < void > {
    const packageJsonPath = path.resolve(process.cwd(), 'node_modules', packageName, 'package.json');

    const packageJon = JSON.parse(await fse.readFile(packageJsonPath));
    if (!packageJon.engines || !packageJon.engines.node) {
        return;
    }

    if (!semver.satisfies(process.version, packageJon.engines.node)) {
        console.error(
            chalk.red(
                'You are currently running Node %s but create-react-native-tizen-app %s.' +
                ' Please use a supported version of Node.\n'
            ),
            process.version,
            packageJon.engines.node
        );
        process.exit(1);
    }
}

function checkAppName(appName: string, packageName: string): void {
    const allDependencies = [
        'react-native-tizen-scripts',
        'react-native-tizen-dotnet',
        'tizen-tv-dev-cli',
        'react',
        'react-native',
    ];

    if (allDependencies.indexOf(appName) >= 0) {
        console.error(
            chalk.red(
                'We cannot create a project called `' +
                appName +
                '` because a dependency with the same name exitst. \n' +
                'Due to the way npm works, the following names are not allowed:\n\n'
            ) +
            chalk.cyan(
                allDependencies
                .map(depName => {
                    return '  ' + depName;
                })
                .join('\n')
            ) +
            chalk.red('\n\nPlease choose a different project name.')
        );
        process.exit(1);
    }
}

async function isSafeToCreateProjectIn(root: string): Promise < boolean > {
    const validFiles = ['.DS_Store', 'Thumbs.db', '.git', '.gitignore', 'README.md', 'LICENSE'];
    return (await fse.readdir(root)).every(file => {
        return validFiles.indexOf(file) >= 0;
    });
}