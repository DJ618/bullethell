using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bullethell
{
    class Fire123 : FirePattern
    {
        public Fire123(ProjectileType projectileType) : base(projectileType)
        {
        }

        //1-2-3 fire pattern
        // . . .
        //  . .
        //   .
        //TODO:all bullets aim towards player
        //      currently only aimed straight down
        public override void Fire(int x, int y)
        {
            List<Projectile> ppattern = new List<Projectile>(6);

            ppattern.Add(CreatePositionedProjectile(x, y));

            ppattern.Add(CreatePositionedProjectile(x - 15, y - 20));

            ppattern.Add(CreatePositionedProjectile(x + 15, y - 20));

            ppattern.Add(CreatePositionedProjectile(x - 30, y - 40));

            ppattern.Add(CreatePositionedProjectile(x, y - 40));

            ppattern.Add(CreatePositionedProjectile(x + 30, y - 40));

            SpawnProjectiles(ppattern);
        }

        private Projectile CreatePositionedProjectile(int x, int y)
        {
            //aim towards player
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

            return CreateProjectile(x, y, new Vector2(xDir, yDir));
        }
    }
}