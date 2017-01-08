$(document).ready(function(){
    var email,pass;
    $("#submit").click(function(){
        username =$("#username").val();
        pass=$("#password").val();
		
		$.ajax({
			url: '/login',
			type: 'GET',
			contentType: 'application/json',
			data: JSON.stringify([username,pass]),
			dataType: 'json'
		});
		$.getJSON("https://insyprojects.ewi.tudelft.nl:8081/login", function(data) {
			console.log(data);
		});/*
		window.location.href ='home.php';*/
		
    });
});