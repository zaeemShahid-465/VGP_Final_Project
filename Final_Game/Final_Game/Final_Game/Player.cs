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
    public class Player
    {
        // Basic Info
        private Texture2D texture;
        public Rectangle rect;
        private float screenHeight;

        //Constants
        private float gravity;
        private int playerIndex;
        public PlayerIndex pIndex;

        // Moveemnt
        public bool grounded;
        private int dashTimer;
        private int dashTime;
        private int speed;
        private int jumpCount;
        private int jumpTime;
        private bool dashing;
        private int speedCap;
        Vector2 velocity;
        private bool justLanded;
        private int jumpStrength = 20;
        public bool isOnGround = true;

        // Guns
        private double angle;
        private List<Bullet> evilBullets;
        private Gun pewpew;

        // Health
        private int health;
        private Rectangle redHealthBar;
        private Rectangle greenHealthBar;

        int gameTimer;



        public Player(Texture2D texture, Vector2 pos, int playerIndex, float screenHeight, int index)
        {
            this.justLanded = false;
            this.rect = new Rectangle((int)pos.X, (int)pos.Y, 30, 30);
            this.texture = texture;
            this.playerIndex = playerIndex;
            gravity = 1;
            grounded = false;
            this.screenHeight = screenHeight;
            velocity = new Vector2(0, 0);
            speed = 5;
            jumpCount = 0;
            jumpTime = 0;
            health = 100;
            redHealthBar = new Rectangle(this.rect.X, this.rect.Y + 90, 50, 10);
            greenHealthBar = new Rectangle(this.rect.X, this.rect.Y + 90, 50, 10);
            evilBullets = new List<Bullet>();
            angle = 0;
            dashTimer = 180;
            speedCap = 10;
            dashTime = 0;
            
            // Sets Controller Index
            if (index == 1)
            {
                pIndex = PlayerIndex.One;
            }
            if (index == 2)
            {
                pIndex = PlayerIndex.Two;
            }
            if (index == 3)
            {
                pIndex = PlayerIndex.Three;
            }
            if (index == 4)
            {
                pIndex = PlayerIndex.Four;
            }

            pewpew = new Gun(pos, texture, 20, pIndex);
        }

        public void HandleCollisions(Level level)
        {
            isOnGround = false;
            Rectangle newBounds = rect;

            for (int i = 0; i < level.tiles.GetLength(0); i++)
            {
                for (int j = 0; j < level.tiles.GetLength(1); j++)
                {
                    Tile tile = level.tiles[i, j];
                    if (tile != null && !tile.passable && tile.rec.Intersects(newBounds))
                    {
                        Vector2 depth = GetIntersectionDepth(newBounds, tile.rec);

                        if (Math.Abs(depth.Y) < Math.Abs(depth.X))
                        {
                            if (depth.Y < 0)
                            {
                                rect.Y = tile.rec.Top - rect.Height;
                                isOnGround = true;
                                velocity.Y = 0;
                                jumpCount = 0;
                            }
                            else
                            {
                                rect.Y += (int)depth.Y;
                                velocity.Y = 0;
                            }
                        }
                        else
                        {
                            rect.X += (int)depth.X;
                            velocity.X = 0;
                        }

                        newBounds = rect;
                    }
                }
            }
        }

        private Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
        {
            Vector2 centerA = new Vector2(rectA.Center.X, rectA.Center.Y);
            Vector2 centerB = new Vector2(rectB.Center.X, rectB.Center.Y);
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = (rectA.Width + rectB.Width) / 2f;
            float minDistanceY = (rectA.Height + rectB.Height) / 2f;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;

            float depthX;
            if (distanceX > 0)
                depthX = minDistanceX - distanceX;
            else
                depthX = -minDistanceX - distanceX;

            float depthY;
            if (distanceY > 0)
                depthY = minDistanceY - distanceY;
            else
                depthY = -minDistanceY - distanceY;


            return new Vector2(depthX, depthY);
        }
        
        
        // Gets a list of all bullets you have fired.
        public List<Bullet> Bullets()
        {
            return pewpew.bullets;
        }

        // Takes in Bullets() of other players to check if you have taken damage
        public void getEnemyBullets(Player x)
        {
            List<Bullet> temp = x.Bullets();
            if (temp.Count != 0)
            {
                for (int i = temp.Count - 1; i > 0; i--)
                {
                    if (temp[i].rect.Intersects(this.rect))
                    {
                        x.pewpew.bullets.Remove(x.pewpew.bullets[i]);
                        takeDamage();
                        
                    }
                 
                }
            }
        }

        //Update method where other methods are called.
        public void Update(Player x, Level l)
        {
            gameTimer++;

            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.ThumbSticks.Right.Length() != 0)
            {
                angle = Math.Atan2(pad1.ThumbSticks.Right.X, pad1.ThumbSticks.Right.Y) - Math.PI / 2;
            }

            getEnemyBullets(x);
            horizontalMove();
            Jump();

            this.rect.X += (int)velocity.X;
            this.rect.Y += (int)velocity.Y;

            HandleCollisions(l);

            Gravity();

            pewpew.Update(velocity, angle);

            if (pad1.IsButtonDown(Buttons.X) && dashTimer >= 180 && !isOnGround)
            {
                dash();
            }
            if (dashTime >= 30)
            {
                dashing = false;
            }

            capSpeed();

            //Console.WriteLine(jumpTime);

            pewpew.rect.X = this.rect.X + 25;
            pewpew.rect.Y = this.rect.Y+ 25;

            redHealthBar.X = this.rect.X - 10;
            redHealthBar.Y = this.rect.Y - 20;
            greenHealthBar.X = this.rect.X - 10;
            greenHealthBar.Y = this.rect.Y - 20;

            jumpTime++;
            dashTimer++;
            dashTime++;

        }

        // Dash method
        public void dash()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);
            this.velocity.X += (int)pad1.ThumbSticks.Left.X * 300;
            dashTimer = 0;
            dashTime = 0;
            dashing = true;
        }

        // Take Damage
        public void takeDamage()
        {
            greenHealthBar.Width -= 10;
            health -= 10;
        }
        // Horizontal Movment
        public void horizontalMove()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);

            velocity.X += pad1.ThumbSticks.Left.X * speed;
            // If not dashing and not moving stick set speed to zero
            if (pad1.ThumbSticks.Left.Length() == 0 && !dashing)
            {
                velocity.X = 0;
            }
            //Console.WriteLine(pad1.ThumbSticks.Left.X);
        }

        // Cap Speed
        public void capSpeed()
        {
            // If not dashing set speed default speed cap
            if (Math.Abs(velocity.X) >= speedCap && !dashing)
            {
                if (velocity.X < 0)
                    velocity.X = -speedCap;
                if (velocity.X > 0)
                    velocity.X = speedCap;
            }

            // if dashing set dashing speed cap
            else if (Math.Abs(velocity.X) >= speedCap && dashing)
            {
                if (velocity.X < 0)
                    velocity.X = -speedCap - 15;
                if (velocity.X > 0)
                    velocity.X = speedCap + 15;
            }
            
        }

        // Jumping
        public void Jump()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.IsButtonDown(Buttons.LeftTrigger) && jumpCount < 1 && isOnGround)
            {
                velocity.Y -= jumpStrength;
                justLanded = false;
                jumpCount++;
                isOnGround = false;
            }    
                /*if (jumpCount >= 2)
                {
                    grounded = false;
                }*/
                   
        }
        // Apply Gravity
        public void Gravity()
        {
            // To be reworked to work with tiles
            if (!isOnGround)
                velocity.Y += (int)gravity;

            if (this.rect.Y >= this.screenHeight - rect.Height)
            {
                velocity.Y = 0;
                jumpCount = 0;
                isOnGround = true;
                rect.Y = (int)screenHeight - rect.Height;
            }
        }

        public void heal()
        {
            greenHealthBar.Width += 50;
            health += 50;
            if (greenHealthBar.Width > 100)
            {
                greenHealthBar.Width = 100;
            }
            if (health > 100)
            {
                health = 100;
            }
        }

        // Draw everything the player has
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
            spriteBatch.Draw(texture, redHealthBar, Color.Red);
            spriteBatch.Draw(texture, greenHealthBar, Color.Green);
            pewpew.Draw(spriteBatch);
        }
    }
}
