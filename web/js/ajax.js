var getAllScores  = function(){
	$.ajax({
		url: 'https://insyprojects.ewi.tudelft.nl:8081/getallscores',
		contentType: 'application/json',
		success: function(response){
			console.log(response);
		},
		error: function(XMLHttpRequest, textStatus, errorThrown) { 
			alert("Status: " + textStatus); alert("Error: " + errorThrown + "  " +this.url); 
		}      
	});
};