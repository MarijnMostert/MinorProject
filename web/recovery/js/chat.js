var sendButton = document.getElementById('sendButton');
var inputField = document.getElementById('inputField');
var messagesPort = document.getElementById('messagesPort');
var messages = [];

inputField.focus();

sendButton.onclick = function (){
	// Create a new JavaScript Date object based on the timestamp
	// multiplied by 1000 so that the argument is in milliseconds, not seconds.
	var date = new Date();
	// Hours part from the timestamp
	var hours = date.getHours();
	// Minutes part from the timestamp
	var minutes = "0" + date.getMinutes();
	// Seconds part from the timestamp
	var seconds = "0" + date.getSeconds();
	
	var formattedTime = hours + ':' + minutes.substr(-2) + ':' + seconds.substr(-2);
	
	//create new HTML
	var newmessage = document.createElement('div');
	var newimage = document.createElement('img');
	var newdiv = document.createElement('div');
	var newpmessage = document.createElement('p');
	var newptime = document.createElement('p');
	
	//assigning classes ans ids
	newmessage.className = "message";
	newpmessage.className = "pmessage";
	newptime.className = "ptime";
	
	//Sorting new HTML
	messagesPort.appendChild(newmessage);
	newmessage.appendChild(newimage);
	newmessage.appendChild(newdiv);
	newdiv.appendChild(newpmessage);
	newdiv.appendChild(newptime);
	
	newpmessage.innerHTML = inputField.value;
	newptime.innerHTML = formattedTime;
	
	var ranDome = Math.random();
	if (ranDome<0.4){
		newimage.src= "/smallchat/chat/img/user_icon/user04.jpg";	
		newmessage.className = "message user";
		var user = 04;
	} else if(ranDome<0.6){
		newimage.src= "/smallchat/chat/img/user_icon/user01.jpg";
		var user = 01;
	} else if(ranDome<0.8){
		newimage.src= "/smallchat/chat/img/user_icon/user02.jpg";
		var user = 02;
	} else {
		newimage.src= "/smallchat/chat/img/user_icon/user03.jpg";
		var user = 03;
	}
	
	var message = {message:inputField.value, time:formattedTime, user:user};
	messages.push(message);
	console.log(messages);
	
	messagesPort.scrollTop = messagesPort.scrollHeight;
	inputField.value='';
	inputField.focus();
	
	
}

inputField.addEventListener("keypress",function(event){
	if (event.keyCode == 13){
		var caret = getCaret(inputField);
		if(event.shiftKey){
			inputField.value = inputField.value.substring(0, caret) + "<br>" + inputField.value.substring(caret, inputField.value.length);
			
        } else {
			event.preventDefault();
			sendButton.click();
		}
	}
})

function getCaret(el) { 
    if (el.selectionStart) { 
        return el.selectionStart; 
    } else if (document.selection) { 
        el.focus();
        var r = document.selection.createRange(); 
        if (r == null) { 
            return 0;
        }
        var re = el.createTextRange(), rc = re.duplicate();
        re.moveToBookmark(r.getBookmark());
        rc.setEndPoint('EndToStart', re);
        return rc.text.length;
    }  
    return 0; 
}