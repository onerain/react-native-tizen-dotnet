# RN-TIZEN-NETCORE-APP

Create Tizen NetCore App with React-Native :)

## Usage
-   $ ```sudo npm i -g create-react-native-tizen-app```
-   $ ```create-create-native-tizen-app myTizenApp```
-   $ ```cd myTizenApp```
-   $ ```vim package.json``` //change "tvip": "192.168.100.1" to your Tv IP
-   $ ```yarn bundle``` // for release mode
-   $ ```yarn bundle --dev``` // for dev mode, js file not ugly
-   $ ```yarn package``` // packaging tpk for Tizen
-   $ ```yarn launch``` // launch tpk to Tizen TV , Before launch you need run shell on tizen board

````shell
sdb root on 
sdb shell  

vconftool set -t int db/sdk/develop/mode 1 -f 
vconftool set -t string db/sdk/develop/ip 192.168.120.100(your dev ip) -f 
````

## Command-line
-   ```yarn server``` // start react-native debug server //
-   ```yarn bundle``` -- dev // bundle js by babel
-   ```yarn bundle```  // bundle js with ugly
-   ```yarn dotnet -- debug``` // build project with debug mode
-   ```yarn dotnet -- release``` //build project with release 