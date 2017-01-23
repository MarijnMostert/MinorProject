'use strict';

var express 	= require("express");
var mysql		= require("mysql");
var https 		= require("https");
var url 		= require("url");
var fs			= require("fs");
var app = express();
var port = 8081;

app.use(express.static(__dirname + "/../"));

var options = {
  key: fs.readFileSync('key.pem'),
  cert: fs.readFileSync('cert.pem')
};


https.createServer(options,app).listen(port);

var connection 	= mysql.createConnection({
	host	:	'localhost',
	user	:	'ewi3620tu1',
	password: 	'RayWriet5',
	database:	'ewi3620tu1'
});

connection.connect(function(err){
	if(!!err){
		console.log("Error connecting database...");
	} else {
		console.log("Database is connected...");
	}
});

app.get("/",function(req,res){
	connection.query('SELECT * from user', function(err, rows, fields){
	connection.end();
		if(!err){
			console.log("The solution is: ",rows);
		}else{
			console.log("Error while performing Query: "+err);
		}
	});
});

