	$(document).ready(function(){

	if(!getCookie("DarkDescent")){
		window.location.href ='';
	}
	
	var id = getCookie("DarkDescent");

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
		default:	
			console.log('url not recognized');
			break;
	}
});

var header = function(){
	if (guild.id === 'null'){
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