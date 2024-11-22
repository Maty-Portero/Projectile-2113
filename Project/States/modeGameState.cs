using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project.Controls;

namespace Project.States
{
    internal class modeGameState : NavigableState
    {
        private GraphicsDeviceManager _graphics;

        public modeGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;

            _game.estado = 1;

            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            // Crear el botón de "Stage Mode"
            var mode1Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 275),
                Text = "Stage Mode",
            };
            mode1Button.Click += Mode1Button_Click;

            // Crear el botón de "Infinite Mode"
            var mode2Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 400),
                Text = "Infinite Mode",
            };
            mode2Button.Click += Mode2Button_Click;

            // Crear el botón de "Infinite Mode"
            var mode3Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 525),
                Text = "Co-Op Mode",
            };
            mode3Button.Click += Mode3Button_Click;

            // Crear el botón de "Back"
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 650),
                Text = "Back",
            };
            backButton.Click += BackButton_Click;

            // Agregar los botones a la lista de componentes
            _components.Add(mode1Button);
            // Si el jugador está logueado, agregar el botón "Infinite Mode" y el "Versus Mode"
            if (playerData.loggedIn)
            {
                _components.Add(mode2Button);
                _components.Add(mode3Button);
            }
            _components.Add(backButton);



            _selectedIndex = 0; // Inicializar el índice del botón seleccionado
        }

        // Método que maneja el clic en el botón "Stage Mode"
        private void Mode1Button_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
        }

        // Método que maneja el clic en el botón "Infinite Mode"
        private void Mode2Button_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content, _graphics));
        }

        // Método que maneja el clic en el botón "Versus Mode"
        private void Mode3Button_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState3(_game, _graphicsDevice, _content, _graphics));
        }

        // Método que maneja el clic en el botón "Back"
        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        // Lógica post-actualización (opcional, si es necesario)
        public override void PostUpdate(GameTime gameTime)
        {
            // No es necesario hacer nada adicional aquí por ahora
        }
    }
}
