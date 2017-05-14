using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Timers;
using System.Diagnostics;


namespace bullethell
{
    class SpinningCircleBlast : FirePattern
    {
        private Timer delayTimer = new Timer(1200);
        private bool started = false;
        private int wavesSent = 0;

        public SpinningCircleBlast(ProjectileType projectileType) : base(projectileType)
        {
            delayTimer.Elapsed += OnTick;
        }

        public override void Fire(int x, int y)
        {
            List<Projectile> ppattern = new List<Projectile>(12);
            ppattern.Clear();

            if (!started)
            {
                started = true;
                delayTimer.Start();
            }

            for (int degrees = 0; degrees < 360; degrees += 20)
            {
                double angle = degrees * Math.PI / 180;
                Vector2 v = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                v.Normalize();
                Projectile p = CreateProjectile(x, y, v);
                SpinningProjectile spinningProjectile = p as SpinningProjectile;
                if (spinningProjectile != null)
                    spinningProjectile.SpinDirection = wavesSent == 1;
                ppattern.Add(spinningProjectile);
            }
            wavesSent++;
            if (wavesSent >= 2)
            {
                delayTimer.Stop();
            }
            SpawnProjectiles(ppattern);
        }

        private void OnTick(object sender, ElapsedEventArgs args)
        {
            SendRequestForXAndY();
            Fire(X, Y);
        }
    }
}
