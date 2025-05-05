using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GameStateManager stateManager;

        Menu menu;

        LevelSelector levelSelector;

        Level level1;

        Player[] playerArr;

        List<Texture2D> playerTextures;

        HealthPowerUp medkit;

        ShieldPowerUp shield;

        int screenW, screenH, timer;

        // Textures
        public Texture2D bullet;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = config.tileSize * config.numTilesHorizontal;
            graphics.PreferredBackBufferHeight = config.tileSize * config.numTilesVertical;
            graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            stateManager = new GameStateManager();
            stateManager.changeState(GameState.MainMenu);

            // TODO: Add your initialization logic here
            screenW = config.tileSize * config.numTilesHorizontal;
            screenH = config.tileSize * config.numTilesVertical;

            // Player Textures
            playerTextures = new List<Texture2D>();
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/bob"));
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/felicia"));
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/albert"));
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/joel"));
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/pedro"));

            bullet = this.Content.Load<Texture2D>("Gun Textures/basic");
            Texture2D greenHealth = this.Content.Load<Texture2D>("Player Textures/greenHealthBar");
            Texture2D redHealth = this.Content.Load<Texture2D>("Player Textures/redHealthBar");
            /*playerArr[0] = new Player(playerTextures, bullet, greenHealth, redHealth, new Vector2(400, 50), 1, screenH, 1);
            playerArr[1] = new Player(playerTextures, bullet, greenHealth, redHealth, new Vector2(400, 100), 2, screenH, 2);*/



            menu = new Menu(Services, stateManager);

            levelSelector = new LevelSelector(Services, stateManager);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

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
            GameState oldState = stateManager.currentState;
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here

            switch (stateManager.currentState)
            {
                case GameState.MainMenu:
                    menu.Update(stateManager.currentState);
                    break;
                case GameState.LevelSelector:
                    levelSelector.Update(stateManager.currentState);
                    break;
                case GameState.Level1:
                    UpdateLevel1();
                    break;
                case GameState.Quit:
                    this.Exit();
                    break;
            }

            // After the switch case ends in Update
            // After the switch case in Update:
            if (oldState != stateManager.currentState)
            {
                if (stateManager.Is(GameState.MainMenu))
                {
                    menu = new Menu(Services, stateManager);
                }
                else if (stateManager.Is(GameState.LevelSelector))
                {
                    levelSelector = new LevelSelector(Services, stateManager);
                }
                else if (stateManager.Is(GameState.Level1))
                {
                    initPlayers(levelSelector.NumPlayers); // <-- call this ONLY when we enter Level1
                }
            }



            base.Update(gameTime);
        }

        // Getters
        public Texture2D getBulletTexture()
        {
            return bullet;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            // TODO: Add your drawing code here
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            switch (stateManager.currentState)
            {
                case GameState.MainMenu:
                    menu.Draw(spriteBatch);
                    break;
                case GameState.LevelSelector:
                    levelSelector.Draw(spriteBatch);
                    break;
                case GameState.Level1:
                    level1.Draw(spriteBatch);
                    for (int i = 0; i < level1.playerArr.Length; i++)
                    {
                        level1.playerArr[i].Draw(spriteBatch);
                    }
                    medkit.Draw(spriteBatch);
                    shield.Draw(spriteBatch);
                    break;
            }






            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateLevel1()
        {
            for (int i = 0; i < level1.playerArr.Length; i++)
            {
                level1.playerArr[i].Update(level1.playerArr[(i + 1) % 2], level1);
            }

            medkit.Update(timer, level1.playerArr);
            level1.Update();

            shield.Update(timer, level1.playerArr);

            timer++;
        }

        public void initPlayers(int numPlayers)
        {
            playerArr = new Player[numPlayers];

            for (int i = 0; i < numPlayers; i++)
            {
                playerArr[i] = new Player(playerTextures, bullet,
                    Content.Load<Texture2D>("Player Textures/greenHealthBar"),
                    Content.Load<Texture2D>("Player Textures/redHealthBar"),
                    new Vector2(400, 50 + i * 50), i + 1, screenH, i + 1);
            }

            shield = new ShieldPowerUp(400, 1040, this.Content.Load<Texture2D>("Item Textures/ShieldPotion"), this.Content.Load<Texture2D>("Item Textures/UsingShieldPotion"), new Rectangle(0, 0, screenW, screenH));
            level1 = new Level(Services, "Level1.txt", "StoneTiles", playerArr);
            medkit = new HealthPowerUp(200, 1040, this.Content.Load<Texture2D>("Item Textures/MedKit"), this.Content.Load<Texture2D>("Item Textures/UsingMedKit"), new Rectangle(0, 0, screenW, screenH));

            timer = 0;


        }
    }
}
