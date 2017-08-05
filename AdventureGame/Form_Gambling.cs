using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Media;
using Engine;

namespace AdventureGame
{
    public partial class Form_Gambling : Form
    {
        Random rand = new Random();

        List<int> Dice = new List<int>();

        public int myCurrentScore;
        int enCurrentScore;
        int stake;
        bool hole = false;

        /*
        private int _dice1;
        private int _dice2;
        private int _dice3;
        private int _dice4;

        public int dice1
        {
            get { return _dice1; }
            set
            {
                if (value >= 1 && value <= 6)
                    _dice1 = value;
            }
        }
        public int dice2
        {
            get { return _dice2; }
            set
            {
                if (value >= 1 && value <= 6)
                    _dice2 = value;
            }
        }
        public int dice3
        {
            get { return _dice3; }
            set
            {
                if (value >= 1 && value <= 6)
                    _dice3 = value;
            }
        }
        public int dice4
        {
            get { return _dice4; }
            set
            {
                if (value >= 1 && value <= 6)
                    _dice4 = value;
            }
        }
        */

        public Form_Gambling(int heroScore)
        {
            InitializeComponent();

            labelMyScore.Text = heroScore.ToString();
            myCurrentScore = heroScore;
            labelEnemyScore.Text = "500";

            pictureBoxDice1.Visible = false;
            pictureBoxDice2.Visible = false;
            pictureBoxDice3.Visible = false;
            pictureBoxDice4.Visible = false;
            pictureBoxResult.Visible = false;

            SoundPlayer musicPlayer = new SoundPlayer(Properties.Resources.BeverlyHillsCop_Theme);
            musicPlayer.PlayLooping();
        }

        private void buttonRoll_Click(object sender, EventArgs e)
        {
            try
            {
                pictureBoxDice1.Visible = true;
                pictureBoxDice2.Visible = true;
                pictureBoxDice3.Visible = true;
                pictureBoxDice4.Visible = true;
                pictureBoxResult.Visible = true;

                myCurrentScore = Convert.ToInt32(labelMyScore.Text);
                enCurrentScore = Convert.ToInt32(labelEnemyScore.Text);

                if (hole) // Hole effect active
                {
                    if (myCurrentScore - (Convert.ToInt32(textBoxStake.Text) * 5) >= 0)
                    {
                        stake = Convert.ToInt32(textBoxStake.Text) * 4;
                        MessageBox.Show("The Hole quadrupled your Stake!\n New Stake: " + stake, "Oh....shit....");
                    }
                    else
                    {
                        stake = myCurrentScore;
                    }
                    hole = false;
                }

                else
                {
                    stake = Convert.ToInt32(textBoxStake.Text);
                }

                if ((myCurrentScore - stake) >= 0 && (enCurrentScore - stake) >= 0)
                {
                    myCurrentScore -= stake;
                    labelMyScore.Text = myCurrentScore.ToString();

                    enCurrentScore -= stake;
                    labelEnemyScore.Text = enCurrentScore.ToString();

                    Dice.Add(0);
                    Dice.Add(0);
                    Dice.Add(0);
                    Dice.Add(0);

                    for (int i = 0; i < 4; i++)
                    {
                        Dice[i] = rand.Next(1, 6);
                    }

                    string check = calculateResult(Dice);

                    if (check == "win")
                    {
                        if (rand.Next(0, 21) == 5)
                        {
                            pictureBoxResult.Image = Properties.Resources.icon_jackpot;
                            myCurrentScore += stake * 5;
                        }   
                        else
                        {
                            pictureBoxResult.Image = Properties.Resources.icon_youwin;
                            myCurrentScore += stake * 2;
                        }
                    }
                    else if (check == "loose")
                    {
                        pictureBoxResult.Image = Properties.Resources.icon_youloose;
                        enCurrentScore += stake * 2;
                    }
                    else if (check == "draw")
                    {
                        if (rand.Next(0, 8) == 5)
                        {
                            pictureBoxResult.Image = Properties.Resources.icon_hole;
                            myCurrentScore += stake;
                            enCurrentScore += stake;
                            hole = true;
                        }
                        else
                        {
                            pictureBoxResult.Image = Properties.Resources.icon_draw;
                            if (myCurrentScore + stake - 10 < 0)
                                myCurrentScore = 0;
                            else
                                myCurrentScore += stake - 10;
                            enCurrentScore += stake;
                        }
                        
                    }

                    labelMyScore.Text = myCurrentScore.ToString();
                    labelEnemyScore.Text = enCurrentScore.ToString();
                }
                else
                {
                    MessageBox.Show("No Gambling without Gold!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        private string calculateResult(List<int> Dice)
        {
            switch (Dice[0])
            {
                case 1:
                    pictureBoxDice1.Image = Properties.Resources.Dice_1;
                    break;
                case 2:
                    pictureBoxDice1.Image = Properties.Resources.Dice_2;
                    break;
                case 3:
                    pictureBoxDice1.Image = Properties.Resources.Dice_3;
                    break;
                case 4:
                    pictureBoxDice1.Image = Properties.Resources.Dice_4;
                    break;
                case 5:
                    pictureBoxDice1.Image = Properties.Resources.Dice_5;
                    break;
                case 6:
                    pictureBoxDice1.Image = Properties.Resources.Dice_5;
                    break;
            }
            switch (Dice[1])
            {
                case 1:
                    pictureBoxDice2.Image = Properties.Resources.Dice_1;
                    break;
                case 2:
                    pictureBoxDice2.Image = Properties.Resources.Dice_2;
                    break;
                case 3:
                    pictureBoxDice2.Image = Properties.Resources.Dice_3;
                    break;
                case 4:
                    pictureBoxDice2.Image = Properties.Resources.Dice_4;
                    break;
                case 5:
                    pictureBoxDice2.Image = Properties.Resources.Dice_5;
                    break;
                case 6:
                    pictureBoxDice2.Image = Properties.Resources.Dice_5;
                    break;
            }
            switch (Dice[2])
            {
                case 1:
                    pictureBoxDice3.Image = Properties.Resources.Dice_1;
                    break;
                case 2:
                    pictureBoxDice3.Image = Properties.Resources.Dice_2;
                    break;
                case 3:
                    pictureBoxDice3.Image = Properties.Resources.Dice_3;
                    break;
                case 4:
                    pictureBoxDice3.Image = Properties.Resources.Dice_4;
                    break;
                case 5:
                    pictureBoxDice3.Image = Properties.Resources.Dice_5;
                    break;
                case 6:
                    pictureBoxDice3.Image = Properties.Resources.Dice_5;
                    break;
            }
            switch (Dice[3])
            {
                case 1:
                    pictureBoxDice4.Image = Properties.Resources.Dice_1;
                    break;
                case 2:
                    pictureBoxDice4.Image = Properties.Resources.Dice_2;
                    break;
                case 3:
                    pictureBoxDice4.Image = Properties.Resources.Dice_3;
                    break;
                case 4:
                    pictureBoxDice4.Image = Properties.Resources.Dice_4;
                    break;
                case 5:
                    pictureBoxDice4.Image = Properties.Resources.Dice_5;
                    break;
                case 6:
                    pictureBoxDice4.Image = Properties.Resources.Dice_5;
                    break;
            }
            if ((Dice[0] + Dice[1]) < (Dice[2] + Dice[3]))
            {
                return "win";
            }
            else if ((Dice[0] + Dice[1]) > (Dice[2] + Dice[3]))
            {
                return "loose";
            }   
            else
            {
                return "draw";
            }  
        }

        public int returnGold()
        {
            return myCurrentScore;
        }
    }
}
