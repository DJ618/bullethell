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
    class PulsatingProjectile : Projectile
    {
        private Timer delayTimer = new Timer(1000);
        private bool started = false;
        private bool spinDirection;

        public bool SpinDirection
        {
            get { return spinDirection;} 
            set { spinDirection = value; }
        }
        private int shiftAmount = 0;
        private Vector2 originalDirectionVector;

        public PulsatingProjectile(Texture2D texture, int x, int y, Vector2 Dir, bool friendly, bool spinDirection = true) : base(texture, x, y, Dir, friendly)
        {
            originalDirectionVector = Dir;
            Direction = Dir;
            delayTimer.Elapsed += OnTick;
            Speed = 3f;
            this.spinDirection = spinDirection;
        }

        public override void Move()
        {
            if (!started)
            {
                started = true;
                delayTimer.Start();
            }
            base.Move();
        }

        private void OnTick(object sender, ElapsedEventArgs args)
        {
            Vector2 newDirection;

            shiftAmount++;

            if(shiftAmount > 2)
            {
                delayTimer.Stop();
            }

            if (spinDirection)
            {
                newDirection = new Vector2(Direction.Y, -Direction.X);
                spinDirection = !spinDirection;
            }
            else
            {
                newDirection = new Vector2(-Direction.Y, Direction.X);
                spinDirection = !spinDirection;
            }
            newDirection.Normalize();
            Direction = newDirection;
        }
    }
}
