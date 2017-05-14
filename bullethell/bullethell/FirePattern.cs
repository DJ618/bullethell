using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;

namespace bullethell
{
    class FirePatternEventArgs : EventArgs
    {
        public List<Projectile> Projectiles { get; set; }

        public FirePatternEventArgs(List<Projectile> projectiles)
        {
            Projectiles = projectiles;
        }
    }

    delegate void SpawnProjectilesEventHandler(object sender, FirePatternEventArgs ars);

    abstract class FirePattern
    {
        public event SpawnProjectilesEventHandler OnSpawnProjectiles;

        protected ProjectileType projectileType;

        public int X { get; set; }

        public int Y { get; set; }

        public EventHandler RequestNewXAndY;

        protected bool stop = false;

        public virtual bool Stop
        {
            get { return stop; }
            set { stop = value; }
        }

        protected FirePattern(ProjectileType projectileType)
        {
            this.projectileType = projectileType;
        }

        public abstract void Fire(int x, int y);

        protected void SpawnProjectiles(List<Projectile> projectiles)
        {
            if (!stop)
            {
                FirePatternEventArgs args = new FirePatternEventArgs(projectiles);
                OnSpawnProjectiles.Invoke(this, args);
            }
        }

        protected Projectile CreateProjectile(int x, int y, Vector2 dir)
        {
            switch (projectileType)
            {
                case ProjectileType.Standard:
                    return new Projectile(TextureFactory.GetTexture("enemyBullet"), x, y, dir, false);
                case ProjectileType.Orange:
                    return new Projectile(TextureFactory.GetTexture("orangeBullet"), x, y, dir, false);
                case ProjectileType.Rope:
                    return new Projectile(TextureFactory.GetTexture("ropeBullet"), x, y, dir, false);
                case ProjectileType.Lazer:
                    return new Lazer(TextureFactory.GetTexture("enemyBullet"), x, y, dir, false);
                case ProjectileType.Pulsating:
                    return new PulsatingProjectile(TextureFactory.GetTexture("bullet4"), x, y, dir, false);
                case ProjectileType.Spinning:
                    return new SpinningProjectile(TextureFactory.GetTexture("bullet3"), x, y, dir, false);
                case ProjectileType.BlueFuzzyStandard:
                    return new Projectile(TextureFactory.GetTexture("bluefuzzyball"), x, y, dir, false)
                    {
                        Scale = .0003f * Settings.GlobalDisplayWidth,
                        Speed = Settings.GlobalDisplayHeight / 360f
                    };
            }
            return new Projectile(TextureFactory.GetTexture("enemyBullet"), x, y, dir, false);
        }

        protected void SendRequestForXAndY()
        {
            RequestNewXAndY.Invoke(this, null);
        }
    }
}