using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace bullethell
{
    class Background
    {
        public Texture2D background;
        public Rectangle rectangle;
        public Background(Texture2D bg, Rectangle rect)
        {
            background = bg;
            rectangle = rect;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, rectangle, Color.White);
        }
        public void Update()
        {
            //if (rectangle.Y + 5 >= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            //    rectangle.Y = -GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            //rectangle.Y += 5;
        }
    }
}
