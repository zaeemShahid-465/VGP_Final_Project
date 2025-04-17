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
    public abstract class Gun
    {
        
        
        // Gun Infro
        public Rectangle rect;
        private Texture2D texture;
        public bool pickedUp;
        public List<Bullet> bullets;
        public Vector2 pos;

        // Shooting
        public double angle;
        public int bulletTimer;
        public int ammo;
        private int capacity;
        private int reloadTimer;
        public Texture2D basic;

        // Info about guns parent player
        public Vector2 playerVel;
        private Vector2 playerPos;
        public PlayerIndex pIndex;
        public Player currPlayer;




        public Gun(Texture2D Texture, Texture2D basic, int ammo, Rectangle rec)
        {
            this.rect = rec;
            /*rect = new Rectangle((int)playerPos.X, (int)playerPos.Y, 15, 10);*/
            pos = new Vector2(rect.X, rect.Y);
            texture = Texture;
            angle = 0;
            bullets = new List<Bullet>();
            bulletTimer = 0;
            playerVel = new Vector2();
            this.ammo = ammo;
            this.capacity = ammo;
            reloadTimer = 0;
            this.basic = basic;
            pickedUp = false;
        }

        public void AssignOwner(Player p)
        {
            this.pIndex = p.pIndex;
            this.playerPos = new Vector2(p.rect.X, p.rect.Y);
            currPlayer = p;
            this.playerVel = p.velocity;
            this.angle = p.angle;
        }

        public void Update(int bobbingTimer, Player[] playerArr)
        {
            Console.WriteLine("Gun Picked up? " + pickedUp);
            // Moving item up and down if not picked up
            if (!pickedUp)
            {
                if (bobbingTimer % 90 < 45)
                    pos.Y -= 0.5f;
                else
                    pos.Y += 0.5f;

                // Assigning gun owner 
                for (int i = 0; i < playerArr.Length; i++)
                {
                    if (playerArr[i].rect.Intersects(rect) && !pickedUp)
                    {
                        pickedUp = true;
                        AssignOwner(playerArr[i]);
                        playerArr[i].pewpew = this;
                    }
                }
            }
            else
            {
                this.playerVel = currPlayer.velocity;
                this.angle = currPlayer.angle;
                pos.X = currPlayer.rect.X + 50;
                pos.Y = currPlayer.rect.Y + 35;
            }

            rect.X = (int)pos.X;
            rect.Y = (int)pos.Y;

            
            bulletTimer++;
            GamePadState pad1 = GamePad.GetState(pIndex);
            

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();
            }
            //Console.WriteLine(bullets.Count());
            Reload();
        }

        public abstract void Shoot();
        
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
