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
 * @providesModule http_test_server
 */
'use strict';

/* eslint-env node */

console.log(`\
Test server for WebSocketExample

This will set a cookie named "wstest" on the response of any incoming request.
`);

const connect = require('connect')
const http = require('http')
var Cookies = require('cookies')

const app = connect();

app.use(function(req, res) {
  console.log('received request')
  const cookieOptions = {
    //httpOnly: true, // the cookie is not accessible by the user (javascript,...)
    secure: false, // allow HTTP
  };
  
  //res.cookie('wstest', 'OK', cookieOptions)
  var cookies = new Cookies( req, res)
  cookies.set( "wstest", "OK", cookieOptions) // set a cookie 
  
  res.end('Cookie has been set!\n')
})

http.createServer(app).listen(5556)

//UEP330x3dbf95fa1febe8253e2849d82cebcb8573a9d2222e4ee3a1c95ffcdeed9f98007f5aa0fda26269f867a7185e3f35eb88b19911ac0252bf647a46586b7c253a655bbc37eb404b6f1f1990a60f375b21bfabe05e145c9036e5388dfe2757c873952fda6dbf3ab6b0f891260bb1410678866fb00a85707b5ca18a6bf47a893c295a32ca5c1f79368f7b04cfa2308fffb071a7bd6d313c085910032af2e30aa20f0e3e55429aa6ea9ce9c82f3e8033f58ab4fae7772abfb84cf4c08a5f4436121942f7434315d64a87779d6e2b0bbb634c20095673576ebae377efe1022e583824293d0e6bc9102bfe53c73fab83141e17b4c5c17341e827e2230e49c8948abbbab3AAACAg==:UEP