<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
  
if(ISSET($_POST["id"])&&ISSET($_POST["coins"])&&ISSET($_POST["level"])){
	$id = mysql_real_escape_string($_POST["id"]);
	$coins = mysql_real_escape_string($_POST["coins"]);	
	$level = mysql_real_escape_string($_POST["level"]);

	//execute the SQL query and return records
	$result = mysql_query("UPDATE user SET coins=".$coins.", level=".$level." WHERE id=".$id."");
}
?>
