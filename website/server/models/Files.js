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
var Files = new keystone.List('Files', {
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
        name: {
            type: String,
            default: 'Files',
            hidden: true
        },
        zipEn: {
            type: Types.File,
            label: 'Zip File (English)',
            storage: azureFile
        },
        zipTm: {
            type: Types.File,
            label: 'Zip File (Tamil)',
            storage: azureFile
        },
        zipHi: {
            type: Types.File,
            label: 'Zip File (Hindi)',
            storage: azureFile
        },
        
        guideEn: {
            type: Types.File,
            label: 'Facilitation Guide PDF (English)',
            storage: azureFile
        },
	    guideImgEn: { type: Types.CloudinaryImage, label: 'Guide Image (English)', folder: 'chhota-bheem',
            autoCleanup: true },

        guideTm: {
            type: Types.File,
            label: 'Facilitation Guide PDF (Tamil)',
            storage: azureFile
        },
        guideHi: {
            type: Types.File,
            label: 'Facilitation Guide PDF (Hindi)',
            storage: azureFile
        }
    },

    'Storybooks', {
        storybook1En: {
            type: Types.File,
            label: 'Storybook 1 PDF (English)',
            storage: azureFile
        },
        storybook2En: {
            type: Types.File,
            label: 'Storybook 1 PDF (English)',
            storage: azureFile
        },
        storybook3En: {
            type: Types.File,
            label: 'Storybook 1 PDF (English)',
            storage: azureFile
        },
        storybook4En: {
            type: Types.File,
            label: 'Storybook 1 PDF (English)',
            storage: azureFile
        },

        storybook1Tm: {
            type: Types.File,
            label: 'Storybook 1 PDF (Tamil)',
            storage: azureFile
        },
        storybook2Tm: {
            type: Types.File,
            label: 'Storybook 2 PDF (Tamil)',
            storage: azureFile
        },
        storybook3Tm: {
            type: Types.File,
            label: 'Storybook 3 PDF (Tamil)',
            storage: azureFile
        },
        storybook4Tm: {
            type: Types.File,
            label: 'Storybook 4 PDF (Tamil)',
            storage: azureFile
        },

        storybook1Hi: {
            type: Types.File,
            label: 'Storybook 1 PDF (Hindi)',
            storage: azureFile
        },
        storybook2Hi: {
            type: Types.File,
            label: 'Storybook 2 PDF (Hindi)',
            storage: azureFile
        },
        storybook3Hi: {
            type: Types.File,
            label: 'Storybook 3 DF (Hindi)',
            storage: azureFile
        },
        storybook4Hi: {
            type: Types.File,
            label: 'Storybook 4 PDF (Hindi)',
            storage: azureFile
        },
    },

    'Posters', {

        poster1En: {
            type: Types.File,
            label: 'Poster 1 PDF (English)',
            storage: azureFile
        },
        poster1Tm: {
            type: Types.File,
            label: 'Poster 1 PDF (Tamil)',
            storage: azureFile
        },
        poster1Hi: {
            type: Types.File,
            label: 'Poster 1 PDF (Hindi)',
            storage: azureFile
        },
        poster2En: {
            type: Types.File,
            label: 'Poster 2 PDF (English)',
            storage: azureFile
        },
        poster2Tm: {
            type: Types.File,
            label: 'Poster 2 PDF (Tamil)',
            storage: azureFile
        },
        poster2Hi: {
            type: Types.File,
            label: 'Poster 2 PDF (Hindi)',
            storage: azureFile
        }
    }
);

/**
 * Model Registration
 */
Files.defaultSort = '-createdAt';
Files.defaultColumns = 'name';
Files.register();