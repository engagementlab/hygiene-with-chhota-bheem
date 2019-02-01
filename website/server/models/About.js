'use strict';
/**
 * Engagement Lab Website v2.x
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

/**
 * Model Fields
 * @main About
 */
About.add({
	name: { type: String, default: "About Page", hidden: true, required: true, initial: true },
	missionStatement: { type: String, label: 'Mission Statement', required: true, initial: true },
	
	summary1: { type: Types.Textarea, label: "Summary Paragraph 1", required: true, note: 'First (required) paragraph'},
	summary2: { type: Types.Textarea, label: "Summart Paragraph 2", required: true, note: 'Second (required) paragraph' },

	images: {
		type: Types.CloudinaryImages,
		label: 'Summary Images (Requires EXACTLY 2)',
		folder: 'homepage-2.0/about',
		autoCleanup: true
	},
	
	research: { type: Types.Textarea, label: "Research Text", required: true },
	workshops: { type: Types.Textarea, label: "Workshops Text", required: true },
	tools: { type: Types.Textarea, label: "Tools Text", required: true },
	teaching: { type: Types.Textarea, label: "Teaching Text", required: true },
	design: { type: Types.Textarea, label: "Design Text", required: true }
	
});

/**
 * Model Registration
 */
About.defaultSort = '-createdAt';
About.defaultColumns = 'name';
About.register();
