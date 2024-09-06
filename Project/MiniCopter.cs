using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Project
{
    public class MiniCopter : Enemy
    {
        private List<Texture2D> _sprites;
        private int _currentFrame;
        private double _animationTimer;
        private const double AnimationSpeed = 0.1; // Velocidad de la animación

        public MiniCopter(ContentManager content, Vector2 position, Random random)
            : base(new Texture2D[0], new Texture2D[0], position, random, 2) // Vida de 2
        {
            // Cargar los sprites del MiniCopter
            _sprites = new List<Texture2D>
            {
                content.Load<Texture2D>("miniCopterSprite_0"),
                content.Load<Texture2D>("miniCopterSprite_1"),
                content.Load<Texture2D>("miniCopterSprite_2"),
                content.Load<Texture2D>("miniCopterSprite_3"),
            };

            // Ajustar la velocidad y dirección si es necesario
            speed = 150f; // Puede ajustar la velocidad según el comportamiento deseado
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Animar el MiniCopter
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= AnimationSpeed)
            {
                _currentFrame = (_currentFrame + 1) % _sprites.Count;
                _animationTimer = 0;
            }

            // Lógica adicional, como disparar
            // Podrías agregar un método para disparar desde aquí si es necesario
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Dibujar el MiniCopter usando el frame actual
            spriteBatch.Draw(_sprites[_currentFrame], Position, Color.White);
        }

        public override Rectangle GetBounds()
        {
            // Reducir el tamaño del hitbox al 50%
            Texture2D currentTexture = _sprites[_currentFrame];
            int hitboxWidth = (int)(currentTexture.Width * 0.5f);
            int hitboxHeight = (int)(currentTexture.Height * 0.5f);
            return new Rectangle(
                (int)(Position.X - hitboxWidth / 2),
                (int)(Position.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
        }

        public void FireProjectile(ContentManager content)
        {
            // Crear y disparar el proyectil específico del MiniCopter
            var projectile = new MiniCopterBullet(content, new Vector2(Position.X, Position.Y + 10)); // Ajustar la posición inicial del proyectil
            // Aquí deberías agregar el proyectil a la lista de proyectiles activos en el GameState
        }
    }
}
