<?php
session_start(); // Iniciar la sesión al principio del archivo

// Verificar si hay un nombre de usuario en la sesión
if (isset($_SESSION['usuario'])) {
    $usuario = $_SESSION['usuario'];
} else {
    $usuario = "Invitado "; // o cualquier valor predeterminado si no hay un usuario registrado
}
?>

<!DOCTYPE html>

<head>
    <?php require_once "includes/header.php"; ?>
    <?php require_once "includes/style-home.php"; ?>
</head>

<body>
    <header>
        <div class="logo">
            <img src="fts/logo_claro_negro.png" style="height: 80px; width: 80px; padding: 5px; margin: 5px;">
            <strong style="font-size: 25px;">Radio París</strong>
        </div>
        <div class="header-right">
            <nav class="asuntos">
                <a href="acerca_de.php">Acerca De</a>
                <a href="sobre_nosotros_log.php">Sobre Nosotros</a>
            </nav>
            <div id="nombreUsuarioContainer">
                <span id="nombreUsuario" style="font-size: 20px;"><?php echo $usuario; ?></span>
                <div id="opcionesUsuario">
                    <a href="home.php">Cerrar sesión</a>
                </div>
            </div>
        </div>
    </header>

    <main> <!--para contenido principal-->
        <section class="py-5 text-center inicio">
            <div>
                <div class="col-lg-6 col-md-8 mx-auto informa">
                <img style="height: 180px; width: 180px; margin: 10px" src="fts/logo_oscuro_blanco.png">
                    <h1 class="fw-light">Radio París</h1>
                    <h3 class="fw-light">¿Qué somos?</h3>
                    <p style="color: #ffffff; font-size: 20px">Radio Paris es un subgrupo de la casa desarrolladora "Typhoon Interactive" conformada por Matias Portero, Lautaro Rodriguez y Axel Matanza la cual tiene como objetivo el desarrollo de videojuegos de estilo Arcade, mientras que a su vez aprenden de programacion en equipo.</p>
                </div>
            </div>
        </section>
    </main>

    <footer class="futer py-5">
        <?php require_once "includes/footer.php"; ?>
    </footer>

</body>

</html>