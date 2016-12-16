<?php 
include('header.php');
?>
	<div class="main">
		<div class="list width-list">
			<div id="forum">
				<div id="forum_name" class="forum Name">
					<h2>FORUM</h2>
				</div>
				<div id="forum_port" class="forum Port">
					<div id="messagesPort" class="messagesPort"></div>
				</div>
				<div id="forum_form" class="forum Form">
					<textarea id="inputField" type="text" name="newmessage" placeholder="Insert message"></textarea>
					<input id="sendButton" type="submit" value="POST">
				</div>
			</div>
			<script src="js/chat.js"></script>
		</div>
	</div>
<?php
include('footer.php');
?>