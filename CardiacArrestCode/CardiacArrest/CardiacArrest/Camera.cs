using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CardiacArrest
{
    static class Camera
    {
        static public Vector2 Location = Vector2.Zero;
        static KeyboardState keyState = Keyboard.GetState();
        static KeyboardState oldKeyState;

        static public void Update(TileMap myMap, int squaresAcross, int squaresDown, Vector2 PlayerPos, Vector2 ScreenSize)
        {
            keyState=Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Left))
            {
                if(PlayerPos.X < ScreenSize.X / 3)
                    Camera.Location.X = MathHelper.Clamp(Camera.Location.X--, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                if(PlayerPos.X > (ScreenSize.X / 3) * 2)
                    Camera.Location.X = MathHelper.Clamp(Camera.Location.X++, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                if(PlayerPos.Y < ScreenSize.Y / 3)
                    Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y--, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                if(PlayerPos.Y > (ScreenSize.Y / 3) * 2)
                    Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y++, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            oldKeyState = keyState;
        }
    }
}
