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

        public void Update(Player[] playerArr, Level level)
        {
            if (pickedUp)
            {
                // Follow the player
                this.playerVel = currPlayer.velocity;
                this.angle = currPlayer.angle;
                pos.X = currPlayer.rect.X + 50;
                pos.Y = currPlayer.rect.Y + 35;
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;
            }
            else
            {
                // Gravity
                velocity.Y += config.gravity;
                pos += velocity;
                rect.X = (int)pos.X;
                rect.Y = (int)pos.Y;

                collidedWithPlatform = false;

                // Tile collision resolution
                for (int i = 0; i < level.tiles.GetLength(0); i++)
                {
                    for (int j = 0; j < level.tiles.GetLength(1); j++)
                    {
                        Tile tile = level.tiles[i, j];
                        if (tile != null && !tile.passable && tile.rec.Intersects(rect))
                        {
                            Vector2 depth = config.GetIntersectionDepth(rect, tile.rec);

                            if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                            {
                                if (depth.Y < 0)
                                {
                                    rect.Y = tile.rec.Top - rect.Height;
                                    pos.Y = rect.Y;
                                    velocity.Y = 0;
                                    collidedWithPlatform = true;
                                }
                                else
                                {
                                    rect.Y += (int)depth.Y;
                                    pos.Y = rect.Y;
                                    velocity.Y = 0;
                                }
                            }
                            else
                            {
                                if (depth.X < 0)
                                    rect.X = tile.rec.Left - rect.Width;
                                else
                                    rect.X = tile.rec.Right;

                                pos.X = rect.X;
                                velocity.X = 0;
                            }
                        }
                    }
                }

                // Pickup logic (only if gun is on the platform)
                if (collidedWithPlatform)
                {
                    for (int i = 0; i < playerArr.Length; i++)
                    {
                        if (playerArr[i].rect.Intersects(rect))
                        {
                            pickedUp = true;
                            AssignOwner(playerArr[i]);
                            playerArr[i].pewpew = this;
                            velocity = Vector2.Zero;
                            break;
                        }
                    }
                }
            }

            bulletTimer++;
            GamePadState pad1 = GamePad.GetState(pIndex);

            for (int i = 0; i < bullets.Count(); i++)
            {
                bullets[i].Update();
            }

            Reload();
        }

        public abstract void Shoot();

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, null, Color.White, (float)angle, new Vector2(0, 10), SpriteEffects.None, 0.3f);
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