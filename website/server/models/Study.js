'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * Study page Model
 * @class Study
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * Study model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var Study = new keystone.List('Study', 
	{
		label: 'Case Studies',
		singular: 'Case Study'
	});

// Storage adapter for Azure
var azureFile = new keystone.Storage({
    adapter: require('keystone-storage-adapter-azure'),
    azure: {
        container: 'resources',
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
 * @main Study
 */
Study.add({
	name: { type: String, required: true, initial: true },	
	summary: { type: Types.Textarea, required: true, initial: true},
	pdf: {
		type: Types.File,
		label: 'PDF',
		storage: azureFile
	}
});

/**
 * Model Registration
 */
Study.defaultSort = '-createdAt';
Study.defaultColumns = 'name';
Study.register();
