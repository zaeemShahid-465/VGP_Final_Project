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
    public class config
    {
        public static int tileSize = 30;
        public static int numTilesHorizontal = 64;
        public static int numTilesVertical = 36;
        public static int screenW = tileSize * numTilesHorizontal, screenH = tileSize * numTilesVertical;
        public static int gravity = 1;

        public enum GameState
        {
            Menu,
            level1,
            level2
        }
        public GameState currentState = GameState.Menu;

        public static Vector2 GetIntersectionDepth(Rectangle rectA, Rectangle rectB)
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


        // Drawing order
        // 0.5 - Map
        // 0.4 - Player
        // 0.3 - Weapons
    }
}
