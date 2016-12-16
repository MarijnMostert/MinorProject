<?php 
include('header.php');
?>
	<div class="main">
		<div class="list width-list">
			<table>
				<th  id="list_header" colspan="2">
					<img src="./img/Rijkswapen_der_Nederlanden.png"></img>
				</th>
				<tr>
					<td>Name: </td>
					<td id="guild_name"></td>
				</tr>
				<tr>
					<td>Leader: </td>
					<td id="guild_leader"></td>
				</tr>
				<tr>
					<td>Score: </td>
					<td id="GuildScore"> </td>
				</tr>
				<tr>
					<td>World Ranking: </td>
					<td id="ranking"> </td>
				</tr>
			</table>
			<div id="player_button">
				<div>Members</div>
				<div>></div>
			</div>
		</div>
	</div>
<?php
include('footer.php');
?>