using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Project
{
    public class Enemy : Component
    {
        private Texture2D[] textures;
        private Texture2D[] damagedTextures;
        private int currentFrame;
        private double animationTime;
        private double timePerFrame = 0.05;
        private float shootCooldown;
        private float shootTimer;
        private Vector2 direction;
        private float speed;
        private Random random;
        private int health;
        private bool isDamaged;
        private double damageFlashTimer;
        private double damageFlashDuration = 0.05;

        public Vector2 Position { get; set; }

        public Enemy(Texture2D[] textures, Texture2D[] damagedTextures, Vector2 position, Random random, int health)
        {
            this.textures = textures;
            this.damagedTextures = damagedTextures;
            Position = position;
            currentFrame = 0;
            animationTime = 0;
            shootCooldown = 1.0f;
            shootTimer = 0;
            this.random = random;
            this.health = 5;

            // Inicializar dirección y velocidad
            direction = position.X == 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
            speed = 100f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isDamaged)
            {
                spriteBatch.Draw(damagedTextures[currentFrame], Position, Color.White);
            }
            else
            {
                spriteBatch.Draw(textures[currentFrame], Position, Color.White);
            }
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, textures[0].Width, textures[0].Height);
        }

        public override void Update(GameTime gameTime)
        {
            // Actualizar animación
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTime >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % textures.Length;
                animationTime -= timePerFrame;
            }

            // Actualizar tiempo de disparo
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Actualizar posición
            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cambiar dirección al llegar a los bordes de la pantalla
            if (Position.X < 0 || Position.X > 800 - textures[0].Width) // Asumiendo que el ancho de la pantalla es 800
            {
                direction.X = -direction.X;
            }

            // Actualizar flash de daño
            if (isDamaged)
            {
                damageFlashTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (damageFlashTimer <= 0)
                {
                    isDamaged = false;
                }
            }
        }

        public bool CanShoot()
        {
            if (shootTimer <= 0)
            {
                shootTimer = shootCooldown;
                return true;
            }
            return false;
        }

        public void TakeDamage(int damage)
        {
            health -= damage;
            isDamaged = true;
            damageFlashTimer = damageFlashDuration;
        }

        public bool IsDead()
        {
            return health <= 0;
        }
    }
}
