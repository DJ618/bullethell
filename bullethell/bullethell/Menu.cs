using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace bullethell
{
    public class MenuOption
    {
        public string Text { get; set; }
        public Vector2 MiddlePoint => Font.MeasureString(Text) / 2;
        public Vector2 Position { get; set; }
        public bool Selected { get; set; } = false;

        public Color FontColor
        {
            get
            {
                if (Selected)
                    return Color.Red;
                return Color.White;
            }
        }

        public SpriteFont Font
        {
            get
            {
                if (Selected)
                    return TextureFactory.GetFont("SelectedMenuFont");

                return TextureFactory.GetFont("MenuFont");
            }
        }
    }

    public class Menu
    {
        public EventHandler OnSelectPlayGame = delegate { };
        public EventHandler OnSelectExit = delegate { };
        private int selectedIndex = 0;
        private MenuOption[] menuOptions;
        private bool btnWasDown = false;
        private bool hasBeenLoaded = false;

        private MenuOption PlayGameOption
        {
            get
            {
                if (menuOptions.Length > 0)
                    return menuOptions[0];
                return null;
            }
        }

        private MenuOption ExitOption
        {
            get
            {
                if (menuOptions.Length > 1)
                    return menuOptions[1];
                return null;
            }
        }

        public void DrawMenu(SpriteBatch spriteBatch)
        {
            if (!hasBeenLoaded)
                LoadMenuOptions();
            // Finds the center of the string in coordinates inside the text rectangle
            foreach (var option in menuOptions)
            {
                spriteBatch.DrawString(option.Font, option.Text, option.Position, option.FontColor, 0, option.MiddlePoint, 1.0f, SpriteEffects.None, 0.5f);
            }
        }

        public void LoadMenuOptions()
        {
            hasBeenLoaded = true;
            menuOptions = new MenuOption[]
            {
                new MenuOption
                {
                    Text = "Play Game",
                    Position = new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2 - 60),
                    Selected = true
                },
                new MenuOption
                {
                    Text = "Exit",
                    Position = new Vector2(Settings.GlobalDisplayWidth / 2, Settings.GlobalDisplayHeight / 2)
                }
            };
        }

        public void DetectMenuInteraction()
        {
            bool down = Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S);
            bool up = Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W);
            bool select = Keyboard.GetState().IsKeyDown(Keys.Enter) || Keyboard.GetState().IsKeyDown(Keys.Z);
            if (!btnWasDown)
            {
                if (select)
                {
                    Select();
                } else if (down || up)
                {
                    PlayGameOption.Selected = ExitOption.Selected;
                    ExitOption.Selected = !PlayGameOption.Selected;
                    btnWasDown = true;
                }
            } else if (!down && !up && !select)
            {
                btnWasDown = false;
            }
        }

        //triggers whatever option is selected
        private void Select()
        {
            if (PlayGameOption.Selected)
                OnSelectPlayGame.Invoke(this, null);
            else if (ExitOption.Selected)
                OnSelectExit.Invoke(this, null);
        }
    }
}
