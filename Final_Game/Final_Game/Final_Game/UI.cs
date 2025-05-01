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
    class UI
    {
        // Player Info
        Texture2D[] textures = new Texture2D[4];
        Rectangle p1R, p2R, p3R, p4R;
        int distBetween;

        List<Player> players;

        public UI(Texture2D bob, Texture2D felica)
        { 
            distBetween = 50;

            textures[0] = bob;
            textures[0] = felica;

            players = new List<Player>();
            p1R = new Rectangle(0, 0, 100, 100);
            p2R = new Rectangle(p1R.X + distBetween, 0, 100, 100);
            p3R = new Rectangle(p2R.X + distBetween, 0, 100, 100);
            p4R = new Rectangle(p3R.X + distBetween, 0, 100, 100);
        }

        public void Update(Player[] players, SpriteBatch sb)
        {
            for (int i = 0; i < players.Length; i++)
            {
                sb.Draw(textures[i], p1R, Color.White);
                
            }
        }
    }
}
