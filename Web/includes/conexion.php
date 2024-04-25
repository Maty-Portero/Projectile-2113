<?php
$conexion = mysqli_connect("localhost", "root", "", "projectile2113");

if($conexion){
    echo "conexion exitosa"."<br>";
}else{
    echo "conexion fallida"."<br>";
}
?>