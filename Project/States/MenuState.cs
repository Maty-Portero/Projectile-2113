using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.Controls;
using System;
using System.Collections.Generic;

namespace Project.States
{
    internal class MenuState : NavigableState
    {
        private GraphicsDeviceManager _graphics;
        // Variables de fondo
        private Texture2D backgroundTexture;
        private Vector2 bgPosition1, bgPosition2;
        private float bgSpeed = 50f; // Ajusta la velocidad del desplazamiento

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;
            _game.estado = 0;

            // Cargar textura de fondo
            backgroundTexture = content.Load<Texture2D>("bgStreets1");

            // Inicializar posiciones para el scrolling
            bgPosition1 = Vector2.Zero;
            bgPosition2 = new Vector2(0, -backgroundTexture.Height);

            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            // Crear título
            var title = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 50),
                Text = "Projectile 2113",
            };

            var gameby = new Title(titleTexture, buttonFont)
            {
                Position = new Vector2(1685, 950),
                Text = "Game by Radio Paris",
            };

            var logInButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(10, 950),
                Text = "Log In",
            };

            if (playerData.loggedIn == true)
            {
                logInButton.Text = "Log Out";
            }

            logInButton.Click += LogInButton_Click;

            // Crear botones
            var playButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 275),
                Text = "Play",
            };
            playButton.Click += PlayButton_Click;

            var helpButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 400),
                Text = "How To Play",
            };
            helpButton.Click += HelpButton_Click;

            var optionsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 525),
                Text = "Settings",
            };
            optionsButton.Click += OptionsButton_Click;

            var exitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 650),
                Text = "Exit",
            };
            exitButton.Click += ExitButton_Click;

            // Añadir componentes a la lista
            _components.Add(playButton);
            _components.Add(helpButton);
            _components.Add(optionsButton);
            _components.Add(exitButton);
            _components.Add(logInButton);
            //_components.Add(title);
            _components.Add(gameby);

            _selectedIndex = 0; // Inicializar el índice seleccionado
            _limitIndex = 1;
        }

        // Métodos que responden a los eventos de clic
        private void PlayButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new modeGameState(_game, _graphicsDevice, _content, _graphics));
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new OptionState(_game, _graphicsDevice, _content, _graphics));
        }

        private void HelpButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new HelpState(_game, _graphicsDevice, _content, _graphics));
        }

        private void LogInButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new LoadScreenLoggedInState(_game, _graphicsDevice, _content, _graphics));
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            _game.Exit(); // Salir del juego
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Actualizar la posición del fondo
            bgPosition1.Y += bgSpeed * deltaTime;
            bgPosition2.Y += bgSpeed * deltaTime;

            // Reiniciar posiciones para el efecto de bucle
            if (bgPosition1.Y >= backgroundTexture.Height)
            {
                bgPosition1.Y = bgPosition2.Y - backgroundTexture.Height;
            }
            if (bgPosition2.Y >= backgroundTexture.Height)
            {
                bgPosition2.Y = bgPosition1.Y - backgroundTexture.Height;
            }
        }
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            base.Draw(gameTime, spriteBatch);
            spriteBatch.Begin();

            // Dibujar el fondo desplazado
            spriteBatch.Draw(backgroundTexture, bgPosition1, Color.White);
            spriteBatch.Draw(backgroundTexture, bgPosition2, Color.White);

            // Dibujar los demás elementos del menú
            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }
        // Este método es opcional, lo mantienes si necesitas hacer limpieza después de actualizar
        public override void PostUpdate(GameTime gameTime)
        {
            // Opcional: lógica post-actualización
        }
    }
}
