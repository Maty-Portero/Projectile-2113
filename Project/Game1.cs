using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using Project.States;
using System.Reflection.Metadata;
using Microsoft.Xna.Framework.Content;
using System.Threading;
using Project.Controls;

namespace Project
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        RenderTarget2D gameplayTarget;
        int gameplayWidth = 1500; // Ancho del área de gameplay
        int gameplayHeight = 1080; // Alto del área de gameplay
        Konami konami;
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
            konami = new Konami();
            base.Initialize();
        }

        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture;
        private int screenheight;
        private int screenwidth;

        //visualizer
        public Texture2D visualizer;
        //splash
        Texture2D splash;
        Texture2D splash2;
        Texture2D splash1;
        Texture2D splash3;
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
            splash = Content.Load<Texture2D>("splash");
            splash2 = Content.Load<Texture2D>("splashtwo");
            splash1 = Content.Load<Texture2D>("splashone");
            splash3 = Content.Load<Texture2D>("splashthree");
            visualizer = Content.Load<Texture2D>("visualizer");
            font = Content.Load<SpriteFont>("Fonts/ArcadeFont");
            myBackground.Load(GraphicsDevice, background);

            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin();
            myBackground.Draw(_spriteBatch, Color.White);
            _spriteBatch.End();
        }

        private float scrollingSpeed = 100;
        double timer = 0;
        double colorAux = 1;
        Color rainbow = Color.White;
        Random random = new Random();

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

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            myBackground.Update(elapsed * scrollingSpeed);

            GraphicsDevice.Clear(Color.SkyBlue);

            // Detecta el código Konami
            konami.Read();
            if (konami.Success)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;
                if (timer > colorAux)
                {
                    rainbow = Color.Lerp(rainbow, new Color(random.Next(0, 256), random.Next(0, 256), random.Next(0, 256)), 1);
                    colorAux += 1;
                }
            }

            _spriteBatch.Begin();
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
            _spriteBatch.Draw(splash, new Rectangle(500, 0, 500, 320), Color.White);
            if (konami.Success)
            {
                _spriteBatch.Draw(splash2, new Rectangle(500 + 125, 200, 75, 75), rainbow);
                _spriteBatch.Draw(splash1, new Rectangle(500 + 210, 200, 30, 75), rainbow);
                _spriteBatch.Draw(splash1, new Rectangle(500 + 260, 200, 30, 75), rainbow);
                _spriteBatch.Draw(splash3, new Rectangle(500 + 300, 200, 75, 75), rainbow);
            }
            else
            {
                _spriteBatch.Draw(splash2, new Rectangle(500 + 125, 200, 75, 75), Color.SkyBlue);
                _spriteBatch.Draw(splash1, new Rectangle(500 + 210, 200, 30, 75), Color.Purple);
                _spriteBatch.Draw(splash1, new Rectangle(500 + 260, 200, 30, 75), Color.Purple);
                _spriteBatch.Draw(splash3, new Rectangle(500 + 300, 200, 75, 75), Color.SkyBlue);
            }
            // Dibuja la información de la UI en otra sección de la pantalla
<<<<<<< Updated upstream
=======
            _spriteBatch.Draw(visualizer, new Rectangle(1500, 0, 500, 1080), Color.White);
            _spriteBatch.DrawString(font, "Score: 1000", new Vector2(1500 + 50, 50), Color.White);
>>>>>>> Stashed changes
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
