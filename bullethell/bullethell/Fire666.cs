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
    class Fire666 : FirePattern
    {
        private Timer delayTimer = new Timer(200);
        private bool started = false;
        private int wavesSent = 0;

        public override bool Stop
        {
            get { return stop; }
            set
            {
                stop = value;
                if (stop)
                {
                    delayTimer.Stop();
                }
            }
        }
        public Fire666(ProjectileType projectileType) : base(projectileType)
        {
            delayTimer.Elapsed += OnTick;
        }

        public override void Fire(int x, int y)
        {
            List<Projectile> ppattern = new List<Projectile>(6);
            ppattern.Clear();

            if (!started)
            {
                started = true;
                delayTimer.Start();
            }

            for (int i = 0; i < 3; i++)
            {
                for (int degrees = 210; degrees <= 330; degrees += 20)
                {
                    double angle = degrees * Math.PI / 180;
                    Vector2 v = new Vector2((float) Math.Cos(angle), -(float) Math.Sin(angle));
                    v.Normalize();
                    ppattern.Add(CreateProjectile(x, y, v));
                }
            }
            wavesSent++;
            if (wavesSent >= 3)
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