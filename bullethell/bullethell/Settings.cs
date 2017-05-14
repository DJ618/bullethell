using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace bullethell
{
    class Settings
    {
        public static int GlobalDisplayWidth = 0;
        public static int GlobalDisplayHeight = 0;
        GraphicsDeviceManager graphics;
        private float playerSpeedFactor = 0f;
        public float PlayerSpeedFactor
        {
            get { return playerSpeedFactor; }
            set
            {
                playerSpeedFactor = value;
                OnPlayerSpeedChange.Invoke(this, null);
            }
        }
        public float playerSpeed1 = 0f;
        public float playerSpeed2 = 0f;

        public EventHandler OnPlayerSpeedChange = delegate { };

        public Settings(GraphicsDeviceManager graphics, float playerSpeed1, float playerSpeed2)
        {
            this.graphics = graphics;
            PlayerSpeedFactor = this.playerSpeed1 = playerSpeed1;
            this.playerSpeed2 = playerSpeed2;
        }

        public int DisplayHeight
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height; }
            set { GlobalDisplayHeight = graphics.PreferredBackBufferHeight = value; }
        }

        public int DisplayWidth
        {
            get { return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width; }
            set { GlobalDisplayWidth = graphics.PreferredBackBufferWidth = value; }
        }

        public void MatchWindowSize()
        {
            DisplayWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            DisplayHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //DisplayWidth = 1000;
            //DisplayHeight = 1000;
        }

        public void Apply()
        {
            graphics.ApplyChanges();
        }

        public void ChangePlayerSpeed()
        {
            if (PlayerSpeedFactor == playerSpeed1)
                PlayerSpeedFactor = playerSpeed2;
            else
                PlayerSpeedFactor = playerSpeed1;
        }
    }
}