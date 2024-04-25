<?php

require_once "../includes/conexion.php";

$nombre_completo = $_POST['nombre_completo'];
$correo = $_POST['correo'];
$usuario = $_POST['usuario'];
$contrasena = $_POST['contrasena'];

$query = "INSERT INTO usuarios(nombre_completo, correo, usuario, contrasena) 
          VALUES('$nombre_completo', '$correo', '$usuario', MD5('$contrasena'))";

//verificar que el correo no se repita en la database
$verificar_correo = mysqli_query($conexion, "SELECT * FROM usuarios WHERE correo ='$correo'");

if (mysqli_num_rows($verificar_correo) > 0) {
    echo '
        <script>
            alert("Este correo ya existe, intentalo de nuevo");
            window.location = "index.php";
        </script>
    ';
    exit();
}
//verificar que el usuario no se repita en la database
$verificar_usuario = mysqli_query($conexion, "SELECT * FROM usuarios WHERE usuario ='$usuario'");

if (mysqli_num_rows($verificar_usuario) > 0) {
    echo '
        <script>
            alert("Este usuario ya existe, intentalo de nuevo");
            window.location = "index.php";
        </script>
    ';
    exit();
}
$ejecutar = mysqli_query($conexion, $query);

if ($ejecutar) {
    echo '
        <script>
            alert("Usuario Registrado");
            window.location = "index.php";
        </script>
    ';
} else {
    echo '
        <script>
            alert("Intente Nuevamente");
            window.location = "index.php";
        </script>
    ';
}
mysqli_close($conexion);

?>