<!doctype html>
<head> 
  <meta charset="utf-8">
  <meta property="og:title" content="In The Dark webpage"/>
  <meta property="og:site_name" content="InTheDark.nl"/>
  <meta property="og:description" content="A Awesome game"/>
  <meta property="og:image:width" content="?" />
  <meta property="og:image:height" content="?" />
  <meta name="theme-color" content="#18121E">
  <link rel="icon" sizes="192x192" href=".png">
  <title>InTheDark</title>
  <link rel="stylesheet" type="text/css" href="css/main.css">
  <meta name="viewport" content="initial-scale=1, user-scalable=no"/>  
  <script src="js/jquery.min.js"></script>
  <script src="js/classes.js"></script>
  <script src="js/app.js"></script>
  <script src="js/ajax.js"></script>
</head>

<body>
	<div id="avatar">
		<div>
			<div id="change-avatar"></div>
			<img src="img/Avatar.jpg">
		</div>
	</div>
	<div id="side-menu">
		<div><button class="leftbutton hamburger"></button></div>
		<div class="menu-item home-menu"><p><a>Home</a></p></div>
		<div class="menu-item statistics-menu"><p><a>Statistics</a></p></div>
		<div class="menu-item dropdown">
			<p><a class="dropbtn">Guilds</a></p>
			<div id="guilds_dropdown" class="dropdown-content guilds_dropdown"></div>
		</div>
		<div class="menu-item settings-menu"><p><a>Settings</a></p></div>
	</div>
	<div id="header">
		<div id="right-top">
			<button class="leftbutton hamburger"></button> 
			<button id="logout_button" class="button"></button>
			<form id="form">	
				<input id="name" type="text" name="name" placeholder="Search..">
			</form>
		</div>
		<div id="menu">
			<div class="menu-item home-menu"><p><a>Home</a></p></div>
			<div class="menu-item statistics-menu"><p><a>Statistics</a></p></div>
			<div class="menu-empty"></div>
			<div class="menu-item dropdown">
				<p><a class="dropbtn">Guilds</a></p>
				<div id="guilds_dropdown" class="dropdown-content guilds_dropdown"></div>
			</div>
			<div class="menu-item settings-menu"><p><a>Settings</a></p></div>
		</div>
		
		<script>
			$(document).ready(function(){
				$('.home-menu').on('click',function(){window.location.href="home.php";});
				$('.statistics-menu').on('click',function(){window.location.href="statistics.php";});
				$('.settings-menu').on('click',function(){window.location.href="settings.php";});
			});
		</script>
		
	</div>
	<div id="divider"></div>
