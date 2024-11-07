using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Project.States;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;

namespace Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        RenderTarget2D gameplayTarget;
        int gameplayWidth = 1500; // Ancho del área de gameplay
        int gameplayHeight = 1080; // Alto del área de gameplay

        Vector2 playerPosition;

        public int estado = 0;

        private State _currentState;

        private State _nextState;
        SpriteFont font;

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        public ContentManager ContentManager { get; private set; }

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            if (GraphicsDevice == null)
            {
                _graphics.ApplyChanges();
            }

            // Change the resolution to match your current desktop
            _graphics.PreferredBackBufferWidth = GraphicsDevice.Adapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsDevice.Adapter.CurrentDisplayMode.Height;
            _graphics.ApplyChanges();
            _graphics.IsFullScreen = true;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;      

    }

        protected override void Initialize()
        {

            IsMouseVisible = true;
            base.Initialize();
        }

        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture;
        private int screenheight;
        private int screenwidth;

        //visualizer
        public Texture2D visualizer;
        public void Load(GraphicsDevice device, Texture2D backgroundTexture)
        {
            mytexture = backgroundTexture;
            screenheight = device.Viewport.Height;
            screenwidth = device.Viewport.Width;

            // Set the origin so that we're drawing from the 
            // center of the top edge.
            origin = new Vector2(mytexture.Width / 2, 0);

            // Set the screen position to the center of the screen.
            screenpos = new Vector2(screenwidth / 2, screenheight / 2);

            // Offset to draw the second texture, when necessary.
            texturesize = new Vector2(0, mytexture.Height);
        }

        ScrollingBackground myBackground;

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _currentState = new MenuState(this, _graphics.GraphicsDevice, Content, _graphics);
            gameplayTarget = new RenderTarget2D(GraphicsDevice, gameplayWidth, gameplayHeight);

            // TODO: use this.Content to load your game content here
            myBackground = new ScrollingBackground();
            Texture2D background = Content.Load<Texture2D>("background");
            visualizer = Content.Load<Texture2D>("visualizer");
            font = Content.Load<SpriteFont>("Fonts/ArcadeFont");
            myBackground.Load(GraphicsDevice, background);

            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin();
            myBackground.Draw(_spriteBatch, Color.White);
            _spriteBatch.End();
        }

        private float scrollingSpeed = 100;

        protected override void Update(GameTime gameTime)
        {
            if (_nextState != null)
            {
                _currentState = _nextState;

                _nextState = null;
            }
                        
            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // The time since Update was called last.
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            // TODO: Add your game logic here.
            myBackground.Update(elapsed * scrollingSpeed);

            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin();
            myBackground.Draw(_spriteBatch, Color.White);
            _spriteBatch.End();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(gameplayTarget);
            _graphics.GraphicsDevice.Clear(Color.SkyBlue);
            _currentState.Draw(gameTime, _spriteBatch);          

            // Cambiar de nuevo al Render Target predeterminado
            GraphicsDevice.SetRenderTarget(null);

            // Dibujar en la pantalla completa
            GraphicsDevice.Clear(Color.Black); // Color de fondo de la pantalla completa

            _spriteBatch.Begin();

            // Dibuja el Render Target (gameplay) en una sección de la pantalla
            _spriteBatch.Draw(gameplayTarget, new Rectangle(0, 0, gameplayWidth, gameplayHeight), Color.White);

            // Dibuja la información de la UI en otra sección de la pantalla
<<<<<<< Updated upstream
            // ejemplo: spriteBatch.DrawString(font, "Puntuación: 1000", new Vector2(850, 50), Color.White);
=======
            _spriteBatch.Draw(visualizer, new Rectangle(1500, 0, 500, 1080), Color.White);
            _spriteBatch.DrawString(font, "Score: 1000", new Vector2(1500 + 50, 50), Color.White);

>>>>>>> Stashed changes
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
