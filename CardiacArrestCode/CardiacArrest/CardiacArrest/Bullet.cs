using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace CardiacArrest
{
    class Bullet : Sprite
    {
        private Vector2 BulletSpeed;
        private Vector2 BulletDirection;
        private bool DrawMe;

        public Bullet(Texture2D InTexture, Rectangle InRect, Vector2 InPosition, Vector2 InBulletSpeed, Vector2 InBulletDirection, SpriteEffects InFlipping, Color InColor, float InRotation)
            : base(InTexture, InRect, InRotation, InFlipping, InColor)
        {
            flipping = InFlipping;
            BulletSpeed = InBulletSpeed;
            BulletDirection = InBulletDirection;
            DrawMe = true;
        }

        public void DeleteMe()
        {
            DrawMe = false;
        }
        public float GetDirection()
        {
            return BulletDirection.X;
        }
        public bool GetDrawMe()
        {
            return DrawMe;
        }

        public bool Update(Rectangle TargetRect, float ScreenWidth, float ScreenHeight)
        {
            if (DrawMe == true)
            {
                rectangle.X += (int)(BulletSpeed.X * BulletDirection.X);
                if (rectangle.Intersects(TargetRect))
                {
                    return true;
                }
            }
            return false;
        }

        public override void Draw(SpriteBatch theSpriteBatch)
        {
            if (DrawMe)
            {
                base.Draw(theSpriteBatch);
            }
        }
    }
}
