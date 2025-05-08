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
        public ContentManager content;

        public Tile[,] tiles;
        int[,] intTiles;
        string levelID, platformSelector;
        int timer;

        public List<Gun> weapons;
        public List<PowerUp> powerUps;
        public int weaponSpawnTimer, powerUpSpawnTimer;
        int weaponDespawnTimer;
        List<int> weaponDespawnTimers, powerUpDespawnTimers;

        public Player[] playerArr;

        public List<int> possibleWeaponSpawnCords;

        public Level(IServiceProvider _content, string levelID, string platformSelector, Player[] players)
        {
            content = new ContentManager(_content, "Content");
            intTiles = new int[config.numTilesVertical, config.numTilesHorizontal];
            LoadMap(levelID, platformSelector);
            weapons = new List<Gun>();
            powerUps = new List<PowerUp>();
            rand = new Random();
            playerArr = players;
            weaponDespawnTimers = new List<int>();
            powerUpDespawnTimers = new List<int>();
/*            possibleWeaponSpawnCords = spawnCords;*/
        }

        public void Update()
        {
            weaponSpawnTimer++;
            powerUpSpawnTimer++;
            timer++;

            //Updating each power up
            foreach (PowerUp powerUp in powerUps)
                powerUp.Update(timer, playerArr, tiles);

            // Updating each weapon
            foreach (Gun weapon in weapons)
                weapon.Update(playerArr, tiles);

            //Updating each player
            for (int i = 0; i < playerArr.Length; i++)
                playerArr[i].Update(playerArr[(i + 1) % 2], this);


            // Incrementing weapon despawn timers
            for (int i = 0; i < weaponDespawnTimers.Count(); i++)
            {
                if (!weapons[i].pickedUp)
                    weaponDespawnTimers[i]++;
            }

            // Allowing all players to shoot
            foreach (Player p in playerArr)
            {
                if (p.pewpew != null)
                    p.pewpew.Shoot();
            }

            // Spawning weapons after a certain amount of time
            if (weaponSpawnTimer % 200 == 0)
            {
                Texture2D bullet = this.content.Load<Texture2D>("Gun Textures/bullet");
                int num = rand.Next(0, 2);
                switch (num)
                {
                    case 0:
                        weapons.Add(
                            new Rifle(
                                this.content.Load<Texture2D>("Gun Textures/AssaultRifle"), 
                                bullet, 
                                20, new Rectangle(rand.Next(0, config.screenW), 20, 32, 16)));
                        break;
                    case 1:
                        weapons.Add(
                            new Revolver(
                                this.content.Load<Texture2D>("Gun Textures/Revolver"),
                                bullet,
                                5,
                                new Rectangle(rand.Next(0, config.screenW), 20, 32, 16)));
                        break;
                                
                }
                
                weaponDespawnTimers.Add(0);
            }

            if (powerUpSpawnTimer % 300 == 0)
            {
                int num = rand.Next(2);
                switch (num)
                {
                    case 0:
                        powerUps.Add(
                            new HealthPowerUp(
                                rand.Next(0, config.screenW),
                                20,
                                this.content.Load<Texture2D>("Item Textures/MedKit"),
                                this.content.Load<Texture2D>("Item Textures/UsingMedKit"),
                                new Rectangle(0, 0, config.screenW, config.screenH)));
                        break;
                    case 1:
                        powerUps.Add(
                            new ShieldPowerUp(
                                rand.Next(0, config.screenW),
                                20,
                                this.content.Load<Texture2D>("Item Textures/ShieldPotion"),
                                this.content.Load<Texture2D>("Item Textures/UsingShieldPotion"),
                                new Rectangle(0, 0, config.screenW, config.screenH)));
                        break;
                }
            }

            // Despawning weapons after a certain amount of time
            for (int i = weaponDespawnTimers.Count() - 1; i >= 0; i--)
            {
                if (weaponDespawnTimers[i] > 300 && !weapons[i].pickedUp)
                {
                    weapons.RemoveAt(i);
                    weaponDespawnTimers.RemoveAt(i);
                }
            }

            // Despawning powerups after a certain amount of time
            for (int i = powerUpDespawnTimers.Count() - 1; i >= 0; i--)
            {
                if (powerUpDespawnTimers[i] > 300 && !powerUps[i].pickedUp)
                {
                    powerUps.RemoveAt(i);
                    powerUpDespawnTimers.RemoveAt(i);
                }
            }

            CheckBulletCollisions();
        }

        public void CheckBulletCollisions()
        {
            foreach (Player target in playerArr)
            {
                foreach (Player shooter in playerArr)
                {
                    if (shooter.pewpew == null) continue;

                    for (int i = shooter.pewpew.bullets.Count - 1; i >= 0; i--)
                    {
                        Bullet b = shooter.pewpew.bullets[i];

                        if (b.rect.Intersects(target.rect) && target.pIndex != b.pIndex)
                        {
                            // Apply damage
                            target.takeDamage();

                            // Remove bullet
                            shooter.pewpew.bullets.RemoveAt(i);
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in tiles)
            {
                if (tile != null)
                    spriteBatch.Draw(tile.tex, tile.rec, null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
            }

            foreach (Gun weapon in weapons)
            {
                weapon.Draw(spriteBatch);
            }

            foreach (PowerUp powerUp in powerUps)
            {
                powerUp.Draw(spriteBatch);
            }

            foreach (Player player in playerArr)
            {
                player.Draw(spriteBatch);
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
