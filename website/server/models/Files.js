'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * Files page Model
 * @class Files
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * Files model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var Files = new keystone.List('Files', 
	{
		label: 'Files',
		singular: 'Files',
        nocreate: true,
        nodelete: true
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
 * @main Files
 */
Files.add({
    name: { type: String, default: 'Files', hidden: true },
    guideEn: { type: Types.File, label: 'Facilitation Guide PDF (English)', storage: azureFile },
    guideTm: { type: Types.File, label: 'Facilitation Guide PDF (Tamil)', storage: azureFile },
    storybookEn: { type: Types.File, label: 'Storybook PDF (English)', storage: azureFile },
    storybookTm: { type: Types.File, label: 'Storybook PDF (Tamil)', storage: azureFile },
	// photo: { type: Types.CloudinaryImage, required: true, initial: true }
});

/**
 * Model Registration
 */
Files.defaultSort = '-createdAt';
Files.defaultColumns = 'name';
Files.register();
