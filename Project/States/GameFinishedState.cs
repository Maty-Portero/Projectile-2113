using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.Controls;
using System;

namespace Project.States
{
    internal class GameFinishedState : NavigableState
    {
        private GraphicsDeviceManager _graphics;
        private int _finalScore; // Variable para almacenar el puntaje final
        private SpriteFont _font; // Fuente para el puntaje

        public GameFinishedState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager, int finalScore)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;
            _finalScore = finalScore; // Asignar el puntaje final correctamente
            game.Leaderboard.AddEntry($"ID#{game.Leaderboard.GetAllEntries().Count}", _finalScore, "stage+");
            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            // Asignar la fuente para mostrar el puntaje
            _font = titleFont;

            // Crear el título de "Game Over"
            var title = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 50),
                Text = "Congratulations, you have finished the stage mode!!\n Test the infite mode",
            };

            // Crear el botón de "Back to Menu"
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 400),
                Text = "Back to Menu",
            };
            backButton.Click += BackButton_Click;

            // Dibujar el puntaje en la pantalla
            var scoreText = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 150),
                Text = $"Score: {_finalScore}",

            };
                // Agregar los componentes a la lista

            _components.Add(backButton);
            _components.Add(title);
            _components.Add(scoreText);
            _selectedIndex = 0; // Inicializar el índice del botón seleccionado
            _limitIndex = 2;
        }

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
