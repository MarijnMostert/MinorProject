$(document).ready(function(){
	console.log('classes.js loaded');
	/*
	 * Generate local tmp data 
	 */
	player = new Player(1,'Joran', 'haksdf', 'jrout@tudelft.nl', Date.now(), 143);
	avatar = 'img/Avatar.jpg';
	rank = new Rank(4, 45640, '09-12-2016');
	totalplayers = 6550;
	
	scoreList = new ScoreList();
	
	scoreList.addScore(new Score(1,1,132, 'Joran', new Date(2016,11,1,1,4,51)));
	scoreList.addScore(new Score(2,3,88, 'Joran', new Date(2016,11,4,14,2,30)));
	scoreList.addScore(new Score(3,2,114, 'Joran', new Date(2016,11,5,15,5,40)));	scoreList.addScore(new Score(4,4,50, 'Joran', new Date(2016,11,7,20,16,15)));

	
	guild = new Guild(1, 'GuildName', 'Joran', ['Joran','member1','member2']);
	/*guild = new Guild('null','null','null','null');*/
	
	console.log(player);
	console.log(scoreList);

	header()
	var pathname = window.location.pathname;
	switch(pathname){
		case '/ewi3620tu1/home.php':
			console.log('url: home');
			homepage();
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
		var guild_market = document.createElement('p').appendChild(document.createElement('a'));
		guild_market.className = 'GuildMarket_menu';
		guild_market.innerHTML = 'Guild Market';
		$('#guilds_dropdown').append(guild_market);
		
			$('.ViewGuild_menu').on('click',function(){window.location.href="guild.php";});
			$('.GuildForum_menu').on('click',function(){window.location.href="guildforum.php";});
			$('.GuildMarket_menu').on('click',function(){window.location.href="guildmarket.php";});
	}
}

var homepage = function(){
	$('#avatar img').attr('src',avatar);
	$('#name').html(player.name);
	$('#level').html('999+');
	$('#highscore').html(rank.highscore + ' ©');
	$('#ranking').html(rank.rank + " /" + totalplayers);
	$('#playtime').html(player.playtime + " Hours");
}