<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
  
if(ISSET($_POST["player_id"])&&ISSET($_POST["score_id"])){
	$player_id = mysql_real_escape_string($_POST["player_id"]);
	$score_id = mysql_real_escape_string($_POST["score_id"]);
	
	//execute the SQL query and return records
	
	$result = mysql_query("
		SELECT r.* FROM (
			SELECT s.id, name,s.player_id, s.date, IF(@score=s.score, @rank:=@rank,  @rank:=@rank+1) rank, @score:=s.score score 
			FROM  (SELECT @score:=0,@rank:=0) r, 
				(SELECT s.id, s.player_id, s.score, name, s.date FROM score s 
				JOIN user ON s.player_id = user.id) s 
			ORDER BY s.score DESC ) r
		WHERE player_id = '".$player_id."' 
			AND r.id > '".$score_id."' 
		ORDER BY r.date DESC
		");
	
	/*$result = mysql_query("SELECT score.id, score.score, name, score.date FROM score JOIN user ON score.player_id = user.id WHERE score.player_id = '".$player_id."' AND score.id > ".$score_id);*/
	//fetch the data from the database
	$arr = array();
	while($obj = mysql_fetch_object($result)) {
		$arr[] = $obj;
	}
	echo '{ "Highscore": '.json_encode($arr).'}';
} else {
	echo "error";
}
/*

//echo '[{"name": "Joran", "score": "43002", "date": "2017-01-17"},{"name": "Tom", "score": "5", "date": "2017-01-18"}]';
?>
