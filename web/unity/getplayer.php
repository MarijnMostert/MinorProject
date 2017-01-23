<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
  
if(ISSET($_POST["player_id"])){
	$player_id = mysql_real_escape_string($_POST["player_id"]);
	  
	//execute the SQL query and return records
	$result = mysql_query("SELECT name,coins,level FROM user WHERE id=".$player_id);
	$rank_query = mysql_query("SELECT MAX(score) as score, player_id FROM score GROUP BY player_id ORDER BY score DESC");
	$total = mysql_query("SELECT COUNT(1) as total_players FROM user");
	//$result2 = mysql_query("SELECT FROM");
	//fetch the data from the database
	$arr = array();
	while($obj = mysql_fetch_object($result)) {
		$arr[] = $obj;
	}
	$rank_count=0;
	$arr1 = array();
	$arr2 = array();
	while($obj = mysql_fetch_object($rank_query)){
		$rank_count++;
		if(($obj -> player_id)==$player_id){
			$rank = new stdClass();
			$rank->rank=$rank_count;
			$arr1[] = $rank;
			$arr2[] = $obj;
		}
	}
	$arr3 = array();
	while($obj = mysql_fetch_object($total)) {
		$arr3[] = $obj;
	}
	echo substr(json_encode($arr),1,-2).','.substr(json_encode($arr1),2,-2).','.substr(json_encode($arr2),2,-2).','.substr(json_encode($arr3),2,-1);
} else {
	echo "error";
}