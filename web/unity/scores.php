<?php
$servername = "localhost";
$username = "ewi3620tu1";
$password = "RayWriet5";
try {
	$conn = new PDO("mysql:host=$servername;dbname=ewi3620tu1",$username,$password);
    // set the PDO error mode to exception
    $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    }
catch(PDOException $e)
    {
    echo "Connection failed: " . $e->getMessage();
    }
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 
$sql = "SELECT * FROM score";
$answer = $conn->query($sql);
?>
<script>console.log(<?php  ?>);</script>
<?php
if($conn->query($sql) === TRUE){
    echo "Database created successfully";
} else {
    echo "Error retrieving data: " . $conn->error;
}
$conn->close();
?>
