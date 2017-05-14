using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System; //used for Convert and Math
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework.Content;

namespace bullethell
{
    class GraphicsDrawer
    {
        private SpriteBatch spriteBatch;

        public GraphicsDrawer(SpriteBatch spriteBatch)
        {
            this.spriteBatch = spriteBatch;
        }
        public void DrawPlayer(Player player)
        {
            Color useColor;
            if (player.hitCD)
            {
                useColor = Color.White * 0.1f;
            }
            else
            {
                useColor = Color.White;
            }
            spriteBatch.Draw(player.Texture,
                player.Position,
                null, useColor,
                0.0f, player.Origin,
                player.Scale,
                player.SpriteEffect,
                player.ZDepth);
        }
        public void DrawEnemies(List<Enemy> enemies)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                DrawGameObject(enemies[i]);
            }
        }

        public void DrawStats(Stats gameStats, int playerHealth)
        {
            spriteBatch.DrawString(gameStats.scoreFont, "HiScore: " + gameStats.highScore, new Vector2(Settings.GlobalDisplayWidth - (float).15 * (Settings.GlobalDisplayWidth), Settings.GlobalDisplayHeight - (float).92 * (Settings.GlobalDisplayHeight)), Color.White);
            spriteBatch.DrawString(gameStats.scoreFont, "Score: " + gameStats.playerScore, new Vector2(Settings.GlobalDisplayWidth - (float).15 * (Settings.GlobalDisplayWidth), Settings.GlobalDisplayHeight - (float).88 * (Settings.GlobalDisplayHeight)), Color.White);
            spriteBatch.DrawString(gameStats.scoreFont, "Power: " + gameStats.power, new Vector2(Settings.GlobalDisplayWidth - (float).15 * (Settings.GlobalDisplayWidth), Settings.GlobalDisplayHeight - (float).80 * (Settings.GlobalDisplayHeight)), Color.White);

            spriteBatch.DrawString(gameStats.scoreFont, "Player: ", new Vector2(Settings.GlobalDisplayWidth - (float).15 * (Settings.GlobalDisplayWidth), Settings.GlobalDisplayHeight - (float).84 * (Settings.GlobalDisplayHeight)), Color.White);

            if (playerHealth >= 0)
            {
                gameStats.playerHealth = playerHealth;
                string currentHpStr = gameStats.playerHealth.ToString();
                currentHpStr = currentHpStr + "hp";
                var hpPic = TextureFactory.GetTexture(currentHpStr);
                DrawHpPicture(hpPic);
            }
            else
            {
                DrawEndGame(gameStats);
            }
            
        }
        
        public void DrawHpPicture(Texture2D hpPic)
        {
            spriteBatch.Draw(hpPic, new Vector2(Settings.GlobalDisplayWidth - (float).1 * (Settings.GlobalDisplayWidth), Settings.GlobalDisplayHeight - (float).84 * (Settings.GlobalDisplayHeight)));
        }

        public void DrawProjectiles(List<Projectile> projectiles)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                DrawProjectile(projectiles[i]);
            }
        }

        public void DrawExplosions(List<ExplodingProjectile> projectiles)
        {
            for (int i = 0; i < projectiles.Count; i++)
            {
                DrawExplosion(projectiles[i].Projectile);
                projectiles[i].Count--;
            }
        }

        public void DrawProjectile(Projectile p)
        {
            if (p != null)
                spriteBatch.Draw(p.Texture,
                p.HitBox,
                null,
                Color.White,
                p.Angle,
                p.Origin,
                p.SpriteEffect,
                p.ZDepth);
        }

        public void DrawExplosion(Projectile p)
        {
            var t = TextureFactory.GetTexture("explosion");
            float scale = .01f * Settings.GlobalDisplayWidth / t.Width;
            if (p.Scale > scale)
                scale = p.Scale * p.Texture.Width / t.Width;
            if (p != null)
                spriteBatch.Draw(t,
                    p.ExplosionLoc,
                    null, Color.White,
                    0.0f, p.Origin,
                    scale,
                    p.SpriteEffect,
                    p.ZDepth);
        }

        public void DrawGameObject(GameObject gameObject)
        {
            if (gameObject != null)
                spriteBatch.Draw(gameObject.Texture,
                   gameObject.Position,
                   null, Color.White,
                   0.0f, gameObject.Origin,
                   gameObject.Scale,
                   gameObject.SpriteEffect,
                   gameObject.ZDepth);
        }
        public void DrawScrollingBackground(Background bg)
        {
            bg.Draw(spriteBatch);
        }
        public void DrawEndGame(Stats gameStats)
        {
            if (gameStats.playerHealth <= 0)
            {
                //lose
                var endpic = TextureFactory.GetTexture("black-screen-2");

                spriteBatch.Draw(endpic, new Vector2(0, 0));
                spriteBatch.DrawString(gameStats.scoreFont, "You lose.", new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2), Color.White);
            }
            //   elseif (UNKNOWN PARAMETER ATM)
            //   {
            //	//win
            //	spriteBatch.Draw(new Texture2D(GraphicsDevice, settings.DisplayWidth, settings.DisplayHeight),
            //		new Rectangle(0, 0, settings.DisplayWidth, settings.DisplayHeight), Color.Black);
            //	spriteBatch.DrawString(gameStats.scoreFont, "You Win!", new Vector2(settings.DisplayWidth - (float).15 * (settings.DisplayWidth), settings.DisplayHeight - (float).92 * (settings.DisplayHeight)), Color.White);
            //}
        }
    }
}
