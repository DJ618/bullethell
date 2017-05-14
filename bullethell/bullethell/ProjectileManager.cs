using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
    class ProjectileManager
    {
        private List<Projectile> enemyProjectiles = new List<Projectile>(1000);
        private List<Projectile> friendlyProjectiles = new List<Projectile>(100);
        private List<Projectile> healthPacks = new List<Projectile>();
        public EventHandler OnProjectileExplode = delegate { };

        public List<Projectile> EnemyProjectiles
        {
            get { return enemyProjectiles; }
        }

        public List<Projectile> FriendlyProjectiles
        {
            get { return friendlyProjectiles; }
        }

        public List<Projectile> HealthPacks
        {
            get
            {
                return healthPacks;
            }
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
                friendlyProjectiles.Add(projectile);
                projectile.OnDestroy += RemoveFriendlyProjectile;
            }
        }

        public void AddEnemyProjectiles(List<Projectile> projectiles)
        {
            foreach (var projectile in projectiles)
            {
                enemyProjectiles.Add(projectile);
                projectile.OnDestroy += RemoveEnemyProjectile;
            }
        }

        public void MoveAllProjectiles()
        {
            //The loops cannot be foreach because lists become immutable when a foreach is called
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (enemyProjectiles[i] != null)
                {
                    enemyProjectiles[i].Move();
                    if (!IsInBounds(enemyProjectiles[i]))
                    {
                        enemyProjectiles[i].Destroy();
                        i--; // removed from list so keep index the same for next iteration of the loop
                    }
                }
            }

            for (int i = 0; i < friendlyProjectiles.Count; i++)
            {
                if (friendlyProjectiles[i] != null)
                {
                    friendlyProjectiles[i].Move();
                    if (!IsInBounds(friendlyProjectiles[i]))
                    {
                        friendlyProjectiles[i].Destroy();
                        i--; // removed from list so keep index the same for next iteration of the loop
                    }
                }
            }
            for (int i = 0; i < healthPacks.Count; i++)
            {
                if (healthPacks[i] != null)
                {
                    healthPacks[i].Move();
                    if (!IsInBounds(healthPacks[i]))
                    {
                        healthPacks[i].Destroy();
                        i--;
                    }
                }
            }
        }

        public void RemoveAllProjectiles()
        {
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (enemyProjectiles[i] != null)
                {
                    enemyProjectiles[i].Destroy();
                }
            }

            for (int i = 0; i < friendlyProjectiles.Count; i++)
            {
                if (friendlyProjectiles[i] != null)
                {
                    friendlyProjectiles[i].Destroy();
                }
            }
            for (int i = 0; i < healthPacks.Count; i++)
            {
                if (healthPacks[i] != null)
                {
                    healthPacks[i].Destroy();
                }
            }
        }

        private void RemoveHealthPack(object sender, EventArgs e)
        {
            var pack = (Projectile)sender;
            healthPacks.Remove(pack);
        }

        private void RemoveFriendlyProjectile(object sender, EventArgs e)
        {
            var projectile = (Projectile) sender;
            friendlyProjectiles.Remove(projectile);
            OnProjectileExplode.Invoke(projectile, null);
        }

        private void RemoveEnemyProjectile(object sender, EventArgs e)
        {
            var projectile = (Projectile)sender;
            enemyProjectiles.Remove(projectile);
            OnProjectileExplode.Invoke(projectile, null);
        }

        public bool IsInBounds(GameObject gameObject)
        {
            return gameObject.XPos + gameObject.Texture.Width > -200 && gameObject.YPos + gameObject.Texture.Height > -200 &&
                   gameObject.XPos < Settings.GlobalDisplayWidth + 200 && gameObject.YPos < Settings.GlobalDisplayHeight;
        }
    }
}