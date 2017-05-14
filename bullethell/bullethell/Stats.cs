using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bullethell
{
    public class Stats
    {
        //game stats
        public int highScore;
        public int playerScore;
        public double power;
        public int playerHealth;

        //spriteFonts
        public SpriteFont scoreFont;

        //constructor
        public Stats()
        {
            //default high score
            highScore = 10000000;
            //default starting player health
            playerHealth = 9;
            //default starting score of 0
            playerScore = 0;
            //starting power of 0
            power = 0.00f;
        }

        //constructor to pass in a previous high score from before
        public Stats(int loadHighScore)
        {
            //passed in (scripted) highscore from before
            highScore = loadHighScore;
            //default starting hp
            playerHealth = 9;
            //default starting score of 0
            playerScore = 0;
            //starting power of 0
            power = 0.00f;
        }

        //constructor to support fully scripted starting stats
        public Stats(int loadHighScore, int loadPlayerScore, double loadPower, int loadHealth)
        {
            highScore = loadHighScore;
            playerHealth = loadHealth;
            playerScore = loadPlayerScore;
            power = loadPower;
        }

        //functions to tally points
        //adjust player score
        public void adjustPlayerScore(int amount)
        {
            playerScore += amount;
        }
        //adjust player health
        public int adjustPlayerHealth(int amount)
        {
            playerHealth += amount;
            return playerHealth;
        }
        //adjust power
        public void adjustPlayerPower(double amount)
        {
            power += amount;
        }
        //adjust hiscore
        public void adjustHighScore(int amount)
        {
            //if the new high score is not greater than the previous, ignore the update
            if(amount > highScore)
            {
                highScore = amount;
            }
        }
    }
}
