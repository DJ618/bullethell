using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;

namespace bullethell
{
    class Player : GameObject
    {
        //these 2 static
        public static float GlobalPlayerMiddleXPos = 0.0f;
        public static float GlobalPlayerMiddleYPos = 0.0f;
        public static Rectangle GlobalPlayerHitBox;

        private Timer fireDelayTimer;
        private Texture2D pTexture;
        public int RightBound { get; set; }
        public int LeftBound { get; set; }
        public int TopBound { get; set; }
        public int BottomBound { get; set; }
        private float hitBoxSizeModifier = 0.3f;
        public int ProjectileDelayTime { get; set; } //milliseconds

        public bool hitCD = false;
        private Timer hitCoolDown;

        public bool Enabled { get; set; } = false;

        public override float Scale
        {
            set
            {
                base.Scale = value;
                hitBox.Width = hitBox.Height = (int) (Texture.Height * Scale * hitBoxSizeModifier);
                GlobalPlayerHitBox = hitBox;
            }
            get { return base.Scale; }
        }

        public override float XPos
        {
            get { return base.XPos; }
            set
            {
                base.XPos = value;
                hitBox.X = (int) (value + Texture.Width * Scale / 2 - hitBox.Width / 2);
                GlobalPlayerHitBox = hitBox;
                GlobalPlayerMiddleXPos = XPos + Texture.Width * Scale / 2;
            }
        }

        public override float YPos
        {
            get { return base.YPos; }
            set
            {
                base.YPos = value;
                hitBox.Y = (int) (value + Texture.Height * Scale / 2 - hitBox.Height / 2);
                GlobalPlayerHitBox = hitBox;
                GlobalPlayerMiddleYPos = YPos + Texture.Height * Scale / 2;
            }
        }

        public Player(Texture2D texture, Texture2D projectileTexture, int x, int y) : base(texture, x, y)
        {
            pTexture = projectileTexture;
            Speed = 1.0f;
            RightBound = LeftBound = TopBound = BottomBound = 0;
            ProjectileDelayTime = 20;
            fireDelayTimer = new Timer(ProjectileDelayTime); //param is num milliseconds
            fireDelayTimer.Elapsed += HandleFire;
            fireDelayTimer.Start();

            hitCoolDown = new Timer(2000);
            hitCoolDown.Elapsed += resetHitCD;
            Health = 9;
        }

        //updates movement vectors based off of keys
        private void UpdateDirection()
        {
            if (Enabled)
            {
                bool down = Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S);
                bool right = Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D);
                bool left = Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A);
                bool up = Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W);

                if (down)
                    HandleDown(right, left); //handles down with the combination of possibly right or left
                else if (up)
                    HandleUp(right, left); //handles up with the combination of possibly right or left
                else if (left)
                    HandleLeft(); //handles left clicked just by itself
                else if (right)
                    HandleRight(); //handles right clicked just by itself
                else
                {
                    XDir = 0;
                    YDir = 0;
                }
            }
        }

        //Handles movement for when the down button is pressed (and possibly right or left with it)
        private void HandleDown(bool right, bool left)
        {
            if (right)
            {
                XDir = 1.0f / (float) Math.Sqrt(2.0);
                YDir = 1.0f / (float) Math.Sqrt(2.0);
            }
            else if (left)
            {
                XDir = -1.0f / (float) Math.Sqrt(2.0);
                YDir = 1.0f / (float) Math.Sqrt(2.0);
            }
            else
            {
                XDir = 0;
                YDir = 1.0f;
            }
        }

        //Handles movement for when the up button is pressed (and possibly right or left with it)
        private void HandleUp(bool right, bool left)
        {
            if (right)
            {
                XDir = 1.0f / (float) Math.Sqrt(2.0);
                YDir = -1.0f / (float) Math.Sqrt(2.0);
            }
            else if (left)
            {
                XDir = -1.0f / (float) Math.Sqrt(2.0);
                YDir = -1.0f / (float) Math.Sqrt(2.0);
            }
            else
            {
                XDir = 0;
                YDir = -1.0f;
            }
        }

        //handles movement when only the left btn is clicked
        private void HandleLeft()
        {
            XDir = -1.0f;
            YDir = 0;
        }

        //handles movement when only the right btn is clicked
        private void HandleRight()
        {
            XDir = 1.0f;
            YDir = 0;
        }

        public override void Move()
        {
            UpdateDirection();
            Position += Direction * Speed;
            CheckBounds();
        }

        public void HandleFire(object sender, EventArgs args)
        {
            if (Enabled)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Z))
                {
                    List<Projectile> pList = new List<Projectile>(2);
                    pList.Add(new Projectile(pTexture, (int)XPos - (int)(.5f * pTexture.Width),
                        (int)YPos + (int)(.7f * Texture.Height * Scale), new Vector2(0f, -1f), true)
                    { Speed = Settings.GlobalDisplayHeight / 50f });
                    pList.Add(new Projectile(pTexture,
                        (int)XPos + (int)(Texture.Width * Scale) - (int)(pTexture.Width * .5f),
                        (int)YPos + (int)(.7f * Texture.Height * Scale), new Vector2(0f, -1f), true)
                    { Speed = Settings.GlobalDisplayHeight / 50f });

                    SpawnProjectiles(pList);
                }
            }
        }

        //Keep the player in the designated bounds (probably the screen)
        private void CheckBounds()
        {
            //Keep player within the bounds designated (probably the bounds of the screen)
            if (XPos < LeftBound)
                XPos = LeftBound;
            else if (XPos > RightBound - Texture.Width * Scale)
                XPos = RightBound - Texture.Width * Scale;

            if (YPos < TopBound)
                YPos = TopBound;
            else if (YPos > BottomBound - Texture.Height * Scale)
                YPos = BottomBound - Texture.Height * Scale;
        }

        public override void TakeDamage(int damage)
        {
            //resets position on hit
            XPos = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width * 0.5f;
            YPos = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height * 0.75f;

            if (!hitCD)
            {
                hitCD = true;
                hitCoolDown.Start();
                Health -= damage;
                if (Health < 0)
                {
                    Destroy();
                }
                hitCoolDown.Start();
            }
        }

        public override void Destroy()
        {
            fireDelayTimer.Stop();
            base.Destroy();
        }

        private void resetHitCD(object sender, EventArgs args)
        {
            hitCD = false;
        }
    }
}