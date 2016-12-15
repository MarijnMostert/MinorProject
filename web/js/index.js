$(document).ready(function(){
    var email,pass;
    $("#submit").click(function(){
        username =$("#username").val();
        pass=$("#password").val();
		window.location.href ='home.php';
		/*
        * Perform some validation here.
        */
		/*
         $.ajax({
			url: 'https://insyprojects.ewi.tudelft.nl:8081/login',
			type: 'POST',
			contentType: 'application/json',
			data: JSON.stringify(this),
			dataType: 'json'
		});
		$.getJSON("https://insyprojects.ewi.tudelft.nl:8081/getUser", function(data) {
			this.id = data.id;
		});*/
    });
});