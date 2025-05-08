using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Final_Game
{
    public abstract class Gun
    {
        // Gun Info
        public Rectangle rect;
        private Texture2D texture;
        public bool pickedUp;
        public List<Bullet> bullets;
        public Vector2 pos;
        public bool collidedWithPlatform;
        Vector2 velocity;

        // Shooting
        public double angle;
        public int bulletTimer;
        public int ammo;
        private int capacity;
        private int reloadTimer;
        public Texture2D basic;

        // Info about gun's parent player
        public Vector2 playerVel;
        private Vector2 playerPos;
        public PlayerIndex pIndex;
        public Player currPlayer;

        public Gun(Texture2D Texture, Texture2D basic, int ammo, Rectangle rec)
        {
            velocity = new Vector2(0, 0);
            this.rect = rec;
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
            collidedWithPlatform = false;
        }

        public void AssignOwner(Player p)
        {
            this.pIndex = p.pIndex;
            this.playerPos = new Vector2(p.rect.X, p.rect.Y);
            currPlayer = p;
            this.playerVel = p.velocity;
            this.angle = p.angle;
        }

        public void Update(Player[] playerArr, Tile[,] map)
        {
            if (!pickedUp)
            {
                // Apply gravity
                if (!collidedWithPlatform)
                    velocity.Y += config.gravity;

                // Move vertically
                pos.Y += velocity.Y;
                rect.Y = (int)pos.Y;

                // Handle platform collisions
                Collide(map);

                // Check for pickup by any player
                foreach (Player p in playerArr)
                {
                    if (p.rect.Intersects(rect) && p.pewpew == null)
                    {
                        pickedUp = true;
                        AssignOwner(p);
                        p.pewpew = this;
                        break;
                    }
                }
            }
            else
            {
                // Follow the owning player
                this.playerVel = currPlayer.velocity;
                this.angle = currPlayer.angle;
                pos.X = currPlayer.rect.X + 50;
                pos.Y = currPlayer.rect.Y + 55;
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;
            }

            // Update bullets
            bulletTimer++;
            foreach (var bullet in bullets)
                bullet.Update();

            Reload();
        }

        public void Collide(Tile[,] map)
        {
            collidedWithPlatform = false;

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Tile tile = map[i, j];
                    if (tile == null || tile.passable) continue;

                    if (tile.rec.Intersects(rect))
                    {
                        Vector2 depth = config.GetIntersectionDepth(rect, tile.rec);

                        if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                        {
                            // Vertical collision
                            rect.Y += (int)depth.Y;
                            pos.Y = rect.Y;
                            velocity.Y = 0;

                            // If the gun landed on top of the tile, it's grounded
                            if (depth.Y < 0)
                                collidedWithPlatform = true;
                        }
                    }
                }
            }
        }


        public abstract void Shoot();

        public void Draw(SpriteBatch spriteBatch)
        {
            if (angle < -1.5)
                spriteBatch.Draw(texture, rect, null, Color.White, (float)angle, new Vector2(0, 10), SpriteEffects.FlipVertically, 0.2f);
            else
                spriteBatch.Draw(texture, rect, null, Color.White, (float)angle, new Vector2(0, 10), SpriteEffects.None, 0.2f);
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
                if (reloadTimer >= 180)
                {
                    ammo = capacity;
                    reloadTimer = 0;
                }
            }
        }
    }
}