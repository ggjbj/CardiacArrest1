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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Game World
        Player player;
        Enemy enemy;
        Gun enemyGun, playerGun;
        #endregion

        Vector2 BulletDir;
        // States of the game
        enum GameState
        {
            title,
            menu,
            pause,
            game,
            text,
            end,
            reset
        }

        GameState state = GameState.game;

        // states of the enemy AI
        enum MoveState
        {
            idle,
            left,
            right,
            fire,
            knife
        }

        MoveState enemyState = MoveState.idle;
        SpriteFont CaseFont;

        #region player Attributes
        PlayerData playerData;
        #endregion

        int noOfClues;
        int noOfCases;

        int enemyHealth = 10;

        // overall cases and clues: player collected ones will go into the PlayerData class
        Dictionary<string, CaseFile> caseList = new Dictionary<string, CaseFile>();

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int screenWidth;
        int screenHeight;
        Vector2 ScreenSize;

        Menu mainMenu;
        Sprite splashScreen;
        
        int mAlphaValue = 1;
        int mFadeIncrement = 5;
        double mFadeDelay = .035;

        int rValue = 255;
        int gValue = 255;
        int bValue = 255;
        
        int titleCounter;
        SoundEffect ixjingle;
        SoundEffect heartBeatIntro;
        HeartBeatTracker heartBeatTracker;
        Background background;
        KeyboardState oldKeystate;

        TileMap myMap = new TileMap();
        //Allows tiles to be drawn offscreen just enough to have no pop-in graphics
        int squaresAcross = 26;
        int squaresDown = 16;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            ScreenSize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            screenWidth = GraphicsDevice.Viewport.Width;
            screenHeight = GraphicsDevice.Viewport.Height;

            BulletDir = Vector2.Zero;
            noOfClues = 5;


            // List of cases
            #region caseList
            Sprite CaseItem = new Sprite(Content.Load<Texture2D>("case"), new Rectangle(0, 0, Content.Load<Texture2D>("case").Width, Content.Load<Texture2D>("case").Height), 0.0f, SpriteEffects.None, Color.White);
            for (int i = 0; i < noOfClues; i++)
            {
                caseList.Add("Case" + i.ToString(), new CaseFile(Content.Load<Texture2D>("popUp"), new Rectangle(10, 10, 150, 200), new Rectangle(0, 0, 3200, 2400), 0.0f, SpriteEffects.None, Color.White, CaseItem.Copy(), "lol", "hehe", Vector2.Zero));
            }
            #endregion

            #region MenuSetup
            mainMenu = new Menu(new Rectangle(0, -5, screenWidth*8/10, screenHeight / 8), Content.Load<Texture2D>("MainMenuTitle"));
            mainMenu.Add(new Rectangle(0, 0, screenWidth / 10, screenHeight / 10), Content.Load<Texture2D>("MainMenuPlay"));
            mainMenu.Add(new Rectangle(0, 0, screenWidth / 5, screenHeight / 9), Content.Load<Texture2D>("MainMenuSettings"));
            mainMenu.Add(new Rectangle(0, 0, screenWidth / 12, screenHeight / 9), Content.Load<Texture2D>("MainMenuExit"));

            Alignment align = Alignment.Left;
            mainMenu.Setup(align, screenHeight, screenWidth);

            mainMenu.backgroundRectangle = new Rectangle(0, 0, screenWidth, screenHeight);
            mainMenu.backgroundTexture = Content.Load<Texture2D>("MainMenuBackground");
            #endregion

            // Creates the user
            player = new Player(
                Content.Load<Texture2D>("Tron_Charcter2"), 8, 3);

            player.Load();

            // Creates the enemy
            enemy = new Enemy(
                Content.Load<Texture2D>("Tron_Charcter2"), 8, 3);

            enemy.Load();
            CaseFont = Content.Load<SpriteFont>("cFont");

            splashScreen = new Sprite(Content.Load<Texture2D>("betaJester1"), new Rectangle(0, 0, screenWidth, screenHeight), 0.0f, SpriteEffects.None, Color.White);
            //ixjingle = Content.Load<SoundEffect>("ixjingle");
            //heartBeatIntro = Content.Load<SoundEffect>("GGJ13_Theme");
            background = new Background(Content.Load<Texture2D>("gamebackground"), Content.Load<Texture2D>("gamebackgroundback"), Content.Load<Texture2D>("gamebackgroundmiddle"), Content.Load<Texture2D>("gamebackgroundfront"), 200 * 32, screenWidth, screenHeight);
            playerData = PlayerData.Load();

            //heartBeatTracker = new HeartBeatTracker(Content.Load<SoundEffect>("heartBeat"));

            if (playerData == null)
            {
                playerData = new PlayerData(100, 0.0f, 0, 0);
            }
			enemyGun = new Gun(new Vector2(enemy.enemyRectangle.X + enemy.enemyRectangle.Width, enemy.enemyRectangle.Y), Content.Load<Texture2D>("bullet"), new Rectangle(0, 0, Content.Load<Texture2D>("bullet").Width, Content.Load<Texture2D>("bullet").Height), 5, 0.0f, SpriteEffects.None, Color.White, 1, null, null, false);
            playerGun = new Gun(new Vector2(player.playerRectangle.X + player.playerRectangle.Width, player.playerRectangle.Y), Content.Load<Texture2D>("bullet"), new Rectangle(0, 0, Content.Load<Texture2D>("bullet").Width, Content.Load<Texture2D>("bullet").Height), 5, 0.0f, SpriteEffects.None, Color.White, 1, null, null, false);

            spriteBatch = new SpriteBatch(GraphicsDevice);
            Tile.TileSetTexture = Content.Load<Texture2D>("TileSet");
            background = new Background(Content.Load<Texture2D>("gameBackground"), Content.Load<Texture2D>("gameBackgroundBack"),
                Content.Load<Texture2D>("gameBackgroundMiddle"), Content.Load<Texture2D>("gameBackgroundFront"),
                (200 * 32), GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            KeyboardState keyState = Keyboard.GetState();

            // Press escape to close game and save score
            #region CloseGame
            if (keyState.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }
            #endregion

            switch (state)
            {
                case GameState.title:
                    #region Title
                    {
                        UpdateTitle(gameTime);                   
                    }
                    #endregion
                    break;

                case GameState.menu:
                    mainMenu.Update(GraphicsDevice);

                    if (keyState.IsKeyDown(Keys.Enter) && oldKeystate.IsKeyUp(Keys.Enter))
                    {
                        if (mainMenu.menuOptionList[0].optionSet == true)
                        {
                            state = GameState.game;
                        }

                        if (mainMenu.menuOptionList[2].optionSet == true)
                        {
                            this.Exit();
                        }
                    }
                    break;

                case GameState.game:

                    if (keyState.IsKeyDown(Keys.P) && oldKeystate.IsKeyUp(Keys.P))
                        state = GameState.pause;

                    if (player.health == 0)
                    {
                        state = GameState.end;
                    }

                    // allow player to knife attack
                    if (player.playerRectangle.Intersects(enemy.enemyRectangle))
                    {
                        if (keyState.IsKeyDown(Keys.A))
                        {
                            enemy.health -= 25;
                        }
                    }

                    switch (enemyState)
                    {
                        case MoveState.idle:

                            if (player.playerRectangle.X + (GraphicsDevice.Viewport.Width / 4) >= enemy.enemyRectangle.X && player.playerRectangle.X + 50 <= enemy.enemyRectangle.X)
                            {
                                enemyState = MoveState.left;
                            }

                            else if (player.playerRectangle.X - (GraphicsDevice.Viewport.Width / 4) <= enemy.enemyRectangle.X && player.playerRectangle.X - 50 >= enemy.enemyRectangle.X)
                            {
                                enemyState = MoveState.right;
                            }

                            if (enemy.Animation != "Idle")
                                enemy.Animation = "Idle";

                            

                            break;

                        case MoveState.right:

                            if (player.playerRectangle.X - (GraphicsDevice.Viewport.Width / 4) <= enemy.enemyRectangle.X && player.playerRectangle.X - 50 >= enemy.enemyRectangle.X)
                            {
                                enemy.Position.X += enemy.enemySpeed;
                                BulletDir = new Vector2(1, 0);
                            }

                            else
                            {
                                enemyState = MoveState.idle;
                            }

                            if (player.playerRectangle.X + (GraphicsDevice.Viewport.Width / 4) >= enemy.enemyRectangle.X && player.playerRectangle.X + 50 <= enemy.enemyRectangle.X)
                            {
                                enemyState = MoveState.left;
                            }

                            if (enemy.enemyRectangle.Intersects(player.playerRectangle))
                            {
                                enemyState = MoveState.knife;
                            }

                            if (enemy.Animation != "Walking")
                                enemy.Animation = "Walking";

                            break;

                        case MoveState.left:
                            if (player.playerRectangle.X + (GraphicsDevice.Viewport.Width / 4) >= enemy.enemyRectangle.X && player.playerRectangle.X + 50 <= enemy.enemyRectangle.X)
                            {
                                enemy.Position.X -= enemy.enemySpeed;
                                BulletDir = new Vector2(-1, 0);
                            }

                            else
                            {
                                enemyState = MoveState.idle;
                            }

                            if (player.playerRectangle.X - (GraphicsDevice.Viewport.Width / 4) <= enemy.enemyRectangle.X && player.playerRectangle.X - 50 >= enemy.enemyRectangle.X)
                            {
                                enemyState = MoveState.right;
                            }

                            if (enemy.enemyRectangle.Intersects(player.playerRectangle))
                            {
                                enemyState = MoveState.knife;
                            }

                            if (enemy.Animation != "Walking")
                                enemy.Animation = "Walking";
                            
                            break;

                        case MoveState.fire:

                            enemyGun.AddBullet(new Vector2(enemy.enemyRectangle.X, enemy.enemyRectangle.Y));

                            if (enemy.Animation != "Fire")
                                enemy.Animation = "Fire";
                            if (keyState.IsKeyUp(Keys.F))
                            {
                                enemyState = MoveState.idle;
                            }
                            break;

                        case MoveState.knife:

                            if (enemy.Animation != "Knife")
                                enemy.Animation = "Knife";

                            //player.health -= 20;

                            break;
                    }

                    Camera.Update(myMap, squaresAcross, squaresDown);


                    if (player.Update(GraphicsDevice))
                    {
                        playerGun.AddBullet(new Vector2(player.playerRectangle.X, player.playerRectangle.Y));
                    }
                    player.Update(gameTime);
                    
                    //heartBeatTracker.UpdateTest(player,new Rectangle(screenWidth/2,screenHeight/2,5,5));
                    //heartBeatTracker.Update3(player, new Rectangle(screenWidth / 2, screenHeight, 5, 5), new Rectangle(0, 0, 5, 5), new Rectangle(screenWidth, 0, 5, 5));
                    player.Collision(myMap,squaresAcross,squaresDown);
                    background.Update(200 * 32, 20);
                    enemy.Update(GraphicsDevice);
                    enemy.Update(gameTime);
                    Vector2 playerVec = new Vector2(0, 0);
                    if (enemy.enemyRectangle.X > player.playerRectangle.X)
                    {
                        playerVec.X = 1;
                    }
                    if (enemy.enemyRectangle.X < player.playerRectangle.X)
                    {
                        playerVec.X = -1;
                    }
                    playerGun.Update(new Vector2(player.playerRectangle.X, player.playerRectangle.Y), player.playerRectangle.Width, new Vector2(player.playerSpeed, 0), playerVec, (int)ScreenSize.X, enemy.enemyRectangle, enemyHealth, 0.0f, (int)ScreenSize.Y);
                    playerData.SetHealth(enemyGun.Update(new Vector2(enemy.enemyRectangle.X, enemy.enemyRectangle.Y), enemy.enemyRectangle.Width, new Vector2(enemy.enemySpeed, 0), BulletDir, (int)ScreenSize.X, player.playerRectangle, playerData.GetHealth(), 0.0f, (int)ScreenSize.Y));
                    foreach (KeyValuePair<string, CaseFile> item in caseList)
                    {
                        if (item.Value.Update(player.playerRectangle))
                        {
                            playerData.AddCase(item.Key);
                        }
                    }
                    break;

                case GameState.pause:

                    if (keyState.IsKeyDown(Keys.P) && oldKeystate.IsKeyUp(Keys.P))
                        state = GameState.game;

                    break;

                case GameState.text:
                    break;

                case GameState.end:
                    break;

                case GameState.reset:
                    break;
            }

            oldKeystate = keyState;

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
            spriteBatch.Begin();

            switch (state)
            {
                case GameState.title:
                    if (titleCounter > 150)
                        GraphicsDevice.Clear(Color.Black);

                    if (titleCounter > 125 && titleCounter<150)
                    {
                        rValue -= 10;
                        gValue -= 10;
                        bValue -= 10;
                    }
                    else
                    {
                        rValue = 255;
                        gValue = 255;
                        bValue = 255;
                    }

                    spriteBatch.Draw(splashScreen.texture, splashScreen.rectangle, new Color(rValue, gValue, bValue, (byte)MathHelper.Clamp(mAlphaValue, 0, 255)));
                    
                    break;

                case GameState.menu:
                    mainMenu.Draw(spriteBatch);
                    break;

                case GameState.game:
                    background.Draw(spriteBatch);

                    
            Vector2 firstSquare = new Vector2(Camera.Location.X / 32, Camera.Location.Y / 32);
            int firstX = (int)firstSquare.X;
            int firstY = (int)firstSquare.Y;

            Vector2 squareOffset = new Vector2(Camera.Location.X % 32, Camera.Location.Y % 32);
            int offsetX = (int)squareOffset.X;
            int offsetY = (int)squareOffset.Y;

            for (int y = 0; y < squaresDown; y++)
            {
                for (int x = 0; x < squaresAcross; x++)
                {
                    spriteBatch.Draw(
                        Tile.TileSetTexture,
                        new Rectangle((x * 32) - offsetX, (y * 32) - offsetY, 32, 32),
                        Tile.GetSourceRectangle(myMap.Rows[y + firstY].Columns[x + firstX].TileID),
                        Color.White);
                }
            }
            

                    player.Draw(spriteBatch);
                    enemy.Draw(spriteBatch);
                    enemyGun.Draw(spriteBatch);
                    playerGun.Draw(spriteBatch);
                    foreach (KeyValuePair<string, CaseFile> item in caseList)
                    {
                        item.Value.Draw(spriteBatch, CaseFont);
                    }
                    break;

                case GameState.pause:
                    break;

                case GameState.text:
                    break;

                case GameState.end:
                    break;

                case GameState.reset:
                    break;
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
        
        private void UpdateTitle(GameTime gameTime)
        {

            #region Fading

            mFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            if (mFadeDelay <= 0)
            {
                mFadeDelay = .035;

                mAlphaValue += mFadeIncrement;

                if (mAlphaValue >= 255 || mAlphaValue <= 0)
                {
                    mFadeIncrement *= -1;
                }
            }

            #endregion

            titleCounter++;

            if (titleCounter >= 300)
            {
                heartBeatIntro.Dispose();
                state = GameState.menu;
            }

            if (titleCounter < 150)
            {
                splashScreen.texture = Content.Load<Texture2D>("betaJester1");
                if (titleCounter == 1)
                   ixjingle.Play();
            }
            else
            {
                if (titleCounter == 150)
                {
                    //ixjingle.Play(0, 0, 0);
                    //betaJesterOut.Play();
                    heartBeatIntro.Play();
                }

                

                if(titleCounter>149) 
                splashScreen.texture = Content.Load<Texture2D>("betaJesterRed1");

                if (titleCounter > 160 && titleCounter<170 || titleCounter > 200 && titleCounter < 210 || titleCounter > 250 && titleCounter < 260 || titleCounter > 290)
                    splashScreen.texture = Content.Load<Texture2D>("betaJesterRed2");

            }
            /*if (titleCounter > 230)
            {
                titleScreen.rectangle.Width -= 160;
                titleScreen.rectangle.Height -= 40;
                titleScreen.rectangle.X -= 80;
                titleScreen.rectangle.Y -= 20;

            }*/

        }

    }


}

 