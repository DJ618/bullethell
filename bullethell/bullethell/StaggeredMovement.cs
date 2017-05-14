using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
    class StaggeredMovement : MovementPattern
    {
        private int hoverDelay = 480; //8 seconds
        private int numFire = 1;

        public override void UpdateDirection(GameObject enemy)
        {
            enemy.Speed = .005f * Settings.GlobalDisplayWidth;
            enemy.YDir = 1;

            if (enemy.YPos >= 0.2f * Settings.GlobalDisplayHeight)
            {
                enemy.Speed = 0.0f;
                if (numFire > 0)
                {
                    numFire--;
                    enemy.Fire();
                }
            }
            if (hoverDelay <= 0)
            {
                enemy.YDir = -1;
                enemy.Speed = .003f * Settings.GlobalDisplayWidth;
            }

            if (enemy.YPos < 0 - enemy.Texture.Height)
                enemy.Destroy();

            hoverDelay--;
        }
    }
}