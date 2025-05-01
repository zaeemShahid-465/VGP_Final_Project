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
    public class HealthPowerUp : PowerUp
    {
        public HealthPowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window) : base(x, y, normalT, usedT, window)
        {
            this.rectangle = new Rectangle(x, y, 35, 35);
        }

        public void Update(int timer, Player[] playerArr, Tile[,] map)
        {
            base.Update(timer, playerArr, map);
            if (base.useTimer < 300 && base.useTimer % 6 == 0 && used)
            {
                currPlayer.heal();
            }
        }
    }
}