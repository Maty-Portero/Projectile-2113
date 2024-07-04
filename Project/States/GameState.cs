using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace Project.States
{
    internal class GameState : State
    {
        private const int MaxEnemies = 5;

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
        float bulletSpeed = 300f; // Velocidad de las balas

        // Enemy bullet variables
        Texture2D[] enemyBulletTextures;
        List<EnemyBullet> enemyBullets;

        // Enemy variables
        Texture2D[] enemyTexture;
        List<Enemy> enemies;
        Random random;

        // Power-up variables
        private Texture2D powerUpTexture;
        private Vector2 powerUpPosition;
        private bool powerUpActive = false;
        private bool powerUpCollected = false;
        private double powerUpDuration = 10.0; // Duración del power-up en segundos
        private double powerUpTimer = 0;
        private double powerUpSpawnChance = 0.05; // Probabilidad de aparición del power-up

        // Player lives variables
        private Texture2D heartFullTexture;
        private Texture2D heartEmptyTexture;
        private int playerLives = 3;
        private bool isInvincible = false;
        private double invincibleTimer = 0;
        private double invincibleFlashTimer = 0;
        private const double FlashDuration = 0.1; // Duración de cada flash
        private List<Vector2> heartPositions;

        // Score variables
        private int score;
        private SpriteFont font;

        // Stage and round variables
        private int stage;
        private int round;

        private bool roundCompleted;
        private double roundCompletionTimer;
        private const double roundCompletionDuration = 5.0;

        private bool stageCompleted;
        private double stageCompletionTimer;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;
            content.RootDirectory = "Content";

            playerSpeed = 200f;
            slowSpeed = 100f;
            deadZone = 4096;

            bullets = new List<PlayerBullet>();
            shootCooldown = 0.2f;
            shootTimer = 0;

            enemyBullets = new List<EnemyBullet>();

            random = new Random();

            playerTexture = content.Load<Texture2D>("Player_Sprite");
            damagedPlayerTexture = content.Load<Texture2D>("Player_Sprite_Damaged");
            playerTextureWithHitbox = content.Load<Texture2D>("NewPlayer_Sprite_ShiftingV2");
            damagedPlayerTextureWithHitbox = content.Load<Texture2D>("NewPlayer_Sprite_Damaged_ShiftingV2");

            // Load bullet textures
            bulletTextures = new Texture2D[2];
            bulletTextures[0] = content.Load<Texture2D>("player_bulletone");
            bulletTextures[1] = content.Load<Texture2D>("player_bullettwo");

            // Load enemy bullet textures
            enemyBulletTextures = new Texture2D[3];
            enemyBulletTextures[0] = content.Load<Texture2D>("EnemyBullet (1)");
            enemyBulletTextures[1] = content.Load<Texture2D>("EnemyBullet (2)");
            enemyBulletTextures[2] = content.Load<Texture2D>("EnemyBullet (3)");

            // Load enemy texture and create enemies
            enemyTexture = new Texture2D[3];
            enemyTexture[0] = content.Load<Texture2D>("EnemyV5");
            enemyTexture[1] = content.Load<Texture2D>("EnemyV5 (1)");
            enemyTexture[2] = content.Load<Texture2D>("EnemyV5 (2)");

            // Load power-up texture
            powerUpTexture = content.Load<Texture2D>("PowerUp-Sprite");

            // Load font for score
            font = content.Load<SpriteFont>("Fonts/ArcadeFont");

            enemies = new List<Enemy>();
            CreateEnemiesRound1();

            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);

            // Initialize score
            score = 0;

            // Initialize stage and round
            stage = 1;
            round = 1;

            // Load heart textures
            heartFullTexture = content.Load<Texture2D>("HeartFull");
            heartEmptyTexture = content.Load<Texture2D>("HeartEmpty");

            // Initialize heart positions
            heartPositions = new List<Vector2>
            {
                new Vector2(20, 60),  // Adjusted position to leave space for score
                new Vector2(60, 60),
                new Vector2(100, 60)
            };

            // Set power-up spawn chance
            powerUpSpawnChance = 0.5; // 50% de probabilidad de aparición del power-up
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            // Draw stage and round
            string stageText = $"Stage {stage} - Round {round}";
            spriteBatch.DrawString(font, stageText, new Vector2(20, 0), Color.White);

            // Draw score
            string scoreText = $"SCORE: {score.ToString("D7")}";
            spriteBatch.DrawString(font, scoreText, new Vector2(20, 20), Color.White);

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

            // Draw player bullets
            foreach (var bullet in bullets)
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

            // Draw power-up
            if (powerUpActive)
            {
                spriteBatch.Draw(powerUpTexture, powerUpPosition, Color.White);
            }

            // Draw hearts
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

            if (roundCompleted)
            {
                string completionText = "Round Completed, Next Round Starting in 5 Seconds";
                Vector2 textSize = font.MeasureString(completionText);
                Vector2 textPosition = new Vector2((_graphics.PreferredBackBufferWidth - textSize.X) / 2, (_graphics.PreferredBackBufferHeight - textSize.Y) / 2);
                spriteBatch.DrawString(font, completionText, textPosition, Color.White);
            }

            if (stageCompleted)
            {
                string completionText = "Stage Completed!";
                Vector2 textSize = font.MeasureString(completionText);
                Vector2 textPosition = new Vector2((_graphics.PreferredBackBufferWidth - textSize.X) / 2, (_graphics.PreferredBackBufferHeight - textSize.Y) / 2);
                spriteBatch.DrawString(font, completionText, textPosition, Color.White);
            }

            spriteBatch.End();
        }

        private void CreateEnemiesRound1()
        {
            // Create 3 enemies on the right side
            for (int i = 0; i < 3; i++)
            {
                int xPosition = _graphics.PreferredBackBufferWidth - enemyTexture[0].Width;
                int yPosition = i * (enemyTexture[0].Height + 10);
                enemies.Add(new Enemy(enemyTexture, new Vector2(xPosition, yPosition), random));
            }

            // Create 2 enemies on the left side
            int yOffset = 30; // Additional offset for enemies on the left
            for (int i = 0; i < 2; i++)
            {
                int xPosition = 0;
                int yPosition = i * (enemyTexture[0].Height + 10) + yOffset;
                enemies.Add(new Enemy(enemyTexture, new Vector2(xPosition, yPosition), random));
            }
        }

        private void CreateEnemiesRound2()
        {
            for (int i = 0; i < 6; i++)
            {
                int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, new Vector2(xPosition, yPosition), random));
            }
        }

        private void CreateEnemiesRound3()
        {
            Vector2[] positions = new Vector2[]
            {
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - enemyTexture[0].Width / 2, 0),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - enemyTexture[0].Width - 5, enemyTexture[0].Height + 5),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 + 5, enemyTexture[0].Height + 5),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - enemyTexture[0].Width * 1.5f - 10, (enemyTexture[0].Height + 5) * 2),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - enemyTexture[0].Width / 2, (enemyTexture[0].Height + 5) * 2),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 + enemyTexture[0].Width / 2 + 5, (enemyTexture[0].Height + 5) * 2),
                new Vector2(_graphics.PreferredBackBufferWidth / 2 - enemyTexture[0].Width * 2 - 15, (enemyTexture[0].Height + 5) * 3),
            };

            foreach (var pos in positions)
            {
                enemies.Add(new Enemy(enemyTexture, pos, random));
            }
        }

        private void CreateEnemiesRound4()
        {
            for (int i = 0; i < 9; i++)
            {
                int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, new Vector2(xPosition, yPosition), random));
            }
        }

        private void CreateEnemiesRound5()
        {
            for (int i = 0; i < 10; i++)
            {
                int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture[0].Width);
                int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4);
                enemies.Add(new Enemy(enemyTexture, new Vector2(xPosition, yPosition), random));
            }
        }

        private void Shoot()
        {
            // Ajusta la posición inicial del proyectil para que salga desde la punta roja
            Vector2 bulletPosition = new Vector2(playerPosition.X, playerPosition.Y - playerTexture.Height / 2);
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed; // Hacia arriba
            bullets.Add(newBullet);
        }

        private void ShootTriple()
        {
            // Ajusta la posición inicial del proyectil para que salga desde la punta roja
            Vector2 bulletPosition = new Vector2(playerPosition.X, playerPosition.Y - playerTexture.Height / 2);
            float angleOffset = MathHelper.ToRadians(20); // Ángulo de 20°

            // Disparo central
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bulletTextures);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed; // Hacia arriba
            bullets.Add(newBullet);

            // Disparo hacia la izquierda
            PlayerBullet leftBullet = new PlayerBullet(bulletPosition, bulletTextures);
            leftBullet.Velocity = new Vector2((float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * bulletSpeed;
            bullets.Add(leftBullet);

            // Disparo hacia la derecha
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
            if (random.NextDouble() <= powerUpSpawnChance)
            {
                powerUpPosition = position;
                powerUpActive = true;
            }
        }

        private Rectangle GetPlayerBounds()
        {
            float scale = 0.7f; // Escala del 70%
            int hitboxWidth = (int)(playerTexture.Width * scale);
            int hitboxHeight = (int)(playerTexture.Height * scale);
            return new Rectangle(
                (int)(playerPosition.X - hitboxWidth / 2),
                (int)(playerPosition.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            float currentSpeed = kstate.IsKeyDown(Keys.LeftShift) || kstate.IsKeyDown(Keys.RightShift) ? slowSpeed : playerSpeed;

            Vector2 direction = Vector2.Zero;

            if (kstate.IsKeyDown(Keys.Up))
            {
                direction.Y -= 1;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                direction.Y += 1;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                direction.X -= 1;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                direction.X += 1;
            }

            if (direction != Vector2.Zero)
            {
                direction.Normalize();
            }

            playerPosition += direction * currentSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (playerPosition.X > _graphics.PreferredBackBufferWidth - playerTexture.Width / 2)
            {
                playerPosition.X = _graphics.PreferredBackBufferWidth - playerTexture.Width / 2;
            }
            else if (playerPosition.X < playerTexture.Width / 2)
            {
                playerPosition.X = playerTexture.Width / 2;
            }

            if (playerPosition.Y > _graphics.PreferredBackBufferHeight - playerTexture.Height / 2)
            {
                playerPosition.Y = _graphics.PreferredBackBufferHeight - playerTexture.Height / 2;
            }
            else if (playerPosition.Y < playerTexture.Height / 2)
            {
                playerPosition.Y = playerTexture.Height / 2;
            }

            // Disparo del jugador
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Z) && shootTimer <= 0)
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

            // Actualizar enemigos
            foreach (var enemy in enemies)
            {
                enemy.Update(gameTime);
            }
            ShootEnemy();

            // Actualizar balas del jugador
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
                            bullets.RemoveAt(i);
                            enemies.RemoveAt(j);
                            //CreateEnemy();
                            SpawnPowerUp(enemyPosition); // Spawns a power-up at the position of the killed enemy with a probability

                            // Incrementar puntaje
                            score += 1000;

                            break;
                        }
                    }
                }
            }

            // Actualizar balas del enemigo
            for (int i = enemyBullets.Count - 1; i >= 0; i--)
            {
                enemyBullets[i].Update(gameTime);
                if (enemyBullets[i].Position.Y > _graphics.PreferredBackBufferHeight)
                {
                    enemyBullets.RemoveAt(i);
                }
                else if (!isInvincible && enemyBullets[i].GetBounds().Intersects(GetPlayerBounds()))
                {
                    enemyBullets.RemoveAt(i);
                    PlayerTakeDamage();
                }
            }

            // Actualizar invencibilidad
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

            // Actualizar power-up
            if (powerUpActive)
            {
                powerUpPosition.Y += 100f * (float)gameTime.ElapsedGameTime.TotalSeconds; // Velocidad de caída del power-up

                if (powerUpPosition.Y > _graphics.PreferredBackBufferHeight)
                {
                    powerUpActive = false;
                }
                else if (GetPlayerBounds().Intersects(new Rectangle((int)powerUpPosition.X, (int)powerUpPosition.Y, powerUpTexture.Width, powerUpTexture.Height)))
                {
                    powerUpActive = false;
                    powerUpCollected = true;
                    powerUpTimer = powerUpDuration;
                }
            }

            // Temporizador del power-up
            if (powerUpCollected)
            {
                powerUpTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (powerUpTimer <= 0)
                {
                    powerUpCollected = false;
                }
            }

            // Comprobar colisiones con enemigos
            foreach (var enemy in enemies)
            {
                if (!isInvincible && enemy.GetBounds().Intersects(GetPlayerBounds()))
                {
                    PlayerTakeDamage();
                }
            }

            // Check for round completion
            if (enemies.Count == 0 && !roundCompleted)
            {
                roundCompleted = true;
                roundCompletionTimer = roundCompletionDuration;
            }

            if (roundCompleted)
            {
                roundCompletionTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (roundCompletionTimer <= 0)
                {
                    round++;
                    roundCompleted = false;
                    if (round == 3)
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
                if (stageCompletionTimer <= 0)
                {
                    _game.ChangeState(new MenuState(_game, _graphicsDevice, _content, _graphics));
                }
            }
        }

        private void PlayerTakeDamage()
        {
            playerLives--;
            isInvincible = true;
            invincibleTimer = 2.0; // 2 segundos de invencibilidad

            // Verificar si el jugador está muerto
            if (playerLives <= 0)
            {
                _game.ChangeState(new GameOverState(_game, _graphicsDevice, _content, _graphics));
                // Implementar lógica para cuando el jugador muere
                // Por ejemplo, reiniciar el juego o mostrar una pantalla de Game Over
            }
        }
    }
}
