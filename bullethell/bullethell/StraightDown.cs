using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
    class StraightDown : MovementPattern
    {
        public override void UpdateDirection(GameObject enemy)
        {
            enemy.Speed = .005f * Settings.GlobalDisplayHeight;
            enemy.XDir = 0;
            enemy.YDir = 1;
            if (enemy.YPos > Settings.GlobalDisplayHeight)
                enemy.Destroy();
        }
    }
}
