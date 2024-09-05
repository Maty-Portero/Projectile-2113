using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Project.Animations
{
    public class PointsAnimation
    {
        private Texture2D[] frames;
        private Vector2 position;
        private int currentFrame;
        private double frameTimer;
        private double frameDuration;
        public bool IsFinished { get; private set; }

        public PointsAnimation(Texture2D[] frames, Vector2 position, double frameDuration)
        {
            this.frames = frames;
            this.position = position;
            this.frameDuration = frameDuration;
            currentFrame = 0;
            frameTimer = 0;
            IsFinished = false;
        }

        public void Update(GameTime gameTime)
        {
            if (IsFinished) return;

            frameTimer += gameTime.ElapsedGameTime.TotalSeconds;

            if (frameTimer >= frameDuration)
            {
                frameTimer = 0;
                currentFrame++;

                if (currentFrame >= frames.Length)
                {
                    IsFinished = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!IsFinished)
            {
                spriteBatch.Draw(frames[currentFrame], position, null, Color.White, 0f,
                    new Vector2(frames[currentFrame].Width / 2, frames[currentFrame].Height / 2), 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
