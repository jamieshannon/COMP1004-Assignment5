/*
 * App Name: Assignment 4
 * Name: Jamie Shannon
 * StudentID: 200328763
 * Date: Dec 16/16
 * Description: Slot Machine Application that allows the user to place a bet and run a slot machine.
 * Winnings are determined based on the roll of the slots and the user has the opportunity to 
 * win a jackpot if they are lucky.
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace COMP1004_Assignment5
{
    public partial class SlotMachineForm : Form
    {

        // Declare variables for the class
        private int playerMoney = 1000;
        private int winnings = 0;
        private int jackpot = 5000;
        private float turn = 0.0f;
        private int playerBet = 0;
        private float winNumber = 0.0f;
        private float lossNumber = 0.0f;
        private string[] spinResult;
        private float winRatio = 0.0f;
        private float lossRatio = 0.0f;
        private int grapes = 0;
        private int bananas = 0;
        private int oranges = 0;
        private int cherries = 0;
        private int bars = 0;
        private int bells = 0;
        private int sevens = 0;
        private int blanks = 0;

        private Random random = new Random();

        public SlotMachineForm()
        {
            InitializeComponent();
        }

        /* Utility function to show Player Stats */
        private void showPlayerStats()
        {
            winRatio = winNumber / turn;
            lossRatio = lossNumber / turn;
            string stats = "";
            stats += ("Jackpot: " + jackpot + "\n");
            stats += ("Player Money: " + playerMoney + "\n");
            stats += ("Turn: " + turn + "\n");
            stats += ("Wins: " + winNumber + "\n");
            stats += ("Losses: " + lossNumber + "\n");
            stats += ("Win Ratio: " + (winRatio * 100) + "%\n");
            stats += ("Loss Ratio: " + (lossRatio * 100) + "%\n");
            MessageBox.Show(stats, "Player Stats");
        }

        /* Utility function to reset all fruit tallies*/
        private void resetFruitTally()
        {
            grapes = 0;
            bananas = 0;
            oranges = 0;
            cherries = 0;
            bars = 0;
            bells = 0;
            sevens = 0;
            blanks = 0;
        }

        /* Utility function to reset the player stats */
        private void resetAll()
        {
            playerMoney = 1000;
            winnings = 0;
            jackpot = 5000;
            turn = 0;
            playerBet = 0;
            winNumber = 0;
            lossNumber = 0;
            winRatio = 0.0f;

            //Reset the labels and the images to the spin logo
            TotalCreditsLabel.Text = "1000";
            BetLabel.Text = "0";
            WiningsLabel.Text = "0";
            JackpotLabel.Text = "5000";
            FirstReelPictureBox.Image = Properties.Resources.spin1;
            SecondReelPictureBox.Image = Properties.Resources.spin1;
            ThirdReelPictureBox.Image = Properties.Resources.spin1;
        }

        /* Check to see if the player won the jackpot */
        private void checkJackPot()
        {
            /* compare two random values */
            var jackPotTry = this.random.Next(51) + 1;
            var jackPotWin = this.random.Next(51) + 1;
            if (jackPotTry == jackPotWin)
            {
                //show a message to indicate the jackpot has been won, pay the player and reset the jackpot amount
                MessageBox.Show("You Won the $" + jackpot + " Jackpot!!", "Jackpot!!");
                playerMoney += jackpot;
                jackpot = 5000;
                JackpotLabel.Text = "JACKPOT " + jackpot.ToString();
            }
            else
            {
                //if the jackpot is not won, increase it by $100
                jackpot += 100;
                JackpotLabel.Text = "JACKPOT " + jackpot.ToString();
            }
        }

        /* Utility function to show a win message and increase player money */
        private void showWinMessage()
        {
            playerMoney += winnings;
            WiningsLabel.Text = winnings.ToString();
            
            resetFruitTally();
            checkJackPot();
        }

        /* Utility function to show a loss message and reduce player money */
        private void showLossMessage()
        {
            playerMoney -= playerBet;
            WiningsLabel.Text = "LOSE";
           
            resetFruitTally();
            jackpot += 100;
            JackpotLabel.Text = "JACKPOT " + jackpot.ToString();
        }

        /* Utility function to check if a value falls within a range of bounds */
        private bool checkRange(int value, int lowerBounds, int upperBounds)
        {
            return (value >= lowerBounds && value <= upperBounds) ? true : false;

        }

        /* When this function is called it determines the betLine results.
    e.g. Bar - Orange - Banana */
        private string[] Reels()
        {
            string[] betLine = { " ", " ", " " };
            int[] outCome = { 0, 0, 0 };

            for (var spin = 0; spin < 3; spin++)
            {
                outCome[spin] = this.random.Next(65) + 1;

                if (checkRange(outCome[spin], 1, 27))
                {  // 41.5% probability
                    betLine[spin] = "blank";
                    blanks++;
                }
                else if (checkRange(outCome[spin], 28, 37))
                { // 15.4% probability
                    betLine[spin] = "Grapes";
                    grapes++;
                }
                else if (checkRange(outCome[spin], 38, 46))
                { // 13.8% probability
                    betLine[spin] = "Banana";
                    bananas++;
                }
                else if (checkRange(outCome[spin], 47, 54))
                { // 12.3% probability
                    betLine[spin] = "Orange";
                    oranges++;
                }
                else if (checkRange(outCome[spin], 55, 59))
                { //  7.7% probability
                    betLine[spin] = "Cherry";
                    cherries++;
                }
                else if (checkRange(outCome[spin], 60, 62))
                { //  4.6% probability
                    betLine[spin] = "Bar";
                    bars++;
                }
                else if (checkRange(outCome[spin], 63, 64))
                { //  3.1% probability
                    betLine[spin] = "Bell";
                    bells++;
                }
                else if (checkRange(outCome[spin], 65, 65))
                { //  1.5% probability
                    betLine[spin] = "Seven";
                    sevens++;
                }

            }
            return betLine;
        }

        /* This function calculates the player's winnings, if any */
        private void determineWinnings()
        {
            if (blanks == 0)
            {
                if (grapes == 3)
                {
                    winnings = playerBet * 10;
                }
                else if (bananas == 3)
                {
                    winnings = playerBet * 20;
                }
                else if (oranges == 3)
                {
                    winnings = playerBet * 30;
                }
                else if (cherries == 3)
                {
                    winnings = playerBet * 40;
                }
                else if (bars == 3)
                {
                    winnings = playerBet * 50;
                }
                else if (bells == 3)
                {
                    winnings = playerBet * 75;
                }
                else if (sevens == 3)
                {
                    winnings = playerBet * 100;
                }
                else if (grapes == 2)
                {
                    winnings = playerBet * 2;
                }
                else if (bananas == 2)
                {
                    winnings = playerBet * 2;
                }
                else if (oranges == 2)
                {
                    winnings = playerBet * 3;
                }
                else if (cherries == 2)
                {
                    winnings = playerBet * 4;
                }
                else if (bars == 2)
                {
                    winnings = playerBet * 5;
                }
                else if (bells == 2)
                {
                    winnings = playerBet * 10;
                }
                else if (sevens == 2)
                {
                    winnings = playerBet * 20;
                }
                else if (sevens == 1)
                {
                    winnings = playerBet * 5;
                }
                else
                {
                    winnings = playerBet * 1;
                }
                winNumber++;
                showWinMessage();
            }
            else
            {
                lossNumber++;
                showLossMessage();
            }

        }

        /// <summary>
        /// Spin the slots. Updaet the images based on what is rolled. Determine winnings
        /// and update labels.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SpinPictureBox_Click(object sender, EventArgs e)
        {
            //check if a bet has been placed
            if (Convert.ToInt16(BetLabel.Text) <= 0)
            {
                MessageBox.Show("Please place a bet!", "No Bet!");
            }
            else
            {
                
                if (playerBet <= playerMoney)
                {
                    spinResult = Reels();
                    if(spinResult[0] == "blank")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.blank;
                    }
                    else if(spinResult[0] == "Grapes")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.grapes;
                    }
                    else if(spinResult[0] == "Banana")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.banana;
                    }
                    else if(spinResult[0] == "Orange")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.orange;
                    }
                    else if(spinResult[0] == "Cherry")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.cherry;
                    }
                    else if(spinResult[0] == "Bar")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.bar;
                    }
                    else if(spinResult[0] == "Bell")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.bell;
                    }
                    else if(spinResult[0] == "Seven")
                    {
                        FirstReelPictureBox.Image = Properties.Resources.seven;
                    }

                    if (spinResult[1] == "blank")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.blank;
                    }
                    else if (spinResult[1] == "Grapes")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.grapes;
                    }
                    else if (spinResult[1] == "Banana")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.banana;
                    }
                    else if (spinResult[1] == "Orange")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.orange;
                    }
                    else if (spinResult[1] == "Cherry")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.cherry;
                    }
                    else if (spinResult[1] == "Bar")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.bar;
                    }
                    else if (spinResult[1] == "Bell")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.bell;
                    }
                    else if (spinResult[1] == "Seven")
                    {
                        SecondReelPictureBox.Image = Properties.Resources.seven;
                    }

                    if (spinResult[2] == "blank")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.blank;
                    }
                    else if (spinResult[2] == "Grapes")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.grapes;
                    }
                    else if (spinResult[2] == "Banana")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.banana;
                    }
                    else if (spinResult[2] == "Orange")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.orange;
                    }
                    else if (spinResult[2] == "Cherry")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.cherry;
                    }
                    else if (spinResult[2] == "Bar")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.bar;
                    }
                    else if (spinResult[2] == "Bell")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.bell;
                    }
                    else if (spinResult[2] == "Seven")
                    {
                        ThirdReelPictureBox.Image = Properties.Resources.seven;
                    }

                    
                    determineWinnings();

                    turn++;
                    
                    playerBet = 0;
                    BetLabel.Text = "0";
                    TotalCreditsLabel.Text = playerMoney.ToString();
                }
            }
            
        }

        /// <summary>
        /// Call AddBet method with the amount of the bet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bet1PictureBox_Click(object sender, EventArgs e)
        {
            AddBet(1);
        }

        /// <summary>
        /// Checks if the player has the money for the desired bet and updates the bet total if the can.
        /// </summary>
        /// <param name="bet"></param>
        private void AddBet(int bet)
        {
            //convert the current BetLabel to an int
            playerBet = Convert.ToInt16(BetLabel.Text);
            int currentCredits = Convert.ToInt16(TotalCreditsLabel.Text);

            //check to ensure that the bet can be placed
            if(currentCredits - playerBet <= 0)
            {
                MessageBox.Show("You do not have enough credits", "Insufficient Funds");
            }
            else
            {
                //add the bet amount to the total bet
                playerBet = playerBet + bet;

                //subtract the bet amount from the credits total
                currentCredits = currentCredits - bet;

                //convert the new current bet and currentCredits amounts to strings and make the 
                //labels equal to the new values
                BetLabel.Text = playerBet.ToString();
                //TotalCreditsLabel.Text = currentCredits.ToString();
            }
            
        }

        /// <summary>
        /// Call the AddBet method with the amount of the bet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bet10PictureBox_Click(object sender, EventArgs e)
        {
            AddBet(10);
        }


        /// <summary>
        /// Call the AddBet method with the amount of the bet.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Bet100PictureBox_Click(object sender, EventArgs e)
        {
            AddBet(100);
        }


        /// <summary>
        /// Close the application when the power button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PowerPictureBox_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        /// <summary>
        /// Reset the game when the reset button is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetPictureBox_Click(object sender, EventArgs e)
        {
            resetAll();
        }
    }

}
