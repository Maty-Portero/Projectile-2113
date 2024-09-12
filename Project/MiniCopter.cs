using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.States;
using System;
using System.Collections.Generic;

namespace Project
{
    public class MiniCopter : Enemy
    {
        private List<Texture2D> _sprites;
        private List<Texture2D> _damagedSprites;
        private int _currentFrame;
        private double _animationTimer;
        private const double AnimationSpeed = 0.1; // Velocidad de la animación

        public MiniCopter(ContentManager content, Vector2 position, Random random)
            : base(new Texture2D[0], new Texture2D[0], position, random, 2) // Vida de 2
        {
            // Cargar los sprites del MiniCopter
            _sprites = new List<Texture2D>
            {
                content.Load<Texture2D>("MiniCopterSprite_0"),
                content.Load<Texture2D>("MiniCopterSprite_1"),
                content.Load<Texture2D>("MiniCopterSprite_2"),
            };

            // Cargar los sprites dañados
            _damagedSprites = new List<Texture2D>
            {
                content.Load<Texture2D>("MiniCopterDamagedSprite_0"),
                content.Load<Texture2D>("MiniCopterDamagedSprite_1"),
                content.Load<Texture2D>("MiniCopterDamagedSprite_2"),
            };

            speed = 150f; // Puede ajustar la velocidad según el comportamiento deseado
            direction = new Vector2(1, 0); // Inicializa la dirección del movimiento hacia la derecha
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

            // Ajustar posición y dirección para mantenerse dentro de los límites de la pantalla
            Position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Cambiar dirección al llegar a los bordes de la pantalla
            if (Position.X < 0 || Position.X > 1920 - _sprites[0].Width) // Ajustar según el tamaño de la pantalla
            {
                direction.X = -direction.X;
                Position = new Vector2(MathHelper.Clamp(Position.X, 0, 1920 - _sprites[0].Width), Position.Y);
            }

            // Lógica adicional, como disparar
            if (CanShoot())
            {
                FireProjectile();
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Dibujar el MiniCopter usando el frame actual
            if (isDamaged)
            {
                spriteBatch.Draw(_damagedSprites[_currentFrame], Position, Color.White);
            }
            else
            {
                spriteBatch.Draw(_sprites[_currentFrame], Position, Color.White);
            }
        }

        public override Rectangle GetBounds()
        {
            // Reducir el tamaño del hitbox al 90%
            Texture2D currentTexture = _sprites[_currentFrame];
            int hitboxWidth = (int)(currentTexture.Width * 0.9f);
            int hitboxHeight = (int)(currentTexture.Height * 0.9f);
            return new Rectangle(
                (int)(Position.X - hitboxWidth / 2),
                (int)(Position.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
        }

        public void FireProjectile()
        {
            var projectile = new MiniCopterBullet(GameState2.Instance.ContentManager, new Vector2(Position.X, Position.Y + 10));
            GameState2.Instance.AddEnemyBullet(projectile);
        }

    }
}
