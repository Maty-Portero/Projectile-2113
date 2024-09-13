using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.Controls;
using System;
using System.Collections.Generic;

namespace Project.States
{
    internal class GameOverState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;
        private int _finalScore; // Variable para almacenar el puntaje final
        private SpriteFont _font; // Fuente para el puntaje

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager, int finalScore)
    : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;
            _finalScore = finalScore; // Asignar el puntaje final correctamente

            // Cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            // Asignar la fuente para mostrar el puntaje
            _font = titleFont;

            var title = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(820, 50),
                Text = "Game Over",
            };

            // Crear el botón de reiniciar
            var restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 400),
                Text = "Restart Level",
            };

            restartButton.Click += restartButton_Click;

            // Crear el botón de volver
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 525),
                Text = "Back to Menu",
            };

            backButton.Click += backButton_Click;

            _components = new List<Component>()
    {
        title,
        backButton,
        restartButton
    };
        }


        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            // Dibujar el puntaje en la pantalla
            string scoreText = $"Score: {_finalScore}";
            Vector2 scorePosition = new Vector2(820, 150); // Ajusta la posición según lo necesites
            spriteBatch.DrawString(_font, scoreText, scorePosition, Color.White);

            spriteBatch.End();
        }


        private void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            // Saca los sprites si no los necesita
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
