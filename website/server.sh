#!/bin/bash

source ~/.nvm/nvm.sh;
cd server;
nvm use;
nodemon app.js;