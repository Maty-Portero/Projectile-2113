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
    internal class LoadScreenLoggedInState : NavigableState
    {
        private GraphicsDeviceManager _graphics;

        public LoadScreenLoggedInState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content, deviceManager)
        {

            _graphics = deviceManager;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");

            var explication = new Title(titleTexture, buttonFont)
            {
                Position = new Vector2(600, 300),
                Text = "You've just logged in. You've gained accesed to aditional content: \n 'Infinite Mode' ",
            };

            if (playerData.loggedIn == false)
            {
                playerData.loggedIn = true;
            }
            else
            {
                playerData.loggedIn = false;
                explication.Text = "\"You have logged out, you have lost access to the following contents: \\n 'Infinite Mode' \\n Please login again to be able to access to the aditional content.\"";
            }

            //creamos el boton de exit
            var acceptButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 650),
                Text = "Accept",
            };

            acceptButton.Click += acceptButton_Click;

            _components.Add(acceptButton);
            _components.Add(explication);
            _limitIndex = 0;
        }

        private void acceptButton_Click(Object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //saca los sprites si no los necesita
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            //hacemos q cierre el juegardovich
            _game.Exit();
        }
    }
}
