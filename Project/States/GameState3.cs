using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Project.States
{
    internal class GameState3 : State
    {
        public static GameState3 Instance { get; private set; }

        public ContentManager ContentManager { get; private set; }
        Texture2D playerTexture;
        Texture2D damagedPlayerTexture;
        Texture2D playerTextureWithHitbox;
        Texture2D damagedPlayerTextureWithHitbox;
        Vector2 playerPosition;
        float playerSpeed;
        float slowSpeed;

        int deadZone;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Bullet variables
        Texture2D[] bulletTextures;
        List<PlayerBullet> bullets;
        float shootCooldown;
        float shootTimer;
        float bulletSpeed = 600f;

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
        private Texture2D plusOneLifePowerUpTexture; // Nuevo power-up de vida extra
        private List<Vector2> powerUpPositions;
        private List<bool> powerUpActiveList;
        private bool powerUpCollected = false;
        private double powerUpDuration = 10.0;
        private double powerUpTimer = 0;
        private double powerUpSpawnChance = 0.005; // Probabilidad para el power-up ya existente
        private double plusOneLifeSpawnChance = 1; // Probabilidad del nuevo power-up
        private Texture2D[] powerUpTimerFrames;
        private int currentPowerUpFrame = 0;
        private double powerUpFrameTimer = 0;
        private double powerUpFrameTime = 0.1;

        // Player lives variables
        private Texture2D heartFullTexture;
        private Texture2D heartEmptyTexture;
        private int playerLives = 3;
        private int maxLives = 5; // Límite de vidas a 5
        private bool isInvincible = false;
        private double invincibleTimer = 0;
        private double invincibleFlashTimer = 0;
        private const double FlashDuration = 0.1;
        private List<Vector2> heartPositions;

        // Score variables
        private int score;
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

        private int currentScore = 0;

        // Background variables
        Texture2D backgroundTexture;
        Texture2D buildingsTexture;
        Texture2D treesTexture;
        Vector2 bgPosition1, bgPosition2, treesPosition2, buildingsPosition2;
        float bgSpeed = 100f;

        // Player 2
        Vector2 player2Position;
        float player2Speed;
        int player2Lives = 3;
        List<PlayerBullet> player2Bullets;
        float player2ShootTimer;
        bool player2PowerUpCollected = false;
        double player2PowerUpTimer;
        List<Vector2> player2HeartPositions;
        int score2 = 0;


        public GameState3(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager)
            : base(game, graphicsDevice, content, deviceManager)
        {
            _game.estado = 4;

            Instance = this;
            ContentManager = content;

            _graphics = deviceManager;
            content.RootDirectory = "Content";

            playerSpeed = 400f;
            slowSpeed = 200f;
            deadZone = 4096;

            bullets = new List<PlayerBullet>();
            shootCooldown = 0.2f;
            shootTimer = 0;

            enemyBullets = new List<EnemyBullet>();

            random = new Random();

            backgroundTexture = content.Load<Texture2D>("bgStreets1");
            treesTexture = content.Load<Texture2D>("bgTrees1");
            buildingsTexture = content.Load<Texture2D>("bgBuildings1");

            bgPosition1 = Vector2.Zero;
            bgPosition2 = new Vector2(0, -backgroundTexture.Height);
            treesPosition2 = new Vector2(0, -treesTexture.Height);
            buildingsPosition2 = new Vector2(0, -buildingsTexture.Height);


            playerTexture = content.Load<Texture2D>("Player_Sprite");
            damagedPlayerTexture = content.Load<Texture2D>("Player_Sprite_Damaged");
            playerTextureWithHitbox = content.Load<Texture2D>("Player_Sprite_Hitbox");
            damagedPlayerTextureWithHitbox = content.Load<Texture2D>("Player_Sprite_Damaged_Hitbox");

            // Load bullet textures
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
            plusOneLifePowerUpTexture = content.Load<Texture2D>("plusOneLife"); // Nuevo power-up

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

            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            // Initialize score
            score = 0;

            // Initialize round
            round = 1;

            // Load heart textures
            heartFullTexture = content.Load<Texture2D>("HP_Icon");
            heartEmptyTexture = content.Load<Texture2D>("HP_Icon_Loss");

            // Initialize heart positions
            heartPositions = new List<Vector2>
            {
                new Vector2(10, 60),
                new Vector2(60, 60),
                new Vector2(110, 60)
            };

            powerUpSpawnChance = 0.05;
            player2Position = new Vector2(_graphics.PreferredBackBufferWidth / 4, _graphics.PreferredBackBufferHeight / 2);
            player2Speed = 400f;
            player2Lives = 3;
            player2Bullets = new List<PlayerBullet>();
            player2ShootTimer = 0f;
            player2HeartPositions = new List<Vector2>
{
    new Vector2(10, 100), // Ajusta las posiciones según el layout de pantalla
    new Vector2(60, 100),
    new Vector2(110, 100)
};

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
            string roundText = $"Infinite Mode - Round {round}";
            spriteBatch.DrawString(font, roundText, new Vector2(20, 0), Color.White);

            string scoreText = $"SCORE: {score.ToString("D7")}";
            spriteBatch.DrawString(font, scoreText, new Vector2(20, 20), Color.White);

            // Dibuja corazones del jugador 2
            for (int i = 0; i < player2HeartPositions.Count; i++)
            {
                Texture2D heartTexture = i < player2Lives ? heartFullTexture : heartEmptyTexture;
                spriteBatch.Draw(heartTexture, player2HeartPositions[i], Color.White);
            }

            // Dibuja el puntaje del jugador 2
            string scoreText2 = $"Player 2 SCORE: {score2.ToString("D7")}";
            spriteBatch.DrawString(font, scoreText2, new Vector2(20, 40), Color.White);

            // Dibuja las balas del jugador 2
            foreach (var bullet in player2Bullets)
            {
                bullet.Draw(spriteBatch);
            }


            Texture2D currentTexture;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift))
            {
                currentTexture = isInvincible && invincibleFlashTimer < FlashDuration / 2 ? damagedPlayerTextureWithHitbox : playerTextureWithHitbox;
            }
            else
            {
                currentTexture = isInvincible && invincibleFlashTimer < FlashDuration / 2 ? damagedPlayerTexture : playerTexture;
            }

            spriteBatch.Draw(currentTexture, playerPosition, null, Color.White, 0f, new Vector2(currentTexture.Width / 2, currentTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }
            spriteBatch.Draw(playerTexture, player2Position, Color.White);

            foreach (var bullet in player2Bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // Dibujar corazones del jugador 2
            for (int i = 0; i < player2HeartPositions.Count; i++)
            {
                Texture2D heartTexture = i < player2Lives ? heartFullTexture : heartEmptyTexture;
                spriteBatch.Draw(heartTexture, player2HeartPositions[i], Color.White);
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

            for (int i = 0; i < heartPositions.Count; i++)
            {
                if (i < playerLives)
                {
                    spriteBatch.Draw(heartFullTexture, heartPositions[i], Color.White);
                }
                else
                {
                    spriteBatch.Draw(heartEmptyTexture, heartPositions[i], Color.White);
                }
            }

            if (nextRoundStarting)
            {
                string message = $"Next round in {nextRoundRemainingSeconds} seconds";
                Vector2 messageSize = font.MeasureString(message);
                Vector2 messagePosition = new Vector2((_graphics.PreferredBackBufferWidth - messageSize.X) / 2, _graphics.PreferredBackBufferHeight / 2);
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
                int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, enemyDamagedTexture, new Vector2(xPosition, yPosition), random, 5));
            }

            for (int i = 0; i < numberOfMiniCopters; i++)
            {
                int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new MiniCopter3(_content, new Vector2(xPosition, yPosition), random));
            }
        }

        private void Shoot()
        {
            Vector2 bulletPosition = new Vector2(playerPosition.X - bulletTextures[0].Width / 2, playerPosition.Y - playerTexture.Height / 2 - 15);
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed;
            bullets.Add(newBullet);
        }

        private void ShootTriple()
        {
            Vector2 bulletPosition = new Vector2(playerPosition.X - bulletTextures[0].Width / 2, playerPosition.Y - playerTexture.Height / 2 - 15);
            float angleOffset = MathHelper.ToRadians(20);

            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed;
            bullets.Add(newBullet);

            PlayerBullet leftBullet = new PlayerBullet(bulletPosition, bulletTextures);
            leftBullet.Velocity = new Vector2((float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * bulletSpeed;
            bullets.Add(leftBullet);

            PlayerBullet rightBullet = new PlayerBullet(bulletPosition, bulletTextures);
            rightBullet.Velocity = new Vector2(-(float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * bulletSpeed;
            bullets.Add(rightBullet);
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
            else if (chance <= plusOneLifeSpawnChance) // Nuevo power-up de vida extra
            {
                powerUpPositions.Add(position);
                powerUpActiveList.Add(true);
                powerUpTexture = plusOneLifePowerUpTexture;
            }
        }

        private Rectangle GetPlayerBounds()
        {
            float scale = 0.05f;
            int hitboxWidth = (int)(playerTexture.Width * scale);
            int hitboxHeight = (int)(playerTexture.Height * scale);
            return new Rectangle(
                (int)(playerPosition.X - hitboxWidth / 2),
                (int)(playerPosition.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
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
            float currentSpeed = kstate.IsKeyDown(Keys.LeftShift) || kstate.IsKeyDown(Keys.RightShift) ? slowSpeed : playerSpeed;

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

            // En el método Update:
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && player2ShootTimer <= 0)
            {
                ShootPlayer2();
                player2ShootTimer = shootCooldown;
            }

            // Actualiza el temporizador de disparo del jugador 2
            player2ShootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;



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

            player2Position += direction2 * player2Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;


            playerPosition += direction1 * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerPosition.X > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
            {
                playerPosition.X = _graphics.PreferredBackBufferWidth - playerTexture.Width / 2;
            }
            else if (playerPosition.X < playerTexture.Width / 2)
            {
                playerPosition.X = playerTexture.Width / 2;
            }

            if (playerPosition.Y > 1035)
            {
                playerPosition.Y = 1035;
            }
            else if (playerPosition.Y < playerTexture.Height / 2)
            {
                playerPosition.Y = playerTexture.Height / 2;
            }

            void ShootPlayer2()
            {
                Vector2 bulletPosition = new Vector2(player2Position.X, player2Position.Y - playerTexture.Height / 2 - 15);
                PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
                newBullet.Velocity = new Vector2(0, -1) * bulletSpeed;
                player2Bullets.Add(newBullet);
            }

            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if ((kstate.IsKeyDown(Keys.Space) && shootTimer <= 0) || (kstate.IsKeyDown(Keys.LeftControl) && shootTimer <= 0))
            {
                if (powerUpCollected)
                {
                    ShootTriple();
                }
                else
                {
                    Shoot();
                }
                shootTimer = shootCooldown;
            }

            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            ShootEnemy();

            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);
                if (bullets[i].Position.Y < 0)
                {
                    bullets.RemoveAt(i);
                }
                else
                {
                    for (int j = enemies.Count - 1; j >= 0; j--)
                    {
                        if (bullets[i].GetBounds().Intersects(enemies[j].GetBounds()))
                        {
                            Vector2 enemyPosition = enemies[j].Position;
                            enemies[j].TakeDamage(1);
                            bullets.RemoveAt(i);
                            if (enemies[j].IsDead())
                            {
                                enemies.RemoveAt(j);
                                SpawnPowerUp(enemyPosition);

                                score += 1000;

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

            foreach (var bullet in enemyBullets)
            {
                if (!isInvincible && bullet.GetBounds().Intersects(GetPlayer2Bounds()))
                {
                    Player2TakeDamage();
                    enemyBullets.Remove(bullet);
                    break;
                }
            }


            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].Update(gameTime);
                if (enemyBullets[i].Position.Y > 1035)
                {
                    enemyBullets.RemoveAt(i);
                }
                else if (!isInvincible && enemyBullets[i].GetBounds().Intersects(GetPlayerBounds()))
                {
                    enemyBullets.RemoveAt(i);
                    PlayerTakeDamage();
                }
            }

            if (isInvincible)
            {
                invincibleTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                invincibleFlashTimer += gameTime.ElapsedGameTime.TotalSeconds;

                if (invincibleFlashTimer >= FlashDuration)
                {
                    invincibleFlashTimer = 0;
                }

                if (invincibleTimer <= 0)
                {
                    isInvincible = false;
                }
            }

            for (int i = 0; i < powerUpPositions.Count; i++)
            {
                if (powerUpActiveList[i])
                {
                    powerUpPositions[i] = new Vector2(powerUpPositions[i].X, powerUpPositions[i].Y + 100f * (float)gameTime.ElapsedGameTime.TotalSeconds);

                    if (powerUpPositions[i].Y > _graphics.PreferredBackBufferHeight)
                    {
                        powerUpActiveList[i] = false;
                    }
                    else if (GetPlayerBounds().Intersects(new Rectangle((int)powerUpPositions[i].X, (int)powerUpPositions[i].Y, powerUpTexture.Width, powerUpTexture.Height)))
                    {
                        powerUpActiveList[i] = false;
                        if (powerUpTexture == plusOneLifePowerUpTexture) // Recolectar el power-up de vida
                        {
                            if (playerLives < maxLives)
                            {
                                playerLives++;
                                heartPositions.Add(new Vector2(heartPositions[playerLives - 2].X + 50, heartPositions[playerLives - 2].Y)); // Agregar un corazón más a la derecha
                            }
                        }
                        else
                        {
                            powerUpCollected = true;
                            powerUpTimer = powerUpDuration;
                            currentPowerUpFrame = 0;
                        }
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
                if (!isInvincible && enemy.GetBounds().Intersects(GetPlayerBounds()))
                {
                    PlayerTakeDamage();
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
        private Rectangle GetPlayer2Bounds()
        {
            float scale = 0.05f; // Ajusta el factor de escala si es necesario
            int hitboxWidth = (int)(playerTexture.Width * scale);
            int hitboxHeight = (int)(playerTexture.Height * scale);

            return new Rectangle(
                (int)(player2Position.X - hitboxWidth / 2),
                (int)(player2Position.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
        }

        private void PlayerTakeDamage()
        {
            playerLives--;
            isInvincible = true;
            invincibleTimer = 2.0;

            if (playerLives <= 0)
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _graphics, score));
            }
        }
        private void Player2TakeDamage()
        {
            player2Lives--;
            isInvincible = true;
            invincibleTimer = 2.0;

            if (player2Lives <= 0)
            {
                // Lógica de game over o eliminación del jugador 2
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
