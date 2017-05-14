using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bullethell
{
    class FireNone : FirePattern
    {
        public FireNone(ProjectileType projectileType) : base(projectileType)
        {
        }

        public override void Fire(int x, int y)
        {
           
        }
    }
}