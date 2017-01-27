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
$result = mysql_query("SELECT * FROM user WHERE name='".$name."'");
if (mysql_num_rows($result)) {
    output(0,"This name already exists.");
}
 
$result = mysql_query("INSERT INTO user (name,password,email) VALUES ('".$name."','".$password."','fake@fake.com')");
$id = mysql_query("SELECT id FROM user WHERE name='".$name."'");
$result = mysql_query("INSERT INTO savedgames (player_id,achievements,items_owned,items_equiped) VALUES ('".mysql_result($id,0)."','0000000000000000000000000000000000000000000000000','00000000000000000000000000000000000000000000000000','00000000000000000000000000000000000000000000000000')");
output(1,mysql_result($id,0));

function output($succes,$message){
	die('{ "succes":"'.$succes.'", "message": "'.$message.'"}');
}
?>
