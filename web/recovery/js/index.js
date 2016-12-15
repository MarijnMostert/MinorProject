'use strict';
$(document).ready( function(){
	$('#button').on('click', function() {
			$('#login').css('display','table-cell');
		});
	$('#login').on('click', function() {
			$('#login').css('display','none');
		}).children().on('click', function(){
			return false;
		});
});