#!/usr/bin/env node
/**
 * Copyright (c) 2015-present, Facebook, Inc.
 * All rights reserved.
 *
 * This source code is licensed under the BSD-style license found in the
 * LICENSE file in the root directory of this source tree. An additional grant
 * of patent rights can be found in the PATENTS file in the same directory.
 *
 * @flow
 * @providesModule websocket_test_server
 */
'use strict';

/* eslint-env node */

const WebSocket = require('ws');
const Cookies = require('cookies')

console.log(`\
Test server for WebSocketExample

This will send each incoming message right back to the other side.
Restart with the '--binary' command line flag to have it respond with an
ArrayBuffer instead of a string.
`);

const respondWithBinary = process.argv.indexOf('--binary') !== -1;
const server = new WebSocket.Server({port: 5555});
server.on('connection', (ws, req, res) => {
  ws.on('message', (message) => {
    console.log('Received message:', message);
    //console.log('Cookie:', ws.upgradeReq.headers.cookie);
    console.log('Cookie:', req.headers.cookie);

    if (respondWithBinary) {
      message = new Buffer(message);
    }
    ws.send(message);
  });

  console.log('Incoming connection!');
  ws.send('Why hello there!');
});

//UEP330x55013919fe4c1ad4055769adaf81bde0a181f6c4883e3155654163c0eba9e8713121931f7b3a022ed0ed69b0d0326024ab869cbb52d5c8ad22b0de285effd33196106b41882b4acd95ddfad1606cbcc41b42f7260c369954fa7f8cff66c0851535ebfff6a3217db132efa3d7c975b7e6af12efe6c287019ade874b977be7c851a6a0bbc340fd318c856262f606aec9db656b35a524a244326eb013863d755cfeff744635e003cc3f6c7b3b9073a056450efe50fd06a9a9d34005dcd9a51acc47a3a2e334a409761cd54ce5fa753af0121b9b820234e5631e683992f35cb830a647edcbbe741297e3f6fd5ba28432e2e6c7b007645b1e8fc52c1ba3b4e8564c9bAAACAg==:UEP