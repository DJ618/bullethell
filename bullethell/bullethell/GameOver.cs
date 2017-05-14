using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bullethell
{
    class GameOver
    {
        public void DrawGameOver(SpriteBatch spriteBatch, int score)
        {
            var gameOver = new MenuOption
            {
                Text = "Game Over",
                Position = new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2 - 60),
                Selected = true
            };
            var scoreOption = new MenuOption
            {
                Text = "Score: " + score,
                Position = new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2),
                Selected = true
            };
            var exitText = new MenuOption
            {
                Text = "Press Enter to Exit",
                Position = new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2 + 60),
                Selected = true
            };
            spriteBatch.DrawString(gameOver.Font, gameOver.Text, gameOver.Position, gameOver.FontColor, 0, gameOver.MiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(scoreOption.Font, scoreOption.Text, scoreOption.Position, scoreOption.FontColor, 0, scoreOption.MiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(exitText.Font, exitText.Text, exitText.Position, exitText.FontColor, 0, exitText.MiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
        }
    }
}
