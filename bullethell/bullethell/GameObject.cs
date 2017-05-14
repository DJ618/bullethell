using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Remoting.Messaging;


namespace bullethell
{
    abstract class GameObject
    {
        public event SpawnProjectilesEventHandler OnSpawnProjectiles;

        public delegate void DestroyEventHandler(object sender, EventArgs e);

        public event DestroyEventHandler OnDestroy;

        public bool Done { get; set; }
        public Texture2D Texture { get; set; } //may decrease accessibility upon further code review
        private Vector2 position;
        public int Health { get; set; }

        public Vector2 Position
        {
            get { return position; }
            set
            {
                XPos = value.X;
                YPos = value.Y;
            }
        } //may decrease accessibility upon further code review

        public virtual float XPos
        {
            get { return position.X; }
            set
            {
                position.X = value;
                hitBox.X = (int) value;
            }
        }

        public virtual float YPos
        {
            get { return position.Y; }
            set
            {
                position.Y = value;
                hitBox.Y = (int) value;
            }
        }

        private Vector2 direction;

        public Vector2 Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public float XDir
        {
            get { return direction.X; }
            set { direction.X = value; }
        }

        public float YDir
        {
            get { return direction.Y; }
            set { direction.Y = value; }
        }

        public float Speed { get; set; }
        public Vector2 Origin { get; set; } //may decrease accessibility upon further code review
        private float scale;

        public virtual float Scale
        {
            get { return scale; }
            set
            {
                scale = value;
                hitBox.Width = (int) (Texture.Width * Scale);
                hitBox.Height = (int) (Texture.Height * Scale);
            }
        }

        public SpriteEffects SpriteEffect { get; set; } //may decrease accessibility upon further code review
        public float ZDepth { get; set; } //may decrease accessibility upon further code review
        protected Rectangle hitBox;

        public Rectangle HitBox
        {
            get { return hitBox; }
            set { hitBox = value; }
        }

        public GameObject(Texture2D texture, int x, int y)
        {
            Texture = texture;
            Position = new Vector2(x, y);
            Origin = new Vector2(0, 0); //0,0 keeps the origin in the top left corner
            ZDepth = 0.1f;
            SpriteEffect = SpriteEffects.None;
            Scale = 1.0f;
            Direction = new Vector2(0.0f, 0.0f);
            Speed = 0.0f;
            Done = false;
            Health = 1; //alive by default
        }

        public virtual void Move()
        {
            Position += Direction * Speed;
        }

        public virtual void Fire()
        {
        }

        public virtual void Destroy()
        {
            Done = true;
            OnDestroy.Invoke(this, null);
        }

        protected virtual void SpawnProjectiles(List<Projectile> projectiles)
        {
            OnSpawnProjectiles.Invoke(this, new FirePatternEventArgs(projectiles));
        }

        public virtual void TakeDamage(int damage)
        {
            Health -= damage;
            if (Health <= 0)
                Destroy();
        }

        public virtual bool Intersects(Rectangle rect) => hitBox.Intersects(rect);
    }
}