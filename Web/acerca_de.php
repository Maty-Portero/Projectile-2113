<!DOCTYPE html>

<head>
    <?php require_once "includes/header.php"; ?>
    <?php require_once "includes/style-home.php"; ?>
</head>

<body>
    <header>
        <div class="logo">
            <img src="https://cdn.discordapp.com/attachments/1225891660987371672/1232107890534514779/Radio_1-removebg-preview.png?ex=66284164&is=6626efe4&hm=fd2b4db38674a175ef0c04642f91fe32a017126f83624a1aa65f7d232b6df33a&" style="height: 80px; width: 80px; padding: 5px; margin: 5px;">
            <strong style="font-size: 25px;">Radio París</strong>
        </div>
        <div class="header-right">
            <nav class="asuntos">
                <a href="home_log.php">Inicio</a>
                <a href="sobre_nosotros_log.php">Sobre Nosotros</a>
            </nav>
            <button class="btnlogin"><a href="login_register/index.php">Login</a></button>
        </div>

    </header>

    <main> <!--para contenido principal-->
        <section class="py-5 text-center inicio">
            <div>
                <div class="col-lg-6 col-md-8 mx-auto informa">
                    <img style="height: 180px; width: 180px; margin: 10px" src="https://cdn.discordapp.com/attachments/1225891660987371672/1232749197267177524/image.png?ex=662be828&is=662a96a8&hm=ea18fe256db2dd170479d354d5c30ef0f85df7c59b6048ae41344278852a1f27&">
                    <h1 class="fw-light">Project Projectile 2113</h1>
                    <h3 class="fw-light">¿Qué es?</h3><br>
                    <p style="color: #000; font-size: 20px; text-align: left;">“Project Projectile 2113” es un juego Bullet Hell al estilo Arkanoid donde el jugador controla una vehículo de guerra o un soldado cuya estadísticas varían según los niveles en donde se encuentra cada uno, el cual debe atravesar distintos niveles con grandes cantidades de enemigos que disparan proyectiles y el jugador debe esquivarlas y derribar a sus oponentes. <br><br>

                    Será un juego un solo jugador. Donde habrá 5 escenarios con 5 stages cada uno, con un total de 25 niveles. Al final de cada escenario habrá un jefe que será de mayor dificultad.<br><br>

                    La temática de este videojuego estará basada en una Guerra Futurista en el año 2113.<br><br>

                    La metodología usada en nuestro grupo es la SCRUM. El juego está diseñado para todas las edades.
                    </p>
                </div>
            </div>
        </section>
    </main>

    <footer class="futer py-5">
        <?php require_once "includes/footer.php"; ?>
    </footer>

</body>

</html>