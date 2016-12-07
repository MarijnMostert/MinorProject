'use strict';
var note = function(id,categorie,title,message,data,priotiet){
  this.id = id;
  this.categorie = categorie;
  this.title = title;
  this.message = message,
  this.data = data,
  this.priotiet = priotiet;
}

note.prototype.setCategorie = function (categorie) {
  this.categorie = categorie;
};

note.prototype.setTitle = function (title) {
  this.title = title;
};

note.prototype.setMessage = function (message) {
  this.message = message;
};

note.prototype.setData = function (data) {
  this.data = data;
};

note.prototype.setPriotiet = function (priotiet) {
  this.priotiet = priotiet;
};
