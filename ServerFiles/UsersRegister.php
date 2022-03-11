<?php

    $connection = mysqli_connect("localhost","id13076363_seifmostafa","dite4P6uyd_CTv8","id13076363_homex") or die ("Error in first part");

    $UserNAme = $_POST['Name'];
    $Password = $_POST['Password'];

    if(mysqli_connect_error()> 0 )
    {
        die("Error in connection \t#0");
    }

    $NameCheck = mysqli_query($connection,"SELECT `username` FROM `users` WHERE `username` = '$UserNAme' ") ;

    if(mysqli_num_rows($NameCheck) > 0 )
    {
        die("This username is already taken \t#1");
    }

    $hash = password_hash($Password, PASSWORD_DEFAULT); 
    
   // $Inseration = "INSERT INTO `users`(`id`,`username`,`password`)  VALUES (NULL,'$UserNAme','$hash')" or die("Error in insertion");

    $UserDataInsert = mysqli_query($connection,"INSERT INTO `users`(`id`, `username`, `password`) VALUES (NULL,'" .$UserNAme. "','" .$hash. "')") or die ("Failed in Registeration\t#2");// el 5** error feen hena 3shan 
                                                                                                  //l code kan sh8al w                                      bas w7do
    echo "User Registered Successfully \t#NoERRORS";
?>
