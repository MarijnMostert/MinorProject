$(document).ready(function(){
	var sendButton = document.getElementById('sendButton');
	var inputField = document.getElementById('inputField');
	var messagesPort = document.getElementById('messagesPort');

	inputField.focus();
	
	mess1 = new Message(1,'Dit is het eerste bericht',1,1,new Date(2016,2,4,5,22,14));
	mess2 = new Message(2,'Dit is het tweede bericht, iets langer dan de vorige zodat we dat kunnen testen',2,1,new Date(2016,4,21,19,13,32));
	mess3 = new Message(2,'Dit is het aanvullende bericht door dezelfde speler',2,1,new Date(2016,4,21,19,15,32));

	messageList = new Messages();
	messageList.addMessage(mess1);
	messageList.addMessage(mess2);
	messageList.addMessage(mess3);
	last_id = -1;

	var displayMessages = function(){
		messageList.messages.forEach(addMessageToPort);
	}
	
	var addMessageToPort = function(message){
		if (message.player !== last_id){
			balloon = document.createElement('div');
			var name = document.createElement('span');
			name.innerHTML = message.player;
			if(player.id === message.player){
				balloon.className = 'message';
			}else{
				balloon.className = 'message messageLeft';
			}
			balloon.appendChild(name);
			messagesPort.appendChild(balloon);
		}
		
		var text = document.createElement('p');
		text.innerHTML = message.message;
		balloon.appendChild(text);
		
		last_id = message.player;
	}
			
	displayMessages();
	
	sendButton.onclick = function (){
		var newMess= new Message(1,$(inputField).val(),1,1,Date.now());
		messageList.addMessage(newMess);
		addMessageToPort(newMess);
	}
});
