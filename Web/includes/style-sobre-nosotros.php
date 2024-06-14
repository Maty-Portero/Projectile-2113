<style>
    @import url('https://fonts.googleapis.com/css2?family=Instrument+Sans&display=swap');

    * {
        font-family: 'Instrument Sans', sans-serif;
    }

    main {
        background-color: #331173;
        color: #000;
    }


    @keyframes fadeIn {
        0% {
            opacity: 0;
        }

        100% {
            opacity: 1;
        }
    }

    /* header */
    header {
        width: 100%;
        background-color: rgb(255, 255, 255);
        display: flex;
        justify-content: space-between;
        align-items: center;
        height: 90px;
    }

    .logo {
        display: flex;
        align-items: center;
        color: #000;
    }

    .asuntos a {
        position: relative;
        font-size: 120%;
        color: black;
        text-decoration: none;
        font-weight: 500;
        margin-right: 30px;
    }

    .asuntos a::after {
        content: '';
        position: absolute;
        width: 100%;
        height: 3px;
        background: #000;
        border-radius: 5px;
        left: 0;
        bottom: -6px;
        transform: scaleX(0);
        transition: transform .5s;
    }

    .asuntos a:hover::after {
        transform-origin: left;
        transform: scaleX(1);
    }

    .header-right {
        display: flex;
        align-items: center;
    }

    .header-right button {
        background: transparent;
        border: 2px solid #000;
        border-radius: 6px;
        cursor: pointer;
        font-size: 20px;
        color: #000;
        margin-right: 30px;
    }

    .header-right button:hover {
        background: #000;
    }

    .header-right button a {
        text-decoration: none;
        cursor: pointer;
        font-size: 20px;
        color: #000;
        padding: 20px;
    }

    .header-right button a:hover {
        color: rgb(255, 255, 255);
    }

    #nombreUsuarioContainer {
        margin-right: 30px;
    }

    #nombreUsuario {
        background-color: #331173;
        color: #ffffff;
        padding: 10px 16px;
        border: 2px solid #331173;
        border-radius: 15px;
    }

    h1{
        color: #ffffff;
    }

    #opcionesUsuario {
        font-size: 20px;
        display: none;
        position: absolute;
        background-color: #f1f1f1;
        min-width: 160px;
        box-shadow: 0px 2px 16px 0px rgba(0, 0, 0, 0.2);
    }

    #opcionesUsuario a {
        color: black;
        padding: 12px 16px;
        text-decoration: none;
        display: block;
    }

    #opcionesUsuario a:hover {
        background-color: #C6D1DF;
    }

    #nombreUsuarioContainer:hover #opcionesUsuario {
        display: block;
    }

    .inicio {
        background-color: #a5bef7;
        color: #000;
    }

</style>