using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Final_Game
{
    class LevelSelector
    {
        Texture2D currentLevel;
        Rectangle currentLlocation;

        ContentManager content;

        Player[] playerArr;

        SpriteFont font;

        int numPlayers = 2;
        public int NumPlayers => numPlayers;


        int inputDelay = 10;

        Button level1, level2, level3, cursor, backButton;
        int screenW = config.screenW, screenH = config.screenH;
        GamePadState oldPad;

        GameStateManager stateManager;

        Texture2D map1, map2, map3;
        Texture2D glitchedMap1, glitchedMap2, glitchedMap3;

        public LevelSelector(IServiceProvider _content, GameStateManager stateManager)
        {
            content = new ContentManager(_content, "Content");

            font = content.Load<SpriteFont>("SpriteFont1");

            this.stateManager = stateManager;

            map1 = content.Load<Texture2D>("level1pic");
            map2 = content.Load<Texture2D>("level1pic");
            map3 = content.Load<Texture2D>("level1pic");

            glitchedMap1 = content.Load<Texture2D>("Map1Glitched");
            glitchedMap2 = content.Load<Texture2D>("Map2Glitched");
            glitchedMap3 = content.Load<Texture2D>("Map3Glitched");

            level1 = new Button(
                content.Load<Texture2D>("Map3Tex"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40, 200, 80), Color.White, 0.8f);
            level2 = new Button(
                content.Load<Texture2D>("Map2Tex"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40 + 120, 200, 80), Color.White, 0.7f);
            level3 = new Button(
                content.Load<Texture2D>("Map3Tex"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40 + 240, 200, 80), Color.White, 0.6f);
            cursor = new Button(
                content.Load<Texture2D>("Map1Glitched"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40, 200, 80), Color.White, 0f);

            currentLevel = map1;
            currentLlocation = new Rectangle(screenW / 2 - 250, screenH / 2 - 400, 500, 300);


            backButton = new Button(content.Load<Texture2D>("backButtonTex"), new Rectangle(10, screenH - 100, 200, 80), Color.White, 0.8f);
        }

        public void Update(GameState gameState)
        {
            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            if (cursor.location.Y == level1.location.Y)
            {
                cursor.tex = glitchedMap1;
                currentLevel = map1;
            }
            if (cursor.location.Y == level2.location.Y)
            {
                cursor.tex = glitchedMap2;
                currentLevel = map2;
            }
            if (cursor.location.Y == level3.location.Y)
            {
               cursor.tex = glitchedMap3;
                currentLevel = map3;
            }

            if (inputDelay > 0)
            {
                inputDelay--;
                oldPad = pad;
                return;
            }

            level1.Update(pad, oldPad, cursor);
            level2.Update(pad, oldPad, cursor);
            level3.Update(pad, oldPad, cursor);

            if (pad.DPad.Down == ButtonState.Pressed && !(oldPad.DPad.Down == ButtonState.Pressed))
                cursor.location.Y += 120;
            if (pad.DPad.Up == ButtonState.Pressed && !(oldPad.DPad.Up == ButtonState.Pressed))
                cursor.location.Y -= 120;

            if (pad.DPad.Left == ButtonState.Pressed && oldPad.DPad.Left != ButtonState.Pressed)
            {
                numPlayers = Math.Max(2, numPlayers - 1); // Minimum 1
            }
            if (pad.DPad.Right == ButtonState.Pressed && oldPad.DPad.Right != ButtonState.Pressed)
            {
                numPlayers = Math.Min(4, numPlayers + 1); // Maximum 4
            }



            if (cursor.location.Y == level1.location.Y - 120)
                cursor.location.Y = level3.location.Y;
            if (cursor.location.Y == level3.location.Y + 120)
                cursor.location.Y = level1.location.Y;

            bool l1 = level1.pressed;
            bool l2 = level2.pressed;
            bool l3 = level3.pressed;

            level1.pressed = false;
            level2.pressed = false;
            level3.pressed = false;

            if (l1)
            {
                
                stateManager.changeState(GameState.Level1);
            }
            if (l2)
            {
                stateManager.changeState(GameState.Level2);
            }
            if (l3)
            {
                stateManager.changeState(GameState.Level3);
            }

            if (pad.Buttons.B == ButtonState.Pressed && !(oldPad.Buttons.B == ButtonState.Pressed))
                stateManager.changeState(GameState.MainMenu);


            oldPad = pad;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            level1.Draw(spriteBatch);
            level2.Draw(spriteBatch);
            level3.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
            backButton.Draw(spriteBatch);
            spriteBatch.DrawString(font, "Players: " + numPlayers, new Vector2(level1.location.X - 20, level3.location.Y + 100), Color.Black);
            spriteBatch.Draw(currentLevel, currentLlocation, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.8f);
        }
    }
}
