<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
    
if(ISSET($_POST["name"])){
	$name = mysql_real_escape_string($_POST["name"]);
	
	//execute the SQL query and return records
	//mysql_query("");
	//$result = mysql_query("SET @rank=0; SELECT @rank:=@rank+1 AS rank, name, score FROM score ORDER BY score DESC;");
	
	$result = mysql_query("SELECT score.id, score.score, name, score.date FROM score JOIN user ON score.player_id = user.id WHERE name LIKE '%".$name."%'");
	//fetch the data from the database
	$arr = array();
	while($obj = mysql_fetch_object($result)) {
		$arr[] = $obj;
	}
	echo '{ "Highscore": '.json_encode($arr).'}';
} else {
	echo "error";
}


//echo '[{"name": "Joran", "score": "43002", "date": "2017-01-17"},{"name": "Tom", "score": "5", "date": "2017-01-18"}]';
?>