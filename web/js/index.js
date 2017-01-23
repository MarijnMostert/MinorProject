function getCookie(cname) {
    var name = cname + "=";
    var decodedCookie = decodeURIComponent(document.cookie);
    var ca = decodedCookie.split(';');
    for(var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}

if(getCookie("DarkDescent")){
	window.location.href ='home.php';
}


$(document).ready(function(){
    var username,password;
    $("#submit").click(function(){
        username =$("#username").val();
        password=$("#password").val();
		console.log({name:username,password:password});
		$.ajax({
			url: './unity/login.php',
			type: 'POST',
			dataType: 'JSON',
			data: {name:username,password:password},
			dataType: 'json',
			success: function(response){
				console.log(response);
				console.log(response.succes);
				if(response.succes==1){
					setCookie(response.message,100);
					window.location.href ='home.php';
				} else {
					alert("Wrong combination of name and password!");
				}
			}
		});		
    });
	
	function setCookie(cid,exdays) {
		var d = new Date();
		d.setTime(d.getTime() + (exdays*24*60*60*1000));
		var expires = "expires=" + d.toGMTString();
		document.cookie = "DarkDescent=" + cid + ";" + expires + ";path=/";
	}
});