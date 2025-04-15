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
    public class HealthPowerUp : Powerup
    {
        public HealthPowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window) : base(x, y, normalT, usedT, window)
        { }

        public void Update(int timer, Player player, GamePadState pad)
        {
            base.Update(timer, player, pad);
            if (base.useTimer == 120)
            {
                player.heal();
            }
        }
    }
}