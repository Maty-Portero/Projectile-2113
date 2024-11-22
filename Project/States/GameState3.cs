using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Project.States
{
    internal class GameState3 : State
    {
        public static GameState3 Instance { get; private set; }

        public ContentManager ContentManager { get; private set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Enemy bullet variables
        Texture2D[] enemyBulletTextures;
        List<EnemyBullet> enemyBullets;

        // Enemy variables
        Texture2D[] enemyTexture;
        Texture2D[] enemyDamagedTexture;
        List<Enemy> enemies;
        Random random;

        // Points animation variables
        private Texture2D[] pointsAnimationFrames;
        private List<PointsAnimation> pointsAnimations;

        // Power-up variables
        private Texture2D powerUpTexture;
        private List<Vector2> powerUpPositions;
        private List<bool> powerUpActiveList;
        private bool powerUpCollected = false;
        private double powerUpDuration = 10.0;
        private double powerUpTimer = 0;
        private double powerUpSpawnChance = 0.005; // Probabilidad para el power-up ya existente

        private Texture2D[] powerUpTimerFrames;
        private int currentPowerUpFrame = 0;
        private double powerUpFrameTimer = 0;
        private double powerUpFrameTime = 0.1;

        private SpriteFont font;

        // Stage and round variables
        private int round;
        private bool roundCompleted;
        private double roundCompletionTimer;
        private const double roundCompletionDuration = 5.0;
        private int remainingSeconds;

        // Variables para el mensaje de próxima ronda
        private bool nextRoundStarting;
        private double nextRoundTimer;
        private int nextRoundRemainingSeconds;

        // Background variables
        Texture2D backgroundTexture;
        Texture2D buildingsTexture;
        Texture2D treesTexture;
        Vector2 bgPosition1, bgPosition2, treesPosition2, buildingsPosition2;
        float bgSpeed = 100f;

        public Player player1;
        public Player player2;

        Texture2D[] bulletTextures;

        public GameState3(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _game.estado = 4;

            Instance = this;
            ContentManager = content;

            _graphics = deviceManager;
            content.RootDirectory = "Content";

            enemyBullets = new List<EnemyBullet>();

            random = new Random();

            backgroundTexture = content.Load<Texture2D>("bgStreets1");
            treesTexture = content.Load<Texture2D>("bgTrees1");
            buildingsTexture = content.Load<Texture2D>("bgBuildings1");

            bgPosition1 = Vector2.Zero;
            bgPosition2 = new Vector2(0, -backgroundTexture.Height);
            treesPosition2 = new Vector2(0, -treesTexture.Height);
            buildingsPosition2 = new Vector2(0, -buildingsTexture.Height);

            player1 = new Player(content);
            player2 = new Player(content);

            player1.Load();
            player2.Load();

            bulletTextures = new Texture2D[2];
            bulletTextures[0] = content.Load<Texture2D>("Player_Bullet (1)");
            bulletTextures[1] = content.Load<Texture2D>("Player_Bullet (2)");

            // Load enemy bullet textures
            enemyBulletTextures = new Texture2D[3];
            enemyBulletTextures[0] = content.Load<Texture2D>("Enemy_Bullet (1)");
            enemyBulletTextures[1] = content.Load<Texture2D>("Enemy_Bullet (2)");
            enemyBulletTextures[2] = content.Load<Texture2D>("Enemy_Bullet (3)");

            // Load enemy texture and create enemies
            enemyTexture = new Texture2D[3];
            enemyTexture[0] = content.Load<Texture2D>("Enemy_V6 (1)");
            enemyTexture[1] = content.Load<Texture2D>("Enemy_V6 (2)");
            enemyTexture[2] = content.Load<Texture2D>("Enemy_V6 (3)");

            enemyDamagedTexture = new Texture2D[3];
            enemyDamagedTexture[0] = content.Load<Texture2D>("Enemy_V6_Damaged (1)");
            enemyDamagedTexture[1] = content.Load<Texture2D>("Enemy_V6_Damaged (2)");
            enemyDamagedTexture[2] = content.Load<Texture2D>("Enemy_V6_Damaged (3)");

            // Load power-up textures
            powerUpTexture = content.Load<Texture2D>("PowerUp_Sprite");

            // Load font for score
            font = content.Load<SpriteFont>("Fonts/ArcadeFont");

            enemies = new List<Enemy>();
            powerUpPositions = new List<Vector2>();
            powerUpActiveList = new List<bool>();

            // Load power-up timer frames
            powerUpTimerFrames = new Texture2D[100];
            for (int i = 0; i < powerUpTimerFrames.Length; i++)
            {
                powerUpTimerFrames[i] = content.Load<Texture2D>($"timerSprite_{i:000}");
            }

            // Load points animation frames
            pointsAnimationFrames = new Texture2D[8];
            for (int i = 0; i < pointsAnimationFrames.Length; i++)
            {
                pointsAnimationFrames[i] = content.Load<Texture2D>($"pointsSprite_{i}");
            }
            pointsAnimations = new List<PointsAnimation>();

            CreateEnemies();

            player1.playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            player2.playerPosition = new Vector2((_graphics.PreferredBackBufferWidth / 2) + 100, _graphics.PreferredBackBufferHeight / 2);

            // Initialize score
            player1.score = 0;
            player2.score = 0;

            // Initialize round
            round = 1;

            powerUpSpawnChance = 0.05;

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTexture, bgPosition1, Color.White);
 
            spriteBatch.Draw(backgroundTexture, bgPosition2, Color.White);
            spriteBatch.Draw(treesTexture, bgPosition1, Color.White);

            spriteBatch.Draw(treesTexture, treesPosition2, Color.White);
            spriteBatch.Draw(buildingsTexture, bgPosition1, Color.White);

            spriteBatch.Draw(buildingsTexture, buildingsPosition2, Color.White);
            string roundText = $"COOP Mode - Round {round}";
            spriteBatch.DrawString(font, roundText, new Vector2(10, 1020), Color.White);

            string scoreText1 = $"SCORE: {player1.score.ToString("D7")}";
            string scoreText2 = $"SCORE: {player2.score.ToString("D7")}";
            spriteBatch.DrawString(font, scoreText1, new Vector2(1225, 980), Color.White);
            spriteBatch.DrawString(font, scoreText2, new Vector2(1225, 1020), Color.White);

            // Dibuja las balas del jugador 2
            foreach (var bullet in player2.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                player1.SlowSpeed();
            else player1.NormalSpeed();
            
            if (Keyboard.GetState().IsKeyDown(Keys.RightShift))
                player2.SlowSpeed();
            else
                player2.NormalSpeed();

            player1.Draw(spriteBatch);
            player2.Draw(spriteBatch);

            foreach (var bullet in player1.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var bullet in player2.bullets)
            {
                bullet.Draw(spriteBatch);
            }


            foreach (var bullet in enemyBullets)
            {
                bullet.Draw(spriteBatch);
            }

            foreach (var enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            for (int i = 0; i < powerUpPositions.Count; i++)
            {
                if (powerUpActiveList[i])
                {
                    spriteBatch.Draw(powerUpTexture, powerUpPositions[i], Color.White);
                }
            }

            if (nextRoundStarting)
            {
                string message = $"Next round in {nextRoundRemainingSeconds} seconds";
                Vector2 messageSize = font.MeasureString(message);
                Vector2 messagePosition = new Vector2((1500 - messageSize.X) / 2, _graphics.PreferredBackBufferHeight / 2);
                spriteBatch.DrawString(font, message, messagePosition, Color.White);
            }

            if (powerUpCollected)
            {
                spriteBatch.Draw(powerUpTimerFrames[currentPowerUpFrame], new Vector2(20, 100), Color.White);
            }

            foreach (var animation in pointsAnimations)
            {
                animation.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        private void CreateEnemies()
        {
            int minEnemies = 2 + round - 1;
            int maxEnemies = 3 + round - 1;
            int minMiniCopters = Math.Max(0, round - 1);
            int maxMiniCopters = Math.Max(1, round);

            int numberOfEnemies = random.Next(minEnemies, maxEnemies + 1);
            int numberOfMiniCopters = random.Next(minMiniCopters, maxMiniCopters + 1);

            for (int i = 0; i < numberOfEnemies; i++)
            {
                int xPosition = random.Next(0, 1500 - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5));
            }

            for (int i = 0; i < numberOfMiniCopters; i++)
            {
                int xPosition = random.Next(0, 1500 - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new MiniCopter3(_content, new Vector2(xPosition, yPosition), random));
            }
        }

        private void ShootEnemy()
        {
            foreach (var enemy in enemies)
            {
                if (enemy.CanShoot())
                {
                    Vector2 bulletPosition = new Vector2(enemy.Position.X + enemyTexture[0].Width / 2, enemy.Position.Y + enemyTexture[0].Height);
                    EnemyBullet newBullet = new EnemyBullet(bulletPosition, enemyBulletTextures);
                    enemyBullets.Add(newBullet);
                }
            }
        }

        private void SpawnPowerUp(Vector2 position)
        {
            double chance = random.NextDouble();

            if (chance <= powerUpSpawnChance)
            {
                powerUpPositions.Add(position);
                powerUpActiveList.Add(true);
            }
        }

        public void AddEnemyBullet(EnemyBullet bullet)
        {
            enemyBullets.Add(bullet);
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float currentSpeed1 = kstate.IsKeyDown(Keys.RightShift) ? player1.slowSpeed : player1.playerSpeed;
            float currentSpeed2 = kstate.IsKeyDown(Keys.LeftShift) ? player2.slowSpeed : player2.playerSpeed;

            bgPosition1.Y += bgSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            bgPosition2.Y += bgSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (bgPosition1.Y >= backgroundTexture.Height)
            {
                bgPosition1.Y = bgPosition2.Y - backgroundTexture.Height;
            }

            if (bgPosition2.Y >= backgroundTexture.Height)
            {
                bgPosition2.Y = bgPosition1.Y - backgroundTexture.Height;
            }


            Vector2 direction1 = Vector2.Zero;
            // Controles para el jugador 1 (existente)
            if (kstate.IsKeyDown(Keys.W)) direction1.Y -= 1;
            if (kstate.IsKeyDown(Keys.S)) direction1.Y += 1;
            if (kstate.IsKeyDown(Keys.A)) direction1.X -= 1;
            if (kstate.IsKeyDown(Keys.D)) direction1.X += 1;

            // Controles para el jugador 2 (nuevo)
            Vector2 direction2 = Vector2.Zero;
            if (kstate.IsKeyDown(Keys.Up)) direction2.Y -= 1;
            if (kstate.IsKeyDown(Keys.Down)) direction2.Y += 1;
            if (kstate.IsKeyDown(Keys.Left)) direction2.X -= 1;
            if (kstate.IsKeyDown(Keys.Right)) direction2.X += 1;

            player2.playerPosition += direction2 * currentSpeed1 * (float)gameTime.ElapsedGameTime.TotalSeconds;


            player1.playerPosition += direction1 * currentSpeed2 * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (player1.playerPosition.X > 1500 - player1.playerTexture.Width / 2)
            {
                player1.playerPosition.X =  1500 - player1.playerTexture.Width / 2;
            }
            else if (player1.playerPosition.X < player1.playerTexture.Width / 2)
            {
                player1.playerPosition.X = player1.playerTexture.Width / 2;
            }

            if (player1.playerPosition.Y > 1035)
            {
                player1.playerPosition.Y = 1035;
            }
            else if (player1.playerPosition.Y < player1.playerTexture.Height / 2)
            {
                player1.playerPosition.Y = player1.playerTexture.Height / 2;
            }

            if (player2.playerPosition.X > 1500 - player2.playerTexture.Width / 2)
            {
                player2.playerPosition.X = 1500 - player2.playerTexture.Width / 2;
            }
            else if (player2.playerPosition.X < player2.playerTexture.Width / 2)
            {
                player2.playerPosition.X = player2.playerTexture.Width / 2;
            }

            if (player2.playerPosition.Y > 1035)
            {
                player2.playerPosition.Y = 1035;
            }
            else if (player2.playerPosition.Y < player2.playerTexture.Height / 2)
            {
                player2.playerPosition.Y = player2.playerTexture.Height / 2;
            }

            player1.shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            player2.shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((kstate.IsKeyDown(Keys.Space) && player1.shootTimer <= 0) || (kstate.IsKeyDown(Keys.LeftControl) && player1.shootTimer <= 0))
            {
                if (powerUpCollected)
                {
                    player1.ShootTriple(bulletTextures);
                }
                else
                {
                    player1.Shoot(bulletTextures);
                }
                player1.shootTimer = player1.shootCooldown;
            }
            if ((kstate.IsKeyDown(Keys.Enter) && player2.shootTimer <= 0))
            {
                if (powerUpCollected)
                {
                    player2.ShootTriple(bulletTextures);
                }
                else
                {
                    player2.Shoot(bulletTextures);
                }
                player2.shootTimer = player2.shootCooldown;
            }
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            ShootEnemy();

            for (int i = player1.bullets.Count - 1; i >= 0; i--)
            {
                player1.bullets[i].Update(gameTime);
                if (player1.bullets[i].Position.Y < 0)
                {
                    player1.bullets.RemoveAt(i);
                }
                else
                {
                    for (int j = enemies.Count - 1; j >= 0; j--)
                    {
                        if (player1.bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                        {
                            Vector2 enemyPosition = enemies[j].Position;
                            enemies[j].TakeDamage(1);
                            player1.bullets.RemoveAt(i);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                SpawnPowerUp(enemyPosition);

                                player1.score += 1000;

                                pointsAnimations.Add(new PointsAnimation(pointsAnimationFrames, enemyPosition));
                            }

                            break;

                            
                        }
                        /*if (player2Bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                            {
                            enemies[j].TakeDamage(1);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                score2 += 1000; // Incrementa el puntaje del jugador 2
                            }
                        }*/
                    }
                }
            }

            for (int i = player2.bullets.Count - 1; i >= 0; i--)
            {
                player2.bullets[i].Update(gameTime);
                if (player2.bullets[i].Position.Y < 0)
                {
                    player2.bullets.RemoveAt(i);
                }
                else
                {
                    for (int j = enemies.Count - 1; j >= 0; j--)
                    {
                        if (player2.bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                        {
                            Vector2 enemyPosition = enemies[j].Position;
                            enemies[j].TakeDamage(1);
                            player2.bullets.RemoveAt(i);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                SpawnPowerUp(enemyPosition);

                                player2.score += 1000;

                                pointsAnimations.Add(new PointsAnimation(pointsAnimationFrames, enemyPosition));
                            }

                            break;


                        }
                        /*if (player2Bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                            {
                            enemies[j].TakeDamage(1);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                score2 += 1000; // Incrementa el puntaje del jugador 2
                            }
                        }*/
                    }
                }
            }

            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].Update(gameTime);
                if (enemyBullets[i].Position.Y > 1035)
                {
                    enemyBullets.RemoveAt(i);
                }
                else if (!player1.isInvincible && enemyBullets[i].GetBounds().Intersects(player1.GetPlayerBounds()))
                {
                    enemyBullets.RemoveAt(i);
                    PlayerTakeDamage();
                }
                else if (!player2.isInvincible && enemyBullets[i].GetBounds().Intersects(player2.GetPlayerBounds()))
                {
                    enemyBullets.RemoveAt(i);
                    Player2TakeDamage();
                }
            }

            if (player1.isInvincible)
            {
                player1.invincibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                player1.invincibleFlashTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (player1.invincibleFlashTimer >= player1.FlashDuration)
                {
                    player1.invincibleFlashTimer = 0;
                }

                if (player1.invincibleTimer <= 0)
                {
                    player1.isInvincible = false;
                }
            }
            if (player2.isInvincible)
            {
                player2.invincibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                player2.invincibleFlashTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (player2.invincibleFlashTimer >= player2.FlashDuration)
                {
                    player2.invincibleFlashTimer = 0;
                }

                if (player2.invincibleTimer <= 0)
                {
                    player2.isInvincible = false;
                }
            }
            // Actualizar power-up
            for (int i = 0; i < powerUpPositions.Count; i++)
            {
                if (powerUpActiveList[i])
                {
                    powerUpPositions[i] = new Vector2(powerUpPositions[i].X, powerUpPositions[i].Y + 100f * (float)gameTime.ElapsedGameTime.TotalSeconds); // Velocidad de caída del power-up

                    if (powerUpPositions[i].Y > 1050)
                    {
                        powerUpActiveList[i] = false;
                    }
                    else if (player1.GetPlayerBounds().Intersects(new Rectangle((int)powerUpPositions[i].X, (int)powerUpPositions[i].Y, powerUpTexture.Width, powerUpTexture.Height)))
                    {
                        powerUpActiveList[i] = false;
                        powerUpCollected = true;
                        powerUpTimer = powerUpDuration;
                        currentPowerUpFrame = 0; // Reinicia la animación del power-up
                    }
                    else if (player2.GetPlayerBounds().Intersects(new Rectangle((int)powerUpPositions[i].X, (int)powerUpPositions[i].Y, powerUpTexture.Width, powerUpTexture.Height)))
                    {
                        powerUpActiveList[i] = false;
                        powerUpCollected = true;
                        powerUpTimer = powerUpDuration;
                        currentPowerUpFrame = 0; // Reinicia la animación del power-up
                    }
                }
            }

            if (powerUpCollected)
            {
                powerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                powerUpFrameTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (powerUpFrameTimer >= powerUpFrameTime)
                {
                    powerUpFrameTimer = 0;
                    currentPowerUpFrame++;

                    if (currentPowerUpFrame >= powerUpTimerFrames.Length)
                    {
                        currentPowerUpFrame = 0;
                    }
                }

                if (powerUpTimer <= 0)
                {
                    powerUpCollected = false;
                    currentPowerUpFrame = 0;
                }
            }

            foreach (var enemy in enemies)
            {
                if (!player1.isInvincible && enemy.GetBounds().Intersects(player1.GetPlayerBounds()))
                {
                    PlayerTakeDamage();
                }
                if (!player2.isInvincible && enemy.GetBounds().Intersects(player2.GetPlayerBounds()))
                {
                    Player2TakeDamage();
                }
            }

            for (int i = pointsAnimations.Count - 1; i >= 0; i--)
            {
                pointsAnimations[i].Update(gameTime);
                if (pointsAnimations[i].IsFinished)
                {
                    pointsAnimations.RemoveAt(i);
                }
            }

            if (enemies.Count == 0 && !roundCompleted)
            {
                roundCompleted = true;
                nextRoundStarting = true;
                nextRoundTimer = 5.0;
                nextRoundRemainingSeconds = (int)Math.Ceiling(nextRoundTimer);
            }

            if (nextRoundStarting)
            {
                nextRoundTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                nextRoundRemainingSeconds = (int)Math.Ceiling(nextRoundTimer);

                if (nextRoundTimer <= 0)
                {
                    nextRoundStarting = false;
                    round++;
                    roundCompleted = false;
                    CreateEnemies();
                }
            }

        }

        private void PlayerTakeDamage()
        {
            player1.playerLives--;
            player1.isInvincible = true;
            player1.invincibleTimer = 2.0;

            if (player1.playerLives <= 0)
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _graphics, player1.score, player2.score));
            }
        }
        private void Player2TakeDamage()
        {
            player2.playerLives--;
            player2.isInvincible = true;
            player2.invincibleTimer = 2.0;

            if (player2.playerLives <= 0)
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _graphics, player1.score, player2.score));
            }
        }

        internal class PointsAnimation
        {
            private Texture2D[] frames;
            private int currentFrame;
            private double timer;
            private double frameTime = 0.1;
            private Vector2 position;
            public bool IsFinished { get; private set; }

            public PointsAnimation(Texture2D[] frames, Vector2 position)
            {
                this.frames = frames;
                this.position = position;
                currentFrame = 0;
                timer = 0;
                IsFinished = false;
            }

            public void Update(GameTime gameTime)
            {
                timer += gameTime.ElapsedGameTime.TotalSeconds;

                if (timer >= frameTime)
                {
                    timer = 0;
                    currentFrame++;

                    if (currentFrame >= frames.Length)
                    {
                        IsFinished = true;
                    }
                }
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                if (!IsFinished)
                {
                    spriteBatch.Draw(frames[currentFrame], position, Color.White);
                }
            }
        }

    }

}
