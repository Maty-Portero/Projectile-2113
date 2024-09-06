using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.Controls;
using System;
using System.Collections.Generic;

namespace Project.States
{
    internal class OptionState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;

        public OptionState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;

            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // Crear los botones de resolución con eventos Click
            var button720p = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(210, 400),
                Text = "720p",
            };
            button720p.Click += (sender, e) => ChangeResolution(1280, 720);

            var button1080p = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(210, 500),
                Text = "1080p",
            };
            button1080p.Click += (sender, e) => ChangeResolution(1920, 1080);

            var button4k = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(210, 600),
                Text = "4K",
            };
            button4k.Click += (sender, e) => ChangeResolution(2560, 1440);

            var button8k = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(210, 700),
                Text = "8K",
            };
            button8k.Click += (sender, e) => ChangeResolution(3840, 2160);

            // Crear los botones de modo de pantalla
            var fullscreenButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 400), // Ubicación central
                Text = "Full Screen",
            };
            fullscreenButton.Click += (sender, e) => SetFullScreen(true);

            var windowedButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 500), // Justo debajo del botón de pantalla completa
                Text = "Windowed",
            };
            windowedButton.Click += (sender, e) => SetFullScreen(false);

            // Crear los botones de volumen a la derecha
            var activateVolumeButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1210, 400), // Posición a la derecha
                Text = "Activate Volume",
            };

            var muteButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(1210, 500), // Justo debajo del botón de activar volumen
                Text = "Mute",
            };

            // Crear el botón de volver
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 925),
                Text = "Back",
            };
            backButton.Click += backButton_Click;

            // Agregar los componentes a la lista
            _components = new List<Component>()
            {
                button720p,
                button1080p,
                button4k,
                button8k,
                fullscreenButton,
                windowedButton,
                activateVolumeButton,
                muteButton,
                backButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void ChangeResolution(int width, int height)
        {
            // Cambiar la resolución del juego
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        private void SetFullScreen(bool isFullScreen)
        {
            // Cambiar entre pantalla completa y modo ventana
            _graphics.IsFullScreen = isFullScreen;
            _graphics.ApplyChanges();
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //saca los sprites si no los necesita
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
