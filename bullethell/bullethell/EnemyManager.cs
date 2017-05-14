using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace bullethell
{
    delegate void SpawnEnemyEventHandler(object sender, Enemy enemy);

    class EnemyManager
    {
        public EventHandler OnSpawnHealthPack = delegate { };
        public SpawnProjectilesEventHandler OnSpawnProjectiles;
        public SpawnEnemyEventHandler OnSpawnEnemy;
        private List<Enemy> unspawnEnemies = new List<Enemy>();
        public List<Enemy> SpawnedEnemies { get; } = new List<Enemy>(1);
        public EventHandler OnGameOver = delegate { };
        public Timer GameTimer { get; set; }

        public void AddEnemy(Enemy enemy)
        {
            unspawnEnemies.Add(enemy);
            SpawnedEnemies.Capacity++;
            enemy.OnSpawn += Spawn;
            enemy.OnSpawnProjectiles += SpawnProjectiles; //trigger enemy manager's spawn projectiles event
            enemy.OnDestroy += RemoveEnemy;
            enemy.OnSpawnHealthPack += SpawnHealthPack;
        }

        private void Spawn(object sender, EventArgs args)
        {
            if (sender != null)
            {
                Enemy enemy = (Enemy)sender;
                //The line below would randomly cause crashes...can leave enemies in list anyways
                //    unspawnEnemies.Remove(enemy);
                SpawnedEnemies.Add(enemy);
                OnSpawnEnemy.Invoke(this, enemy);
            }
        }

        public void MoveAllEnemies()
        {
            //Do not use for each loop, list may be modified during this loop
            for (int i = 0; i < SpawnedEnemies.Count; i++)
            {
                if (SpawnedEnemies[i] != null)
                    SpawnedEnemies[i].Move();
            }
        }

        private void SpawnProjectiles(object sender, FirePatternEventArgs args)
        {
            OnSpawnProjectiles.Invoke(this, args);
        }

        public void Start()
        {
            foreach (Enemy enemy in unspawnEnemies)
            {
                enemy.StartTimer();
            }
            GameTimer.Elapsed += HandleGameTimeEnd;
            GameTimer.Start();
        }

        private void RemoveEnemy(object sender, EventArgs args)
        {
            Enemy enemy = (Enemy) sender;
            if (enemy.Done)
            {
                if (unspawnEnemies.Contains(enemy))
                    unspawnEnemies.Remove(enemy);
                if (SpawnedEnemies.Contains(enemy))
                    SpawnedEnemies.Remove(enemy);
            }
        }

        private void HandleGameTimeEnd(object sender, ElapsedEventArgs args)
        {
            GameTimer.Stop();
            OnGameOver.Invoke(this, null);
        }

        private void SpawnHealthPack(object sender, EventArgs args)
        {
            OnSpawnHealthPack.Invoke(sender, null);
        }
    }
}