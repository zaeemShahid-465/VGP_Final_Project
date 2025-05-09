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
    class BakerBlastPowerUp : PowerUp
    {
        public BakerBlastPowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window) : base(x, y, normalT, usedT, window)
        { }

        public override void Update(int timer, Player[] playerArr, Tile[,] map)
        {
            
        }
    }
}
