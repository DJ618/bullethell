using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace bullethell
{
    class FireLazers : FirePattern
    {
        private int textureLen = 75;
        public FireLazers(ProjectileType projectileType) : base(projectileType)
        {
        }

        public override void Fire(int x, int y)
        {
            List<Projectile> ppattern = new List<Projectile>(8);
            
            ppattern.Clear();
            x += textureLen / 2;
            y += textureLen / 2;

            for (int degrees = 0; degrees < 360; degrees += 360 / 8)
            {
                double angle = degrees * Math.PI / 180;
                Vector2 v = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
                v.Normalize();
                int tempX = x + (int) (v.X * (textureLen + 20) / 2);
                int tempY = y + (int) (v.Y * (textureLen + 20) / 2);
                v = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle - 90));
                v.Normalize();
                float r1 = (float) (Game1.Rand.NextDouble() * Math.PI);
                float r2 = (float) (Game1.Rand.NextDouble() * Math.PI);
                if (v.X < 0)
                    r1 *= -1;
                if (v.Y < 0)
                    r2 *= -1;
                v.X += r1;
                v.Y += r2;
                if (Game1.Rand.Next() % 2 == 0)
                    v.X *= -1;
                if (Game1.Rand.Next() % 2 == 0)
                    v.Y *= -1;
                v.Normalize();
                ppattern.Add(CreateProjectile(tempX, tempY, v));
            }

            SpawnProjectiles(ppattern);
        }
    }
}
