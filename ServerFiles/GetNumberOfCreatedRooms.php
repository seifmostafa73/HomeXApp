<?php

    $connection = mysqli_connect("localhost","id13076363_seifmostafa","dite4P6uyd_CTv8","id13076363_homex") or die ("Error in first part");


    $UserNAme = $_POST['Name'];

    if(mysqli_connect_error()> 0 )
    {
        die("Error in connection \t#0");
    }

    $NameCheck = mysqli_query($connection,"SELECT `username` FROM `rooms` WHERE `username` = '$UserNAme' ") ;
    
    echo mysqli_num_rows($NameCheck)."\t#NoERRORS";
?>
