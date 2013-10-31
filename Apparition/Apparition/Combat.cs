﻿#region Using Statements
using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DesktopBattle
{
    /// <summary>
    /// Handles all combat-related events ingame, such as damage checks.
    /// </summary>
    public class Combat 
    {
        #region Class Variables
        static Random rnd = new Random();
        private int timeToSpawn;
        private int nextSpawn = 0;
        public bool stopSpawn = false;
        #endregion

        /// <summary>
        /// Creates a queue of enemies that the game can cycle through.
        /// </summary>
        /// <param name="numEnemies"></param>
        public void CreateEnemyQueue(int numEnemies)
        {
            for (int i = 0; i < (int)(numEnemies / 4); i++)
            {
                Game1.qEnemies.enqueue(new Key());
                for (int j = 0; j <= 3; j++)
                {
                    Game1.qEnemies.enqueue(new Clippy());
                }
            }
            //failsafe to fill up to actual number of desired enemies
            while (Game1.qEnemies.size() < numEnemies)
            {
                Game1.qEnemies.enqueue(new Clippy());
            }
        }

        /// <summary>
        /// Runs on every frame to handle collision detection and enemy spawning.
        /// </summary>
        public void Update(GameTime gameTime) 
        {
            //Spawns new enemies into the room
            if (Game1.cArea.Rooms[Game1.cArea.currentRoom].totalEnemies - Game1.cArea.enemiesSpawnedInRoom > 0
                && Game1.qEnemies.size() > 0 && !stopSpawn && timeToSpawn >= nextSpawn)
            {
                Game1.lEnemies.Add(Game1.qEnemies.dequeue());
                Game1.cArea.enemiesSpawnedInRoom++;
                //if it's been more than 1.2 secs then spawn 2 enemies
                if (Game1.cArea.Rooms[Game1.cArea.currentRoom].totalEnemies - Game1.cArea.enemiesSpawnedInRoom > 0
                    && nextSpawn >= 1200)
                {
                    Game1.lEnemies.Add(Game1.qEnemies.dequeue());
                    Game1.cArea.enemiesSpawnedInRoom++;
                }
                timeToSpawn = nextSpawn;
                nextSpawn += 500 + rnd.Next(2300);
            }
            else
            {
                timeToSpawn += gameTime.ElapsedGameTime.Milliseconds;
            }
            //begin collision detection
            foreach (Sprite Enemy in Game1.lEnemies)
            {
                foreach (Bullet Bullet in Hero.lBullets)
                {
                    //bullet collides with enemy
                    if ((Bullet.Size).Intersects(Enemy.Size))
                    {
                        //Console.Beep();
                        Enemy.HP -= Bullet.bulletDamage;
                        Bullet.isAlive = false;
                    }
                }
                // Check collision with player, if hit then lower HP and move away
                if ((Game1.cHero.Size).Intersects(Enemy.Size))
                {
                    Game1.cHero.HP -= 10;
                    Game1.cHero.Position.X -= 50; //this is bad knockback code
                }
            }

            //removes enemies if necessary
            for (int i = Game1.lEnemies.Count() - 1; i >= 0; i--)
            {
                if (!Game1.lEnemies[i].isAlive)
                {
                    Game1.lEnemies[i].newlyCreated = true;
                    Game1.lEnemies[i].isAlive = true;
                    Game1.qEnemies.enqueue(Game1.lEnemies[i]);
                    Game1.lEnemies.Remove(Game1.lEnemies[i]);
                    Game1.cArea.killsInRoom++;
                }
            }
        }
    }
}
