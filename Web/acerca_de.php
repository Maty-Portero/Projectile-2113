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
                <a href="home_log.php">Inicio</a>
                <a href="sobre_nosotros_log.php">Sobre Nosotros</a>
            </nav>
            <div id="nombreUsuarioContainer">
                <span id="nombreUsuario" style="font-size: 20px;"><?php echo $usuario; ?></span>
                <div id="opcionesUsuario">
                    <a href="">Cerrar sesión</a>
                </div>
            </div>
        </div>

    </header>

    <main> <!--para contenido principal-->
        <section class="py-5 text-center inicio">
            <div>
                <div class="col-lg-6 col-md-8 mx-auto informa">
                    <img style="height: 180px; width: 180px; margin: 10px" src="fts/Projectile_2113_Logo_2.png">
                    <h1 class="fw-light">Projectile 2113</h1>
                    <h3 class="fw-light">¿Qué es?</h3><br>
                    <p style="color: #ffffff; font-size: 20px; text-align: left;">“Project Projectile 2113” es un juego Bullet Hell al estilo Arkanoid donde el jugador controla una vehículo de guerra o un soldado cuya estadísticas varían según los niveles en donde se encuentra cada uno, el cual debe atravesar distintos niveles con grandes cantidades de enemigos que disparan proyectiles y el jugador debe esquivarlas y derribar a sus oponentes. <br><br>

                        Será un juego single player. Donde habrá 5 escenarios con 5 stages cada uno, con un total de 25 niveles. Al final de cada escenario habrá un jefe que será de mayor dificultad.<br><br>

                        La temática de este videojuego estará basada en una Guerra Futurista en el año 2113.<br><br>

                        La metodología usada en nuestro grupo es la SCRUM. El juego está diseñado para todas las edades.
                    </p><br>
                    <h4>Ahora, te vas a preguntar, ¿A quién le vendemos este juego?</h4>
                    <button type="button" style="background-color: #007bff;"><a style="color: #ffffff; text-decoration:none;" href="https://www.dotemu.com/">DOTEMU</a></button><br>
                    <h4>¿Queres saber más de Dotemu? clickeá acá</h4>
                    <button type="button" style="background-color: #007bff;"><a style="color: #ffffff; text-decoration:none;" href="https://www.dotemu.com/about/">Saber más</a></button><br><br><br>

                    <h4>¿Queres dejarnos una opinión? Clickeá en este botón</h4>
                    <button type="button" style="background-color: #007bff;"><a style="color: #ffffff; text-decoration:none;" href="../Encuesta/index.html">Ir a la Encuesta</a></button>
                </div>
            </div>
        </section>
    </main>

    <footer class="futer py-5">
        <?php require_once "includes/footer.php"; ?>
    </footer>

</body>

</html>