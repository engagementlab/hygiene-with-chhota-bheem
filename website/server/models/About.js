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

/**
 * Model Fields
 * @main About
 */
About.add({
	
	name: { type: String, default: "About Page", hidden: true, required: true, initial: true },
	intro: { type: Types.Textarea, label: 'Intro Text', required: true, initial: true},
	para1: { type: Types.Textarea, label: 'Paragraph 1 Text', required: true, initial: true},
	para2: { type: Types.Textarea, label: 'Paragraph 2 Text', required: true, initial: true},

	introTm: { type: Types.Textarea, label: 'Intro Text (Tamil)', required: true, initial: true},
	para1Tm: { type: Types.Textarea, label: 'Paragraph 1 Text (Tamil)', required: true, initial: true},
	para2Tm: { type: Types.Textarea, label: 'Paragraph 2 Text (Tamil)', required: true, initial: true}
	
});

/**
 * Model Registration
 */
About.defaultSort = '-createdAt';
About.defaultColumns = 'name';
About.register();
