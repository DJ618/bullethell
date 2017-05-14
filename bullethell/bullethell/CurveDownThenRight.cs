using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; //for debugging

namespace bullethell
{
    class CurveDownThenRight : MovementPattern
    {
        private int numProjectileWaves = 1;

        public override void UpdateDirection(GameObject enemy)
        {
            //override me in subclass

            //check if need to spawn enemy
            enemy.Speed = .005f * Settings.GlobalDisplayWidth;

            float twentyPerc = (.2f * Settings.GlobalDisplayHeight);
            float thirtyPerc = (.3f * Settings.GlobalDisplayHeight);

            //move enemy
            if (enemy.YPos < twentyPerc)
            {
                //move straight down
                enemy.YDir = 1;
                enemy.XDir = 0;
            }
            else if (enemy.YPos >= twentyPerc & enemy.YPos < thirtyPerc)
            {
                float calcedX = (enemy.YPos - .2f) / twentyPerc;
                float calcedY = calcedX;
                enemy.YDir = calcedY;
                enemy.XDir = calcedX;
            }
            else
            {
                enemy.XDir = 1;
                enemy.YDir = 0;
            }
            if (numProjectileWaves > 0 && enemy.YPos > twentyPerc)
            {
                ((Enemy) enemy).Fire();
                numProjectileWaves--;
            }
            if (enemy.XPos < 0 - enemy.Texture.Width)
            {
                enemy.Destroy();
            }
        }
    }
}