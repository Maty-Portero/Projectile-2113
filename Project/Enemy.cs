using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace Project
{
    public class Enemy : Component
    {
        private Texture2D[] textures;
        private int currentFrame;
        private double animationTime;
        private double timePerFrame = 0.05;
        private float shootCooldown;
        private float shootTimer;
        private Vector2 direction;
        private float speed;
        private Random random;

        public Vector2 Position { get; set; }

        public Enemy(Texture2D[] textures, Vector2 position, Random random)
        {
            this.textures = textures;
            Position = position;
            currentFrame = 0;
            animationTime = 0;
            shootCooldown = 1.0f;
            shootTimer = 0;
            this.random = random;

            // Inicializar dirección y velocidad
            direction = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
            direction.Normalize();
            speed = 100f;
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textures[currentFrame], Position, Color.White);
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

            // Cambiar dirección aleatoriamente
            if (random.NextDouble() < 0.01)
            {
                direction = new Vector2((float)(random.NextDouble() * 2 - 1), (float)(random.NextDouble() * 2 - 1));
                direction.Normalize();
            }

            // Limitar el movimiento dentro de los bordes de la pantalla
            if (Position.X < 0 || Position.X > 800 - textures[0].Width) // Asumiendo que el ancho de la pantalla es 800
            {
                direction.X = -direction.X;
            }

            if (Position.Y < 0 || Position.Y > 150 - textures[0].Height) // Asumiendo que la altura de la pantalla es 480
            {
                direction.Y = -direction.Y;
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
    }
}


