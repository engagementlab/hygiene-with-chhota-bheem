'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * Workshops page Model
 * @module workshops
 * @class workshops
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * workshops model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var Workshops = new keystone.List('Workshops', 
	{
		label: 'Workshops Page',
		singular: 'Workshops Page',
		nodelete: true,
		nocreate: true
	});

    // Storage adapter for Azure
    var azureFile = new keystone.Storage({
        adapter: require('keystone-storage-adapter-azure'),
        azure: {
            container: 'chhota-bheem',
            generateFilename: function (file) {
                // Cleanup filename
                return file.originalname.replace(/[\\'\-\[\]\/\{\}\(\)\*\+\?\\\^\$\|]/g, "").replace(/ /g, '_').toLowerCase();
            }
        },
        schema: {
            path: true,
            originalname: true,
            url: true
        }
    });

/**
 * Model Fields
 * @main Workshops
 */
Workshops.add({
	
	name: { type: String, default: "Workshops Page", hidden: true, required: true, initial: true },
    
    introEn: { type: Types.Textarea, label: 'Intro Text (English)', required: true, initial: true},
    intro2En: { type: Types.Textarea, label: 'Intro Text 2 (English)'},
    introTm: { type: Types.Textarea, label: 'Intro Text (Tamil)', required: true, initial: true},
    intro2Tm: { type: Types.Textarea, label: 'Intro Text 2 (Tamil)'},
    introHi: { type: Types.Textarea, label: 'Intro Text (Hindi)', required: true, initial: true},
    intro2Hi: { type: Types.Textarea, label: 'Intro Text 2 (Hindi)'}},
    
    'One-Day Workshop', {
        oneDayIntroEn: { type: Types.Textarea, label: 'Intro (English)', required: true, initial: true},
        oneDayObjEn: { type: Types.Textarea, label: 'Objectives (English)', required: true, initial: true},
        oneDayIntroTm: { type: Types.Textarea, label: 'Intro (Tamil)', required: true, initial: true},
        oneDayObjTm: { type: Types.Textarea, label: 'Objectives (Tamil)', required: true, initial: true},
        oneDayIntroHi: { type: Types.Textarea, label: 'Intro (Hindi)', required: true, initial: true},
        oneDayObjHi: { type: Types.Textarea, label: 'Objectives (Hindi)', required: true, initial: true},
        
        oneDayfacGuideEn: { type: Types.File, label: 'Facilitation Plan (English)', storage: azureFile },
        oneDayfacGuideTm: { type: Types.File, label: 'Facilitation Plan (Tamil)', storage: azureFile },
        oneDayfacGuideHi: { type: Types.File, label: 'Facilitation Plan (Hindi)', storage: azureFile },
    },

    'Four-Day Workshop', {
        fourDayIntroEn: { type: Types.Textarea, label: 'Intro (English)', required: true, initial: true},
        fourDayObjEn: { type: Types.Textarea, label: 'Objectives (English)', required: true, initial: true},
        fourDayIntroTm: { type: Types.Textarea, label: 'Intro (Tamil)', required: true, initial: true},
        fourDayObjTm: { type: Types.Textarea, label: 'Objectives (Tamil)', required: true, initial: true},
        fourDayIntroHi: { type: Types.Textarea, label: 'Intro (Hindi)', required: true, initial: true},
        fourDayObjHi: { type: Types.Textarea, label: 'Objectives (Hindi)', required: true, initial: true},

        fourDayfacGuideEn: { type: Types.File, label: 'Facilitation Plan (English)', storage: azureFile },
        fourDayfacGuideTm: { type: Types.File, label: 'Facilitation Plan (Tamil)', storage: azureFile },
        fourDayfacGuideHi: { type: Types.File, label: 'Facilitation Plan (Hindi)', storage: azureFile }
    }
);

/**
 * Model Registration
 */
Workshops.defaultSort = '-createdAt';
Workshops.defaultColumns = 'name';
Workshops.register();
