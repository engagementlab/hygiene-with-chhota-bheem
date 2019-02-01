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

var buildData = (options, res) => {

    let list = keystone.list('Project').model;
    let data;

    if (options.id !== undefined)
        data = list.findOne({
            key: options.id
        });
    else if (options.limit)
        data = list.find({}, ).sort([
            ['sortOrder', 'ascending']
        ]);
    else
        data = list.find({});

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

    let options = {};
    if (req.params.id)
        options.id = req.params.id;

    return buildData(options, res);

}