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
  
  <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
</head>

<body>
	<div id="avatar">
		<div>
			<div id="change-avatar">&#9786;</div>
			<img src="img/Avatar.jpg">
		</div>
	</div>
	<div id="header">
		<div id="right-top">
			<button class="button"></button>
			<form>	
				<input type="text" name="search" placeholder="Search..">
			</form>
		</div>
		<div id="menu">
			<div class="menu-item home-menu"><p><a>Home</a></p></div>
			<div class="menu-item statistics-menu"><p><a>Statistics</a></p></div>
			<div class="menu-empty"></div>
			<div class="menu-item dropdown">
				<p><a class="dropbtn">Friends</a></p>
				<div class="dropdown-content">
					<p><a>View friends</a></p>
					<p><a>Friend Forum</a></p>
					<p><a>Add friend</a></p>
				</div>
			</div>
			<div class="menu-item"><p><a>Settings</a></p></div>
		</div>
		<script>
			$('.home-menu').on('click',function(){window.location.href="main.php";});
			$('.statistics-menu').on('click',function(){window.location.href="statistics.php";});
		</script>
		
	</div>
	<div id="divider"></div>
