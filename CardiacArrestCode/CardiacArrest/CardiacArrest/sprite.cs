using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using EmitterTester;

namespace CardiacArrest
{
    class Sprite
    {
        public Texture2D texture;
        public Rectangle rectangle;
        protected float rotation;
        protected SpriteEffects flipping;
        protected Color color;

        public Sprite(Texture2D _texture, Rectangle _rectangle, float _rotation, SpriteEffects _flipping, Color _color)
        {
            texture = _texture;
            rectangle = _rectangle;
            rotation = _rotation;
            flipping = _flipping;
            color = _color;
        }

        public virtual void Update()
        {

        }

        public virtual void Update(GraphicsDevice graphicsDevice)
        {

        }

        public virtual void FlipSprite(SpriteEffects _flipping)
        {
            flipping = _flipping;
        }

        public virtual void ChangeRotation(float _rotation)
        {
            rotation = _rotation;
        }

        /// <summary>
        /// returns the rectangle.x and rectangle.y as a cute lil vector
        /// </summary>
        /// <returns></returns>
        public virtual Vector2 GetPos()
        {
            return new Vector2(rectangle.X, rectangle.Y);
        }
        public virtual Sprite Copy()
        {
            return new Sprite(texture, rectangle, rotation, flipping, color);
        }
        public virtual void SetPos(Vector2 pos)
        {
            rectangle.X = (int)pos.X;
            rectangle.Y = (int)pos.Y;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            // Cuts down on spritebatch draw code within the game by calling this spriteBatch instead
            spriteBatch.Draw(texture, rectangle, null, color, rotation, Vector2.Zero, flipping, 0.0f);
        }

        /// <summary>
        /// gets the dimensions of the sprite
        /// </summary>
        /// <returns>vector of size; x = width; y = height</returns>
        public Vector2 GetSize()
        {
            return new Vector2(rectangle.Width, rectangle.Height);
        }

        /// <summary>
        /// resets the sprite to a given position
        /// </summary>
        /// <param name="pos"></param>
        public virtual void Reset(Vector2 pos)
        {
            rectangle.X = (int)pos.X;
            rectangle.Y = (int)pos.Y;
        }
    }

    class Weapon
    {
        protected int damage;

        public Weapon(int inDamage)
        {
            damage = inDamage;
        }

        public void Update(GraphicsDevice graphicsDevice)
        {

        }
    }

    class Knife : Weapon
    {
        public Knife(int inDamage)
            : base(inDamage)
        {
        }
    }

    class PopUp : Sprite
    {
        protected string text;
        protected Vector2 TextPos;
        protected bool ShowMe;
        public PopUp(Texture2D newTexture, Rectangle newRectangle, float inRotation, SpriteEffects inFlipping, Color inColor, string _text, Vector2 _textPos)
            : base(newTexture, newRectangle, inRotation, inFlipping, inColor)
        {
            text = _text;
            TextPos = _textPos;
        }

        public override void Update(GraphicsDevice graphicsDevice)
        {
        }

        public virtual void Draw(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            if (ShowMe)
            {
                base.Draw(theSpriteBatch);
                theSpriteBatch.DrawString(font, text, TextPos, Color.Black);
            }
        }
    }



    class Pause : PopUp
    {
        public Pause(Texture2D newTexture, Rectangle newRectangle, float inRotation, SpriteEffects inFlipping, Color inColor, string text, Vector2 TextPos)
            : base(newTexture, newRectangle, inRotation, inFlipping, inColor, text, TextPos)
        {
        }

        public override void Update(GraphicsDevice graphicsDevice)
        {
        }
    }

    class ClueAssembler : PopUp
    {
        public ClueAssembler(Texture2D newTexture, Rectangle newRectangle, float inRotation, SpriteEffects inFlipping, Color inColor, string text, Vector2 TextPos)
            : base(newTexture, newRectangle, inRotation, inFlipping, inColor, text, TextPos)
        {
        }

        public override void Update(GraphicsDevice graphicsDevice)
        {
        }
    }

}
