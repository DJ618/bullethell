using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System.Xml;


namespace bullethell
{
    class ConfigParser
    {
        private XmlTextReader reader;
        private GraphicsDeviceManager graphics;

        public ConfigParser(string filename, GraphicsDeviceManager graphics)
        {
            reader = new XmlTextReader(filename);
            this.graphics = graphics;
        }

        public Settings ParseSettings()
        {
            float playerSpeed1 = 0f;
            float playerSpeed2 = 0f;

            while (reader.Read()) //for now, just make skip to game section
            {
                if (reader.Name == "game")
                {
                    Settings settings = new Settings(graphics, playerSpeed1, playerSpeed2);
                    return settings;
                }
                switch (reader.Name)
                {
                    case "playerSpeed1":
                        if (playerSpeed1 == 0f)
                        {
                            var speed = GetValue();
                            var speedF = double.Parse(speed);
                            playerSpeed1 = (float)speedF;
                        }
                        break;
                    case "playerSpeed2":
                        if (playerSpeed2 == 0f)
                        {
                            var speed2 = GetValue();
                            var speed2F = double.Parse(speed2);
                            playerSpeed2 = (float)speed2F;
                        }
                        break;
                }
            }
            return null;
        }

        public EnemyManager ParseEnemies()
        {
            EnemyBuilder builder = new EnemyBuilder(reader);
            EnemyManager manager = new EnemyManager();
            bool gameOverTimerFound = false;

            while (reader.Read())
            {
                if (reader.Name == "enemy")
                {
                    reader.Read();
                    manager.AddEnemy(builder.ConstructEnemy());
                }
                if (reader.Name == "gameOver" && !gameOverTimerFound)
                {
                    manager.GameTimer = new Timer(int.Parse(GetValue()));
                    gameOverTimerFound = true;
                }
                if (reader.ReadState == ReadState.EndOfFile)
                    return manager;
            }

            return manager; 
        }

        public void CloseReader()
        {
            reader.Close();
        }

        private string GetValue()
        {
            while (reader.Value == "" || string.IsNullOrWhiteSpace(reader.Value))
            {
                reader.Read();
            }
            return reader.Value;
        }
    }
}