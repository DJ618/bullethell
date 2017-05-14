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
    class SpinningProjectile : Projectile
    {
        private Timer delayTimer = new Timer(100);
        private Timer speedDelay = new Timer(500);
        private Timer exitDelay = new Timer(3000);
        private bool started = false;
        private bool checkpoint = false;
        private bool spinDirection;

        public bool SpinDirection
        {
            get { return spinDirection;}
            set { spinDirection = value; }
        }

        public SpinningProjectile(Texture2D texture, int x, int y, Vector2 Dir, bool friendly, bool spinDirection = true) : base(texture, x, y, Dir, friendly)
        {
            delayTimer.Elapsed += OnTick;
            exitDelay.Elapsed += beginExit;
            speedDelay.Elapsed += changeSpeed;
            this.spinDirection = spinDirection;
        }

        public override void Move()
        {
            if (!started)
            {
                started = true;
                delayTimer.Start();
                exitDelay.Start();
                speedDelay.Start();
            }
            base.Move();
        }

        private void OnTick(object sender, ElapsedEventArgs args)
        {
            Vector2 newDirection;

            if (spinDirection && checkpoint == true)
            {
                newDirection = new Vector2(Direction.X, Direction.Y);
                newDirection = RotateBy(newDirection, .1);


            }
            else if (spinDirection == false && checkpoint == true)
            {
                newDirection = new Vector2(Direction.X, Direction.Y);
                newDirection = RotateBy(newDirection, -.1);
            }
            else
            {
                newDirection = new Vector2(Direction.X, Direction.Y);
            }
            newDirection.Normalize();
            Direction = newDirection;
        }

        public void changeSpeed(object sender, ElapsedEventArgs args)
        {
            Speed = 4.5f;
            checkpoint = true;
            speedDelay.Stop();
        }

        public static Vector2 RotateBy(Vector2 v, double rotdeg)
        {
            var ca = Math.Cos(rotdeg);
            var sa = Math.Sin(rotdeg);
            var rx = v.X * ca - v.Y * sa;

            return new Vector2((float)rx, (float)(v.X * sa + v.Y * ca));
        }

        private void beginExit(object sender, ElapsedEventArgs args)
        {
            checkpoint = false;
        }
    }
}
