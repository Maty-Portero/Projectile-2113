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

            //creamos el boton de play
            var volumenButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Volumen",
            };

            volumenButton.Click += volumenButton_Click;

            //creamos el boton de options
            /*var optionsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Options",
            };

            optionsButton.Click += optionsButton_Click;

            //creamos el boton de exit
            var exitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Exit",
            };

            exitButton.Click += exitButton_Click;
            */
            _components = new List<Component>()
            {
                volumenButton
                /*optionsButton,
                exitButton*/
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void volumenButton_Click(object sender, EventArgs e)
        {
            //_game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            //throw new NotImplementedException();
        }
    }
}
