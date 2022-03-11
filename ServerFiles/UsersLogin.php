<?php

    $connection = mysqli_connect("localhost","id13076363_seifmostafa","dite4P6uyd_CTv8","id13076363_homex") or die ("Error in first part");

    $UserNAme = $_POST['Name'];
    $Password = $_POST['Password'];

    if(mysqli_connect_error()> 0 )
    {
        die("Error in connection \t#0");
    }
    echo 'Connection established';

    $NameCheck = mysqli_query($connection,"SELECT `username`,`password` FROM `users` WHERE `username` = '$UserNAme' ") ;

    if(mysqli_num_rows($NameCheck) > 1 )
    {
        die("there was a bug in your session please retry late \t#1");
    }
    else if(mysqli_num_rows($NameCheck) == 0 )
    {
        die("No User Registered.\t#1");
    }
    
    if($NameCheck){
        $savedpassword = mysqli_fetch_assoc($NameCheck);
        $hashed = $savedpassword['password'];
    }else{die("No Data\t#ERROR NO TABLE FOUND");}
    if(password_verify ($Password ,$hashed))
    {
        echo " Login Successful \t#NoERRORS";
       
    }else{ echo " Wrong Password entered\t#WrongPass";}
?>
