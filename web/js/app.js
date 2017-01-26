$(document).ready(function(){
	$hamburger = true;
	if(!getCookie("DarkDescent")){
		window.location.href ='';
	}
	
	var id = getCookie("DarkDescent");

	$("#name").keyup(function(event){
		if(event.keyCode == 13){
			window.location.href ='search.php?name='.$("name").val();
		}
	});
	
	console.log('classes.js loaded');
	
	guild = new Guild(1, 'GuildName', 'Joran');
	guild.addMember('member1');
	guild.addMember('member2');
	/*guild = new Guild('null','null','null','null');*/
	
	console.log(guild);

	header()
	var pathname = window.location.pathname;
	switch(pathname){
		case '/ewi3620tu1/home.php':
			console.log('url: home');
			homepage();
			break;
		case '/ewi3620tu1/statistics.php':
			console.log('url: statistics.php');
			statisticspage();
			break;
		case '/ewi3620tu1/guild.php':
			console.log('url: guild view');
			viewguildpage();
			break;
		case '/ewi3620tu1/members.php':
			console.log('url: members');
			memberpage();
			break;
		case '/ewi3620tu1/search.php':
			console.log('url: search');
			if (document.location.search.indexOf('name=') >= 0) {
				var name = document.location.search.substring(6);
				searchscore(name)
			} else {
				searchscore('');
			}
			break;
		default:	
			console.log('url not recognized');
			break;
	}
});

var header = function(){
	if (guild === 'null'){
		var add_guild = document.createElement('p').appendChild(document.createElement('a'));
		add_guild.innerHTML = 'Add Guild';
		$('#guilds_dropdown').append(add_guild);
	} else {
		var view_guild = document.createElement('p').appendChild(document.createElement('a'));
		view_guild.className = 'ViewGuild_menu';
		view_guild.innerHTML = 'View Guild';
		$('#guilds_dropdown').append(view_guild);
		var guild_forum = document.createElement('p').appendChild(document.createElement('a'));
		guild_forum.className = 'GuildForum_menu';
		guild_forum.innerHTML = 'Guild Forum';
		$('#guilds_dropdown').append(guild_forum);
		$('.ViewGuild_menu').on('click',function(){window.location.href="guild.php";});
		$('.GuildForum_menu').on('click',function(){window.location.href="guildforum.php";});
	}
	$('.dropdown').on('click',function(){$('dropdown-content').css('display','block')});
	$('#change-avatar').on('click',function(){});
	$('.hamburger').on('click',function(){
		if($hamburger){
			$('#side-menu').css('display','block');
		}else{
			$('#side-menu').css('display','none');
		}
		$hamburger = !$hamburger;
	});
	$('#logout_button').on('click',function(){
		setCookie("",0);
		window.location.href="index.php";
	});
}

var homepage = function(){
	var id = getCookie("DarkDescent");
	var player;
	$.ajax({
			url: './unity/getplayer.php',
			type: 'POST',
			dataType: 'JSON',
			data: {player_id:id},
			dataType: 'json',
			success: function(response){
				console.log('response:::');
				console.log(response);
				player = new Player(response.player_id,response.name, 'haksdf', 'jrout@tudelft.nl', Date.now(), 143);
				
			avatar = 'img/Avatar.jpg';
			$('#avatar img').attr('src',avatar);
			$('#name').html(player.name);
			$('#level').html(response.level);
			$('#highscore').html(response.score);
			$('#coins').html(response.coins + " ©");
			$('#ranking').html(response.rank + " /" + response.total_players);
			$('#playtime').html(player.playtime + " Hours");
			}
		});	
}

var viewguildpage = function(){
	$('#guild_name').html(guild.name);
	$('#guild_leader').html(guild.leader);
	$('#GuildScore').html('Undefined');
	$('#ranking').html('Undefined');
	$('#player_button').on('click',function(){window.location.href = "members.php";});
}

var memberpage = function(){
	$('#guild_button').on('click',function(){window.location.href = "guild.php";});
	guild.members.forEach(function(member){
		var row = document.createElement('tr');
		var name = document.createElement('td');
		name.innerHTML = member;
		row.appendChild(name);
		document.getElementById('members').appendChild(row);
	});
}

var statisticspage = function(){
	scoreList = new ScoreList();	
	var id = getCookie("DarkDescent");
	$.ajax({
			url: './unity/scores.php',
			type: 'POST',
			dataType: 'JSON',
			data: {player_id:id,score_id:0},
			dataType: 'json',
			success: function(response){
				console.log('response:::');
				console.log(response.Highscore);
				for(i=0; i<response.Highscore.length;i++){
					tmpscore = response.Highscore[i];
					scoreList.addScore(new Score(tmpscore.id,1,tmpscore.score, tmpscore.name, new Date(tmpscore.date)));
				}
				drawCanvas();
			}
		});
}

var searchscore = function($name){
	$.ajax({
		url: './unity/getscoresbyname.php',
		type: 'POST',
		dataType: 'JSON',
		data: {name:$name},
		dataType: 'json',
		success: function(response){
			console.log('response:::');
			console.log(response);
			response.Highscore.forEach(drawRow);
		}
	});
	
	var drawRow = function(score){
		var rank_table = document.createElement("td");
		var score_table = document.createElement("td");
		var player_table = document.createElement("td");
		var date_table = document.createElement("td");
		rank_table.innerHTML = score.id;
		score_table.innerHTML = score.score;
		player_table.innerHTML = score.name;
		date_table.innerHTML = score.date;
		var tr = document.createElement('tr');
		tr.appendChild(rank_table);
		tr.appendChild(score_table);
		tr.appendChild(player_table);
		tr.appendChild(date_table);
		$('#scoreTable table').append(tr);
	}
}

var setCookie = function(cid,exdays) {
	var d = new Date();
	d.setTime(d.getTime() + (exdays*24*60*60*1000));
	var expires = "expires=" + d.toGMTString();
	document.cookie = "DarkDescent=" + cid + ";" + expires + ";path=/";
}

var getCookie = function(cname){
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