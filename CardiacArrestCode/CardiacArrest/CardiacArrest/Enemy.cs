using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CardiacArrest
{
    class Enemy : SpriteAnimation
    {
        public Rectangle enemyRectangle;
        public int enemySpeed = 2;
        public int health = 50;

        public Enemy(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
        }

        public void Reset(Vector2 NewPos)
        {
            Position = NewPos;
        }

        public void Load()
        {
            // Set user screen position
            Position = new Vector2(500, 200);

            AnimationClass EnemyAni = new AnimationClass();

            Color enemyColour = new Color();
            enemyColour = Color.Red;
            float scale = 0.3f;

            // Create instances in the library for each animation
            EnemyAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Walking", 2, 8, EnemyAni.Copy(), enemyColour, scale);

            EnemyAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Fire", 3, 4, EnemyAni.Copy(), enemyColour, scale);

            EnemyAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Knife", 3, 4, EnemyAni.Copy(), enemyColour, scale);

            EnemyAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Idle", 1, 8, EnemyAni.Copy(), enemyColour, scale);

            // Set default user animation
            Animation = "Idle";
        }

        public override void Update(GraphicsDevice graphicsDevice)
        {
            enemyRectangle = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), (int)(width/3), (int)(height/3));
        }
    }
}
