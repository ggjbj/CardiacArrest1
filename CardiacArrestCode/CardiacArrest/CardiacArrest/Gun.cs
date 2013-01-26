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
using EmitterTester;

namespace CardiacArrest
{
    class Gun : Weapon
    {
        #region Fields
        List<Bullet> Bullets;               // list of bullets the gun has fired - deleted from when they hit a target or go off screen
        Texture2D BulletTex;                // texture of bullets
        Rectangle BulletRec;                // rectangle of bullets
        Vector2 Direction;                      // direction to fire in
        Vector2 BulletPosition;             // Bullet start position
        SoundEffect FireE;                  // fire sound effect
        SoundEffect Hit;                    // bullet-hits-enemy sound effect
        bool HitTarget;
        private int ticks;
        bool PlayFire;
        bool PlayHit;
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor for the gun class
        /// </summary>
        /// <param name="InBulletTex">texture of the bullet</param>
        /// <param name="InBulletRec">rectangle of the bullet</param>
        /// <param name="InBulletPower">how much the bullet decreases the target's health</param>
        /// <param name="inRotation">rotation of the gun...usually 0.0f</param>
        /// <param name="inFlipping">SpriteEffects - so flipping horizontally/vertically/none. Default none.</param>
        /// <param name="inColor">Color to shine on it</param>
        /// <param name="BulletDirection">direction the bullets shoot in from the gun</param>
        /// <param name="InFlippedPosition">position the gun should be at if the character flips - would usually be the character's position, minus the width of the gun rectangle</param>
        /// <param name="InFire">sound effect for firing the gun</param>
        /// <param name="InHit">sound effect for a bullet hitting a player</param>
        /// <param name="InSoundEffectsOn">bool to say whether sound effects are currently on</param>
        public Gun(Vector2 BulletPos, Texture2D InBulletTex, Rectangle InBulletRec, int InBulletPower, float inRotation, SpriteEffects inFlipping, Color inColor, int BulletDirection, SoundEffect InFire, SoundEffect InHit, bool InSoundEffectsOn)
            : base(InBulletPower)
        {
            FireE = InFire;
            Hit = InHit;
            BulletTex = InBulletTex;
            BulletRec = InBulletRec;
            BulletPosition = BulletPos;
            Direction = new Vector2(BulletDirection, 0);
            Bullets = new List<Bullet>();
        }
        #endregion
        #region Mutators
        /// <summary>
        /// deletes all the bullets in the list
        /// </summary>
        public void ResetBullets()
        {
            Bullets.RemoveRange(0, Bullets.Count);
        }
        /// <summary>
        /// removes bullets that are no longer being drawn on screen
        /// </summary>
        public void DeleteBullets()
        {
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].GetDrawMe() == false)
                {
                    Bullets.Remove(Bullets[i]);
                }
            }
        }
        public int Update(Vector2 CharPos, int CharWidth, Vector2 Velocity, Vector2 inDirection, int ScreenWidth, Rectangle targetRect, int health, float InRotation, float ScreenHeight)
        {
            BulletPosition = CharPos;
            Direction = inDirection;
            if (Bullets.Count > 0)
            {
                health = Fire(ScreenWidth, targetRect, health, ScreenHeight);
            }
            // remove the bullets that have hit the target, or else gone off screen
            DeleteBullets();
            return health;
        }
        public void ResetBools()
        {
            PlayHit = false;
            PlayFire = false;
        }
        /// <summary>
        /// adds a bullet to the list - use this when selecting a fire button
        /// </summary>
        public void AddBullet(Vector2 CharPos)
        {
            BulletRec.X = (int)BulletPosition.X;
            BulletRec.Y = (int)BulletPosition.Y;
            /*if (!PlayFire)
            {
                FireE.Play();
                PlayFire = false;
            }*/
            Bullets.Add(new Bullet(BulletTex, BulletRec, BulletPosition, new Vector2(2, 0), Direction, SpriteEffects.None, Color.White, 0.0f));
        }
        public void Draw(SpriteBatch theSpriteBatch)
        {
            foreach (Bullet item in Bullets)
            {
                item.Draw(theSpriteBatch);
            }
        }
        public int Fire(int ScreenWidth, Rectangle targetRect, int health, float ScreenHeight)
        {
            foreach (Bullet obj in Bullets)
            {
                if (HitTarget == false)
                {
                    PlayHit = false;
                    HitTarget = obj.Update(targetRect, ScreenWidth, ScreenHeight);
                }
                if (HitTarget)
                {
                    // delete the bullet
                    // decrease target's health
                    ticks++;
                    if (ticks > 30)
                    {
                        // health coding shit will go in here bro
                        health -= damage;
                        HitTarget = false;
                        obj.DeleteMe();
                        /*if (!PlayHit && SoundEffectsOn)
                        {
                            Hit.Play();
                            PlayHit = false;
                        }*/
                    }
                }
                if (obj.GetPos().X + obj.GetSize().X > ScreenWidth || obj.GetPos().X + obj.GetSize().X < 0)
                {
                    // delete the bullet
                    obj.DeleteMe();
                }
            }
            List<int> DeletedBullets = new List<int>();
            for (int i = 0; i < Bullets.Count; i++)
            {
                if (Bullets[i].GetDrawMe() == false)
                {
                    DeletedBullets.Add(i);
                }
            }
            return health;
        }
        #endregion
        #region Accessors
        public List<Bullet> GetBullets()
        {
            return Bullets;
        }
        public bool BulletMoved()
        {
            if (Bullets.Count > 0)
            {
                if (Direction.X == -1)
                {
                    if (Bullets.Last().GetDirection() == -1)
                    {
                        if (Bullets.Last().GetPos().X + Bullets.Last().GetSize().X < BulletPosition.X - Bullets.Last().GetSize().X - 50)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                if (Direction.X == 1)
                {
                    if (Bullets.Last().GetDirection() == 1)
                    {
                        if (Bullets.Last().GetPos().X > BulletPosition.X + Bullets.Last().GetSize().X + 200)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                return false;
            }
            return true;

        }
        #endregion

    }
}
