'use strict';

var Player = function(id,name,password,email,date, playtime){
	this.id = id;
	this.name = name;
	this.password = password;
	this.email = email;
	this.date = date;
	this.playtime = playtime;
}

var Score = function(id,rank,score,player,date){
	this.id = id;
	this.rank = rank; /*js only*/
	this.score = score;
	this.player = player;
	this.date = date;
}

Score.prototype.getDays = function(){
	return this.date.getTime()/(1000*60*60*24);
};
					
Score.prototype.getPosX = function(first_date, scale){
	return scale*Math.abs(this.getDays()-first_date);
};
			
function ScoreList(){
	this.scores = [];
	this.scale = 0;
}
					
ScoreList.prototype.addScore = function(score){
	this.scores.push({score});
}
					
ScoreList.prototype.getScoreScale = function(canvas_height){
	var maxscore = -Number.MAX_VALUE;
	var minscore = Number.MAX_VALUE;
	for(var tmp in this.scores){
		if(maxscore<this.scores[tmp].score.score){
			maxscore = this.scores[tmp].score.score;
		}
		if(minscore>this.scores[tmp].score.score){
			minscore = this.scores[tmp].score.score;
		}
	}
	this.maxscore = maxscore;
	this.minscore = minscore;
		
	console.log("maxscore: " + maxscore+", minscore: " + minscore);
	this.scoreScale = canvas_height/Math.abs(maxscore-minscore);
	return this.scoreScale;
}
					
ScoreList.prototype.getScale = function(canvas_width,canvas_height){
	console.log('canvas_width: '+canvas_width + ' canvas_height' + canvas_height);
	this.scale = (canvas_width)/Math.abs(this.getLast().score.getDays()-(this.getFirst().score.getDays()));
	return this.scale;
};

ScoreList.prototype.getFirst = function(){
	return this.scores[0];
};

ScoreList.prototype.getLast = function(){
	return this.scores[this.scores.length-1];
};
					
ScoreList.prototype.DrawGrid = function (ctx, canvas_height, canvas_width){
	var begin = Math.floor(this.minscore/10)*10;
	var diff = Math.round((this.maxscore-this.minscore)/100)*15;
	for(var height = begin; height<this.maxscore+diff;height+=(diff/5)){
		ctx.beginPath();		
		var grid_height = canvas_height-(this.scoreScale*(height-this.minscore));
		if ((height-begin)%diff===0){
			ctx.strokeStyle='#984B43';
			ctx.fillStyle = '#984B43';
			ctx.fillText(height,13,grid_height+7);		
			ctx.moveTo(55,grid_height);
		} else {
			ctx.strokeStyle='#CD9B67';
			ctx.moveTo(5,grid_height);
		}		
		ctx.lineTo(canvas_width-5,grid_height);
		ctx.stroke();
	}
};

ScoreList.prototype.Draw = function(context, canvas_height, canvas_width){
	var first_day = this.scores[0].score.getDays();
	var scale = this.scale;
	var scoreScale = this.getScoreScale(canvas_height);
	console.log("scale: " + scale + ", first day: " + first_day);
	for (var tmp in this.scores){
		var score = this.scores[tmp].score;
		console.log("score: "+score.id + ", "+ score.getPosX(first_day,scale), canvas_height-(scoreScale*(score.score-this.minscore)));
		context.lineTo(score.getPosX(first_day,scale), canvas_height-(scoreScale*(score.score-this.minscore)));
		var scoreClick = document.createElement('div');
		var scoreInfo = document.createElement('div');
		var innerDiv = document.createElement('div');
		innerDiv.innerHTML = score.score +" | "+score.date.dateLite();
		scoreInfo.appendChild(innerDiv);
		scoreClick.className = 'ScoreClick';
		$(scoreClick).css('top',(canvas_height-(scoreScale*(score.score-this.minscore)))/canvas_height);
		$(scoreClick).css('left',score.getPosX(first_day,scale)/canvas_width);
		scoreInfo.className = 'scoreInfo';
		scoreClick.appendChild(scoreInfo);
		$('#graph').append(scoreClick);
	}
};			

var Guild = function(id, name, leader){
	this.id = id,
	this.name = name;
	this.leader = leader;
	this.members = [leader]; /*js only*/
}

var Member = function(id,name,playtime){ /*js only*/
	this.id = is;
	this.name = name;
	this.playtime = playtime;
}

var Rank = function(rank, highscore, date){
	this.rank = rank;
	this.highscore = highscore;
	this.date = date;
}

Guild.prototype.addMember = function(member){
	this.members.push(member);
}

Guild.prototype.addMembers = function(members){
	members.foreach(addMember);
}

/*
 * extern classes
 */

 Date.prototype.dateLite = function() {
  var month = this.getMonth() + 1;
  var day = this.getDate();

  return [(day>9 ? '' : '0') + day,
          (month>9 ? '' : '0') + month,
		  this.getFullYear()
         ].join(' - ');
};
