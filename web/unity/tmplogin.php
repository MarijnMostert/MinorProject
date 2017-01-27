<?php
$hostname = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";

$dbhandle = mysql_connect($hostname, $username, $password) 
  or die("Unable to connect to MySQL");

$selected = mysql_select_db("ewi3620tu1",$dbhandle) 
  or die("Could not select ewi3620tu1");
  
if(!($_POST["name"]&&$_POST["password"])){
  output(0,"please provide all data");
}
$name = mysql_real_escape_string($_POST["name"]);
$password = mysql_real_escape_string($_POST["password"]);

//fetch tha data from the database 
$result = mysql_query("SELECT id,coins,level,LPAD(achievements, 50, '0') as achievements,LPAD(items_owned, 50, '0') as items_owned,LPAD( items_equiped, 50, '0') as items_equiped FROM user JOIN savedgames ON savedgames.player_id = user.id WHERE name='".$name."' AND password= '".$password."'");
if (mysql_num_rows($result)) {
    $row = mysql_fetch_row($result);
	output(1,$row[0],$row[1],$row[2],$row[3],$row[4],$row[5]);
} else {
	output(0,"This combination is not familiar, please try again...",0,0,0,0,0);
}

function output($succes,$message,$coins,$level,$achievements,$items_owned,$items_equiped){
	die('{ "succes":"'.$succes.'", "message": "'.$message.'", "coins": "'.$coins.'", "level": "'.$level.'", "achievements":"'.$achievements.'", "items_owned": "'.$items_owned.'", "items_equiped":"'.$items_equiped.'"}');
}
?>
