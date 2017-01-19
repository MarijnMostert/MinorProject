<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
//execute the SQL query and return records
$result = mysql_query("SELECT score.id, score.score, name, score.date FROM score JOIN user ON score.player_id = user.id WHERE score.player_id = score.player_id AND score.id > 0");
//fetch tha data from the database
$arr = array();
while($obj = mysql_fetch_object($result)) {
$arr[] = $obj;
}
$bad = array("{","}");
$good = array("[","]");
echo '{ "Highscore": '.json_encode($arr).'}';
/*

//echo '[{"name": "Joran", "score": "43002", "date": "2017-01-17"},{"name": "Tom", "score": "5", "date": "2017-01-18"}]';
?>
