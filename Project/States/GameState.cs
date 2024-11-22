using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;
using static System.Formats.Asn1.AsnWriter;

namespace Project.States
{
    internal class GameState : State
    {
        private const int MaxEnemies = 5;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        #region enemy
        // Enemy bullet variables
        Texture2D[] enemyBulletTextures;
        List<EnemyBullet> enemyBullets;
        // Enemy variables
        Texture2D[] enemyTexture;
        Texture2D[] enemyDamagedTexture;
        List<Enemy> enemies;
        Random random;
        #endregion

        #region powerup
        private Texture2D powerUpTexture;
        private List<Vector2> powerUpPositions;
        private List<bool> powerUpActiveList;
        private bool powerUpCollected = false;
        private double powerUpDuration = 10.0; // Duración del power-up en segundos
        private double powerUpTimer = 0;
        private double powerUpSpawnChance = 0.005; // Probabilidad de aparición del power-up
        private Texture2D[] powerUpTimerFrames; // Array de frames para la animación del timer
        private int currentPowerUpFrame = 0; // Frame actual de la animación del power-up
        private double powerUpFrameTimer = 0; // Temporizador para cambiar frames
        private double powerUpFrameTime = 0.1; // Tiempo entre frames
        #endregion

        private SpriteFont font;

        // Points animation variables
        private Texture2D[] pointsAnimationFrames; // Array de frames para la animación de los 1000 puntos
        private List<PointsAnimation> pointsAnimations; // Lista para almacenar animaciones activas

        // Stage and round variables
        private int stage;
        private int round;

        private bool roundCompleted;
        private double roundCompletionTimer;
        private const double roundCompletionDuration = 5.0;
        private int remainingSeconds;

        private bool stageCompleted;
        private double stageCompletionTimer;


        // Singleton instance for GameState
        public static GameState Instance { get; private set; }

        // Propiedad pública para acceder al ContentManager
        public ContentManager ContentManager { get; private set; }
        // Background variables
        Texture2D backgroundTexture;
        Texture2D buildingsTexture;
        Texture2D treesTexture;
        Vector2 bgPosition1, bgPosition2, treesPosition2, buildingsPosition2;
        float bgSpeed = 100f;
        int score2;
        public Player player;

        Texture2D[] bulletTextures;



        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _game.estado = 2;

            _graphics = deviceManager;
            content.RootDirectory = "Content";

            // Asigna el ContentManager pasado al constructor a la propiedad pública
            ContentManager = content;

            enemyBullets = new List<EnemyBullet>();
            backgroundTexture = content.Load<Texture2D>("bgStreets1");
            treesTexture = content.Load<Texture2D>("bgTrees1");
            buildingsTexture = content.Load<Texture2D>("bgBuildings1");

            bgPosition1 = Vector2.Zero;
            bgPosition2 = new Vector2(0, -backgroundTexture.Height);
            treesPosition2 = new Vector2(0, -treesTexture.Height);
            buildingsPosition2 = new Vector2(0, -buildingsTexture.Height);
            random = new Random();

            player = new Player(content);

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

            // Load power-up texture
            powerUpTexture = content.Load<Texture2D>("PowerUp_Sprite");

            // Load font for score
            font = content.Load<SpriteFont>("Fonts/ArcadeFont");

            enemies = new List<Enemy>();
            powerUpPositions = new List<Vector2>();
            powerUpActiveList = new List<bool>();

            player.Load();

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

            CreateEnemiesRound1();

            player.playerPosition = new Vector2(1050 / 2, _graphics.PreferredBackBufferHeight / 2);

            // Initialize score
            player.score = 0;

            // Initialize stage and round
            stage = 1;
            round = 1;

            // Set power-up spawn chance
            powerUpSpawnChance = 0.05; // 50% de probabilidad de aparición del power-up

            // Set the singleton instance
            Instance = this;
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
            string stageText = $"Stage {stage} - Round {round}";
            spriteBatch.DrawString(font, stageText, new Vector2(10, 1020), Color.White);

            // Draw score
            string scoreText = $"SCORE: {player.score.ToString("D7")}";
            spriteBatch.DrawString(font, scoreText, new Vector2(1225, 1020), Color.White);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                player.SlowSpeed();
            }
            else
            {
                player.NormalSpeed();
            }

            player.Draw(spriteBatch);

            // Draw player bullets
            foreach (var bullet in player.bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // Draw enemy bullets
            foreach (var bullet in enemyBullets)
            {
                bullet.Draw(spriteBatch);
            }

            // Draw enemies
            foreach (var enemy in enemies)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            // Draw power-ups
            for (int i = 0; i < powerUpPositions.Count; i++)
            {
                if (powerUpActiveList[i])
                {
                    spriteBatch.Draw(powerUpTexture, powerUpPositions[i], Color.White);
                }
            }

            // Draw power-up timer if collected
            if (powerUpCollected)
            {
                // Reemplaza el texto del power-up con la animación
                spriteBatch.Draw(powerUpTimerFrames[currentPowerUpFrame], new Vector2(20, 100), Color.White);
            }

            // Draw points animations
            foreach (var animation in pointsAnimations)
            {
                animation.Draw(spriteBatch);
            }


            if (roundCompleted && round <= 5)
            {
                string completionText = $"Round completed, next round starting in {remainingSeconds} seconds";
                Vector2 textSize = font.MeasureString(completionText);
                Vector2 textPosition = new Vector2((1500 - textSize.X) / 2, (_graphics.PreferredBackBufferHeight - textSize.Y) / 2);
                spriteBatch.DrawString(font, completionText, textPosition, Color.White);
            }

            if (stageCompleted)
            {
                string completionText = "Stage completed!";
                Vector2 textSize = font.MeasureString(completionText);
                Vector2 textPosition = new Vector2((1500 - textSize.X) / 2, (_graphics.PreferredBackBufferHeight - textSize.Y) / 2);
                spriteBatch.DrawString(font, completionText, textPosition, Color.White);
                _game.ChangeState(new GameFinishedState(_game, _graphicsDevice, _content, _graphics, player.score));
            }

            spriteBatch.End();
        }
        public void Shoot()
        {
            Vector2 bulletPosition = new Vector2(player.playerPosition.X - 50 / 2, player.playerPosition.Y - player.playerTexture.Height / 2 - 15);
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * player.bulletSpeed;
            player.bullets.Add(newBullet);
        }

        public void ShootTriple()
        {
            Vector2 bulletPosition = new Vector2(player.playerPosition.X - 50 / 2, player.playerPosition.Y - player.playerTexture.Height / 2 - 15);
            float angleOffset = MathHelper.ToRadians(20);

            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * player.bulletSpeed;
            player.bullets.Add(newBullet);

            PlayerBullet leftBullet = new PlayerBullet(bulletPosition, bulletTextures);
            leftBullet.Velocity = new Vector2((float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * player.bulletSpeed;
            player.bullets.Add(leftBullet);

            PlayerBullet rightBullet = new PlayerBullet(bulletPosition, bulletTextures);
            rightBullet.Velocity = new Vector2(-(float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * player.bulletSpeed;
            player.bullets.Add(rightBullet);
        }
        private void CreateEnemiesRound1()
        {
            // Create 3 enemies on the right side
            for (int i = 0; i < 3; i++)
            {
                int xPosition = 1050 - enemyTexture[0].Width;
                int yPosition = i * (enemyTexture[0].Height + 10);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5)); // Cambiado a 5 golpes
            }

            // Create 2 enemies on the left side
            int yOffset = 30; // Additional offset for enemies on the left
            for (int i = 0; i < 2; i++)
            {
                int xPosition = 100;
                int yPosition = i * (enemyTexture[0].Height + 10) + yOffset;
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5)); // Cambiado a 5 golpes
            }
        }

        private void CreateEnemiesRound2()
        {
            Vector2[] positions = new Vector2[]
            {
                new Vector2(1050 / 2 - enemyTexture[0].Width / 2, 0),
                new Vector2(1050 / 2 - enemyTexture[0].Width - 5, enemyTexture[0].Height + 5),
                new Vector2(1050 / 2 + 5, enemyTexture[0].Height + 5),
                new Vector2(1050 / 2 - enemyTexture[0].Width * 1.5f - 10, (enemyTexture[0].Height + 5) * 2),
                new Vector2(1050 / 2 - enemyTexture[0].Width / 2, (enemyTexture[0].Height + 5) * 2),
                new Vector2(1050 / 2 + enemyTexture[0].Width / 2 + 15, (enemyTexture[0].Height + 5) * 2), // Ajuste aquí para mover más a la derecha
                new Vector2(1050 / 2 - enemyTexture[0].Width * 2 - 15, (enemyTexture[0].Height + 5) * 3),
                new Vector2(1050 / 2 - enemyTexture[0].Width / 2, (enemyTexture[0].Height + 5) * 3),
                new Vector2(1050 / 2 + enemyTexture[0].Width / 2 + 10, (enemyTexture[0].Height + 5) * 3),
            };

            foreach (var pos in positions)
            {
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, pos, random, 5)); // Cambiado a 5 golpes
            }
        }

        private void CreateEnemiesRound3()
        {
            for (int i = 0; i < 9; i++)
            {
                int xPosition = random.Next(0, 1050 - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5)); // Cambiado a 5 golpes
            }
        }

        private void CreateEnemiesRound4()
        {
            // Añadir 5 enemigos normales
            for (int i = 0; i < 5; i++)
            {
                int xPosition = random.Next(100, 1050 - 100);
                int yPosition = random.Next(50, 150);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5)); // Cambiado a 5 golpes
            }

            // Añadir 6 MiniCopters
            for (int i = 0; i < 6; i++)
            {
                int xPosition = random.Next(100, 1050 - 100);
                int yPosition = random.Next(50, 150);
                enemies.Add(new MiniCopter(ContentManager, new Vector2(xPosition, yPosition), random));
            }
        }

        private void CreateEnemiesRound5()
        {
            // Crear 10 enemigos MiniCopter en posiciones aleatorias
            for (int i = 0; i < 10; i++)
            {
                int xPosition = random.Next(0, 1050 - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new MiniCopter(ContentManager, new Vector2(xPosition, yPosition), random));
            }

            // Mantener los enemigos originales
            for (int i = 0; i < 10; i++)
            {
                int xPosition = random.Next(0, 1050 - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5)); // Cambiado a 5 golpes
            }
        }


        private void CreateMiniCopters()
        {
            for (int i = 0; i < 3; i++)
            {
                int xPosition = random.Next(100, 1050 - 100);
                int yPosition = random.Next(50, 150);
                enemies.Add(new MiniCopter(ContentManager, new Vector2(xPosition, yPosition), random));
            }
        }

        public void AddEnemyBullet(EnemyBullet bullet)
        {
            enemyBullets.Add(bullet);
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
            if (random.NextDouble() <= powerUpSpawnChance)
            {
                powerUpPositions.Add(position);
                powerUpActiveList.Add(true);
            }
        }



        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float currentSpeed = kstate.IsKeyDown(Keys.LeftShift) || kstate.IsKeyDown(Keys.RightShift) ? player.slowSpeed : player.playerSpeed;
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
            Vector2 direction = Vector2.Zero;

            if (kstate.IsKeyDown(Keys.Up) || kstate.IsKeyDown(Keys.W))
            {
                direction.Y -= 1;
            }

            if (kstate.IsKeyDown(Keys.Down) || kstate.IsKeyDown(Keys.S))
            {
                direction.Y += 1;
            }

            if (kstate.IsKeyDown(Keys.Left) || kstate.IsKeyDown(Keys.A))
            {
                direction.X -= 1;
            }

            if (kstate.IsKeyDown(Keys.Right) || kstate.IsKeyDown(Keys.D))
            {
                direction.X += 1;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            player.playerPosition += direction * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Mantén al jugador dentro de los límites de la pantalla
            if (player.playerPosition.X > 1500 - player.playerTexture.Width / 2)
            {
                player.playerPosition.X = 1500 - player.playerTexture.Width / 2;
            }
            else if (player.playerPosition.X < player.playerTexture.Width / 2)
            {
                player.playerPosition.X = player.playerTexture.Width / 2;
            }

            if (player.playerPosition.Y > 1035)
            {
                player.playerPosition.Y = 1035;
            }
            else if (player.playerPosition.Y < player.playerTexture.Height / 2)
            {
                player.playerPosition.Y = player.playerTexture.Height / 2;
            }

            for (int i = enemies.Count - 1; i >= 0; i--)
                if (player.rocketRemaining > 0)
                {
                    if (kstate.IsKeyDown(Keys.K) || kstate.IsKeyDown(Keys.G))
                    {
                        player.score += 1000 * enemies.Count;
                        enemies.Clear();
                        player.rocketRemaining--;
                    }
                }

            // Disparo del jugador
            player.shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((kstate.IsKeyDown(Keys.Space) && player.shootTimer <= 0) || (kstate.IsKeyDown(Keys.LeftControl) && player.shootTimer <= 0))
            {
                if (powerUpCollected)
                {
                    ShootTriple();
                }
                else
                {
                    Shoot();
                }
                player.shootTimer = player.shootCooldown;
            }

            // Actualizar enemigos
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            ShootEnemy();

            // Actualizar balas del jugador
            for (int i = player.bullets.Count - 1; i >= 0; i--)
            {
                player.bullets[i].Update(gameTime);
                if (player.bullets[i].Position.Y < 0)
                {
                    player.bullets.RemoveAt(i);
                }
                else
                {
                    for (int j = enemies.Count - 1; j >= 0; j--)
                    {
                        if (player.bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                        {
                            Vector2 enemyPosition = enemies[j].Position;
                            enemies[j].TakeDamage(1);
                            player.bullets.RemoveAt(i);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                SpawnPowerUp(enemyPosition); // Spawns a power-up at the position of the killed enemy with a probability

                                // Incrementar puntaje
                                player.score += 1000;

                                // Agregar animación de puntos
                                pointsAnimations.Add(new PointsAnimation(pointsAnimationFrames, enemyPosition));
                            }

                            break;
                        }
                    }
                }
            }

            // Actualizar balas del enemigo
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].Update(gameTime);
                if (enemyBullets[i].Position.Y > 1035)
                {
                    enemyBullets.RemoveAt(i);
                }
                else if (!player.isInvincible && enemyBullets[i].GetBounds().Intersects(player.GetPlayerBounds()))
                {
                    enemyBullets.RemoveAt(i);
                    PlayerTakeDamage();
                }
            }

            // Actualizar invencibilidad
            if (player.isInvincible)
            {
                player.invincibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                player.invincibleFlashTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (player.invincibleFlashTimer >= player.FlashDuration)
                {
                    player.invincibleFlashTimer = 0;
                }

                if (player.invincibleTimer <= 0)
                {
                    player.isInvincible = false;
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
                    else if (player.GetPlayerBounds().Intersects(new Rectangle((int)powerUpPositions[i].X, (int)powerUpPositions[i].Y, powerUpTexture.Width, powerUpTexture.Height)))
                    {
                        powerUpActiveList[i] = false;
                        powerUpCollected = true;
                        powerUpTimer = powerUpDuration;
                        currentPowerUpFrame = 0; // Reinicia la animación del power-up
                    }
                }
            }

            // Temporizador del power-up
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
                        currentPowerUpFrame = 0; // Reinicia la animación si llega al final
                    }
                }

                if (powerUpTimer <= 0)
                {
                    powerUpCollected = false;
                    currentPowerUpFrame = 0; // Resetea el frame al final del power-up
                }
            }

            // Actualizar animaciones de puntos
            for (int i = pointsAnimations.Count - 1; i >= 0; i--)
            {
                pointsAnimations[i].Update(gameTime);
                if (pointsAnimations[i].IsFinished)
                {
                    pointsAnimations.RemoveAt(i);
                }
            }

            // Comprobar colisiones con enemigos
            foreach (var enemy in enemies)
            {
                if (!player.isInvincible && enemy.GetBounds().Intersects(player.GetPlayerBounds()))
                {
                    PlayerTakeDamage();
                }
            }

            // Check for round completion
            if (enemies.Count == 0 && !roundCompleted)
            {
                roundCompleted = true;
                roundCompletionTimer = roundCompletionDuration;
                remainingSeconds = (int)Math.Ceiling(roundCompletionDuration);
            }

            if (roundCompleted)
            {
                roundCompletionTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                remainingSeconds = (int)Math.Ceiling(roundCompletionTimer);
                if (roundCompletionTimer <= 0)
                {
                    round++;
                    roundCompleted = false;
                    if (round == 2)
                    {
                        CreateEnemiesRound2();
                    }
                    else if (round == 3)
                    {
                        CreateEnemiesRound3();
                    }
                    else if (round == 4)
                    {
                        CreateEnemiesRound4();
                    }
                    else if (round == 5)
                    {
                        CreateEnemiesRound5();
                    }
                    else if (round > 5)
                    {
                        stageCompleted = true;
                        stageCompletionTimer = roundCompletionDuration;
                    }
                }
            }


            if (stageCompleted)
            {
                stageCompletionTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                remainingSeconds = (int)Math.Ceiling(stageCompletionTimer);
                if (stageCompletionTimer <= 0)
                {
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
                }
            }
        }
        public void PlayerTakeDamage()
        {
            player.playerLives--;
            player.isInvincible = true;
            player.invincibleTimer = 2.0; // 2 segundos de invencibilidad

            // Verificar si el jugador está muerto
            if (player.playerLives <= 0)
            {
                // Pasa el puntaje actual al cambiar al estado de Game Over
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _graphics, player.score, score2));
            }
        }

        // Clase para la animación de los 1000 puntos
        internal class PointsAnimation
        {
            private Texture2D[] frames;
            private int currentFrame;
            private double timer;
            private double frameTime = 0.1; // Duración de cada frame
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
