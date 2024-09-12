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
        protected Vector2 direction; // Cambiado a protected para acceso en clases derivadas
        protected float speed; // Cambiado a protected para acceso en clases derivadas
        private Random random;
        private int health;
        protected bool isDamaged; // Cambiado a protected para permitir acceso desde clases derivadas
        private double damageFlashTimer;
        private double damageFlashDuration = 0.05;

        public Vector2 Position { get; set; }

        public Enemy(Texture2D[] textures, Texture2D[] damagedTextures, Vector2 position, Random random, int health = 5) // Cambiado la vida por defecto a 5
        {
            this.textures = textures;
            this.damagedTextures = damagedTextures;
            Position = position;
            currentFrame = 0;
            animationTime = 0;
            shootCooldown = 1.0f;
            shootTimer = 0;
            this.random = random;
            this.health = health;

            // Inicializar dirección y velocidad
            direction = position.X == 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
            speed = 100f;
        }

        // Implementación del método Draw heredado de Component
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (isDamaged)
            {
                if (currentFrame < damagedTextures.Length)
                {
                    spriteBatch.Draw(damagedTextures[currentFrame], Position, Color.White);
                }
            }
            else
            {
                if (currentFrame < textures.Length)
                {
                    spriteBatch.Draw(textures[currentFrame], Position, Color.White);
                }
            }
        }

        // Implementación del método Update heredado de Component
        public override void Update(GameTime gameTime)
        {
            // Actualizar animación
            animationTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (textures.Length > 0 && animationTime >= timePerFrame)
            {
                currentFrame = (currentFrame + 1) % textures.Length;
                animationTime -= timePerFrame;
            }

            // Actualizar tiempo de disparo
            shootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Actualizar posición
            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cambiar dirección al llegar a los bordes de la pantalla
            if (textures.Length > 0 && (Position.X < 0 || Position.X > 1920 - textures[0].Width)) // Ajustar según el tamaño de la pantalla
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

        public virtual Rectangle GetBounds()
        {
            return textures.Length > 0 ? new Rectangle((int)Position.X, (int)Position.Y, textures[0].Width, textures[0].Height) : Rectangle.Empty;
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

        public virtual void TakeDamage(int damage) // Cambiado a virtual para permitir override en clases derivadas
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
