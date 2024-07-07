using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Project
{
    public class Enemy : Component
    {
        private Texture2D[] textures;
        private Texture2D[] damagedTextures; // Agrega texturas dañadas
        private int currentFrame;
        private double animationTime;
        private double timePerFrame = 0.05;
        private float shootCooldown;
        private float shootTimer;
        private Vector2 direction;
        private float speed;
        private Random random;
        private bool isDamaged;
        private double damageTimer;
        private const double damageDuration = 0.05; // Duración del sprite dañado

        public Vector2 Position { get; set; }
        public int Health { get; private set; }

        public Enemy(Texture2D[] textures, Texture2D[] damagedTextures, Vector2 position, Random random)
        {
            this.textures = textures;
            this.damagedTextures = damagedTextures; // Inicializa texturas dañadas
            Position = position;
            currentFrame = 0;
            animationTime = 0;
            shootCooldown = 1.0f;
            shootTimer = 0;
            this.random = random;
            Health = 5; // Vida inicial

            // Inicializar dirección y velocidad
            direction = position.X == 0 ? new Vector2(1, 0) : new Vector2(-1, 0);
            speed = 100f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D textureToDraw = isDamaged ? damagedTextures[currentFrame] : textures[currentFrame];
            spriteBatch.Draw(textureToDraw, Position, Color.White);
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

            // Actualizar temporizador de daño
            if (isDamaged)
            {
                damageTimer -= gameTime.ElapsedGameTime.TotalSeconds;
                if (damageTimer <= 0)
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
            Health -= damage;
            isDamaged = true;
            damageTimer = damageDuration;
        }
    }
}
