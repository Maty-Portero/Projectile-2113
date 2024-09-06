using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.States;
using System.Collections.Generic;

namespace Project
{
    public class MiniCopterBullet : EnemyBullet
    {
        private List<Texture2D> _sprites;
        private int _currentFrame;
        private double _animationTimer;
        private const double AnimationSpeed = 0.1; // Velocidad de la animación

        public MiniCopterBullet(ContentManager content, Vector2 position)
            : base(position, new Texture2D[0]) // La base puede requerir un arreglo no vacío
        {
            // Cargar los sprites del proyectil
            _sprites = new List<Texture2D>
            {
                content.Load<Texture2D>("enemy2BulletSprite_0"),
                content.Load<Texture2D>("enemy2BulletSprite_1"),
                content.Load<Texture2D>("enemy2BulletSprite_2"),
                content.Load<Texture2D>("enemy2BulletSprite_3"),
            };

            // Inicializar otros valores específicos del proyectil
            Speed = 400f; // Puedes ajustar la velocidad según el comportamiento deseado
        }

        public override void Update(GameTime gameTime)
        {
            // Mover el proyectil hacia abajo
            Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Animar el proyectil
            _animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (_animationTimer >= AnimationSpeed)
            {
                _currentFrame = (_currentFrame + 1) % _sprites.Count;
                _animationTimer = 0;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Dibujar el proyectil usando el frame actual de la animación
            spriteBatch.Draw(_sprites[_currentFrame], Position, Color.White);
        }

        public override Rectangle GetBounds()
        {
            Texture2D currentTexture = _sprites[_currentFrame];
            return new Rectangle((int)Position.X, (int)Position.Y, currentTexture.Width, currentTexture.Height);
        }
    }
}
