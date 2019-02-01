'use strict';
/**
 * Hygiene with Chhota Bheem website
 * Developed by Engagement Lab, 2019
 * ==============
 * App start
 *
 * @author Johnny Richardson
 *
 * ==========
 */

// Load .env vars
if(process.env.NODE_ENV !== 'test')
	require('dotenv').load();

const bootstrap = require('el-bootstrapper'), express = require('express');

var app = express();
bootstrap.start(
	'./config.json', 
	app,
	__dirname + '/', 
	{
		'name': 'Hygiene with Chhota Bheem website CMS'
	},
	() => {
		app.listen(process.env.PORT);
	}
);