'use strict';
/**
 * Hygiene with Chhota Bheem website
 * Developed by Engagement Lab, 2019
 * ==============
 * App utilities
 *
 * @author Johnny Richardson
 *
 * ==========
 */
// TODO: Make npm package
var validator = require('validator');
var urlValidator = {
    validator: function(val) {
        return !val || validator.isURL(val, {
            protocols: ['http', 'https'],
            require_tld: true,
            require_protocol: false,
            allow_underscores: true
        });
    },
    msg: 'Invalid link URL (e.g. needs http:// and .abc/)'
};

exports.url = urlValidator;