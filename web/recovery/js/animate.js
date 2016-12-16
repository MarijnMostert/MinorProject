'use strict';
var expand = function(e) {
 if (e.className !== 'todoItem') {
   e.className = 'todoItem';
 } else {
   e.className = 'todoItemExpanded';
 }
};

var toggleAddTODO = function() {
  if (document.getElementById("addTODO").className !== "AddContainer") {
    document.getElementById("addTODO").className = "AddContainer";
  } else {
    document.getElementById("addTODO").className = "AddContainerDisplay";
  }
 };
