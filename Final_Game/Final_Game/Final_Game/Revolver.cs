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
    class Revolver : Gun
    {
        GamePadState oldPadState;
        public Revolver(Texture2D Texture, Texture2D basic, int ammo, Rectangle rec, Level l) : base(Texture, basic, ammo, rec, l)
        {
            oldPadState = GamePad.GetState(base.pIndex);
        }

        public override void Shoot()
        {
            GamePadState pad1 = GamePad.GetState(base.pIndex);
            if (pad1.IsButtonDown(Buttons.RightTrigger) && !oldPadState.IsButtonDown(Buttons.RightTrigger) && bulletTimer > 5 && ammo > 0)
            {
                bulletTimer = 0;
                bullets.Add(new Bullet(new Vector2((float)Math.Cos(angle) * 50 + playerVel.X / 3, (float)Math.Sin(angle) * 20 + playerVel.Y / 3), new Rectangle(rect.X, rect.Y, 5, 5), basic, pIndex));
                ammo--;
            }
            oldPadState = pad1;
        }
    }
}
