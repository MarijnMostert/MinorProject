<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
  
if(ISSET($_POST["id"])&&ISSET($_POST["score"])&&ISSET($_POST["date"])){
	$id = mysql_real_escape_string($_POST["id"]);
	$score = mysql_real_escape_string($_POST["score"]);	
	$date = mysql_real_escape_string($_POST["date"]);
	
	//execute the SQL query and return records
	$result = mysql_query("INSERT INTO score (score,player_id,date) VALUES ('".$score."','".$id."','".$date."')");
	$result = mysql_query("SELECT id,date FROM score WHERE id='".mysql_insert_id()."'");
	//fetch the data from the database
	$arr = array();
	while($obj = mysql_fetch_object($result)) {
		$arr[] = $obj;
	}
	echo substr(json_encode($arr),1,-1);
} else {
	echo "error";
}
?>
