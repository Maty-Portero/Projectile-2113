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
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            //creamos el boton de cambiar a res1
            var res1Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(50, 200),
                Text = "800x400",
            };

            res1Button.Click += res1Button_Click;

            //creamos el boton de cambiar a res2
            var res2Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(230, 200),
                Text = "1280x720",
            };

            res2Button.Click += res2Button_Click;

            //creamos el boton de cambiar a res2
            var res3Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(410, 200),
                Text = "1920x1080",
            };

            res3Button.Click += res3Button_Click;

            //creamos el boton de cambiar a res2
            var res4Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(590, 200),
                Text = "Full Screen",
            };

            res4Button.Click += res4Button_Click;

            //creamos el boton de volver
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(320, 300),
                Text = "Back",
            };

            backButton.Click += backButton_Click;

            _components = new List<Component>()
            {
                res1Button,
                res2Button,
                res3Button,
                res4Button,
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

        private void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        private void res1Button_Click(object sender, EventArgs e)
        {
            //aa
        }

        private void res2Button_Click(object sender, EventArgs e)
        {
            //aa
        }

        private void res3Button_Click(object sender, EventArgs e)
        {
            //aa
        }

        private void res4Button_Click(object sender, EventArgs e)
        {
            //aa
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
