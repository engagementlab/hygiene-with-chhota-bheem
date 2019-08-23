'use strict';
/**
 * Developed by Engagement Lab, 2019
 * ==============
 * Route to retrieve data by url
 * @class api
 * @author Johnny Richardson
 *
 * ==========
 */
const keystone =  global.keystone,
    mongoose = require('mongoose'),
    Bluebird = require('bluebird');

mongoose.Promise = require('bluebird');

var buildData = (res, lang) => {

    let list = keystone.list('About').model;
    let fields = 'intro para1 para2 contact';
    
    if(lang === 'tm')
        fields = 'introTm para1Tm para2Tm contactTm';
    else if(lang === 'hi')
        fields = 'introHi para1Hi para2Hi contactHi';

    fields += ' image.public_id email videoId videoThumbnail.public_id -_id';
    
    let data = list.findOne({}, fields);

    Bluebird.props({
            jsonData: data
        })
        .then(results => {
            return res.status(200).json({
                status: 200,
                data: results.jsonData
            });
        }).catch(err => {
            console.log(err);
        })

}

/*
 * Get data
 */
exports.get = function (req, res) {

    let lang = null;
    if (req.params.lang)
        lang = req.params.lang;

    return buildData(res, lang);

}