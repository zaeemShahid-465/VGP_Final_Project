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
    class Menu
    {
        ContentManager content;

        Button play, quit, help, cursor;
        int screenW = config.screenW, screenH = config.screenH;
        GamePadState oldPad;

        GameStateManager stateManager;

        Texture2D playButtonTex;
        Texture2D helpButtonTex;
        Texture2D quitButtonTex;

        Texture2D glitchedPlayTex;
        Texture2D glitchedHelpTex;
        Texture2D glitchedQuitTex;


        public Menu(IServiceProvider _content, GameStateManager stateManager)
        {
            content = new ContentManager(_content, "Content");

            playButtonTex = content.Load<Texture2D>("PlayButtonTex");
            helpButtonTex = content.Load<Texture2D>("ControlsButton");
            quitButtonTex = content.Load<Texture2D>("Gun Textures/basic");

            glitchedPlayTex = content.Load<Texture2D>("PlayGlitched");
            glitchedHelpTex = content.Load<Texture2D>("ControlsGlitched");
            glitchedQuitTex = content.Load<Texture2D>("QuitGlitched");


            this.stateManager = stateManager;

            oldPad = GamePad.GetState(PlayerIndex.One);

            play = new Button(
                content.Load<Texture2D>("PlayButtonTex"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40, 200, 80), Color.White, 0.8f);
            quit = new Button(
                content.Load<Texture2D>("QuitButtonTEx"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40 + 240, 200, 80), Color.White, 0.7f);
            help = new Button(
                content.Load<Texture2D>("ControlsButton"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40 + 120, 200, 80), Color.White, 0.6f);
            cursor = new Button(
                content.Load<Texture2D>("PlayGlitched"),
                new Rectangle(screenW / 2 - 100, screenH / 2 - 40, 200, 80), Color.White, 0f);
        }

        public void Update(GameState gameState)
        {
            


            if (cursor.location.Y == play.location.Y)
                cursor.tex = glitchedPlayTex;
            if (cursor.location.Y == help.location.Y)
                cursor.tex = glitchedHelpTex;
            if (cursor.location.Y == quit.location.Y)
                cursor.tex = glitchedQuitTex;

            GamePadState pad = GamePad.GetState(PlayerIndex.One);

            play.Update(pad, oldPad, cursor);
            help.Update(pad, oldPad, cursor);
            quit.Update(pad, oldPad, cursor);

            if (pad.DPad.Down == ButtonState.Pressed && !(oldPad.DPad.Down == ButtonState.Pressed))
                cursor.location.Y += 120;
            if (pad.DPad.Up == ButtonState.Pressed && !(oldPad.DPad.Up == ButtonState.Pressed))
                cursor.location.Y -= 120;

            if (cursor.location.Y == play.location.Y - 120)
                cursor.location.Y = quit.location.Y;
            if (cursor.location.Y == quit.location.Y + 120)
                cursor.location.Y = play.location.Y;

            bool playWasPressed = play.pressed;
            bool helpWasPressed = help.pressed;
            bool quitWasPressed = quit.pressed;

            // Reset pressed states before using them
            play.pressed = false;
            help.pressed = false;
            quit.pressed = false;

            if (playWasPressed)
            {
                stateManager.changeState(GameState.LevelSelector);
            }
            if (helpWasPressed)
            {
                stateManager.changeState(GameState.HelpScreen);
            }
            if (quitWasPressed)
            {
                stateManager.changeState(GameState.Quit);
            }


            oldPad = pad;
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            play.Draw(spriteBatch);
            help.Draw(spriteBatch);
            quit.Draw(spriteBatch);
            cursor.Draw(spriteBatch);
        }
    }
}
