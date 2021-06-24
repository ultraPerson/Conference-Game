<?php
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
