const fs = require('fs');
const download = require('download'), 
			request = require('request'),
			decompress = require('decompress'),
			_ = require('underscore'),
			moment = require('moment');
var http = require('https');

var user = 'username';
var pass = 'password';

// var auth = new Buffer('cac03437-6ed3-497d-9a83-9da5319aeaba:88be2b4eb2a68f4b95431fc46a1e74a1').toString('base64');
// var options = {
//   host: 'analytics.cloud.unity3d.com',
//   // port: 80,
//   path: '/api/v2/projects/cac03437-6ed3-497d-9a83-9da5319aeaba/rawdataexports/1252f8e1-a495-499b-9bf0-2d5223b72f0c',
//   headers: {
//     'Authorization': 'Basic ' + auth
//   }
// };

// http.get(options, function(res) {
//   res.on('data', function (chunk) {
//  		let body = JSON.parse(chunk);
//  		_.each(body.result.fileList, (file) => {
 
// 			download(file.url, 'files').then(() => {
			    
// 			});
//  		})
//   });
// });

var dir = require('node-dir');
var jsonUnsorted = [];
var finalJson = {};

dir.readFiles(__dirname + '/files', {
	    match: /.json$/
	  }, function(err, content, next) {
			if (err) throw err;
			// let str = JSON.stringify(content);

			// console.log("=====");
			let cleaned = content.replace(new RegExp('"type":"custom"}', "g"), '"type":"custom"},');
			cleaned = "["+cleaned.substring(0, cleaned.length-2)+"]";
			let str = JSON.stringify(cleaned);

			let a;
			try {
		    a = _.filter(JSON.parse(cleaned), (obj) => {return obj.name === "gameStart";});
		    _.each(a, (i) => {
		    	i.submit_time = new Date(i.submit_time)
		    	jsonUnsorted.push(i);
		  	});
			} catch(e) {
					console.log(content)
			    console.log(e); // error in the above string (in this case, yes)!
			}

      next();
  },
  function(err, files) {
	  if (err) throw err;
	  let listedSorted = _.sortBy(jsonUnsorted, (o) => {
		  return o.submit_time;
	  });
      // console.log('finished reading files:',files);
      _.each(listedSorted, (i) => {
		// console.log(i.submit_time)
		    i.submit_time =	moment(i.submit_time).format("MM/DD/YYYY HH:MM:SS");
      	finalJson += JSON.stringify(i)+',';
    	});
      var util = require('util');
			fs.writeFileSync('./data2.json', util.inspect(finalJson) , 'utf-8');
  });