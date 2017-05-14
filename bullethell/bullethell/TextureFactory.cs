using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bullethell
{
    class TextureFactory
    {
        private static Dictionary<string, Texture2D> Textures = new Dictionary<string, Texture2D>();
        private static Dictionary<string, SpriteFont> Fonts = new Dictionary<string, SpriteFont>();
        public static Texture2D GetTexture(string name)
        {
            if (!Textures.ContainsKey(name))
                Textures[name] = Game1.StaticContentManager.Load<Texture2D>(name);

            return Textures[name];
        }

        public static void LoadTextures()
        {
            string[] textureNames = { "ghost", "enemyBullet", "playerBullet", "Spaceship", "monster", "explosion", "orangeBullet", "Space003", "bluefuzzyball", "black-screen-2" };
            foreach (string textureName in textureNames)
            {
                var texture = Game1.StaticContentManager.Load<Texture2D>(textureName);
                Textures[textureName] = texture;
            }
        }

        public static SpriteFont GetFont(string fontName)
        {
            if (!Fonts.ContainsKey(fontName))
                Fonts[fontName] = Game1.StaticContentManager.Load<SpriteFont>(fontName);

            return Fonts[fontName];
        }
    }
}