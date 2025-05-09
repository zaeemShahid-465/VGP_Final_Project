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
    public abstract class PowerUp
    {
        public Rectangle rectangle, window;
        public Texture2D normalT, usedT, currT;
        public Boolean used, pickedUp, collidedWithPlatform;
        public Vector2 velocity, pos, pickedUpOffset, usingOffset;
        public int useTimer, timer;
        public Player currPlayer;
        public PlayerIndex currPlayerIndex;

        public PowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window)
        {
            rectangle = new Rectangle(x, y, 30, 30);
            this.normalT = normalT;
            this.usedT = usedT;
            this.window = window;
            currT = normalT;
            used = false;
            pickedUp = false;
            velocity = new Vector2(0, 0);
            pos = new Vector2(rectangle.X, rectangle.Y);
            useTimer = 0;
            collidedWithPlatform = false;
        }

        public virtual void Update(int timer, Player[] playerArr, Tile[,] map)
        {
            this.timer = timer;

            if (!pickedUp)
            {
                velocity.Y += config.gravity;

                pos.Y += velocity.Y;

                Collide(map);

                for (int i = 0; i < playerArr.Length; i++)
                {
                    if (playerArr[i].rect.Intersects(rectangle) && !pickedUp && !playerArr[i].hasItem && !playerArr[i].isDead())
                    {
                        PickUp();
                        currPlayer = playerArr[i];
                        currPlayer.hasItem = true;
                        switch (i)
                        {
                            case 0:
                                currPlayerIndex = PlayerIndex.One;
                                break;
                            case 1:
                                currPlayerIndex = PlayerIndex.Two;
                                break;
                        }
                        velocity = new Vector2(-4, 7);
                    }
                }
            }
            else
            {
                if (!used)
                {
                    pos.X = currPlayer.rect.X + pickedUpOffset.X;
                    pos.Y = currPlayer.rect.Y + pickedUpOffset.Y;
                    rectangle.Width = 25;
                    rectangle.Height = 25;
                    if (GamePad.GetState(currPlayerIndex).IsButtonDown(Buttons.Y))
                    {
                        Use();
                    }
                }
                else
                {
                    rectangle.Width = 27;
                    rectangle.Height = 27;
                    if (useTimer == 300)
                    {
                        pos.X += velocity.X;
                        pos.Y -= velocity.Y;
                        currPlayer.hasItem = false;
                        if (!IsOffScreen())
                        {
                            velocity.Y -= 0.4f;
                        }
                    }
                    else
                    {
                        useTimer++;
                        pos.X = currPlayer.rect.X + usingOffset.X;
                        pos.Y = currPlayer.rect.Y + usingOffset.Y;
                    }
                }
            }
            rectangle.X = (int)pos.X;
            rectangle.Y = (int)pos.Y;
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

                    if (tile.rec.Intersects(rectangle))
                    {
                        Vector2 depth = config.GetIntersectionDepth(rectangle, tile.rec);

                        if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                        {
                            // Vertical collision
                            rectangle.Y += (int)depth.Y;
                            pos.Y = rectangle.Y;
                            velocity.Y = 0;

                            // If the gun landed on top of the tile, it's grounded
                            if (depth.Y < 0)
                                collidedWithPlatform = true;
                        }
                    }
                }
            }
        }

        public Boolean IsUsed()
        {
            return used;
        }

        public void Use()
        {
            used = true;
            currT = usedT;
        }

        public void PickUp()
        {
            pickedUp = true;
        }

        public Boolean IsOffScreen()
        {
            if (rectangle.X < -1 * rectangle.Width || rectangle.X > window.Width || rectangle.Y < -1 * rectangle.Height || rectangle.Y > window.Height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currT, new Rectangle(rectangle.X, rectangle.Y - 5, rectangle.Width, rectangle.Height), Color.White);
        }
    }
}