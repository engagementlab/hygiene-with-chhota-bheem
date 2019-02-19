'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * Module page Model
 * @module Module
 * @class Module
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * Module model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var Module = new keystone.List('Module', {
    label: 'Modules',
    singular: 'Module',
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
 * @main Module
 */
Module.add({
    name: {
        type: String,
        required: true,
        initial: true
    },
    summary: {
        type: Types.Textarea,
        required: true,
        initial: true
    },
    nameTm: {
        type: String,
        label: 'Name (Tamil)',
        required: true,
        initial: true
    },
    summaryTm: {
        type: Types.Textarea,
        required: true,
        label: 'Summary (Tamil)',
        initial: true
    },
    pdf: {
        type: Types.File,
        label: 'PDF',
        storage: azureFile
    },
    video1Url: {
        type: Types.Url,
        required: true,
        initial: true
    },
    video2Url: {
        type: Types.Url,
        required: true,
        initial: true
    },
	videoThumbnailImages: {
		type: Types.CloudinaryImages,
		label: 'Thumbnail images for Videos (retina resolution)',
		folder: 'chhota-bheem',
		autoCleanup: true
	},

});

/**
 * Model Registration
 */
Module.defaultSort = '-createdAt';
Module.defaultColumns = 'name';
Module.register();