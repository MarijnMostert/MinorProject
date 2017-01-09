'use strict';

var bodyParser = require('body-parser');
var express = require('express');
var mysql = require('mysql');
var fs = require('fs');
var https = require("https");

var port = process.env.PORT || 8081;
var app = express();

app.use(express.static(__dirname));
app.use(bodyParser.json());

var connection = mysql.createConnection({
	host: 'localhost',
	user: 'ewi3620tu1',
	password: 'RayWriet5',
	database: 'ewi3620tu1'
});


connection.connect(function(error){
	if(!!error){
		console.log('Error');
	} else {
		console.log('Connected');
	}
});

app.get('/login', function(req,res){
	connection.query("SELECT * FROM user", function(error, rows, fields){
		if(!!error){
			console.log('Error in the query');
		} else {
			res.send(rows);
			console.log(rows);
			console.log('Succesful query');
		}
	});
});

app.get('/getallscores', function(req,res){
	connection.query("SELECT score.id, score.score, user.name, score.date FROM score INNER JOIN user ON score.player_id=user.id", function(error, rows, fields){
		if(!!error){
			console.log('Error in the query');
		} else {
			res.send(rows);
			console.log(rows);
			console.log('Succesful query');
		}
	});
});
 
 /*
https.createServer({
	key: fs.readFileSync('key.pem'),
	cert: fs.readFileSync('cert.pem')
}, app).listen(port);
*/

app.listen(port,function(){
	console.log('Applications running on port '+port);
});