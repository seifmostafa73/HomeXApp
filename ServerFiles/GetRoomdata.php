<?php

    $connection = mysqli_connect("localhost","id13076363_seifmostafa","dite4P6uyd_CTv8","id13076363_homex") or die ("Error in first part");


    $UserNAme = $_POST['Name'];

    if(mysqli_connect_error()> 0 )
    {
        die("Error in connection \t#0");
    }

    $NameCheck = mysqli_query($connection,"SELECT `roomname`,`roomtype`,`NoDevices` FROM `rooms` WHERE `username` = '$UserNAme'") or die("Error") ;
    
    $allRows = [];

    while($row = mysqli_fetch_assoc($NameCheck)) {
        array_push($allRows,$row);
    }  
    for($i=0;$i<mysqli_num_rows($NameCheck);$i++){
        foreach($allRows[$i] as $e)
        {
            echo$e."-";
        }
        echo "\n";
    }
?>
