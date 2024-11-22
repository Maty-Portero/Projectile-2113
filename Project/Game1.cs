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
        public int gameplayWidth = 1500; // Ancho del área de gameplay
        public int gameplayHeight = 1080; // Alto del área de gameplay
        Konami konami;
        Vector2 playerPosition;
        Visualizer visualizer;
        public Leaderboard Leaderboard { get; private set; }

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
            visualizer = new Visualizer(Content.Load<Texture2D>("visualizer"), Content.Load<SpriteFont>("Fonts/orbitron"), new Vector2(1500, 0));
            Leaderboard = new Leaderboard("leaderboard.json");
           // LEADORBARD CLEAR! Leaderboard.Clear();
            base.Initialize();
        }
        // Método para agregar un puntaje desde cualquier lugar en el juego
        public void AddScore(string playerName, int score, string mode)
        {
            Leaderboard.AddEntry(playerName, score, mode);
        }

        // class ScrollingBackground
        private Vector2 screenpos, origin, texturesize;
        private Texture2D mytexture;
        private int screenheight;
        private int screenwidth;

        //splash
        Texture2D splash;
        Texture2D splash2;
        Texture2D splash1;
        Texture2D splash3;
        Texture2D heartEmptyTexture;
        Texture2D heartFullTexture;
        Texture2D rocketTexture;
        Texture2D rocketEmptyTexture;
        private List<Vector2> rocketPositions;

        // Initialize heart positions
        List<Vector2> heartPositions = new List<Vector2>
                {
                    new Vector2(1600, 400),  // Adjusted position to leave space for score
                    new Vector2(1600+50, 400),
                    new Vector2(1600+100, 400)
                };
        // Initialize heart positions
        List<Vector2> heartPositions2 = new List<Vector2>
                {
                    new Vector2(1600, 300),  // Adjusted position to leave space for score
                    new Vector2(1600+50, 300),
                    new Vector2(1600+100, 300)
                };
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
            // Load heart textures
            heartFullTexture = Content.Load<Texture2D>("HP_Icon");
            heartEmptyTexture = Content.Load<Texture2D>("HP_Icon_Loss");
            font = Content.Load<SpriteFont>("Fonts/orbitron");

            // Load rocket textures
            rocketTexture = Content.Load<Texture2D>("RocketIcon");
            rocketEmptyTexture = Content.Load<Texture2D>("RocketEmpty");

            // Initialize rocket positions
            rocketPositions = new List<Vector2>
            {
                new Vector2(1600, 500)
            };
            myBackground.Load(GraphicsDevice, background);

            GraphicsDevice.Clear(Color.SkyBlue);

            _spriteBatch.Begin();
            myBackground.Draw(_spriteBatch, Color.White);
            _spriteBatch.End();
        }

        private float scrollingSpeed = 100;
        double timer = 0;
        double colorAux = 0.5;
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
                    colorAux += 0.5;
                }
                if (estado == 2 || estado == 3) konami.Success = false;
            }
           // visualizer.Update(gameTime, konami); // También actualiza el visualizer según el estado de Konami, si es necesario
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
            _spriteBatch.Draw(splash, new Rectangle(500+1010, 0, 500 - 100, 320 - 100), Color.White);

            if (konami.Success)
            {
                _spriteBatch.Draw(splash2, new Rectangle(500+1000 + 100, 150, 65, 65), rainbow);
                _spriteBatch.Draw(splash1, new Rectangle(500+1000 + 180, 150, 30, 75), rainbow);
                _spriteBatch.Draw(splash1, new Rectangle(500+1000 + 230, 150, 30, 75), rainbow);
                _spriteBatch.Draw(splash3, new Rectangle(500+1000 + 270, 150, 65, 65), rainbow);
            }
            else
            {
                _spriteBatch.Draw(splash2, new Rectangle(500+1000 + 100, 150, 65, 65), Color.SkyBlue);
                _spriteBatch.Draw(splash1, new Rectangle(500+1000 + 180, 150, 30, 75), Color.MediumPurple);
                _spriteBatch.Draw(splash1, new Rectangle(500+1000 + 230, 150, 30, 75), Color.MediumPurple);
                _spriteBatch.Draw(splash3, new Rectangle(500+1000 + 270, 150, 65, 65), Color.SkyBlue);
            }
            if (estado == 2)
            {
                if (_currentState is GameState gameState)
                {
                    // Validar que las texturas y posiciones estén inicializadas
                    if (heartFullTexture != null && heartEmptyTexture != null && heartPositions != null && gameState.player != null)
                    {
                        for (int i = 0; i < heartPositions.Count; i++)
                        {
                            if (i < gameState.player.playerLives)
                            {
                                _spriteBatch.Draw(heartFullTexture, heartPositions[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(heartEmptyTexture, heartPositions[i], Color.White);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Las texturas o posiciones de los corazones no están inicializadas.");
                    }
                    if (rocketTexture != null && rocketEmptyTexture != null && rocketPositions != null && gameState.player != null)
                    {
                        for (int i = 0; i < rocketPositions.Count; i++)
                        {
                            if (i < gameState.player.rocketRemaining)
                            {
                                _spriteBatch.Draw(rocketTexture, rocketPositions[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(rocketEmptyTexture, rocketPositions[i], Color.White);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("_currentState no es GameState o está null.");
                }


            }
            else if (estado == 3)
            {
                if (_currentState is GameState2 gameState)
                {
                    // Validar que las texturas y posiciones estén inicializadas
                    if (heartFullTexture != null && heartEmptyTexture != null && heartPositions != null && gameState.player != null)
                    {
                        for (int i = 0; i < heartPositions.Count; i++)
                        {
                            if (i < gameState.player.playerLives)
                            {
                                _spriteBatch.Draw(heartFullTexture, heartPositions[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(heartEmptyTexture, heartPositions[i], Color.White);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Las texturas o posiciones de los corazones no están inicializadas.");
                    }
                    if (rocketTexture != null && rocketEmptyTexture != null && rocketPositions != null && gameState.player != null)
                    {
                        for (int i = 0; i < rocketPositions.Count; i++)
                        {
                            if (i < gameState.player.rocketRemaining)
                            {
                                _spriteBatch.Draw(rocketTexture, rocketPositions[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(rocketEmptyTexture, rocketPositions[i], Color.White);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("_currentState no es GameState o está null.");
                }
            }
            else if (estado == 4)
            {
                if (_currentState is GameState3 gameState)
                {
                    // Validar que las texturas y posiciones estén inicializadas
                    if (heartFullTexture != null && heartEmptyTexture != null && heartPositions != null && gameState.player1 != null)
                    {
                        for (int i = 0; i < heartPositions.Count; i++)
                        {
                            if (i < gameState.player1.playerLives)
                            {
                                _spriteBatch.Draw(heartFullTexture, heartPositions[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(heartEmptyTexture, heartPositions[i], Color.White);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Las texturas o posiciones de los corazones no están inicializadas.");
                    }

                    if (heartFullTexture != null && heartEmptyTexture != null && heartPositions != null && gameState.player2 != null)
                    {
                        for (int i = 0; i < heartPositions2.Count; i++)
                        {
                            if (i < gameState.player2.playerLives)
                            {
                                _spriteBatch.Draw(heartFullTexture, heartPositions2[i], Color.White);
                            }
                            else
                            {
                                _spriteBatch.Draw(heartEmptyTexture, heartPositions2[i], Color.White);
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Las texturas o posiciones de los corazones no están inicializadas.");
                    }
                }
            }
            
            _spriteBatch.DrawString(font, "v1.0", new Vector2(1500 + 60, 1000), Color.White);
            visualizer.Draw(estado, _spriteBatch, konami.Success, Leaderboard);  // Dibuja el visualizer con el contenido adecuado
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}
