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
    public enum GameState
    {
        MainMenu,
        LevelSelector,
        HelpScreen,
        Level1,
        Level2,
        Level3,
        Paused,
        Quit
    }
    public class GameStateManager
    {
        public GameState currentState;

        public GameStateManager()
        {
            
        }

        public void changeState(GameState state)
        {
            currentState = state;
        }

        public Boolean Is(GameState state)
        {
            if (currentState == state)
                return true;
            return false;
        }
    }
}
