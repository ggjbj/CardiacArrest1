using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CardiacArrest
{
    class CaseFile : PopUp
    {
        protected bool isFound;
        protected bool ClueItemOnScreen;
        protected Sprite clueItem;
        protected Rectangle Area;
        Random random;
        protected int timer;
        protected string CaseDesc;
        protected string Clue;
        
        /// <summary>
        /// constructor for the case file class
        /// </summary>
        /// <param name="newTexture">the texture to use for the popup</param>
        /// <param name="newRectangle">the rectangle to use for the popup</param>
        /// <param name="inArea">the area coordinates in which to place the clueitem in</param>
        /// <param name="inRotation">rotation of the popup</param>
        /// <param name="inFlipping">spriteeffects of popup</param>
        /// <param name="inColor">color light to shine on the popup</param>
        /// <param name="_clueItem">sprite for the user to pick up</param>
        public CaseFile(Texture2D newTexture, Rectangle newRectangle, Rectangle inArea, float inRotation, SpriteEffects inFlipping, Color inColor, Sprite _clueItem, string _CaseDesc, string _clue, Vector2 TextPos)
            : base(newTexture, newRectangle, inRotation, inFlipping, inColor, _CaseDesc + _clue, TextPos)
        {
            isFound = false;
            clueItem = _clueItem;
            random = new Random();
            Area = inArea;
            CaseDesc = _CaseDesc;
            Clue = _clue;
        }

        public bool CaseIsFound()
        {
            return isFound;
        }
        public string GetCase()
        {
            return CaseDesc;
        }
        public string GetClue()
        {
            return Clue;
        }
        public Sprite GetClueItem()
        {
            return clueItem;
        }
        public void SetFoundFlag(bool _isFound)
        {
            isFound = _isFound;
        }
        /// <summary>
        /// Update method for the case file - updates both the showing of the popup and the pos of the sprite
        /// you pick up
        /// </summary>
        /// <param name="playerRect">the player's rectangle</param>
        /// <returns></returns>
        public virtual bool Update(Rectangle playerRect)
        {
            if (!isFound)
            {
                if (!ClueItemOnScreen)
                {
                    clueItem.SetPos(new Vector2(random.Next(Area.X, Area.X + Area.Width), random.Next(Area.Y, Area.Y + Area.Height)));
                    ClueItemOnScreen = true;
                }
                if (playerRect.Intersects(clueItem.rectangle))
                {
                    isFound = true;
                    ShowMe = true;
                }
            }
            if (ShowMe == true)
            {
                timer++;
            }
            if (timer >= 60 * 5)
            {
                ShowMe = false;
                timer = 0;
            }
            return isFound;
        }
        public virtual void Draw(SpriteBatch theSpriteBatch, SpriteFont font)
        {
            base.Draw(theSpriteBatch, font);
            clueItem.Draw(theSpriteBatch);
        }
    }
}
