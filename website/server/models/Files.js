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
        guideImgEn: {
            type: Types.CloudinaryImage,
            label: 'Guide Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },

        guideTm: {
            type: Types.File,
            label: 'Facilitation Guide PDF (Tamil)',
            storage: azureFile
        },
        guideImgTm: {
            type: Types.CloudinaryImage,
            label: 'Guide Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        guideHi: {
            type: Types.File,
            label: 'Facilitation Guide PDF (Hindi)',
            storage: azureFile
        },
        guideImgHi: {
            type: Types.CloudinaryImage,
            label: 'Guide Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
    },

    'Storybooks', {
        storybook1En: {
            type: Types.File,
            label: 'Storybook 1 PDF (English)',
            storage: azureFile
        },
        storybook1ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Storybook 1 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook2En: {
            type: Types.File,
            label: 'Storybook 2 PDF (English)',
            storage: azureFile
        },
        storybook2ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Storybook 2 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook3En: {
            type: Types.File,
            label: 'Storybook 3 PDF (English)',
            storage: azureFile
        },
        storybook3ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Storybook 3 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook4En: {
            type: Types.File,
            label: 'Storybook 4 PDF (English)',
            storage: azureFile
        },
        storybook4ImgEn: {
            label: 'Storybook 4 Image (English)',
            folder: 'chhota-bheem',
            type: Types.CloudinaryImage,
            autoCleanup: true
        },

        storybook1Tm: {
            type: Types.File,
            label: 'Storybook 1 PDF (Tamil)',
            storage: azureFile
        },
        storybook1ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Storybook 1 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook2Tm: {
            type: Types.File,
            label: 'Storybook 2 PDF (Tamil)',
            storage: azureFile
        },
        storybook2ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Storybook 2 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook3Tm: {
            type: Types.File,
            label: 'Storybook 3 PDF (Tamil)',
            storage: azureFile
        },
        storybook3ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Storybook 3 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook4Tm: {
            type: Types.File,
            label: 'Storybook 4 PDF (Tamil)',
            storage: azureFile
        },
        storybook4ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Storybook 4 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },

        storybook1Hi: {
            type: Types.File,
            label: 'Storybook 1 PDF (Hindi)',
            storage: azureFile
        },
        storybook1ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Storybook 1 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook2Hi: {
            type: Types.File,
            label: 'Storybook 2 PDF (Hindi)',
            storage: azureFile
        },
        storybook2ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Storybook 2 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook3Hi: {
            type: Types.File,
            label: 'Storybook 3 PDF (Hindi)',
            storage: azureFile
        },
        storybook3ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Storybook 3 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        storybook4Hi: {
            type: Types.File,
            label: 'Storybook 4 PDF (Hindi)',
            storage: azureFile
        },
        storybook4ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Storybook 4 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        }
    },

    'Posters', {

        poster1En: {
            type: Types.File,
            label: 'Poster 1 PDF (English)',
            storage: azureFile
        },
        poster1ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Poster 1 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster1Tm: {
            type: Types.File,
            label: 'Poster 1 PDF (Tamil)',
            storage: azureFile
        },
        poster1ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Poster 1 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster1Hi: {
            type: Types.File,
            label: 'Poster 1 PDF (Hindi)',
            storage: azureFile
        },
        poster1ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Poster 1 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster2En: {
            type: Types.File,
            label: 'Poster 2 PDF (English)',
            storage: azureFile
        },
        poster2ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Poster 2 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster2Tm: {
            type: Types.File,
            label: 'Poster 2 PDF (Tamil)',
            storage: azureFile
        },
        poster2ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Poster 2 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster2Hi: {
            type: Types.File,
            label: 'Poster 2 PDF (Hindi)',
            storage: azureFile
        },
        poster2ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Poster 2 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster3En: {
            type: Types.File,
            label: 'Poster 3 PDF (English)',
            storage: azureFile
        },
        poster3ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Poster 3 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster3Tm: {
            type: Types.File,
            label: 'Poster 3 PDF (Tamil)',
            storage: azureFile
        },
        poster3ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Poster 3 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster3Hi: {
            type: Types.File,
            label: 'Poster 3 PDF (Hindi)',
            storage: azureFile
        },
        poster3ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Poster 3 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster4En: {
            type: Types.File,
            label: 'Poster 4 PDF (English)',
            storage: azureFile
        },
        poster4ImgEn: {
            type: Types.CloudinaryImage,
            label: 'Poster 4 Image (English)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster4Tm: {
            type: Types.File,
            label: 'Poster 4 PDF (Tamil)',
            storage: azureFile
        },
        poster4ImgTm: {
            type: Types.CloudinaryImage,
            label: 'Poster 4 Image (Tamil)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
        poster4Hi: {
            type: Types.File,
            label: 'Poster 4 PDF (Hindi)',
            storage: azureFile
        },
        poster4ImgHi: {
            type: Types.CloudinaryImage,
            label: 'Poster 4 Image (Hindi)',
            folder: 'chhota-bheem',
            autoCleanup: true
        },
    }
);

/**
 * Model Registration
 */
Files.defaultSort = '-createdAt';
Files.defaultColumns = 'name';
Files.register();