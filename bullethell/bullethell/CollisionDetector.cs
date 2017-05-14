using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace bullethell
{
    class CollisionDetector
    {
        private List<Projectile> FriendlyProjectiles;
        private List<Projectile> EnemyProjectiles;
        private List<Projectile> healthPacks;
        private List<GameObject> enemies;
        private Player player;
        public EventHandler OnEnemyHit = delegate { };

        public CollisionDetector(Player player)
        {
            FriendlyProjectiles = new List<Projectile>(100);
            EnemyProjectiles = new List<Projectile>(1000);
            enemies = new List<GameObject>(20);
            healthPacks = new List<Projectile>();
            this.player = player;
        }

        public void AddHealthPacks(List<Projectile> packs)
        {
            foreach (var pack in packs)
            {
                healthPacks.Add(pack);
                pack.OnDestroy += RemoveHealthPack;
            }
        }

        public void AddFriendlyProjectiles(List<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                FriendlyProjectiles.Add(projectile);
                projectile.OnDestroy += RemoveFriendlyProjectile;
            }
        }

        public void AddEnemyProjectiles(List<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                EnemyProjectiles.Add(projectile);
                projectile.OnDestroy += RemoveEnemyProjectile;
            }
        }

        public void AddEnemy(GameObject enemy)
        {
            enemies.Add(enemy);
            enemy.OnDestroy += RemoveEnemy;
        }

        public void ScanForCollisions()
        {
            for (int i = 0; i < healthPacks.Count; i++)
            {
                if (healthPacks[i] != null)
                {
                    if (healthPacks[i].Intersects(player.HitBox))
                    {
                        if (player.Health < 9)
                        {
                            player.Health++;
                        }
                        healthPacks[i].Destroy();
                    }
                }
            }
            for (int i = 0; i < EnemyProjectiles.Count; i++) //enemy p
            {
                if (EnemyProjectiles[i] != null)
                {
                    if (EnemyProjectiles[i].Intersects(player.HitBox))
                    {
                        player.TakeDamage(1);
                        EnemyProjectiles[i].Destroy();
                    }
                }
                
            }
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i] != null)
                {
                    if (enemies[i].Intersects(player.HitBox))
                    {
                        player.TakeDamage(1);
                    }

                    for (int a = 0; a < FriendlyProjectiles.Count; a++) //fp
                    {
                        if (i < enemies.Count && a < FriendlyProjectiles.Count)
                        {
                            if (FriendlyProjectiles[a] != null && enemies[i] != null)
                            {
                                if (enemies[i].Intersects(FriendlyProjectiles[a].HitBox))
                                {
                                    OnEnemyHit.Invoke(enemies[i], null);
                                    enemies[i].TakeDamage(1);
                                    FriendlyProjectiles[a].Destroy();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void RemoveFriendlyProjectile(object sender, EventArgs e)
        {
            if (((Projectile)sender).Done)
                FriendlyProjectiles.Remove((Projectile)sender);
        }

        private void RemoveEnemyProjectile(object sender, EventArgs e)
        {
            if (((Projectile)sender).Done)
                EnemyProjectiles.Remove((Projectile)sender);
        }

        private void RemoveEnemy(object sender, EventArgs e)
        {
            if (((Enemy)sender).Done)
                enemies.Remove((Enemy)sender);
        }

        private void RemoveHealthPack(object sender, EventArgs e)
        {
            if (((Projectile)sender).Done)
                healthPacks.Remove((Projectile)sender);
        }
    }
}