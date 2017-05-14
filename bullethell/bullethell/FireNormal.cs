using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Timers;

namespace bullethell
{
    class FireNormal : FirePattern
    {
        private int numFired = 0;
        private Timer timer = new Timer(200);

        public FireNormal(ProjectileType projectileType) : base(projectileType)
        {
        }

        public override void Fire(int x, int y)
        {
            timer.Elapsed += OnTick;
            timer.Start();

        }

        private void OnTick(object sender, EventArgs args)
        {
            List<Projectile> projectiles = new List<Projectile>(1);
            SendRequestForXAndY();
            projectiles.Add(CreateAimedProjectile(X, Y));
            numFired++;
            if (numFired >= 10)
            {
                timer.Stop();
            }
                

            SpawnProjectiles(projectiles);
        }

        private Projectile CreateAimedProjectile(int x, int y)
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

            return CreateProjectile(x, y, new Vector2(xDir, yDir));
        }
    }
}
