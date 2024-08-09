using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Project.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project.States
{
    internal class GameOverState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;

        public GameOverState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            var title = new Title(titleTexture, titleFont)
            {
                Position = new Vector2(820, 50),
                Text = "Game Over",
            };

            //creamos el boton de volver
            var restartButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 400),
                Text = "Restart Level",
            };

            restartButton.Click += restartButton_Click;

            //creamos el boton de volver
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
            //saca los sprites si no los necesita
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var component in _components)
                component.Update(gameTime);
        }
    }
}
