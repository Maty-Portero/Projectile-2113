using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.Controls;
using System;
using System.Collections.Generic;

namespace Project.States
{
    internal class GameOverState : NavigableState
    {
        private GraphicsDeviceManager _graphics;
        private int _finalScore; // Variable para almacenar el puntaje final
        private int _finalScore2;
        private SpriteFont _font; // Fuente para el puntaje

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager, int finalScore,
            int finalScore2)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _graphics = deviceManager;
            _finalScore = finalScore; // Asignar el puntaje final correctamente
            _finalScore2 = finalScore2;
            playerData.highscore = _finalScore;
            // Cargar texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            if (_game.estado == 2) game.Leaderboard.AddEntry($"ID#{game.Leaderboard.GetAllEntries().Count}", _finalScore, "stage");
            else if (_game.estado == 3) game.Leaderboard.AddEntry($"ID#{game.Leaderboard.GetAllEntries().Count}", _finalScore, "infinite");
            // Asignar la fuente para mostrar el puntaje
            _font = titleFont;

            // Crear el título de "Game Over"
            var title = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 50),
                Text = "Game Over",
            };

            // Crear el botón de "Restart Level"
            var restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 400),
                Text = "Restart Level",
            };
            restartButton.Click += RestartButton_Click;

            // Crear el botón de "Back to Menu"
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 525),
                Text = "Back to Menu",
            };
            backButton.Click += BackButton_Click;

            // Dibujar el puntaje en la pantalla
            var scoreText = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 150),
                Text = $"Score: {_finalScore}",

            };
            // Dibujar el puntaje en la pantalla
            var scoreText2 = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(600, 200),
                Text = $"Score 2: {_finalScore2}",

            };

                // Agregar los componentes a la lista

                _components.Add(restartButton);
            _components.Add(backButton);
            _components.Add(title);
            _components.Add(scoreText);
            if (_game.estado == 4)
            {
                _components.Add(scoreText2 );
            }
            _selectedIndex = 0; // Inicializar el índice del botón seleccionado
            _limitIndex = 2;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        private void RestartButton_Click(object sender, EventArgs e)
        {
            if (_game.estado == 2)
                _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
            else if (_game.estado == 3)
                _game.ChangeState(new GameState2(_game, _graphicsDevice, _content, _graphics));
            else if (_game.estado == 4)
                _game.ChangeState(new GameState3(_game, _graphicsDevice, _content, _graphics));
        }

        // Lógica post-actualización (opcional, si es necesario)
        public override void PostUpdate(GameTime gameTime)
        {
            // No es necesario hacer nada adicional aquí por ahora
        }
    }
}
