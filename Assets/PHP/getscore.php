<?php
    $host = "109.106.242.224"; // Host name
    $db_username = "u155534022_N21D"; // Mysql username
    $db_password = "w1Cin4N~y"; // Mysql password
    $db_name = "u155534022_MXcnF"; // Database name

    
    $scoreboard_connection = mysqli_connect($host, $db_username, $db_password, $db_name) or die("Cannot connect!");
    if ($scoreboard_connection -> connect_errno) {
        die("Failed to connect to MySQL: " . $scoreboard_connection -> connect_error);
    }
    $select = "SELECT username, score FROM Scoreboard";
    $result = $scoreboard_connection->query($select);

    if($result->num_rows > 0)
    {
        $output = "Successfully found records";
        while($row = $result->fetch_assoc())
        {
            $output = $output . "|" . $row["username"] . "," . $row["score"];
        }
        print $output;
    }
?>