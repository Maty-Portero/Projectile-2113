using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.States
{
    public class EnemyBullet
    {
        public Vector2 Position;
        public float Speed = 300f;
        private Texture2D[] textures;
        private int currentFrame;
        private double animationTime;
        private double timePerFrame = 0.1;

        public EnemyBullet(Vector2 position, Texture2D[] textures)
        {
            Position = position;
            this.textures = textures;
            currentFrame = 0;
            animationTime = 0;
        }

        public void Update(GameTime gameTime)
        {
            Position.Y += Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

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
}
