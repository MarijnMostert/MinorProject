<?php 
include('header.php');
?>
	<div class="main">
		<div class="list statistics">
			<div id="graph">
				<canvas id="myCanvas" height="500px" width="1000px"></canvas>
				<script src="js/statistics.js"></script>
			</div>
			<div id="scoreTable">
				<table>
					<thead>
						<th class="rank_table">Rank</th>
						<th class="score_table">Score</th>
						<th class="player_table">Player</th>
						<th class="date_table">Date</th>
					</thead>
				</table>
			</div>
		</div>
	</div>
<?php
include('footer.php');
?>