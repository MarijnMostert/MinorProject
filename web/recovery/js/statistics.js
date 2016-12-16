	
				$(window).ready(function(){
					drawCanvas();
				});
				
				var drawCanvas = function(){
					/*
					 * Display graph for scores
					 */
					var canvas = document.getElementById('myCanvas');
					var context = canvas.getContext('2d');				
					var canvas_width = $('#myCanvas').width();
					var canvas_height = $('#myCanvas').height();
					
					var scale = scoreList.getScale(canvas_width,canvas_height);
					console.log("scale = "+scale);
					
					var canvas = document.getElementById('myCanvas');
					var context = canvas.getContext('2d');
					context.font = "20px Arial";
					
					context.globalCompositeOperation = "destination-over";
					
					context.beginPath();
					context.strokeStyle="#18121E";
					context.lineWidth = 3;
					scoreList.Draw(context,canvas_height, canvas_width);
					context.stroke();
					
					scoreList.DrawGrid(context,canvas_height, canvas_width);
					canvas.style.width = 'calc(100% - 4px)';
					var scoreClicks = document.getElementsByClassName('ScoreClick');
					for (i = 0; i<scoreClicks.length;i++){
						scoreClicks[i].style.left = 'calc(' + parseFloat(scoreClicks[i].style.left,10)*99.5 + '% - '+ 9 + 'px)';
						scoreClicks[i].style.top = 'calc('+ parseFloat(scoreClicks[i].style.top,10)*99.5 + '% - '+ 9 + 'px)';
					}
										
					var drawRow = function(score){
						var rank_table = document.createElement("td");
						var score_table = document.createElement("td");
						var player_table = document.createElement("td");
						var date_table = document.createElement("td");
						rank_table.innerHTML = score.score.rank;
						score_table.innerHTML = score.score.score;
						player_table.innerHTML = score.score.player;
						date_table.innerHTML = score.score.date.dateLite();
						var tr = document.createElement('tr');
						tr.appendChild(rank_table);
						tr.appendChild(score_table);
						tr.appendChild(player_table);
						tr.appendChild(date_table);
						$('#scoreTable table').append(tr);
					}
					
					scoreList.scores.forEach(drawRow);
				}