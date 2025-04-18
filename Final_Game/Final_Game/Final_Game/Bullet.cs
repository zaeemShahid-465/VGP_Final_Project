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
    public class Bullet
    {
        // Bullet info
        private Vector2 dir;
        public Rectangle rect;
        public Texture2D texture;

        // Player info
        PlayerIndex pIndex;

        public Bullet(Vector2 dir, Rectangle rect, Texture2D texture, PlayerIndex index)
        {
            this.dir = dir;
            this.rect = rect;
            this.texture = texture;

            this.dir.Normalize();
            this.dir.X *= 20;
            this.dir.Y *= 20;

            pIndex = index;
        }

        public void Update()
        {
            rect.X += (int)dir.X;
            rect.Y += (int)dir.Y;

            dir.Y += 0.2f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.Yellow);
            spriteBatch.Draw(texture, rect, null, Color.Yellow, 0f, Vector2.Zero, SpriteEffects.None, 0.3f);
        }
    }
}
