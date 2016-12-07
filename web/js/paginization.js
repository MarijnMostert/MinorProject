'use strict';
var shared = 4;
var categories = ["School","Work"];
$('.menuListSub').append('<ul class="categorieList"></ul>');
categories.forEach(setCategories);

function setCategories(item,index){
	$('.categorieList').append('<li>'+	item+'</li>');
}
$('.categorieList').append('<li>+</li>');

function settingsPage(){
	$('.main').empty()
			.append('Display all settings');
}

function todoPage(item){
	$('#main').empty();
	var todoList = document.createElement('ul');
	var todoItem = document.createElement('li');
	todoItem.innerHTML = '<b>{{'+item+'}}</b>{{Text}}<br>Due date: {{Date}} Prio: {{Prio}} Categorie {{Cat}}<button class="completed" type="button">Com</button><button class="edit" type="button">edit</button><button class="delete" type="button">delete</button>';
	todoItem.className = "todoItem Expanded";
	todoList.appendChild(todoItem);
	
	var newTodoItem = document.createElement('li');
	newTodoItem.innerHTML= '<li class="addTodoItem"><h4><input class="imageButton" onclick="toggleAddTODO()" type="image" src="img/main/add.png"></h4></li>';
	document.getElementById('main').appendChild(todoList);
	todoList.appendChild(newTodoItem);
}



$(document).ready(function(){
	$('.shared').attr('data-after',shared);
	$('.todoItem').on('click',function(){
		console.log("clicked todo");
		if ($(this).hasClass('Expanded')){
			$(this).removeClass('Expanded');
		} else {
			$(this).addClass('Expanded');
		}
	});
	$('.menu li').on('click', function(){
		$('.menu li').removeClass('active');
		$(this).addClass('active');
		var chosen = $(this).clone()
						.children()
						.remove()
						.end()
						.text();
		switch (chosen) {
			case 'Home':
				console.log('Home');
				todoPage(chosen);
				break;
			case 'Settings':
				console.log('settings');
				settingsPage();
				break;
			case 'Account':
				console.log('account');
				break;
			case 'Log Out':
				console.log('logout');
				window.location.assign("./index.html");
				break;
			case 'Categories':
				break;
			default:
				console.log('default: |'+chosen+'|');
				todoPage(chosen);
				break;
		}
	}).parent().on('click',function(){
		return false;
	});
});
