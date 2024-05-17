<!DOCTYPE html>

<head>
    <?php require_once "includes/header.php"; ?>
    <?php require_once "includes/style-home.php"; ?>
</head>

<body>
    <header>
        <div class="logo">
            <img style="height: 75px; width: 75px; margin: 8px" src="fts/empresa_sin_fondo.png">
            <strong style="font-size: 25px;">Radio París</strong>
        </div>
        <div class="header-right">
            <nav class="asuntos">
                <a href="login_register/index.php">Acerca De</a>
                <a href="sobre_nosotros.php">Sobre Nosotros</a>
            </nav>
            <button class="btnlogin"><a href="login_register/index.php">Login</a></button>
        </div>

    </header>

    <main> <!--para contenido principal-->
        <section class="py-5 text-center inicio">
            <div>
                <div class="col-lg-6 col-md-8 mx-auto informa">
                    <img style="height: 180px; width: 180px; margin: 10px" src="fts/empresa_sin_fondo.png">
                    <h1 class="fw-light">Radio París</h1>
                    <h3 class="fw-light">¿Qué somos?</h3>
                    <p style="color: #ffffff; font-size: 20px">Radio Paris es un subgrupo de la casa desarrolladora "Typhoon Interactive" conformada por Matias Portero, Lautaro Rodriguez y Axel Matanza la cual tiene como objetivo el desarrollo de videojuegos de estilo Arcade, mientras que a su vez aprenden de programacion en equipo.
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