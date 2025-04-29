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
        public Boolean used, pickedUp;
        public Vector2 velocity, pos;
        public int useTimer;
        public Player currPlayer;
        public PlayerIndex currPlayerIndex;

        public PowerUp(int x, int y, Texture2D normalT, Texture2D usedT, Rectangle window)
        {
            this.rectangle = new Rectangle(x, y, 30, 30);
            this.normalT = normalT;
            this.usedT = usedT;
            this.window = window;
            currT = normalT;
            used = false;
            pickedUp = false;
            velocity = new Vector2(-4, 7);
            pos = new Vector2(rectangle.X, rectangle.Y);
            useTimer = 0;
        }

        public void Update(int timer, Player[] playerArr)
        {
            if (!pickedUp)
            {
                if (timer % 90 < 45)
                {
                    pos.Y -= 0.5f;
                }
                else
                {
                    pos.Y += 0.5f;
                }
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
                    }
                }
            }
            else
            {
                if (!used)
                {
                    pos.X = currPlayer.rect.X + 30;
                    pos.Y = currPlayer.rect.Y + 50;
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
                        pos.X = currPlayer.rect.X + 28;
                        pos.Y = currPlayer.rect.Y + 48;
                    }
                }
            }
            rectangle.X = (int)pos.X;
            rectangle.Y = (int)pos.Y;
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
            spriteBatch.Draw(currT, rectangle, Color.White);
        }
    }
}