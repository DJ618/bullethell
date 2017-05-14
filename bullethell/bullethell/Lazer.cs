using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace bullethell
{
    class Lazer : Projectile
    {
        private int length;
        private int Length
        {
            get { return length; }
            set
            {
                length = value;
                hitBox.Height = value;
            } 
        }

        private int maxLength;
        private List<Rectangle> miniHitBoxes;

        public override Vector2 ExplosionLoc
        {
            get
            {
                if (miniHitBoxes != null)
                {
                    foreach (var box in miniHitBoxes)
                    {
                        if (box.Intersects(Player.GlobalPlayerHitBox))
                            return new Vector2(box.X, box.Y);
                    }
                }
                return Position;
            } 
        }

        public Lazer(Texture2D texture, int x, int y, Vector2 Dir, bool friendly) : base(texture, x, y, Dir, friendly)
        {
            Length = texture.Height;
            maxLength = (int) (.4f * Settings.GlobalDisplayHeight);
            Speed = Settings.GlobalDisplayHeight / 360f;
        }

        public override void Move()
        {
            Angle = (float)Math.Atan2(-XDir, YDir);
            if (Length < maxLength)
            {
                Length += (int) Speed;
                if (Length > maxLength)
                    Length = maxLength;
            }
            else
            {
                base.Move();
            }
        }

        private float Distance()
        {
            if (YDir != 0)
            {
                float cos = (float) Math.Cos(Angle);
                if (cos == 0f)
                {
                    return hitBox.Width;
                }
                return (float) Math.Abs(hitBox.Width / Math.Cos(Angle));
            }
            return hitBox.Width;
        }

        private bool CheckIfMiniHitBoxesCoverLazer()
        {
            if (miniHitBoxes.Count <= 0)
                return false;
            Vector2 finalPos = Position + Direction * Length;
            if (Direction.Y < 0) //going up
            {
                if (miniHitBoxes[miniHitBoxes.Count - 1].Y <= finalPos.Y)
                    return true;
                return false;
            }
            if (Direction.Y > 0) //going down
            {
                if (miniHitBoxes[miniHitBoxes.Count - 1].Y + hitBox.Width >= finalPos.Y)
                    return true;
                return false;
            }
            if (Direction.X < 0) //going left
            {
                if (miniHitBoxes[miniHitBoxes.Count - 1].X <= finalPos.X)
                    return true;
                return false;
            }
            if (Direction.X > 0) //going right
            {
                if (miniHitBoxes[miniHitBoxes.Count - 1].X + miniHitBoxes[miniHitBoxes.Count - 1].Width >= finalPos.X)
                    return true;
                return false;
            }
            return true;
        }

        public override bool Intersects(Rectangle rect)
        {
            miniHitBoxes = new List<Rectangle>();
            
            for (int i = 0; !CheckIfMiniHitBoxesCoverLazer(); i++)
            {
                var d = Distance();
                Vector2 pos = Position + Direction * i * Distance();
                Rectangle rect2 = new Rectangle((int)pos.X, (int)pos.Y, hitBox.Width, hitBox.Width);
                miniHitBoxes.Add(rect2);
            }
            
            foreach (Rectangle r in miniHitBoxes)
            {
                if (r.Intersects(rect))
                    return true;
            }
            return false;
        }
    }
}
