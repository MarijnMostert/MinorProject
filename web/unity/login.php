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
$result = mysql_query("SELECT id FROM user WHERE name='".$name."' AND password= '".$password."'");
if (mysql_num_rows($result)) {
	output(1,mysql_result($result,0));
} else {
	output(0,"This combination is not familiar, please try again...");
}

function output($succes,$message){
	die('{ "succes":"'.$succes.'", "message": "'.$message.'"}');
}
?>
