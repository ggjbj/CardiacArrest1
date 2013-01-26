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

        static public void Update(TileMap myMap, int squaresAcross, int squaresDown)
        {
            keyState=Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Left))
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (keyState.IsKeyDown(Keys.Right))
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (keyState.IsKeyDown(Keys.Up))
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            if (keyState.IsKeyDown(Keys.Down))
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            oldKeyState = keyState;
        }
    }
}
