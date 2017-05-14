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
    class FireCircle : FirePattern
    {
        Timer delayTimer = new Timer(200);
        int wavesSent = 0;
        bool started = false;
        public FireCircle(ProjectileType projectileType) : base(projectileType)
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

            for (int degrees = 0; degrees < 360; degrees += 30)
            {
                double angle = degrees * Math.PI / 180;
                Vector2 v = new Vector2((float) Math.Cos(angle), (float) Math.Sin(angle));
                v.Normalize();
                ppattern.Add(CreateProjectile(x, y, v));
            }
            wavesSent++;

            if (wavesSent > 3)
            {
                delayTimer.Stop();
            }
            SpawnProjectiles(ppattern);

        }
        public void OnTick(object sender, ElapsedEventArgs e)
        {
            SendRequestForXAndY();
            Fire(X, Y);
        }
    }
}