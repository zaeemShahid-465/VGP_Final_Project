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
    public class Gun
    {
        
        
        // Gun Infro
        public Rectangle rect;
        private Texture2D texture;

        
        public List<Bullet> bullets;

        // Shooting
        private double angle;
        private int bulletTimer;
        private int ammo;
        private int capacity;
        private int reloadTimer;
        public Texture2D basic;

        // Info about guns parent player
        private Vector2 playerVel;
        private Vector2 playerPos;
        PlayerIndex pIndex;




        public Gun(Vector2 PlayerPos, Texture2D Texture, Texture2D basic, int ammo, PlayerIndex index)
        {
            playerPos = PlayerPos;
            rect = new Rectangle((int)playerPos.X, (int)playerPos.Y, 15, 10);
            texture = Texture;
            angle = 0;
            bullets = new List<Bullet>();
            bulletTimer = 0;
            playerVel = new Vector2();
            this.ammo = ammo;
            this.capacity = ammo;
            reloadTimer = 0;
            pIndex = index;
            this.basic = basic;
        }

        public void Update(Vector2 playerVel, double angle)
        {
            this.playerVel = playerVel;
            bulletTimer++;
            GamePadState pad1 = GamePad.GetState(pIndex);
            this.angle = angle;

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();
            }
            //Console.WriteLine(bullets.Count());
            Shoot();
            Reload();
        }

        public void Shoot()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.IsButtonDown(Buttons.RightTrigger) && bulletTimer > 5 && ammo > 0)
            {
                bulletTimer = 0;
                bullets.Add(new Bullet(new Vector2((float)Math.Cos(angle) * 50 + playerVel.X/3, (float)Math.Sin(angle) * 20 + playerVel.Y/3), new Rectangle(rect.X, rect.Y, 5, 5), basic, pIndex));
                ammo--;
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(basic, rect, new Rectangle(0, 0, 20, 20), Color.Black, (float)angle, new Vector2(0, 10), SpriteEffects.None, 0f);
            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Draw(spriteBatch);
            }
        }

        public void Reload()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.IsButtonDown(Buttons.B) || reloadTimer != 0)
            {
                reloadTimer++;
                // has enough time to reloed passed
                if (reloadTimer >= 180)
                {
                    ammo = capacity;
                    reloadTimer = 0;
                }
            }
        }
    }
}
