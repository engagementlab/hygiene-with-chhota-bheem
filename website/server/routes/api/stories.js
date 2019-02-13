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
    
    let list = keystone.list('Story').model;


var getAdjacent = (results, res) => {
    
    let fields = 'key name photo.public_id';

    // Get one next/prev person from selected person's sortorder
    let nextPerson = list.findOne({sortOrder: {
        $gt: results.jsonData.sortOrder
    }}, fields).limit(1);
    let prevPerson = list.findOne({sortOrder: {
        $lt: results.jsonData.sortOrder
    }}, fields).sort({sortOrder: -1}).limit(1);
    
    // Poplulate next/prev and output response
    Bluebird.props({next: nextPerson, prev: prevPerson}).then(nextPrevResults => {
        let output = Object.assign(nextPrevResults, {person: results.jsonData});
        return res.status(200).json({
            status: 200,
            data: output
        });
    }).catch(err => {
        console.log(err);
    });

};

var buildData = (storyId, res) => {

    let data = null;
    let fields = 'key name subtitle photo.public_id';

    if(storyId)
        data = list.findOne({ key: storyId }, fields + ' description.html sortOrder -_id');
    else 
        data = list.find({}, fields + ' -_id').sort([['sortOrder', 'descending']]);

    Bluebird.props({
            jsonData: data
        })
        .then(results => {
            // When retrieving one story, also get next/prev ones
            if(storyId)
                getAdjacent(results, res);
            else
            {
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
    if (req.params.id)
        id = req.params.id;

    return buildData(id, res);

}