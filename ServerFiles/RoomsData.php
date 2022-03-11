<?php

    $connection = mysqli_connect("localhost","id13076363_seifmostafa","dite4P6uyd_CTv8","id13076363_homex") or die ("Error in first part");


    $UserNAme = $_POST['Name'];
    $RoomName = $_POST['Room_Name'];
    $RoomType = $_POST['Room_Type'];

    if(mysqli_connect_error()> 0 )
    {
        die("Error in connection \t#0");
    }

    //NameCheck = mysqli_query($connection,"SELECT `username` FROM `rooms` WHERE `username` = '$UserNAme' ") or die ("NameNotFound#1");
    
    $Insertion = "INSERT INTO `rooms`(`id`, `username`, `roomname`, `roomtype`) VALUES (NULL,'$UserNAme','$RoomName','$RoomType')" or die("Error in insertion\t#1");
    
    $UserDataInsert = mysqli_query($connection,$Insertion) or die ("Error in insertion\t#2");
    echo "Room Added \t#NoERRORS";
?>
