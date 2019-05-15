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
const keystone = global.keystone,
    mongoose = require('mongoose'),
    Bluebird = require('bluebird');
mongoose.Promise = require('bluebird');

let list = keystone.list('Story').model;


var getAdjacent = (results, res, lang) => {

    let fields = 'key photo.public_id ';

    if (lang === 'en')
        fields += 'name';
    else if (lang === 'tm')
        fields += 'nameTm';
    else if (lang === 'hi')
        fields += 'nameHi';

    // Get one next/prev person from selected person's sortorder
    let nextPerson = list.findOne({
        sortOrder: {
            $gt: results.jsonData.sortOrder
        }
    }, fields).limit(1);
    let prevPerson = list.findOne({
        sortOrder: {
            $lt: results.jsonData.sortOrder
        }
    }, fields).sort({
        sortOrder: -1
    }).limit(1);

    // Poplulate next/prev and output response
    Bluebird.props({
        next: nextPerson,
        prev: prevPerson
    }).then(nextPrevResults => {
        let output = Object.assign(nextPrevResults, {
            person: results.jsonData
        });
        return res.status(200).json({
            status: 200,
            data: output
        });
    }).catch(err => {
        console.log(err);
    });

};

var buildData = (storyId, res, lang) => {

    let data = null;
    let fields = 'key photo.public_id ';

    if (lang === 'en')
        fields += 'name subtitle';
    else if (lang === 'tm')
        fields += 'nameTm subtitleTm';
    else if (lang === 'hi')
        fields += 'nameHi subtitleHi';

    if (storyId) {
        let subFields = ' description.html ';
        if (lang === 'tm')
            subFields = ' descriptionTm.html ';
        else if (lang === 'hi')
            subFields = ' descriptionHi.html ';


        data = list.findOne({
            key: storyId
        }, fields + subFields + 'sortOrder -_id');
    } else
        data = list.find({}, fields + ' -_id').sort([
            ['sortOrder', 'descending']
        ]);

    Bluebird.props({
            jsonData: data
        })
        .then(results => {
            // When retrieving one story, also get next/prev ones
            if (storyId)
                getAdjacent(results, res, lang);
            else {
                return res.status(200).json({
                    status: 200,
                    data: results.jsonData
                });
            }
        }).catch(err => {
            console.log(err);
        })

}

/*
 * Get data
 */
exports.get = function (req, res) {

    let id = null;
    if (req.query.id)
        id = req.query.id;

    let lang = null;
    if (req.params.lang)
        lang = req.params.lang;

    return buildData(id, res, lang);

}