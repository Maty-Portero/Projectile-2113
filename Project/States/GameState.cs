using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Project.States
{
    public class Bullet
    {
        public Vector2 Position;
        public float Speed = 300f;
        private Texture2D[] textures;
        private int currentFrame;
        private double animationTime;
        private double timePerFrame = 0.1;

        public Bullet(Vector2 position, Texture2D[] textures)
        {
            Position = position;
            this.textures = textures;
            currentFrame = 0;
            animationTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            Position.Y -= Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Update animation
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTime >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % textures.Length;
                animationTime -= timePerFrame;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[currentFrame], Position, Color.White);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, textures[0].Width, textures[0].Height);
        }
    }

    

    internal class GameState : State
    {
        Texture2D playerTexture;
        Vector2 playerPosition;
        float playerSpeed;
        float slowSpeed;

        int deadZone;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // Bullet variables
        Texture2D[] bulletTextures;
        List<Bullet> bullets;
        float shootCooldown;
        float shootTimer;

        // Enemy variables
        Texture2D enemyTexture;
        Enemy enemy;
        Random random;

        public GameState(Game1 game, GraphicsDevice graphicsDevice, ContentManager content, GraphicsDeviceManager deviceManager) : base(game, graphicsDevice, content)
        {
            _graphics = deviceManager;
            content.RootDirectory = "Content";

            
            playerSpeed = 200f;
            slowSpeed = 100f;

            deadZone = 4096;

            bullets = new List<Bullet>();
            shootCooldown = 0.2f;
            shootTimer = 0;

            random = new Random();

            playerTexture = content.Load<Texture2D>("Player_Sprite");

            // Load bullet textures
            bulletTextures = new Texture2D[2];
            bulletTextures[0] = content.Load<Texture2D>("player_bulletone"); // Reemplaza con el nombre correcto
            bulletTextures[1] = content.Load<Texture2D>("player_bullettwo"); // Reemplaza con el nombre correcto

            // Load enemy texture and create an enemy
            enemyTexture = content.Load<Texture2D>("enemy_helicopter"); // Reemplaza con el nombre correcto
            CreateEnemy();

            playerPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2,
                _graphics.PreferredBackBufferHeight / 2);

            // TODO: use this.Content to load your game content here
            playerTexture = content.Load<Texture2D>("Player_Sprite");
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(playerTexture, playerPosition, null, Color.White, 0f, new Vector2(playerTexture.Width / 2, playerTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);

            // Dibujar balas
            foreach (var bullet in bullets)
            {
                bullet.Draw(spriteBatch);
            }

            // Dibujar enemigo
            if (enemy != null)
            {
                enemy.Draw(gameTime, spriteBatch);
            }

            spriteBatch.End();
        }

        private void CreateEnemy()
        {
            int xPosition = random.Next(0, _graphics.PreferredBackBufferWidth - enemyTexture.Width);
            int yPosition = random.Next(0, _graphics.PreferredBackBufferHeight / 4); // Limitar a la parte superior
            enemy = new Enemy(enemyTexture, new Vector2(xPosition, yPosition));
        }
        private void Shoot()
        {
            Vector2 bulletPosition = new Vector2(playerPosition.X, playerPosition.Y - playerTexture.Height / 2);
            Bullet newBullet = new Bullet(bulletPosition, bulletTextures);
            bullets.Add(newBullet);
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

            // Disparo
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (kstate.IsKeyDown(Keys.Z) && shootTimer <= 0)
            {
                Shoot();
                shootTimer = shootCooldown;
            }

            // Actualizar balas
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                bullets[i].Update(gameTime);
                if (bullets[i].Position.Y < 0)
                {
                    bullets.RemoveAt(i);
                }
                else if (enemy != null && bullets[i].GetBounds().Intersects(enemy.GetBounds()))
                {
                    bullets.RemoveAt(i);
                    enemy = null; // Elimina al enemigo
                    CreateEnemy(); // Crea un nuevo enemigo
                }
            }
        }
    }
}
