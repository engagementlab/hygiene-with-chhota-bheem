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

var buildData = (res, lang) => {

    let list = keystone.list('Workshops').model;
    let fields = ['intro', 'intro2', 'oneDayIntro', 'oneDayObj', 'oneDayfacGuide', 'fourDayIntro', 'fourDayObj', 'fourDayfacGuide', 'story1', 'story2'];
    
    fields = _.map(fields, (f) => {
       return f += lang.slice(0, 1).toUpperCase() + lang.slice(1);
    });
    fields[4] += '.url';
    fields[7] += '.url';
    fields[8] += '.html';
    fields[9] += '.html';
    
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