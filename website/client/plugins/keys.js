const {
    registerPlugin
} = require('@scullyio/scully');
const axios = require('axios')
const _ = require('underscore');

const stories = async (route, config) => {
    const routes = [];
    const keys = 
[
    'whatsapp-group',
    'kamakshi',
    'students-from-adwps',
    'panchayat-union-primary-school',
    'adidravidar-welfare-primary-school',
    'j-revathi',
    'j-selvarani',
    'santhoshini',
    'femi',
    'suman'
  ];
  keys.forEach((res) => {
        routes.push({
            route: `/stories/${res}`
        });
    });
    return Promise.resolve(routes);
};

const validator = async (config) => [];
registerPlugin('router', 'home',  async (route, config) => {
    return Promise.resolve({
        route: '/'
    },
    {
        route: '/?tm='
    },
    {
        route: '/?hi='
    });
}, validator);
registerPlugin('router', 'stories', stories, validator);
