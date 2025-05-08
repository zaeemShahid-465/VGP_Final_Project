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
    class Button
    {
        public Texture2D tex;
        public Rectangle location;
        public Color c;
        float layer;

        public bool pressed;

        public Button(Texture2D tex, Rectangle rec, Color c, float layer)
        {
            this.tex = tex;
            this.location = rec;
            this.c = c;
            this.layer = layer;

            pressed = false;
        }

        public void Update(GamePadState pad, GamePadState oldPad, Button cursor)
        {
            if (
                pad.Buttons.A == ButtonState.Pressed && 
                !(oldPad.Buttons.A == ButtonState.Pressed) && 
                cursor.location.Intersects(location))
                pressed = true;
            else
                pressed = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(tex, location, null, c, 0f, Vector2.Zero, SpriteEffects.None, layer);
        }
    }
}
