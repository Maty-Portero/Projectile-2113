using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Project.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Project
{
    internal class Player
    {
        ContentManager content;
        public Texture2D playerTexture;
        public Texture2D damagedPlayerTexture;
        public Texture2D playerTextureWithHitbox;
        public Texture2D damagedPlayerTextureWithHitbox;
        public Vector2 playerPosition;
        public float playerSpeed = 400f;
        public float slowSpeed = 200f;
        // Player lives variables
        public Texture2D heartFullTexture;
        public Texture2D heartEmptyTexture;
        public Texture2D _currentTexture;
        public int playerLives = 3;
        public bool isInvincible = false;
        public double invincibleTimer = 0;
        public double invincibleFlashTimer = 0;
        public double FlashDuration = 0.1; // Duración de cada flash
        public int rocketRemaining = 1;
        public List<PlayerBullet> bullets = new List<PlayerBullet>();
        // Score variables
        public int score;
        Texture2D currentTexture;

        //player bullet variables
        public float shootCooldown = 0.2f;
        public float shootTimer = 0;
        public float bulletSpeed = 600f;

        public Player(ContentManager cm)
        { 
            this.content= cm;
        }
        public void Load()
        {
            playerTexture = content.Load<Texture2D>("Player_Sprite");
            damagedPlayerTexture = content.Load<Texture2D>("Player_Sprite_Damaged");
            playerTextureWithHitbox = content.Load<Texture2D>("Player_Sprite_Hitbox");
            damagedPlayerTextureWithHitbox = content.Load<Texture2D>("Player_Sprite_Damaged_Hitbox");
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentTexture, playerPosition, null, Color.White, 0f, new Vector2(currentTexture.Width / 2, currentTexture.Height / 2), Vector2.One, SpriteEffects.None, 0f);
        }
        public void SlowSpeed()
        {
            currentTexture = isInvincible && invincibleFlashTimer < FlashDuration / 2 ? damagedPlayerTextureWithHitbox : playerTextureWithHitbox;
        }
        public void NormalSpeed()
        {
            currentTexture = isInvincible && invincibleFlashTimer < FlashDuration / 2 ? damagedPlayerTexture : playerTexture;
        }

        public Rectangle GetPlayerBounds()
        {
            float scale = 0.05f; // Escala del 70%
            int hitboxWidth = (int)(playerTexture.Width * scale);
            int hitboxHeight = (int)(playerTexture.Height * scale);
            return new Rectangle(
                (int)(playerPosition.X - hitboxWidth / 2),
                (int)(playerPosition.Y - hitboxHeight / 2),
                hitboxWidth,
                hitboxHeight
            );
        }
        public void Shoot(Texture2D[] bTex)
        {
            Vector2 bulletPosition = new Vector2(playerPosition.X - 50 / 2, playerPosition.Y - playerTexture.Height / 2 - 15);
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bTex);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed;
            bullets.Add(newBullet);
        }
        public void ShootTriple(Texture2D[] bTex)
        {
            Vector2 bulletPosition = new Vector2(playerPosition.X - 50 / 2, playerPosition.Y - playerTexture.Height / 2 - 15);
            float angleOffset = MathHelper.ToRadians(20);
            PlayerBullet newBullet = new PlayerBullet(bulletPosition, bTex);
            newBullet.Velocity = new Vector2(0, -1) * bulletSpeed;
            bullets.Add(newBullet);

            PlayerBullet leftBullet = new PlayerBullet(bulletPosition, bTex);
            leftBullet.Velocity = new Vector2((float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * bulletSpeed;
            bullets.Add(leftBullet);

            PlayerBullet rightBullet = new PlayerBullet(bulletPosition, bTex);
            rightBullet.Velocity = new Vector2(-(float)Math.Sin(angleOffset), -(float)Math.Cos(angleOffset)) * bulletSpeed;
            bullets.Add(rightBullet);
        }

    }
}
