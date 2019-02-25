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

    let fields = '';
    fields += (lang === 'en') ? 'name summary pdf.url poster.url': 'nameTm summaryTm pdfTm.url posterTm.url';
    fields += ' video1Url video2Url videoThumbnailImages.public_id';

    let fileFields = (lang === 'en') ? 'storybookEn.url guideEn.url' : 'storybookTm.url guideTm.url';

    let list = keystone.list('Module').model;
    let listFiles = keystone.list('Files').model;
    let data = list.find({}, fields);
    let dataFiles = listFiles.findOne({}, fileFields);
    
    Bluebird.props({
            content: data,
            files: dataFiles
        })
        .then(results => {
            return res.status(200).json({
                status: 200,
                data: {
                    content: results.content,
                    files: results.files
                }
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