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
    internal class HelpState : NavigableState
    {
        private GraphicsDeviceManager _graphics;

        public HelpState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content, deviceManager)
        {

            _graphics = deviceManager;

            _game.estado = 5;
            //cargamos texturas y fuentes
            var buttonTexture = _content.Load<Texture2D>("Controls/Button");
            var titleTexture = _content.Load<Texture2D>("Controls/Title");
            var buttonFont = _content.Load<SpriteFont>("Fonts/Font");

            var explication = new Title(titleTexture, buttonFont)
            {
                Position = new Vector2(600, 300),
                Text = "Welcome to Projectile 2113, a bullet hell game where you are in a war during the year 2113. \n This is an early build just made to showcase the main mechanics of the game, the following aspects are still missing: \n 'Full Story Mode', 'Bosses', 'Bombs', 'Upgrading your character', 'The mayority of power ups'. \n This technical demo contains: \n '1 Level', 'Infinite mode', '2 Enemys', 'One power up', 'Fully playable character', 'Some of the settings' \n There are yet a lot of bugs and aspects to be fixed, expect them to be finished in future updates. \n Thanks for your patience! Here's a brief explanation about how to play the game. \n WASD - Move trough the map \n CTRL - Shoot \n Shift - Slow Down \n Your objective is to take down the enemy helicopters while avoiding to get shot.\n You have 3 lives, if you get hitted 3 times, you die.\n There are 2 enemys: 'Helicopter' and 'MiniCopter':\n - The Helicopter is slow but tankier, it can take up to 5 hits.\n - The MiniCopter is faster but weaker, it can take up to 2 hits. \n [ALMOST EVERYTHING IS STILL UNDER DEVELOPMENT, EXPECT NAMES DESIGNS MODELS ETC CHANGE IN FUTURE UPDATES]",
            };

            //creamos el boton de volver
            var backButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(500, 600),
                Text = "Back",
            };

            backButton.Click += backButton_Click;

            _components.Add(backButton);
            _components.Add(explication);
            _limitIndex = 0;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //saca los sprites si no los necesita
        }

    }
}
