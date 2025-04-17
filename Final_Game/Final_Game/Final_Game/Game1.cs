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
            // TODO: Add your initialization logic here
            screenW = config.tileSize * config.numTilesHorizontal;
            screenH = config.tileSize * config.numTilesVertical;

            // Player Textures
            playerTextures = new List<Texture2D>();
            playerTextures.Add(this.Content.Load<Texture2D>("Player Textures/bob_right"));

            level1 = new Level(Services, "Level1.txt", "StoneTiles");
            bullet = this.Content.Load<Texture2D>("Gun Textures/basic");
            playerArr = new Player[2];
            playerArr[0] = new Player(playerTextures, bullet, new Vector2(50, 50), 1, screenH, 1);
            playerArr[1] = new Player(playerTextures, bullet, new Vector2(100, 100), 2, screenH, 2);

            medkit = new HealthPowerUp(200, 1040, this.Content.Load<Texture2D>("Item Textures/MedKit"), this.Content.Load<Texture2D>("Item Textures/UsingMedKit"), new Rectangle(0, 0, screenW, screenH));

            shield = new ShieldPowerUp(400, 1040, this.Content.Load<Texture2D>("Item Textures/ShieldPotion"), this.Content.Load<Texture2D>("Item Textures/UsingShieldPotion"), new Rectangle(0, 0, screenW, screenH));

            timer = 0;

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
            KeyboardState kb = Keyboard.GetState();
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || kb.IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            for (int i = 0; i < playerArr.Length; i++)
            {
                playerArr[i].Update(playerArr[(i + 1) % 2], level1);
            }

            medkit.Update(timer, playerArr);

            shield.Update(timer, playerArr);

            timer++;

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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            level1.Draw(spriteBatch);
            for (int i = 0; i < playerArr.Length; i++)
            {
                playerArr[i].Draw(spriteBatch);
            }
            medkit.Draw(spriteBatch);
            shield.Draw(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
