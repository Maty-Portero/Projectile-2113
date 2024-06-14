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
    internal class MenuState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;

        public MenuState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {

            _graphics = deviceManager;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");


            //creamos el boton de play
            var playButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Play",
            };

            playButton.Click += playButton_Click;

            //creamos el boton de how to play
            var helpButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "How To Play",
            };

            helpButton.Click += helpButton_Click;

            //creamos el boton de options
            var optionsButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Settings",
            };

            optionsButton.Click += optionsButton_Click;

            //creamos el boton de exit
            var exitButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 350),
                Text = "Exit",
            };

            exitButton.Click += exitButton_Click;

            _components = new List<Component>() 
            {
                playButton,
                helpButton,
                optionsButton,
                exitButton
            };
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);

            spriteBatch.End();
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
        }
        private void optionsButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new OptionState(_game, _graphicsDevice, _content, _graphics));
        }

        private void helpButton_Click(Object sender, EventArgs e)
        {
            _game.ChangeState(new HelpState(_game, _graphicsDevice, _content, _graphics));
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
