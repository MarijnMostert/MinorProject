<?php include('header.php')?>
	<div class="main">
		<div class="list mainpage">
			<table>
				<th  id="playername" colspan="2"></th>
				<tr>
					<td>Level: </td>
					<td id="level"></td>
				</tr>
				<tr>
					<td>Highscore: </td>
					<td id="highscore"> </td>
				</tr>
				<tr>
					<td>Coins: </td>
					<td id="coins"> </td>
				</tr>
				<tr>
					<td>World Ranking: </td>
					<td id="ranking"> </td>
				</tr>
				<tr>
					<td>Playtime: </td>
					<td id="playtime"> </td>
				</tr>
			</table>
		</div>
		<div style="width: 100px; height: 50px;"></div>
		<div class="list width-list">
			<div id="graph">
				<canvas id="myCanvas" height="400px" width="1000px"></canvas>
				<script src="js/statistics.js"></script>
			</div>
			<div id="scoreTable">
				<table>
					<thead>
						<th class="rank_table">Rank</th>
						<th class="score_table">Score</th>
						<th class="date_table">Date</th>
					</thead>
				</table>
			</div>
		</div>
	</div>
<?php include('footer.php')?>