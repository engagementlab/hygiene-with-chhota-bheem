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
    Bluebird = require('bluebird'),
    _ = require('underscore');

mongoose.Promise = require('bluebird');

var buildData = (res) => {

    let list = keystone.list('Files').model;
    let allFields = [];
    let fields = ['guide', 'zip', 'storybook1', 'storybook2', 'storybook3', 'storybook4', 'poster1', 'poster2'];
    let imgFields = ['guideImg', 'storybook1Img', 'storybook2Img', 'storybook3Img', 'storybook4Img', 'poster1Img', 'poster2Img'];

    _.each(fields, (f) => {
       allFields.push(f + 'En.url');
       allFields.push(f + 'Tm.url');
       allFields.push(f + 'Hi.url');
    });
    _.each(imgFields, (f) => {
       allFields.push(f + 'En.public_id');
       allFields.push(f + 'Tm.public_id');
       allFields.push(f + 'Hi.public_id');
    });
        
    let data = list.findOne({}, allFields);

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