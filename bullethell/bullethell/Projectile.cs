using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;


namespace bullethell
{
    public enum ProjectileType
    {
        Standard,
        Orange,
        Rope,
        Lazer,
        Pulsating,
        Spinning,
        BlueFuzzyStandard
    }

    class Projectile : GameObject
    {
        public bool Friendly { get; set; }
        public float Angle { get; set; } = 0f;
        public virtual Vector2 ExplosionLoc { get { return Position; } }

        public Projectile(Texture2D texture, int x, int y, Vector2 Dir, bool friendly) : base(texture, x, y)
        {
            Direction = Dir;
            Speed = Settings.GlobalDisplayHeight / 180f;
            Friendly = friendly;
        }
    }
}