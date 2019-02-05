'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * About page Model
 * @module about
 * @class about
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * about model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var About = new keystone.List('About', 
	{
		label: 'About Page',
		singular: 'About Page',
		nodelete: true,
		nocreate: true
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
 * @main About
 */
About.add({
	name: { type: String, default: "About Page", hidden: true, required: true, initial: true },
    pdf: {
        type: Types.File,
        label: 'Facilitation Guide PDF',
        storage: azureFile
    }
	
});

/**
 * Model Registration
 */
About.defaultSort = '-createdAt';
About.defaultColumns = 'name';
About.register();
