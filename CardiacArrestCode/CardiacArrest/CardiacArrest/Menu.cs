using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text;

namespace CardiacArrest
{
    enum Alignment
    {
        Left,
        Center,
        Right,
    }

    class Menu
    {
        public KeyboardState keystate = Keyboard.GetState();
        public KeyboardState oldKeystate;
        public Rectangle titleRectangle;
        public Texture2D titleTexture;
        public Rectangle backgroundRectangle;
        public Texture2D backgroundTexture;
        public bool onScreen = true;
        public Random Random = new Random();
        public Color colour = Color.White;
        public Color selectedColour = Color.DarkRed;
        public List<menuOption> menuOptionList = new List<menuOption>();

        //UPDATE: Set a new random position in the game window
        public Menu(Rectangle rectangle, Texture2D texture)
        {
            titleRectangle = rectangle;
            titleTexture = texture;
        }

         public Menu()
        {

        }

        
        public virtual void Add(Rectangle rectangle, Texture2D texture)
        {
            menuOption option = new menuOption();
            option.rectangle = rectangle;
            option.texture = texture;
            menuOptionList.Add(option);
        }

        public virtual void Setup(Alignment alignment, int screenHeight, int screenWidth) 
        {
            titleRectangle.X = screenWidth / 2 - titleRectangle.Width / 2;
            
            int topMargin; 

            topMargin = screenHeight / 2 - (menuOptionList[0].rectangle.Height * menuOptionList.Count) / 2;
            menuOptionList[0].rectangle.Y = topMargin;
            for (int i = 1; i < menuOptionList.Count; i++)
            {
                menuOptionList[i].rectangle.Y = topMargin + (menuOptionList[0].rectangle.Height * i);
            }

            for (int i = 0; i < menuOptionList.Count; i++)
            {
                if (alignment == Alignment.Left)
                {
                    menuOptionList[i].rectangle.X = screenWidth / 100;
                }
                if (alignment == Alignment.Center)
                {
                    menuOptionList[i].rectangle.X = screenWidth / 2 - menuOptionList[i].rectangle.Width / 2;
                }
                if (alignment == Alignment.Right)
                {
                    menuOptionList[i].rectangle.X = screenWidth - menuOptionList[i].rectangle.Width - screenWidth / 100;
                }
            }
            menuOptionList[0].optionSet = true;
            menuOptionList[0].inUse = true;
        }

        public virtual void Update(GraphicsDevice graphicsdevice)
        {
            keystate = Keyboard.GetState();

            if (keystate.IsKeyDown(Keys.S) && oldKeystate.IsKeyUp(Keys.S))
            {
                for (int i = 0; i < menuOptionList.Count; i++)
                {
                    if (menuOptionList[i].inUse == true && menuOptionList[i].optionSet == true)
                    {
                        menuOptionList[i].optionSet = false;
                        menuOptionList[i].inUse = false;

                        if (i < menuOptionList.Count-1)
                        {
                            menuOptionList[i + 1].inUse = true;
                        }
                        else
                        {
                            menuOptionList[0].inUse = true;
                        }
                    }
                }
                for (int i = 0; i < menuOptionList.Count; i++)
                {
                    if (menuOptionList[i].inUse == true)
                    {
                        menuOptionList[i].optionSet = true;
                    }
                }
            }

            if (keystate.IsKeyDown(Keys.W) && oldKeystate.IsKeyUp(Keys.W))
            {
                for (int i = 0; i < menuOptionList.Count; i++)
                {
                    if (menuOptionList[i].inUse == true && menuOptionList[i].optionSet == true)
                    {
                        menuOptionList[i].optionSet = false;
                        menuOptionList[i].inUse = false;

                        if (i > 0)
                        {
                            menuOptionList[i - 1].inUse = true;
                        }
                        else
                        {
                            menuOptionList[menuOptionList.Count - 1].inUse = true;
                        }
                    }
                }
                for (int i = 0; i < menuOptionList.Count; i++)
                {
                    if (menuOptionList[i].inUse == true)
                    {
                        menuOptionList[i].optionSet = true;
                    }
                }
            }

            oldKeystate = keystate;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundTexture != null)
            {
                spriteBatch.Draw(backgroundTexture, backgroundRectangle, Color.White);
            }
            spriteBatch.Draw(titleTexture, titleRectangle, colour);

            for (int i = 0; i < menuOptionList.Count; i++)
            {
                if (menuOptionList[i].onScreen == true)
                {
                    if (menuOptionList[i].inUse == true)
                    {
                        spriteBatch.Draw(menuOptionList[i].texture, menuOptionList[i].rectangle, selectedColour);
                    }
                    else
                    {
                        spriteBatch.Draw(menuOptionList[i].texture, menuOptionList[i].rectangle, colour);
                    }
                }

            }
        }
        public virtual void Reset()
        {

        }
    }
}
