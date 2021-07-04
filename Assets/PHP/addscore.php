<?php
    $host = "localhost"; // Host name
    $db_username = "u155534022_N21D"; // Mysql username
    $db_password = "w1Cin4N~y"; // Mysql password
    $db_name = "u155534022_MXcnF"; // Database name

    $scoreboard_connection = mysqli_connect($host, $db_username, $db_password, $db_name) or die("Cannot connect!");
    
    if(isset($_POST["user_id"]) && isset($_POST["username"]) && isset($_POST["score"])){
        $userid = mysqli_real_escape_string($_GET["user_id"], $scoreboard_connection);
        $username = mysqli_real_escape_string($_GET["username"], $scoreboard_connection);
        $score = mysqli_real_escape_string($_GET["score"], $scoreboard_connection);

        require dirname(__FILE__).'/database.php';
        $sth = $scoreboard_connection->prepare("INSERT INTO Scoreboard (username, user_id, score) VALUES (?, ?, ?)")
                                    ->bind_param("ssi", $username, $userid, $score);
                                    

        
    }
    else
    {
        echo "Missing input data!";
    }
