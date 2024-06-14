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
<!-- saved from url=(0049)https://getbootstrap.com/docs/5.3/examples/album/ -->

<head>
    <?php require_once "includes/header.php"; ?>
    <?php require_once "includes/style-sobre-nosotros.php"; ?>
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
                <a href="acerca_de.php">Acerca De</a>
            </nav>
            <div id="nombreUsuarioContainer">
                <span id="nombreUsuario" style="font-size: 20px;"><?php echo $usuario; ?></span>
                <div id="opcionesUsuario">
                    <a href="">Cerrar sesión</a>
                </div>
            </div>
        </div>
    </header>
    <main>
        <section class="py-5 text-center inicio" style="background-color: #802fde;">
            <div>
                <div class="col-lg-6 col-md-8 mx-auto informa">
                    <img style="height: 180px; width: 180px; margin: 8px" src="fts/logo_oscuro_morado.png">
                    <h1 class="fw-light">Sobre Nosotros</h1>
                </div>
            </div>
        </section>

        <div class="nosotros py-5">
            <div class="container">

                <div class="row row-cols-1 row-cols-sm-2 row-cols-md-3 g-3">
                    <div class="col" style="width: 400px;">
                        <div class="card shadow-sm">
                            <img style="height: 200px; width: 200px; margin-bottom: 20px; padding-left: 20px; padding-top: 20px;" src="fts/porter.png">
                            <div class="card-body">
                                <p class="card-text" style="color: black;"><b>Matías Portero</b></p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <p>
                                        <b>Scrum Master y Analista de Sistemas</b> <br>
                                        - Jugar F1.<br>
                                        - Fanático Neo Pistea, Trueno, YSY A y el Duko. <br>
                                        - Es el Smooth Operator.<br>
                                        - Le gusta el HTML, C#, PHP Y JS.
                                    </p>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col" style="width: 400px;">
                        <div class="card shadow-sm">
                            <img style="height: 200px; width: 200px; margin-bottom: 20px; padding-left: 20px; padding-top: 20px;" src="fts/rodra.png">
                            <div class="card-body">
                                <p class="card-text" style="color: black;"><b>Lautaro Rodríguez</b></p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="btn-group">
                                        <p>
                                            <b>Programador y Diseñador Gráfico</b> <br>
                                            - Jugar videojuegos <br>
                                            - Investigar hardware o desarrollo de videojuegos <br>
                                            - Desarrollar su propio juego <br>
                                            - Juntarse con amigos
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="col" style="width: 400px;">
                        <div class="card shadow-sm">
                            <img style="height: 200px; width: 200px; margin-bottom: 20px; padding-left: 20px; padding-top: 20px;" src="fts/choko.png">
                            <div class="card-body">
                                <p class="card-text" style="color: black;"><b>Axel Matanza</b></p>
                                <div class="d-flex justify-content-between align-items-center">
                                    <div class="btn-group">
                                        <p>
                                            <b>Programador y Especialista en Sonido</b> <br>
                                            - Investigar e indagar sobre hardware <br>
                                            - Escuchar música underground <br>
                                            - Videojuegar <br>
                                            - Filosofar <br>
                                            - Aprender sobre la programación e informatica en general
                                        </p>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </main>

    <footer class="futer py-5">
        <?php require_once "includes/footer.php"; ?>
    </footer>

    <script src="./Album example · Bootstrap v5.3_files/bootstrap.bundle.min.js.descarga" integrity="sha384-ENjdO4Dr2bkBIFxQpeoTz1HIcje39Wm4jDKdf19U8gI4ddQ3GYNS7NTKfAdVQSZe" crossorigin="anonymous"></script>
</body>

</html>