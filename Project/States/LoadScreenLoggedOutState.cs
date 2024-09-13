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
    internal class LoadScreenLoggedOutState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;

        public LoadScreenLoggedOutState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {

            _graphics = deviceManager;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            var gameby = new Title(titleTexture, buttonFont)
            {
                Position = new Vector2(1685, 950),
                Text = "Game by Radio Paris",
            };

            var explication = new Title(titleTexture, buttonFont)
            {
                Position = new Vector2(800, 300),
                Text = "You have logged out, you have lost access to the following contents: \n 'Infinite Mode' \n Log In again to be able to access to the aditional content.",
            };

            //creamos el boton de exit
            var acceptButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 650),
                Text = "OK",
            };

            acceptButton.Click += acceptButton_Click;

            _components = new List<Component>()
            {
                acceptButton,
                explication,
                gameby
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void acceptButton_Click(Object sender, EventArgs e)
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

        private void exitButton_Click(object sender, EventArgs e)
        {
            //hacemos q cierre el juegardovich
            _game.Exit();
        }
    }
}
