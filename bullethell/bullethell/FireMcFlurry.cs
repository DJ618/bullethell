using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Timer = System.Timers.Timer;

namespace bullethell
{
    //Approximately 120 bullets fired
    //Bullets are created by two patterns one clockwise and one counterclockwise
    //the speed is uniform
    //there is an element of randomness in regards to the rate of spawning. example: sometimes 1 sec in between bullets, sometimes 2
    //Not aimed at the player

    class FireMcFlurry : FirePattern
    {
        //Timer for when to shoot
        private Timer myTimer = new Timer(50);
        //List of projectiles for the fire pattern
        List<Projectile> projList;  //list of length one that gets rewritten every time
        private int _x;
        private int _y;
        private int degrees = 0;
        private int shotsFired = 0;
        private int theta = 0;
        private Random myRan = new Random();

        public FireMcFlurry(ProjectileType projectileType) : base(projectileType)
        {

        }

        public override void Fire(int x, int y)
        {
             _x = 0;
             _y = 0;
            degrees = 0;
            shotsFired = 0;
            theta = 0;
            _x = x;
            _y = y + 25;

            //Starting myTimer
            myTimer.Elapsed += new ElapsedEventHandler(shoot);
            myTimer.Elapsed += new ElapsedEventHandler(shoot2);
            myTimer.AutoReset = true;
            myTimer.Enabled = true;
            myTimer.Start();
        }

        public void shoot(object sender, ElapsedEventArgs e)
        {
            projList = new List<Projectile>(1);
            double angle = degrees * Math.PI / 180;                                     //getting the angle to fire
            Vector2 v = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));
            v.Normalize();

            int cx = (int)(_x + 25 * Math.Sin(theta));                                  //getting the x and y coords for the perimeter of a circle
            int cy = (int)(_y - 25 * (1 - Math.Cos(theta)));
            theta += 5;                                                                 //increasing our theta by 5 degrees

            Projectile newP = CreateProjectile(cx, cy, v);
            newP.Speed = 3f;
            projList.Add(newP);

            degrees = myRan.Next(0, 360);                                               //random angle at which to fire

            shotsFired++;
            if (shotsFired >= 120)
            {
                myTimer.AutoReset = false;
            }

            SpawnProjectiles(projList);                                                 //Spawning projectiles
        }
        public void shoot2(object sender, ElapsedEventArgs e)
        {
            projList = new List<Projectile>(1);
            double angle = degrees * Math.PI / 180;                                     //getting the angle to fire
            Vector2 v = new Vector2((float)Math.Cos(angle), -(float)Math.Sin(angle));
            v.Normalize();

            int cx = (int)((_x + 45) + 25 * Math.Sin(theta));                             //getting the x and y coords for the perimeter of a circle
            int cy = (int)((_y + 45) - 25 * (1 - Math.Cos(theta)));
            theta += 5;                                                                 //increasing our theta by 5 degrees

            Projectile newP = CreateProjectile(cx, cy, v);
            newP.Speed = 3f;
            projList.Add(newP);

            degrees = myRan.Next(0, 360);                                               //random angle at which to fire

            shotsFired++;
            if (shotsFired >= 120)
            {
                myTimer.AutoReset = false;
            }

            SpawnProjectiles(projList);                                                 //Spawning projectiles
        }
    }
}
