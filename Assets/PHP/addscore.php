<?php
    $host = "109.106.242.224"; // Host name
    $db_username = "u155534022_N21D"; // Mysql username
    $db_password = "w1Cin4N~y"; // Mysql password
    $db_name = "u155534022_MXcnF"; // Database name

    
    $scoreboard_connection = mysqli_connect($host, $db_username, $db_password, $db_name) or die("Cannot connect!");
    if ($scoreboard_connection -> connect_errno) 
    {
        die("Failed to connect to MySQL: " . $scoreboard_connection -> connect_error);
    }
    if(isset($_POST["username"]) && isset($_POST["score"]))
    {
        $username = $scoreboard_connection -> real_escape_string($_POST["username"]);
        $score = $scoreboard_connection -> real_escape_string($_POST["score"]);

        $find = "SELECT * FROM Scoreboard WHERE username = '" . $username . "'";
        $result = $scoreboard_connection->query($find);
        if($result->num_rows > 0)
        {
            $replace = "UPDATE Scoreboard SET score = '" . $score . "' WHERE username = '" . $username . "'";
            if($scoreboard_connection->query($replace) === TRUE)
            {
                print "Succesfully updated record";
            }
            else
            {
                die("Error: " . $replace . "<br>" . $scoreboard_connection->error);
            }
        }
        else
        {
            $add = "INSERT INTO Scoreboard (username, score) VALUES (?, ?)";
            $stmt = $scoreboard_connection->prepare($add);
            $stmt->bind_param("si", $username, $score);
            
            if($stmt -> execute() === TRUE)
            {
                print "New record created succesfully";
            }
            else {
                die("Error: " . $stmt . "<br>" . $scoreboard_connection->error);
            }
        }

    }
    else
    {
        print "Missing input data!";
    }
?>