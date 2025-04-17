using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Final_Game
{
    public class Level : IDisposable
    {
        Random rand;
        ContentManager content;

        public Tile[,] tiles;
        int[,] intTiles;
        string levelID, platformSelector;

        public List<Gun> weapons;
        public int weaponSpawnTimer;
        public int weaponBobTimer;

        public Player[] playerArr;

        public Level(IServiceProvider _content, string levelID, string platformSelector, Player[] players)
        {
            content = new ContentManager(_content, "Content");
            intTiles = new int[config.numTilesVertical, config.numTilesHorizontal];
            LoadMap(levelID, platformSelector);
            weapons = new List<Gun>();
            rand = new Random();
            playerArr = players;
        }

        public void Update()
        {
            weaponSpawnTimer++;
            weaponBobTimer++;

            foreach (Gun weapon in weapons)
                weapon.Update(weaponBobTimer, playerArr);

            foreach (Player p in playerArr)
            {
                if (p.pewpew != null)
                    p.pewpew.Shoot();
            }

            if (weaponSpawnTimer % 200 == 0)
                weapons.Add(new Rifle(this.content.Load<Texture2D>("Gun Textures/basic"), this.content.Load<Texture2D>("Gun Textures/bullet"), 20, new Rectangle(rand.Next(0, 500), 500, 15, 10)));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                if (tile != null)
                    spriteBatch.Draw(tile.tex, tile.rec, Color.White);
            }

            foreach (Gun weapon in weapons)
            {
                weapon.Draw(spriteBatch);
            }
        }

        private void LoadMap(string levelID, string platformSelector)
        {
            using (StreamReader read = new StreamReader(@"Content/LevelData/" + levelID))
            {
                while (!read.EndOfStream)
                {
                    for (int i = 0; i < intTiles.GetLength(0); i++)
                    {
                        string line = read.ReadLine();
                        string[] parts = line.Split(' ');
                        for (int j = 0; j < intTiles.GetLength(1); j++)
                        {
                            if (parts[j].Equals("."))
                            {
                                intTiles[i, j] = -1;
                            }
                            else
                            {
                                intTiles[i, j] = int.Parse(parts[j]);
                            }
                        }
                    }
                }
            }

            tiles = new Tile[config.numTilesVertical, config.numTilesHorizontal];
            Vector2 pos = new Vector2();
            for (int i = 0; i < intTiles.GetLength(0); i++)
            {
                for (int j = 0; j < intTiles.GetLength(1); j++)
                {
                    switch (intTiles[i, j])
                    {
                        case 0:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/0"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), true);
                            break;
                        case 1:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/1"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 2:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/2"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 3:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/3"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 4:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/4"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 5:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/5"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 6:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/6"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 7:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/7"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 8:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/8"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 9:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/9"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        case 10:
                            tiles[i, j] = new Tile(content.Load<Texture2D>(platformSelector + "/10"), new Rectangle((int)pos.X, (int)pos.Y, config.tileSize, config.tileSize), false);
                            break;
                        default:
                            tiles[i, j] = null;
                            break;
                    }

                    pos.X += config.tileSize;
                }
                pos.X = 0;
                pos.Y += config.tileSize;
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
