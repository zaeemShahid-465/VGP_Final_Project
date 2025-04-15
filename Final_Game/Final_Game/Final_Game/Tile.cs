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
    public class Tile
    {
        public Texture2D tex;
        public Rectangle rec;
        public bool passable;

        public Tile(Texture2D tex, Rectangle rec, bool passable)
        {
            this.tex = tex;
            this.rec = rec;
            this.passable = passable;
        }
    }
}
