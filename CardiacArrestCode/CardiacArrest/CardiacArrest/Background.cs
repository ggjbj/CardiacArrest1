using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CardiacArrest
{
    class Background
    {
        
        Texture2D backgroundTexture;
        Texture2D backgroundBackTexture;
        Texture2D backgroundMiddleTexture;
        Texture2D backgroundFrontTexture;

        Rectangle backgroundRectangle;
        Rectangle backgroundBackRectangle;
        Rectangle backgroundMiddleRectangle;
        Rectangle backgroundFrontRectangle;

        KeyboardState keystate = Keyboard.GetState();
        KeyboardState oldKeystate;

        Color colour = Color.White;

        Vector2 backgroundBackspeed;
        Vector2 backgroundMiddlespeed;

        public Background(Texture2D _back, Texture2D back, Texture2D middle, Texture2D front, int mapWidth, int screenWidth, int screenHeight)
        {
            backgroundTexture = _back;
            backgroundRectangle = new Rectangle(0, 0, screenWidth, screenHeight);

            backgroundBackTexture = back;
            backgroundBackRectangle = new Rectangle(-mapWidth / 4, -screenHeight, mapWidth, screenHeight * 2);

            backgroundMiddleTexture = middle;
            backgroundMiddleRectangle = new Rectangle(-mapWidth / 2, -screenHeight, mapWidth, screenHeight * 2);

            backgroundFrontTexture = front;
            backgroundFrontRectangle = new Rectangle(-mapWidth / 2, -screenHeight, mapWidth, screenHeight * 2);

            backgroundBackspeed.X = backgroundBackRectangle.X;
            backgroundBackspeed.X = backgroundMiddleRectangle.X;
        }

        public void Update(int mapWidth, int movementSpeed, Vector2 PlayerPos, Vector2 ScreenSize)
        {
            keystate = Keyboard.GetState();
            int basicSpeed = (mapWidth / 2) / movementSpeed;
            int divider = 20;

            if(keystate.IsKeyDown(Keys.Left) && PlayerPos.X < ScreenSize.X / 3)
            {
                backgroundFrontRectangle.X += basicSpeed / (2 * divider);
                backgroundMiddleRectangle.X += basicSpeed / (4 * divider);
                backgroundBackRectangle.X += basicSpeed / (6 * divider);
            }

            if (keystate.IsKeyDown(Keys.Right) && PlayerPos.X > (ScreenSize.X / 3) * 2)
            {
                backgroundFrontRectangle.X -= basicSpeed / (2 * divider);
                backgroundMiddlespeed.X -= (float)basicSpeed / (4*divider);
                backgroundBackspeed.X -= (float)basicSpeed / (6 * divider);
            }

            if (keystate.IsKeyDown(Keys.Up) && PlayerPos.Y < (ScreenSize.Y / 3))
            {
                backgroundFrontRectangle.Y -= basicSpeed / (2 * divider);
                backgroundMiddleRectangle.Y -= basicSpeed / (4 * divider);
                backgroundBackRectangle.Y -= basicSpeed / (6 * divider);
            }

            if (keystate.IsKeyDown(Keys.Down) && PlayerPos.Y > (ScreenSize.Y / 3) * 2)
            {
                backgroundFrontRectangle.Y += basicSpeed / (2 * divider);
                backgroundMiddleRectangle.Y += basicSpeed / (4 * divider);
                backgroundBackRectangle.Y += basicSpeed / (6*divider);
            }

            backgroundBackRectangle.X = (int)backgroundBackspeed.X;
            backgroundMiddleRectangle.X = (int)backgroundMiddlespeed.X;
            oldKeystate = keystate;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backgroundTexture, backgroundRectangle, colour);
            spriteBatch.Draw(backgroundBackTexture, backgroundBackRectangle, colour);
            spriteBatch.Draw(backgroundMiddleTexture, backgroundMiddleRectangle, colour);
            spriteBatch.Draw(backgroundFrontTexture, backgroundFrontRectangle, colour);
        }
    }
}
