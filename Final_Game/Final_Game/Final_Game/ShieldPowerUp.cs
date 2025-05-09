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
    public class ShieldPowerUp : PowerUp
    {
        public ShieldPowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window) : base(x, y, normalT, usedT, window)
        { }

        public override void Update(int timer, Player[] playerArr, Tile[,] map)
        {
            base.Update(timer, playerArr, map);
            if (pickedUp)
            {
                if (!used)
                {
                    rectangle.X += 4;
                    rectangle.Y += 6;
                    rectangle.Width = 20;
                    rectangle.Height = 20;
                }
                else
                {
                    rectangle.Width = 35;
                    rectangle.Height = 35;
                    if (useTimer != 300)
                    {
                        rectangle.Y -= 9;
                    }
                    if (base.useTimer < 300 && base.useTimer % 6 == 0 && used)
                    {
                        currPlayer.healShield();
                    }
                }
            }
        }
    }
}
