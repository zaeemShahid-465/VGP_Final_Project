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
        public Texture2D texture;
        public Texture2D left;
        public Texture2D right;
        public Rectangle rect;
        public Rectangle source_rect;

        private float screenHeight;
        enum PlayerDir
        {
            idle_right,
            idle_left,
            walk_right,
            walk_left,
        }
        PlayerDir playerDir;


        //Constants
        private int playerIndex;
        public PlayerIndex pIndex;

        // Movement
        public bool grounded;
        private int dashTimer;
        private int dashTime;
        private int speed;
        private int jumpCount;
        private int jumpTime;
        private bool dashing;
        private int speedCap;
        public Vector2 velocity;
        private bool justLanded;
        private int jumpStrength = 25;
        public bool isOnGround = true;
        private int animationTimer;

        // Guns
        public double angle;
        private List<Bullet> evilBullets;
        private Texture2D basic;

        // Health
        public int health;
        public int lives;
        private Rectangle redHealthBar;
        private Rectangle greenHealthBar;
        Texture2D redHealthTex, greenHealthTex;

        //Shield
        public int shield;
        private Rectangle blueShieldBar;
        private Rectangle redShieldBar;

        int gameTimer;

        int randTimer;

        public Gun pewpew;

        //Items
        public Boolean hasItem;



        public Player(List<Texture2D> textures, Texture2D basic, Texture2D green, Texture2D red, Vector2 pos, int playerIndex, float screenHeight, int index)
        {

            this.justLanded = false;
            this.rect = new Rectangle((int)pos.X, (int)pos.Y, 24 * 3, 18 * 5);
            this.playerIndex = playerIndex;
            grounded = false;
            this.screenHeight = screenHeight;
            velocity = new Vector2(0, 0);
            speed = 4;
            jumpCount = 0;
            jumpTime = 0;
            health = 50;
            shield = 50;
            redHealthBar = new Rectangle(this.rect.X, this.rect.Y + 90, 50, 10);
            greenHealthBar = new Rectangle(this.rect.X, this.rect.Y + 90, 25, 10);
            blueShieldBar = new Rectangle(this.rect.X, this.rect.Y + 75, 25, 10);
            redShieldBar = new Rectangle(this.rect.X, this.rect.Y + 75, 50, 10);
            evilBullets = new List<Bullet>();
            angle = 0;
            dashTimer = 180;
            speedCap = 10;
            dashTime = 0;
            hasItem = false;
            this.basic = basic;
            redHealthTex = red;
            greenHealthTex = green;

            animationTimer = 0;
            lives = 3;

            this.source_rect = new Rectangle(0, 6, 24, 24);


            // Sets Controller Index
            if (index == 1)
            {
                pIndex = PlayerIndex.One;
                this.right = textures[0];
                this.texture = right;
                this.playerDir = PlayerDir.idle_right;
            }
            if (index == 2)
            {
                pIndex = PlayerIndex.Two;
                this.texture = textures[1];
            }
            if (index == 3)
            {
                pIndex = PlayerIndex.Three;
                this.texture = textures[2];
            }
            if (index == 4)
            {
                pIndex = PlayerIndex.Four;
                this.texture = textures[3];
            }

        }

        public void decideTexture()
        {
            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.ThumbSticks.Left.Length() != 0)
            {

                if (pad1.ThumbSticks.Left.X > 0)
                {
                    playerDir = PlayerDir.walk_right;
                }
                else if (pad1.ThumbSticks.Left.X < 0)
                {
                    playerDir = PlayerDir.walk_left;
                }

            }

            else
            {

                if (playerDir == PlayerDir.walk_right)
                {
                    playerDir = PlayerDir.idle_right;
                }
                else if (playerDir == PlayerDir.walk_left)
                {
                    playerDir = PlayerDir.idle_left;
                }
            }

            //Console.WriteLine(playerDir);
        }

        public void ChangeTexture()
        {
            if (animationTimer >= 10)
            {
                if (playerDir == PlayerDir.walk_right || playerDir == PlayerDir.walk_left)
                {
                    source_rect.Y = 32;
                    if (source_rect.X + 24 < 96)
                        source_rect.X += 24;
                    else
                        source_rect.X = 0;

                    animationTimer = 0;
                }

                if (playerDir == PlayerDir.idle_right || playerDir == PlayerDir.idle_left)
                {
                    source_rect.Y = 0;
                    if (source_rect.X + 24 < 24 * 5)
                        source_rect.X += 24;
                    else
                        source_rect.X = 0;

                    animationTimer = 0;
                }

                if (!isOnGround && velocity.Y < 0)
                {
                    source_rect.Y = 62;
                    source_rect.X = 0;
                }
                else if (!isOnGround && velocity.Y > 0)
                {
                    source_rect.Y = 92;
                    source_rect.X = 0;
                }

            }
            animationTimer++;
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
                        Vector2 depth = config.GetIntersectionDepth(newBounds, tile.rec);

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

        
/*        // Gets a list of all bullets you have fired.
        public List<Bullet> Bullets()
        {

        }*/

        // Takes in Bullets() of other players to check if you have taken damage
/*        public void getEnemyBullets(Player x)
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
https://www.desmos.com/calculator
                }
            }
        }*/

        //Update method where other methods are called.

        public void Die()
        {
            lives--;
            if (lives == 0)
            {

            }
        }
        public void Update(Player x, Level l)
        {
            gameTimer++;
            randTimer++;

            GamePadState pad1 = GamePad.GetState(pIndex);
            if (pad1.ThumbSticks.Right.Length() != 0)
            {
                angle = Math.Atan2(pad1.ThumbSticks.Right.X, pad1.ThumbSticks.Right.Y) - Math.PI / 2;
            }

/*            getEnemyBullets(x);*/
            horizontalMove();
            Jump();

            this.rect.X += (int)velocity.X;
            this.rect.Y += (int)velocity.Y;

            HandleCollisions(l);
            decideTexture();
            ChangeTexture();

            Gravity();

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

            redHealthBar.X = this.rect.X + 10;
            redHealthBar.Y = this.rect.Y - 20;
            greenHealthBar.X = redHealthBar.X;
            greenHealthBar.Y = redHealthBar.Y;

            redShieldBar.X = this.rect.X + 10;
            redShieldBar.Y = this.rect.Y - 35;
            blueShieldBar.X = this.rect.X + 10;
            blueShieldBar.Y = this.rect.Y - 35;

            jumpTime++;
            dashTimer++;
            dashTime++;

            KeyboardState keyState = Keyboard.GetState();


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
            if (health < 0)
                health = 0;
            if (greenHealthBar.Width < 0)
                greenHealthBar.Width = 0;
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
            // If not dashing set speed default speed cap
            if (Math.Abs(velocity.Y) >= speedCap + 20 && !dashing)
            {
                if (velocity.Y < 0)
                    velocity.Y = -speedCap + 20;
                if (velocity.Y > 0)
                    velocity.Y = speedCap + 20;
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
            if (!isOnGround)
                velocity.Y += (int)config.gravity;

            if (this.rect.Y >= this.screenHeight - rect.Height)
            {
                velocity.Y = 0;
                jumpCount = 0;
                isOnGround = true;
                rect.Y = (int)screenHeight - rect.Height;
            }
        }

        //Gradually adds health to player until it's 100
        public void heal()
        {
            greenHealthBar.Width += 1;
            health += 2;
            if (health > 100)
            {
                health = 100;
            }
            if (greenHealthBar.Width > 50)
            {
                greenHealthBar.Width = 50;
            }

        }

        //Gradually adds shield to player until it's 100
        public void healShield()
        {
            blueShieldBar.Width += 1;
            shield += 2;
            if (shield > 100)
            {
                shield = 100;
            }
            if (blueShieldBar.Width > 50)
            {
                blueShieldBar.Width = 50;
            }
        }

        // Draw everything the player has
        public void Draw(SpriteBatch spriteBatch)
        {
            if (playerDir == PlayerDir.walk_left || playerDir == PlayerDir.idle_left)
            {
                spriteBatch.Draw(texture, rect, source_rect, Color.White, 0f, Vector2.Zero, SpriteEffects.FlipHorizontally, 0);
            }
            else
            {
                spriteBatch.Draw(texture, rect, source_rect, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0);
            }
            spriteBatch.Draw(redHealthTex, redHealthBar, Color.Red);
            spriteBatch.Draw(greenHealthTex, greenHealthBar, Color.Green);
            spriteBatch.Draw(basic, redShieldBar, Color.Red);
            spriteBatch.Draw(basic, blueShieldBar, Color.Blue);
        }
    }
}
