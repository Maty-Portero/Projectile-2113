using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.Controls;
using System;
using System.Collections.Generic;

namespace Project.States
{
    internal class OptionState : NavigableState
    {
        private GraphicsDeviceManager _graphics;

        public OptionState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;

            _game.estado = 6;

            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // Crear los botones de modo de pantalla
            var fullscreenButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 400), // Ubicación central
                Text = "Full Screen",
            };
            fullscreenButton.Click += (sender, e) => SetFullScreen(true);

            var windowedButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 500), // Justo debajo del botón de pantalla completa
                Text = "Windowed",
            };
            windowedButton.Click += (sender, e) => SetFullScreen(false);

            // Crear el botón de volver
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 925),
                Text = "Back",
            };
            backButton.Click += BackButton_Click;

            // Agregar los botones a la lista de componentes
            _components.Add(fullscreenButton);
            _components.Add(windowedButton);
            _components.Add(backButton);

            _selectedIndex = 0; // Inicializar el índice seleccionado
        }

        // Método que maneja el evento de clic en el botón de "Back"
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        // Cambiar entre pantalla completa y modo ventana
        private void SetFullScreen(bool isFullScreen)
        {
            _graphics.IsFullScreen = isFullScreen;
            _graphics.ApplyChanges();
        }

        // Cambiar la resolución del juego (no se usa en este ejemplo pero queda para futuras mejoras)
        private void ChangeResolution(int width, int height)
        {
            _graphics.PreferredBackBufferWidth = width;
            _graphics.PreferredBackBufferHeight = height;
            _graphics.ApplyChanges();
        }

        // Lógica post-actualización, opcional si quieres realizar acciones adicionales después de actualizar
        public override void PostUpdate(GameTime gameTime)
        {
            // No es necesario hacer nada adicional aquí por ahora
        }
    }
}
