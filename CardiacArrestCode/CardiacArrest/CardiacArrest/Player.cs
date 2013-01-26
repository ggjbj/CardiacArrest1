using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CardiacArrest
{
    class Player : SpriteAnimation
    {
        public Rectangle playerRectangle;
        public float playerSpeed = 0f;
        public int health = 100;

        public Player(Texture2D Texture, int frames, int animations)
            : base(Texture, frames, animations)
        {
        }

        public void Load()
        {
            // Set user screen position
            Position = new Vector2(80, 200);

            AnimationClass playerAni = new AnimationClass();

            Color playerColour = new Color();
            playerColour = Color.Blue;
            float scale = 0.3f;

            // Create instances in the library for each animation
            playerAni.Rotation = 0f;
            AddAnimation("Climb", 1, 8, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = 0f;
            AddAnimation("Crouch", 3, 4, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Walking", 1, 8, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Fire", 3, 4, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Knife", 3, 4, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Idle", 2, 8, playerAni.Copy(), playerColour, scale);

            playerAni.Rotation = MathHelper.PiOver4 * 2;
            AddAnimation("Jump", 2, 8, playerAni.Copy(), playerColour, scale);

            // Set default user animation
            Animation = "Idle";
        }

        public void Reset(Vector2 NewPos)
        {
            Position = NewPos;
        }

        public void pushCharacter(string directionToPush, TileMap myMap, int squaresAcross, int squaresDown)
        {
            if (directionToPush == "Left")
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X - 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (directionToPush == "Right")
            {
                Camera.Location.X = MathHelper.Clamp(Camera.Location.X + 2, 0, (myMap.MapWidth - squaresAcross) * 32);
            }

            if (directionToPush == "Up")
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y - 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }

            if (directionToPush == "Down")
            {
                Camera.Location.Y = MathHelper.Clamp(Camera.Location.Y + 2, 0, (myMap.MapHeight - squaresDown) * 32);
            }
        }


        public void Collision(TileMap myMap, int squaresAcross, int squaresDown)
        {
            KeyboardState kbState = Keyboard.GetState();

            int tileColumn = (int) (((Camera.Location.X+Position.X)/32));
            int tileRow = (int)((Camera.Location.Y+Position.Y) / 32);

            if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Floor"] || myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Cracked Floor"])
            {
                pushCharacter("Up", myMap, squaresAcross, squaresDown);
            }
            else if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Empty"] && myMap.Rows[tileRow + 1].Columns[tileColumn].TileID == myMap.Tiles["Empty"])
                {
                    pushCharacter("Down", myMap, squaresAcross, squaresDown);
                }
            else if(myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Cracked Ladder"] || myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Left Wall Ladder"] || myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Wall Ladder"] || myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Right Wall Ladder"] || myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Transparent Front Ladder"])
            {
                if(kbState.IsKeyDown(Keys.Up))
                {
                    pushCharacter("Up",myMap,squaresAcross,squaresDown);
                }

                if (kbState.IsKeyDown(Keys.Down))
                {
                    pushCharacter("Down", myMap, squaresAcross, squaresDown);
                }
            }
            else if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Trap Door"])
            {
                pushCharacter("Down", myMap, squaresAcross, squaresDown);
            }
            else if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Seweage Man Hole"])
            {
                if (kbState.IsKeyDown(Keys.E))
                {
                    if (kbState.IsKeyDown(Keys.Up))
                    {
                        pushCharacter("Up", myMap, squaresAcross, squaresDown);
                    }

                    if (kbState.IsKeyDown(Keys.Down))
                    {
                        pushCharacter("Down", myMap, squaresAcross, squaresDown);
                    }
                }
            }
            else if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Ceiling"])
            {
                pushCharacter("Down", myMap, squaresAcross, squaresDown);
            }
            else if (myMap.Rows[tileRow].Columns[tileColumn].TileID == myMap.Tiles["Ceiling"])
            {

            }
            
            
            }


        public bool Update(GraphicsDevice graphicsDevice)
        {
            KeyboardState kbState = Keyboard.GetState();

            playerRectangle = new Rectangle((int)Math.Round(Position.X), (int)Math.Round(Position.Y), (int)(width/3), (int)(height/3));

            if (kbState.IsKeyDown(Keys.Right))
            {
                Position.X += playerSpeed;

                if (Animation != "Walking")
                    Animation = "Walking";
            }

            else if (kbState.IsKeyDown(Keys.Left))
            {
                Position.X -= playerSpeed;
 
                if (Animation != "Walking")
                    Animation = "Walking";
            }

            else if (kbState.IsKeyDown(Keys.Down))
            {
                Position.Y += playerSpeed;

                if (Animation != "Crouch")
                    Animation = "Crouch";
            }

            else if (kbState.IsKeyDown(Keys.Up))
            {
                Position.Y -= playerSpeed;

                if (Animation != "Climb")
                    Animation = "Climb";
            }

            else if (kbState.IsKeyDown(Keys.Space))
            {
                if (Animation != "Jump")
                    Animation = "Jump";
            }

            else if (kbState.IsKeyDown(Keys.F))
            {
                if (Animation != "Fire")
                    Animation = "Fire";
            }

            else if (kbState.IsKeyDown(Keys.A))
            {
                if (Animation != "Knife")
                    Animation = "Knife";
            }

            else
            {
                if (Animation != "Idle")
                    Animation = "Idle";
            }

            

            if (Animation == "Fire")
            {
                return true;
            }
            return false;
        }
    }
}
