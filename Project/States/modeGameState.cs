﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Project.Controls;

namespace Project.States
{
    internal class modeGameState : State
    {
        private GraphicsDeviceManager _graphics;
        private List<Component> _components;

        public modeGameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");
            var titleFont = _content.Load<SpriteFont>("Fonts/FontTitle");


            //creamos el boton de how to play
            var mode1Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 275),
                Text = "Stage Mode",
            };

            mode1Button.Click += mode1Button_Click;

            //creamos el boton de options
            var mode2Button = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 400),
                Text = "Infinite Mode",
            };

            mode2Button.Click += mode2Button_Click;

            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(710, 525),
                Text = "Back",
            };

            backButton.Click += backButton_Click;

            _components = new List<Component>()
            {
                mode1Button,
                backButton
            };
            if (playerData.loggedIn == true)
            {
                _components.Add(mode2Button);
            }
        }
 

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            foreach (var component in _components)
                component.Draw(gameTime, spriteBatch);
            spriteBatch.End();
        }

        private void mode1Button_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState(_game, _graphicsDevice, _content, _graphics));
        }
        private void mode2Button_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new GameState2(_game, _graphicsDevice, _content, _graphics));
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

        private void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }
    }
}