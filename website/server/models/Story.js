'use strict';
/**
 * Hygiene with Chhota Bheem website
 * 
 * Story page Model
 * @class Story
 * @author Johnny Richardson
 * 
 * For field docs: http://keystonejs.com/docs/database/
 *
 * ==========
 */

var keystone = global.keystone;
var Types = keystone.Field.Types;

/**
 * Story model
 * @constructor
 * See: http://keystonejs.com/docs/database/#lists-options
 */
var Story = new keystone.List('Story', 
	{
		label: 'Stories',
		singular: 'Story',
        autokey: {
            path: 'key',
            from: 'name',
            unique: true
		},
		sortable: true
    });
    
/**
 * Model Fields
 * @main Story
 */
Story.add({
	name: { type: String, label: "Person's Name", required: true, initial: true, index: true },	
	subtitle: { type: String, label: "Story Subtitle", required: true, initial: true},
	description: { type: Types.Markdown, label: "Story Description", required: true, initial: true},
	photo: { type: Types.CloudinaryImage, required: true, initial: true }
});

/**
 * Model Registration
 */
Story.defaultSort = '-createdAt';
Story.defaultColumns = 'name';
Story.register();
