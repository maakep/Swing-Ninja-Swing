<?php
if ($_POST['apikey'] == "A_key_of_your_choosing"){

	class DbLayer {
		public function DbLayer () {
			$con = new mysqli("host","user","pwd","db");
			if ($con->connect_errno)
			{
				echo "Failed to connect to db: " . $con->connect_error;
			}
			

			if ($_POST['select'] == "true") 
			{
				$name = $con->real_Escape_string($_POST['name']);
				$user = $con->real_Escape_string($_POST['user']);
				$pwd = $con->real_Escape_string($_POST['pwd']);
				$getLevel = $con->real_Escape_string($_POST['getlevel']);

				if ($name || $user && $getLevel == "true") {
					DbLayer::GetLevel($name, $user, $con);
				} elseif ($user && $pwd) {
					DbLayer::LoginUser($user, $pwd, $con);
				}
			}

			if ($_POST['insert'] == "true") {
				$name = $con->real_Escape_string($_POST['name']);
				$level = $con->real_Escape_string($_POST['level']);
				$user = $con->real_Escape_string($_POST['user']);
				$pwd = $con->real_Escape_string($_POST['pwd']);
				
				if ($level && $name && $user) {
					DBLayer::InsertLevel($level, $name, $user, $con);
				} elseif ($user && $pwd) {
					DBLayer::CreateUser($user, $pwd, $con);
				}
			}

			if ($_POST['delete'] == "true"){
				$name = $con->real_Escape_string($_POST['name']);
				$user = $con->real_Escape_string($_POST['user']);
				$q = "delete from NinjaRope_Levels where Name = '" . $name . "' and Username = '" . $user . "'";

				if ($res = $con->query($q)){
					echo 'Level deleted';
				} else {
					echo $con->error;
				}
			}
		}

		public function Update($id, $level, $con){
			if ($res = $con->query('update NinjaRope_Levels set SerializedLevel = "' . $level . '", LastModified = default where Id = "' . $id . '"')){
				echo 'Update done: ' . $id;
			} else {
				echo $con->error;
			}
		}

		public function InsertLevel($level, $name, $user, $con){
			// If it already exists, we update that level instead.
			if ($res = $con->query('select * from NinjaRope_Levels where Name = "' . $name . '" and Username = "' . $user . '"')){
				if($res->num_rows > 0){
					$row = $res->fetch_assoc();
					$firstId = $row['ID'];
					DbLayer::Update($firstId, $level, $con);
				} else {
					if ($res = $con->query('insert into NinjaRope_Levels (Name, Username, SerializedLevel) values ("' . $name . '", "' . $user . '", "' . $level . '")')){
						echo 'Level saved';
					} else {
						echo $con->error;
					}
				}
			} else {
				echo $con->error;
			}
		}

		public function CreateUser($user, $pwd, $con){
			$q = 'select * from NinjaRope_Users where Username = "' . $user . '"';
			if ($res = $con->query($q)) {
				if($res->num_rows > 0){
					echo 'Username already used';
				} else {
					$q = 'insert into NinjaRope_Users (Username, Password, LastLoggedIn) values ("' . $user . '", "' . $pwd . '", now())';
					if ($res = $con->query($q)) {
						echo 'User created';
					} else {
						echo 'Error creating user - ' . $con->error;
					}
				}				
			} else {
				echo $con->error;
			}
		}

		public function LoginUser($user, $pwd, $con) {
			$q = 'select * from NinjaRope_Users where Username = "' . $user . '" and Password = "' . $pwd . '"';
			if ($res = $con->query($q)) {
				if ($res->num_rows > 0) {
					$row = $res->fetch_assoc();
					$firstId = $row['Username'];
					$q = 'update NinjaRope_Users set LastLoggedIn = now() where Username = "' . $user . '"';
					if ($res = $con->query($q)) {
						header('Success: true');
						echo $firstId;
					} else {
						header('Success: false');
						echo $con->error;
					}
				} else {
					header('Success: false');
					echo 'Wrong Username or Password';
				}
			} else {
				echo $con->error;
			}
		}

		public function GetLevel($name, $user, $con){
			$q = "select SerializedLevel from NinjaRope_Levels";

			if ($name) {
				$q .= ' where Name = "' . $name . '"';
			} elseif ($user) {
				$q .= ' where Username = "' . $user . '"';
			}
			$res = $con->query($q);
			if($res->num_rows > 0){
				echo $res->fetch_row()[0];
			} else {
				echo 'Error';
			}
		}
	}

	new DbLayer();
}
?>