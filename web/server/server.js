'use strict';

var express = require('express');
var session = require('express-session');
var bodyParser = require('body-parser');
var app = express();

app.set('views', __dirname + '/views');
app.engine('html', require('ejs').renderFile);

app.use(session({secret:'sssshhhhh'}));
app.use(bodyParser.json());
app.use(bodyParser.urlencoded({extended: true}));

var sessie;

app.get('/', function(req,res){
	sessie = req.session;
	if (sessie.email){
		res.redirect('/admin');
	} else {
		res.render('index.php');
	}
	sessie.username;
});

app.post('/login',function(req,res){
	sess = req.session;
	sess.email = req.body.email;
	res.end('done');
});

app.get('/admin',function(req,res){
	sess = req.session;
	if(sess.email) {
		res.write('<h1>Hello '+sess.email+'</h1>');
		res.end('<a href="+">Logout</a>');
	} else {
		res.write('<h1>Please login first.</h1>');
		res.end('<a href="+">Login</a>');
	}
});

app.get('/logout',function(req,res){
	req.session.destroy(function(err){
		if(err){
			console.log(err);
		} else {
			res.redirect('/');
		}
	});
});

app.listen(8081,function(){
	console.log('Applications running on port 8081');
});