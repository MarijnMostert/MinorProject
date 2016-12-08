<?php 
include('header.php');
?>
	<div class="main">
		<div class="list statistics">
			<div id="graph">
				<canvas id="myCanvas" height="181px" width="363px"></canvas>
				<script>
					
				$(window).ready(function(){
					var canvas = document.getElementById('myCanvas');
					var context = canvas.getContext('2d');				
					var canvas_width = $('#myCanvas').width();
					var canvas_height = $('#myCanvas').height();
					
					console.log(canvas_width);
					console.log(canvas_height);
					
					/**
					  * Object containing all scores
					  */
					function ScoreList(){
						this.scores = [];
						this.scale = 0;
					}
					
					ScoreList.prototype.addScore = function(score){
						this.scores.push({score});
					}
					
					ScoreList.prototype.getScoreScale = function(){
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
						
						return canvas_height/(maxscore-minscore);
					}
					
					ScoreList.prototype.getScale = function(){
						this.scale = (canvas_width-10)/(Math.abs(this.scores[0].score.getDays() - this.scores[this.scores.length-1].score.getDays()));
						return this.scale;
					};
					
					ScoreList.prototype.getFirst = function(){
						return this.scores[0];
					};
					
					ScoreList.prototype.DrawGrid = function (){

					};

					ScoreList.prototype.Draw = function(context){
						var first_day = this.scores[0].score.getDays();
						var scale = this.scale;
						var scoreScale = this.getScoreScale();
						console.log("scale: " + scale + ", first day: " + first_day);
						for (var tmp in this.scores){
							var score = this.scores[tmp].score;
							console.log("score: "+score.id + ", "+ score.getPosX(first_day,scale),
										canvas_height-(scoreScale*(score.score-this.minscore)));
							context.lineTo(score.getPosX(first_day,scale), canvas_height-(scoreScale*(score.score-this.minscore)));
						}
					};
					
					/**
					  * Object representing score
					  */
					function Score(id,player_id,score,date) {
						this.id = id;
						this.player_id = player_id;
						this.score = score;
						this.date = date;
					};
					
					Score.prototype.getDays = function(){
						return this.date.getTime()/(1000*60*60*24);
					};
					
					Score.prototype.getPosX = function(first_date, scale){
						return scale*Math.abs(first_date-this.getDays());
					};
				
				
					/**
					  * filling scorelist
					  */
					var score1 = new Score(1,245,90,new Date(1991,11,10,12,45));
					var score2 = new Score(2,245,60,new Date(1991,11,14,12,45));
					var score3 = new Score(3,245,80,new Date(1991,11,15,12,45));
					
					var scoreList = new ScoreList();
					scoreList.addScore(score1);
					scoreList.addScore(score2);
					scoreList.addScore(score3);

					console.log(scoreList);
					
					var scale = scoreList.getScale();
					console.log("scale = "+scale);
					
					/**
					  * Building canvas 
					  */
					var canvas = document.getElementById('myCanvas');
					var context = canvas.getContext('2d');
					context.font = "30px Arial";
					context.strokeStyle="black";
					
					context.beginPath();
					scoreList.Draw(context);
					//context.lineTo(scale*(score2.getDays()-score1.getDays()), score2.score);
					context.stroke();
				});
				</script>
			</div>
			<div id="scoreTable">
				<table>
					<thead>
						<th class="rank_table">Rank</th>
						<th class="score_table">Score</th>
						<th class="player_table">Player</th>
						<th class="date_table">Date</th>
					</thead>
					<tr>
						<td class="rank_table">1</td>
						<td class="score_table">123</td>
						<td class="player_table">jorna</td>
						<td class="date_table">11-13-14</td>
					</tr>
					<tr>
						<td class="rank_table">2</td>
						<td class="score_table">213</td>
						<td class="player_table">joarn</td>
						<td class="date_table">34-13-14</td>
					</tr>
					<tr>
						<td class="rank_table">3</td>
						<td class="score_table">523</td>
						<td class="player_table">jfhfdgsf</td>
						<td class="date_table">34-01-14</td>
					</tr>
					<tr>
						<td class="rank_table">4</td>
						<td class="score_table">155</td>
						<td class="player_table">johydgarn</td>
						<td class="date_table">32-12-14</td>
					</tr>
				</table>
			</div>
		</div>
	</div>
<?php
include('footer.php');
?>