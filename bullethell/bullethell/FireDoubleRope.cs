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
    class FireDoubleRope : FirePattern
    {
        private Timer delayTimer = new Timer(5);
        private bool started = false;
        private int degrees = 0;
        private double radius = 0;
        private int cx;
        private int c2x;
        private int cy;
        private int c2y;

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
        public FireDoubleRope(ProjectileType projectileType) : base(projectileType)
        {
            delayTimer.Elapsed += OnTick;
        }
        public override void Fire(int x, int y)
        {
            List<Projectile> ppattern = new List<Projectile>(2);

            ppattern.Clear();

            if (!started)
            {
                cx = x;
                cy = y;
                started = true;
                delayTimer.Start();
            }

            double angle = degrees * Math.PI / 180;
            double angle2 = (degrees + 180) * Math.PI / 180;

            //get spawn position for rope 1
            cx = (int)(((x * Math.Cos(angle)) + (y * Math.Sin(angle))) * (radius * 0.005));
            cy = (int)(((-1 * x * Math.Sin(angle)) + (y * Math.Cos(angle))) * (radius * 0.005));
            //get spawn position for rope 2
            c2x = (int)(((x * Math.Cos(angle2)) + (y * Math.Sin(angle2))) * (radius * 0.005));
            c2y = (int)(((-1 * x * Math.Sin(angle)) + (y * Math.Cos(angle))) * -(radius * 0.005));

            //get direction from spawn position
            Vector2 v = getPlayerDirection(cx+x, cy+y);
            Vector2 v2 = getPlayerDirection(c2x+x, c2y+y);

            ppattern.Add(CreateProjectile(c2x + x, c2y + y, v2));
            ppattern.Add(CreateProjectile(cx + x, cy + y, v));
            ppattern[0].Speed = 4f;
            ppattern[1].Speed = 4f;

            degrees += 10;
            radius += .5;

            if (degrees >= 720)
            {
                delayTimer.Stop();
                started = false;
                degrees = 0;
                radius = 0;
            }
            SpawnProjectiles(ppattern);
        }
        private Vector2 getPlayerDirection(int x, int y)
        {
            float yDir = Player.GlobalPlayerMiddleYPos - y;
            float xDir = Player.GlobalPlayerMiddleXPos - x;
            float tempYDir = yDir;
            float tempXDir = xDir;
            if (tempXDir < 0)
                tempXDir *= -1;
            if (tempYDir < 0)
                tempYDir *= -1;
            float total = tempYDir + tempXDir;
            yDir /= total;
            xDir /= total;
            return new Vector2(xDir, yDir);
        }
        private void OnTick(object sender, ElapsedEventArgs args)
        {
            SendRequestForXAndY();
            Fire(X, Y);
        }
    }
}
