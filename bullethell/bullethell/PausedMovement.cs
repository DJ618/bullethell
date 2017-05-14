using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics; //for debugging

namespace bullethell
{
	class PausedMovement : MovementPattern
	{
		private Random random = new Random();
		private int waitTime = -1;
		private int timer = 0;
	    private int moveDownTimer = 100;

		public override void UpdateDirection(GameObject enemy)
		{
			//override me in subclass

			//check if need to spawn enemy
			//we will leave this here in case we want to have more than 1 boss at a time
			enemy.Speed = .005f * Settings.GlobalDisplayWidth;
		    if (moveDownTimer > 0)
		    {
		        enemy.XDir = 0;
		        enemy.YDir = 1;
		        moveDownTimer--;
		        if (enemy.YPos >= .0005f * Settings.GlobalDisplayHeight)
		        {
		            moveDownTimer = 0;
		        }
		    }
		    else
		    {
                //calculate the 20% and 40% boundaries of the screen
                float tenPercHeight = (.1f * Settings.GlobalDisplayHeight);
                float thirtyPercHeight = (.3f * Settings.GlobalDisplayHeight);
                float tenPercWidthLeft = (.1f * Settings.GlobalDisplayWidth);
                float tenPercWidthRight = (.9f * Settings.GlobalDisplayWidth);


                //move the boss(es) for loop
                //check if we are supposed to be waiting at this position
                if (timer > 0)
                {
                    //move enemy
                    if (enemy.YPos < tenPercHeight) //if we are in the top 10% of the screen
                    {
                        //move straight down
                        //((Enemy)enemies[i]).Firing = true; 

                        enemy.YDir = 1;
                        enemy.XDir = 0;
                    }

                    if (enemy.YPos >= thirtyPercHeight) //we moved below the 30% range
                    {
                        enemy.YDir = -1;
                        enemy.XDir = random.Next(-1, 2);
                    }

                    //if we hit the left horizontal bounds..
                    if (enemy.XPos <= tenPercWidthLeft)
                    {
                        //Move back the other way
                        enemy.XDir = 1;
                    }
                    //if we hit the right horizontal bounds..
                    if (enemy.XPos >= tenPercWidthRight)
                    {
                        //Move back the other way
                        enemy.XDir = -1;
                    }
                } //end of checking for timer
                else
                {
                    if (waitTime < 0)
                    {
                        waitTime = 500; //change to make sure that 3 ropes can be fired at once
                    }
                    else if (waitTime == 500)
                    {
                        ((Enemy)enemy).Fire();
                        enemy.YDir = 0;
                        enemy.XDir = 0;
                        waitTime--;
                    }
                    else if (waitTime > 0)
                    {

                        enemy.YDir = 0;
                        enemy.XDir = 0;
                        waitTime--;
                    }
                    else if (waitTime == 0)
                    {
                        timer = 100;
                        waitTime = -1;
                        enemy.XDir = random.Next(-1, 2);
                        enemy.YDir = random.Next(-1, 2);
                    }

                    if (waitTime == 400)
                    {
                        ((Enemy)enemy).Fire();
                    }
                    if (waitTime == 300)
                        ((Enemy)enemy).Fire();
                }
                //decrement timer and waitTime
                timer--;
            }

			
		} //end of update direction
	} //end of class
}