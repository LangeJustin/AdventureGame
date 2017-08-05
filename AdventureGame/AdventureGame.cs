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
    public partial class AdventureGame : Form
    {
        // Tools
        Random rand = new Random();
        List<Monster> AllMonsters = new List<Monster>();
        public Player hero;

        SoundPlayer musicPlayer;

        bool OnlyAtStart = true;        // build things only at the beginning
        bool check = true;              // Danger Icon blink
        bool mainTheme = false;         // Main music theme
        bool battleTheme = false;       // Battle music theme

        public int goldGambling;
        public int direction = 3;
        public int fadeOut = 0;         // Helps Fade out Timer for Location Scroll

        public DateTime statTimeStart = System.DateTime.Now;        // counts playing time for stats
        public int statSteps = 0;       // counts steps for stats
        public int statdmgMade = 0;         // counts dmg made by player
        public int statdmgReceived = 0;     // dounts dmg received by player
        public int statMonstersKilled = 0;  // counts dead monsters
        public int statDeaths = 0;          // counts times you died
        public int statGoldCollected = 0;
        private void Main_FormClosing(object sender, FormClosingEventArgs e)  //hinzugefügt
        {
            characterStatsToolStripMenuItem_Click(new Object(), new EventArgs());
        }

        public int playerRightCount = 0;
        public int playerLeftCount = 0;
        public int playerUpCount = 0;
        public int playerDownCount = 0;

        // Graphics
        Brush maroon = new SolidBrush(Color.Maroon);
        Brush dimgray = new SolidBrush(Color.DimGray);
        Brush gold = new SolidBrush(Color.Gold);
        Brush violet = new SolidBrush(Color.Purple);
        Brush yellowGreen = new SolidBrush(Color.YellowGreen);

        // Items
        HealingPotion HPpotion = new HealingPotion(1, "a HP Potion", "HP Potions", 50, 1);
        AttackPotion ATKpotion = new AttackPotion(2, "an ATK Potion", "ATK Potions", 5, 20, 2);
        GodModePotion GMpotion = new GodModePotion(3, "a GM Potion", "GM Potions", 6, 30, 1);

        Food Apple = new Food(4, "an Apple", "Apples", 25, 25, 1);
        Food Steak = new Food(5, "a Steak", "Steaks", 40, 40, 1);
        Food Beer = new Food(6, "Beer", "Much Drunk", 15, 65, 1);

        // Set board size and create two dimensional array
        static int GameBoardWidth = 60;
        static int GameBoardHeight = 60;
        Tile[,] boardTile = new Tile[GameBoardWidth, GameBoardHeight]; // For collision detection

        public AdventureGame()
        {
            InitializeComponent();

            // Easier to work with during development
            pictureBoxStartScreen.BringToFront();
            musicPlayer = new SoundPlayer(Properties.Resources.Ambient_campfire);
            musicPlayer.PlayLooping();
        }

        /// <summary>
        /// +++++++++++++ Start Journey ++++++++++++++++
        /// </summary>
        private void beginNewJourneyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            musicPlayer.Stop();
            musicPlayer = new SoundPlayer(Properties.Resources.LoZ_HyruleFieldTheme);
            musicPlayer.PlayLooping();

            pictureBox2.Image = Properties.Resources.verz_links;
            pictureBox3.Image = Properties.Resources.verz_rechts;
            pictureBoxStartScreen.Enabled = false;
            pictureBoxStartScreen.Visible = false;

            pictureBoxScroll.Visible = true;
            timerFadeOut.Enabled = true;

            // Forces KeyEvents on object of class AdventureGame
            this.KeyPreview = true;

            // Play main music theme
            musicPlayer.Play();
            mainTheme = true;

            hero = new Player(100, 15, 100, 0, 500, 0, 1, 12, 14); // Build main hero

            beginNewJourneyToolStripMenuItem.Enabled = false;

            drawGameBoard();
            refreshStats();
        }

        /// <summary>
        /// +++++++++++++++++++++++ Draw complete game board ++++++++++++++++++++++++
        /// </summary>
        private void drawGameBoard()
        {
            // Untergrundtiles einfügen
            drawAllSquares();
            
            // Random und feste Objekte
            drawBorderTrees();
            drawRandomTrees();

            // Spezifische Objekte
            drawForest1();
            drawStronghold1();
            drawStronghold2();

            buildLootChest();

            drawHero(direction);

            refreshStats();

            if (OnlyAtStart)
            {
                OnlyAtStart = false;
                buildMonsters();
            }
        }
        private void drawAllSquares()
        {
            for (int x = 0; x < GameBoardWidth; x++)
            {
                for (int y = 0; y < GameBoardHeight; y++)
                {
                    int tilePixLeft = 25 * x;
                    int tilePixTop = 25 * y;

                    boardTile[x, y] = new Tile(x, y, false, tilePixLeft, tilePixTop);
                }
            }
        }
       
        /// <summary>
        /// ++++++++++++++ Draw scenery +++++++++++++++++
        /// </summary>
        private void drawForest1()
        {
            Graphics treeObject = board1.CreateGraphics();
            Brush green = new SolidBrush(Color.Green);
            Brush brown = new SolidBrush(Color.Brown);
            Brush darkgreen = new SolidBrush(Color.DarkGreen);

            int x = 0;
            int y = 0;
            bool solid = true;
            int tilePixLeft = 25 * x;
            int tilePixTop = 25 * y;

            for (x = 4; x < 30; x++)
            {
                for (y = 4; y < 9; y++)
                {
                    tilePixLeft = 25 * x;
                    tilePixTop = 25 * y;

                    boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                    if (((x * y) % 10) >= 4)
                    {
                        treeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(darkgreen, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                    else
                    {
                        treeObject.FillRectangle(brown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                }
            }
            for (x = 5; x < 17; x++)
            {
                for (y = 4; y < 14; y++)
                {
                    tilePixLeft = 25 * x;
                    tilePixTop = 25 * y;

                    boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                    if (((x * y) % 10) >= 6)
                    {
                        treeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(darkgreen, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                    else
                    {
                        treeObject.FillRectangle(brown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                }
            }
            for (x = 6; x < 12; x++)
            {
                for (y = 13; y < 20; y++)
                {
                    tilePixLeft = 25 * x;
                    tilePixTop = 25 * y;

                    boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                    if (((x * y) % 10) >= 5)
                    {
                        treeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(darkgreen, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                    else
                    {
                        treeObject.FillRectangle(brown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                }
            }

            // Draw cave
            x = 12;
            y = 13;
            tilePixLeft = 25 * x;
            tilePixTop = 25 * y;

            treeObject.FillRectangle(dimgray, tilePixLeft + 2, tilePixTop + 15, 21, 10);
            treeObject.FillRectangle(dimgray, tilePixLeft + 3, tilePixTop + 9, 19, 7);
            treeObject.FillRectangle(dimgray, tilePixLeft + 5, tilePixTop + 4, 15, 7);
            treeObject.FillEllipse(new SolidBrush(Color.Black), tilePixLeft + 6, tilePixTop + 10, 12, 12);
            treeObject.FillRectangle(new SolidBrush(Color.Black), tilePixLeft + 6, tilePixTop + 15, 13, 10);
        }  
        private void drawBorderTrees()
        {
            Graphics bordertreeObject = board1.CreateGraphics();
            Brush green = new SolidBrush(Color.DarkGreen);
            Brush brown = new SolidBrush(Color.Brown);

            int x = 0;
            int y = 0;
            bool solid = true;

            for (x = 0; x < GameBoardWidth; x++)
            {
                int tilePixLeft = 25 * x;
                int tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                bordertreeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                bordertreeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
            }

            x = 0;
            for (y = 0; y < GameBoardHeight; y++)
            {
                int tilePixLeft = 25 * x;
                int tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                bordertreeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                bordertreeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                
            }

            y = 51;
            for (x = 0; x < GameBoardWidth; x++)
            {
                int tilePixLeft = 25 * x;
                int tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                bordertreeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                bordertreeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
            }

            x = 52;
            for (y = 0; y < GameBoardHeight; y++)
            {
                int tilePixLeft = 25 * x;
                int tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                bordertreeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                bordertreeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
            }
        }
        private void drawRandomTrees()
        {
            Graphics treeObject = board1.CreateGraphics();
            Brush green = new SolidBrush(Color.Green);
            Brush brown = new SolidBrush(Color.Brown);
            Brush darkgreen = new SolidBrush(Color.DarkGreen);

            int x = 0;
            int y = 0;
            bool solid = true;
            int tilePixLeft;
            int tilePixTop;

            // Fast zufällige Bäume
            for (x = 2; x < GameBoardWidth; x++)
            {
                for (y = 2; y < GameBoardHeight; y++)
                {
                    if (y == 26) // Festungseingänge freihalten!
                        y = 36;

                    if (((x * y) % 10) == 2)
                    {
                        tilePixLeft = 25 * x;
                        tilePixTop = 25 * y;
                        boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                        treeObject.FillRectangle(brown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                    if (((x * y) % 10) == 3)
                    {
                        tilePixLeft = 25 * x;
                        tilePixTop = 25 * y;
                        boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                        treeObject.FillRectangle(maroon, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(darkgreen, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                }
            }

            /* Komplett zufällige Bäume:
              
            int z = 0;

            for (x = 2; x < GameBoardWidth; x++)
            {
                for(y = 2; y < GameBoardHeight; y++)
                {
                    if (y == 26) // Festungseingänge freihalten!
                        y = 36;
                    z = rand.Next(0, 19);
                    if (z == 1)
                    {
                        int tilePixLeft = 25 * x;
                        int tilePixTop = 25 * y;
                        boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                        treeObject.FillRectangle(brown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        treeObject.FillRectangle(green, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                    if (z == 2)
                    {
                        int tilePixLeft = 25 * x;
                        int tilePixTop = 25 * y;
                        boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

                        darktreeObject.FillRectangle(darkbrown, tilePixLeft + 10, tilePixTop + 13, 5, 12);
                        darktreeObject.FillRectangle(darkgreen, tilePixLeft + 5, tilePixTop + 5, 13, 13);
                    }
                }
            }           */

        }
        private void drawStronghold1()
        {
            Graphics strongholdObject = board1.CreateGraphics();
            Brush black = new SolidBrush(Color.Black);
            Brush gray = new SolidBrush(Color.Gray);

            int tilePixLeft;
            int tilePixTop;
            bool solid = true;

            int x;
            int y = 25;

            for (int x2 = 20; x2 < 40; x2++)
            {
                for (int y2 = 25; y2 < 35; y2++)
                {
                    tilePixLeft = 25 * x2;
                    tilePixTop = 25 * y2;

                    strongholdObject.FillRectangle(new SolidBrush(Color.DimGray), tilePixLeft, tilePixTop, 25, 25);
                    strongholdObject.FillRectangle(new SolidBrush(Color.SlateGray), tilePixLeft, tilePixTop, 22, 22);
                }
            }

            for (x = 20; x < 40; x++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
            y = 35;
            for (x = 20; x < 40; x++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);

                if (x == 32)
                {
                    x = 36;
                }
            }
            x = 20;
            for (y = 25; y < 35; y++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
            x = 40;
            for (y = 25; y < 36; y++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
        }
        private void drawStronghold2()
        {
            Graphics strongholdObject = board1.CreateGraphics();
            Brush black = new SolidBrush(Color.Black);
            Brush gray = new SolidBrush(Color.Gray);

            int tilePixLeft;
            int tilePixTop;
            bool solid = true;

            int x;
            int y = 28;

            for (x = 24; x < 37; x++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
            y = 32;
            for (x = 24; x < 37; x++)
            {
                if (x == 29)
                    x = 32;
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
            x = 24;
            for (y = 28; y < 32; y++)
            {
                if (y == 30)
                {
                    y = 31;
                }
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
            x = 28;
            for (y = 28; y < 33; y++)
            {
                tilePixLeft = 25 * x;
                tilePixTop = 25 * y;

                boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);
                strongholdObject.FillRectangle(black, tilePixLeft, tilePixTop, 25, 25);
                strongholdObject.FillRectangle(gray, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            }
        }

        /// <summary>
        /// +++++++++++++++ Loot Chests +++++++++++++++++
        /// </summary>
        private void buildLootChest()
        {
            LootChest chest1 = new LootChest(HPpotion, Apple, Beer, 50, 28, 18);
            LootChest chest2 = new LootChest(HPpotion, ATKpotion, 25, 48, 10);
            LootChest chest3 = new LootChest(Apple, Steak, 100, 10, 30);
            LootChest chest4 = new LootChest(ATKpotion, Apple, Steak, 75, 47, 47);
            LootChest chest5 = new LootChest(HPpotion, Beer, 20, 2, 2);
            LootChest chestStronghold = new LootChest(GMpotion, 250, 26, 30);

            List<LootChest> AllChests = new List<LootChest>();
            AllChests.Add(chest1);
            AllChests.Add(chest2);
            AllChests.Add(chest3);
            AllChests.Add(chest4);
            AllChests.Add(chest5);
            AllChests.Add(chestStronghold);

            for(int i=0; i<AllChests.Count; i++)
            {
                drawLootChest(AllChests[i]);
            }
        }
        public void drawLootChest(LootChest chest)
        {
            Graphics chestObject = board1.CreateGraphics();
            Brush black = new SolidBrush(Color.Black);
            Brush brown = new SolidBrush(Color.Brown);
            Brush green = new SolidBrush(Color.YellowGreen);
            Brush gold = new SolidBrush(Color.Gold);

            int x = chest.xCoo;
            int y = chest.yCoo;

            bool solid = true;

            int tilePixLeft = 25 * x;
            int tilePixTop = 25 * y;

            boardTile[x, y] = new Tile(x, y, solid, tilePixLeft, tilePixTop);

            // Chests in Festungen anderer Untergrund
            if (!(chest.xCoo >= 20 && chest.xCoo <= 40 && chest.yCoo >= 25 && chest.yCoo <= 34))
                chestObject.FillRectangle(green, tilePixLeft, tilePixTop, 25, 25);
            else
            {
                chestObject.FillRectangle(new SolidBrush(Color.DimGray), tilePixLeft, tilePixTop, 25, 25);
                chestObject.FillRectangle(new SolidBrush(Color.SlateGray), tilePixLeft, tilePixTop, 22, 22);
            }
            chestObject.FillRectangle(black, tilePixLeft + 5, tilePixTop + 5, 15, 15);
            chestObject.FillRectangle(brown, tilePixLeft + 6, tilePixTop + 6, 13, 2);
            chestObject.FillRectangle(brown, tilePixLeft + 6, tilePixTop + 11, 13, 7);
            chestObject.FillRectangle(gold, tilePixLeft + 11, tilePixTop + 10, 3, 3);   
        } 

        /// <summary>
        /// ++++++++++++++++ Draw Hero ++++++++++++++++++
        /// </summary>
        public void drawHero(int d)
        {
            refreshStats();

            Graphics heroObject = board1.CreateGraphics();
            Brush gray = new SolidBrush(Color.DimGray); // helmet
            Brush black = new SolidBrush(Color.Black);
            Brush blue = new SolidBrush(Color.Blue);    // sword
            Brush white = new SolidBrush(Color.Snow);
            Brush red = new SolidBrush(Color.Red);
            Brush blue2 = new SolidBrush(Color.PowderBlue);
            Brush lightblue = new SolidBrush(Color.DodgerBlue);

            hero.left = hero.xCoo * 25;
            hero.top = hero.yCoo * 25;

            if (ATKpotion.active)
            {
                heroObject.FillEllipse(lightblue, hero.left, hero.top, 25, 25);
                heroObject.FillEllipse(blue2, hero.left, hero.top, 23, 23);
            }
            if (GMpotion.active)
            {
                heroObject.FillEllipse(black, hero.left, hero.top, 25, 25);
                heroObject.FillEllipse(white, hero.left, hero.top, 23, 23);
            }

            if (d == 1) // Up
            {
                heroObject.FillRectangle(black, hero.left + 10, hero.top + 7, 5, 16);       // body
                heroObject.FillRectangle(lightblue, hero.left + 11, hero.top + 5, 3, 16);   // *
                heroObject.FillRectangle(red, hero.left + 18, hero.top + 12, 2, 9);         // dagger
                heroObject.FillRectangle(blue, hero.left + 5, hero.top + 6, 2, 13);         // sword
                heroObject.FillRectangle(black, hero.left + 8, hero.top + 3, 8, 7);         // head
                heroObject.FillRectangle(lightblue, hero.left + 8, hero.top + 3, 7, 6);     // helmet
                heroObject.FillRectangle(violet, hero.left + 10, hero.top + 1, 4, 7);       // helmet feathers
                heroObject.FillRectangle(black, hero.left + 13, hero.top + 14, 7, 3);       // hands right
                heroObject.FillRectangle(black, hero.left + 4, hero.top + 14, 11, 3);       // hands left
            }
            if (d == 2) // Left
            {            
                heroObject.FillRectangle(black, hero.left + 10, hero.top + 7, 5, 16);       // body
                heroObject.FillRectangle(lightblue, hero.left + 11, hero.top + 5, 3, 16);   // *
                heroObject.FillRectangle(blue, hero.left + 5, hero.top + 6, 2, 13);         // sword
                heroObject.FillRectangle(black, hero.left + 8, hero.top + 3, 8, 7);         // head
                heroObject.FillRectangle(lightblue, hero.left + 12, hero.top + 2, 5, 6);    // helmet
                heroObject.FillRectangle(lightblue, hero.left + 8, hero.top + 1, 6, 2);     // *
                heroObject.FillRectangle(violet, hero.left + 9, hero.top, 6, 2);            // helmet feathers
                heroObject.FillRectangle(violet, hero.left + 14, hero.top + 2, 3, 4);       // *
                heroObject.FillRectangle(blue2, hero.left + 9, hero.top + 4, 2, 3);         // eye left
                heroObject.FillRectangle(red, hero.left + 9, hero.top + 5, 1, 1);           // *
                // heroObject.FillRectangle(red, hero.left + 9, hero.top + 8, 3, 2);        // mouth
                heroObject.FillRectangle(black, hero.left + 5, hero.top + 14, 8, 3);        // hands left
            }
            if (d == 3) // Down
            {
                heroObject.FillRectangle(black, hero.left + 10, hero.top + 7, 5, 16);
                heroObject.FillRectangle(lightblue, hero.left + 11, hero.top + 7, 3, 14);   // body
                heroObject.FillRectangle(blue, hero.left + 18, hero.top + 6, 2, 13);        // sword
                heroObject.FillRectangle(red, hero.left + 5, hero.top + 12, 2, 9);          // dagger
                heroObject.FillRectangle(black, hero.left + 8, hero.top + 3, 8, 7);         // head     
                heroObject.FillRectangle(blue2, hero.left + 9, hero.top + 4, 2, 3);         // eye left
                heroObject.FillRectangle(red, hero.left + 10, hero.top + 5, 1, 1);          // *
                heroObject.FillRectangle(blue2, hero.left + 13, hero.top + 4, 2, 3);        // eye right
                heroObject.FillRectangle(red, hero.left + 14, hero.top + 5, 1, 1);          // *
                //heroObject.FillRectangle(red, hero.left + 10, hero.top + 8, 4, 2);        // mouth
                heroObject.FillRectangle(lightblue, hero.left + 8, hero.top + 1, 8, 2);     // helmet
                heroObject.FillRectangle(lightblue, hero.left + 7, hero.top + 2, 1, 5);     // *
                heroObject.FillRectangle(lightblue, hero.left + 16, hero.top + 2, 1, 5);    // *
                heroObject.FillRectangle(violet, hero.left + 10, hero.top, 4, 2);           // helmet feathers
                heroObject.FillRectangle(black, hero.left + 13, hero.top + 14, 7, 3);       // hands right
                heroObject.FillRectangle(black, hero.left + 5, hero.top + 14, 11, 3);       // hands left
            }
            if (d == 4) // Right
            {
                heroObject.FillRectangle(black, hero.left + 10, hero.top + 7, 5, 16);       // body
                heroObject.FillRectangle(lightblue, hero.left + 11, hero.top + 5, 3, 16);   // *
                heroObject.FillRectangle(red, hero.left + 17, hero.top + 12, 2, 9);         // dagger
                heroObject.FillRectangle(black, hero.left + 8, hero.top + 3, 8, 7);         // head
                heroObject.FillRectangle(lightblue, hero.left + 7, hero.top + 2, 5, 6);     // helmet
                heroObject.FillRectangle(lightblue, hero.left + 10, hero.top + 1, 6, 2);    // *
                heroObject.FillRectangle(violet, hero.left + 9, hero.top, 6, 2);            // helmet feathers
                heroObject.FillRectangle(violet, hero.left + 7, hero.top + 2, 3, 4);        // *
                heroObject.FillRectangle(blue2, hero.left + 14, hero.top + 4, 2, 3);        // eye right
                heroObject.FillRectangle(red, hero.left + 15, hero.top + 5, 1, 1);          // *
                // heroObject.FillRectangle(red, hero.left + 13, hero.top + 8, 3, 2);       // mouth
                heroObject.FillRectangle(black, hero.left + 13, hero.top + 14, 7, 3);       // hands right
            }

        }

        /// <summary>
        /// ++++++++++++ Update Hero stats ++++++++++++++
        /// </summary>
        public void refreshStats()
        {
            // Stats Labels
            labelHitpoints.Text = hero.CurrentHP.ToString() + "/" + hero.MaxHP.ToString(); ;
            labelATK.Text = hero.CurrentATK.ToString();
            labelGold.Text = hero.Gold.ToString();
            labelExp.Text = hero.Exp.ToString() + "/100";
            labelLevel.Text = hero.Level.ToString();
            labelAmountBlock.Text = hero.Armor.ToString();

            // Item Buttons
            buttonHPpotion.Text = HPpotion.potCount.ToString();
            buttonATKpotion.Text = ATKpotion.potCount.ToString();
            buttonGMpotion.Text = GMpotion.potCount.ToString();
            buttonApple.Text = Apple.foodCount.ToString();
            buttonSteak.Text = Steak.foodCount.ToString();
            buttonBeer.Text = Beer.foodCount.ToString();

            // Stats bars
            Graphics hpbarObject = panelHP.CreateGraphics();
            Graphics expbarObject = panelExp.CreateGraphics();
            Graphics atkbarObject = panelATK.CreateGraphics();

            SolidBrush gray = new SolidBrush(Color.Gray);

            double hpbar = hero.CurrentHP * 1.85;
            double expbar = hero.Exp * 1.85;
            double atkbar = hero.CurrentATK * 1.85;

            hpbarObject.FillRectangle(gray, 0, 0, 185, 21);
            hpbarObject.FillRectangle(new SolidBrush(Color.Red), 0, 0, Convert.ToInt32(hpbar), 21);
            expbarObject.FillRectangle(gray, 0, 0, 185, 21);
            expbarObject.FillRectangle(new SolidBrush(Color.Green), 0, 0, Convert.ToInt32(expbar), 21);
            atkbarObject.FillRectangle(gray, 0, 0, 185, 21);
            atkbarObject.FillRectangle(new SolidBrush(Color.Blue), 0, 0, Convert.ToInt32(atkbar), 21);

            if (hero.MaxHP * 0.25 > hero.CurrentHP)
                lowHPalert(true);
            else
                lowHPalert(false);
        }

        /// <summary>
        /// +++++++++++++++++++++++ Movement, Controls, Events +++++++++++++++++++++++
        /// </summary>
        private void cleanSquare(int d) // Hero Footsteps 
        {
            Graphics clearTileObject = board1.CreateGraphics();
            Brush footstep = new SolidBrush(Color.OliveDrab);

            // Footprints nur auf Waldboden!
            if (!(hero.xCoo >= 20 && hero.xCoo <= 40 && hero.yCoo >= 25 && hero.yCoo <= 34))
            {
                if (d == 1 || d == 3) // Footsteps Up / Down
                {
                    if (!GMpotion.active)
                    {
                        clearTileObject.FillRectangle(yellowGreen, hero.left, hero.top, 25, 25);
                        clearTileObject.FillRectangle(footstep, hero.left + 13, hero.top + 14, 3, 5);
                        clearTileObject.FillRectangle(footstep, hero.left + 8, hero.top + 14, 3, 5);
                    }
                    else
                    {
                        clearTileObject.FillRectangle(yellowGreen, hero.left, hero.top, 25, 25);
                        clearTileObject.FillRectangle(new SolidBrush(Color.Snow), hero.left + 13, hero.top + 14, 3, 5);
                        clearTileObject.FillRectangle(new SolidBrush(Color.Snow), hero.left + 8, hero.top + 14, 3, 5);
                    }
                    
                }
                if (d == 2 || d == 4) // Footsteps Left / Right
                {
                    if (!GMpotion.active)
                    {
                        clearTileObject.FillRectangle(yellowGreen, hero.left, hero.top, 25, 25);
                        clearTileObject.FillRectangle(footstep, hero.left + 14, hero.top + 17, 5, 3);
                        clearTileObject.FillRectangle(footstep, hero.left + 14, hero.top + 12, 5, 3);
                    }
                    else
                    {
                        clearTileObject.FillRectangle(yellowGreen, hero.left, hero.top, 25, 25);
                        clearTileObject.FillRectangle(new SolidBrush(Color.Snow), hero.left + 13, hero.top + 14, 3, 5);
                        clearTileObject.FillRectangle(new SolidBrush(Color.Snow), hero.left + 8, hero.top + 14, 3, 5);
                    }

                }
            }
            else
            {
                clearTileObject.FillRectangle(new SolidBrush(Color.DimGray), hero.left, hero.top, 25, 25);
                clearTileObject.FillRectangle(new SolidBrush(Color.SlateGray), hero.left, hero.top, 22, 22);
            }
        }
        private void AdventureGame_KeyDown(object sender, KeyEventArgs e) // KeyEvents 
        {
            if (e.KeyCode == Keys.W)
                buttonUp.PerformClick();
            if (e.KeyCode == Keys.A)
                buttonLeft.PerformClick();
            if (e.KeyCode == Keys.S)
                buttonDown.PerformClick();
            if (e.KeyCode == Keys.D)
                buttonRight.PerformClick();
        }
        private void buttonUp_Click(object sender, EventArgs e)
        {
            int x = hero.xCoo;
            int y = hero.yCoo - 1;

            if (boardTile[x, y].solid == false)
            {
                direction = 1;
                cleanSquare(direction);
                hero.yCoo = hero.yCoo - 1;

                drawHero(direction);
                playerUpCount++;
                statSteps++;        //hinzugefügt
            }
            if (playerDownCount > 0)
            {
                playerDownCount -= 1;
            }

            if (playerUpCount == 3)
            {
                board1.Top = board1.Top + 75;
                playerUpCount = 0;
                drawGameBoard();
            }
            MonsterAction();
            oncePerRound();
        }
        private void buttonDown_Click(object sender, EventArgs e)
        {
            int x = hero.xCoo;
            int y = hero.yCoo + 1;

            if (boardTile[x, y].solid == false)
            {
                direction = 3;
                cleanSquare(direction);
                hero.yCoo = hero.yCoo + 1;

                drawHero(direction);
                playerDownCount++;
                statSteps++;        //hinzugefügt
            }

            if (playerUpCount > 0)
            {
                playerUpCount -= 1;
            }

            if (playerDownCount == 3)
            {
                board1.Top = board1.Top - 75;
                playerDownCount = 0;
                drawGameBoard();
            }
            MonsterAction();
            oncePerRound();
        }
        private void buttonLeft_Click(object sender, EventArgs e)
        {
            int x = hero.xCoo - 1;
            int y = hero.yCoo;

            if (boardTile[x, y].solid == false)
            {
                direction = 2;
                cleanSquare(direction);
                hero.xCoo = hero.xCoo - 1;

                drawHero(direction);
                playerLeftCount++;
                statSteps++;        //hinzugefügt
            }

            if (playerRightCount > 0)
            {
                playerRightCount -= 1;
            }

            if (playerLeftCount == 3)
            {
                board1.Left = board1.Left + 75;
                playerLeftCount = 0;
                drawGameBoard();
            }
            MonsterAction();
            oncePerRound();
        }
        private void buttonRight_Click(object sender, EventArgs e)
        {
            int x = hero.xCoo + 1;
            int y = hero.yCoo;

            if (boardTile[x, y].solid == false)
            {
                direction = 4;
                cleanSquare(direction);
                hero.xCoo = hero.xCoo + 1;

                drawHero(direction);
                playerRightCount++;
                statSteps++;        //hinzugefügt
            }

            if (playerLeftCount > 0)
            {
                playerLeftCount -= 1;
            }

            if (playerRightCount == 3)
            {
                board1.Left = board1.Left - 75;
                playerRightCount = 0;
                drawGameBoard();
            }
            MonsterAction();
            oncePerRound();
        }
        private void oncePerRound()
        {
            // ATK Potion effect
            if (ATKpotion.DurationATK > 0 && ATKpotion.active == true)
            {
                ATKpotion.DurationATK--;
            }
            else if (ATKpotion.active)
            {
                hero.CurrentATK -= ATKpotion.AmountToAdd;
                ATKpotion.DurationATK = ATKpotion.DurationATK_backup;

                labelTextLog.Text = "...";
                ATKpotion.active = false;
                getRidOfBubble();
            }

            // GM Potion effect
            if (GMpotion.DurationGM > 0 && GMpotion.active)
            {
                GMpotion.DurationGM--;
            }
            else if (GMpotion.DurationGM == 0 && GMpotion.active)
            {
                hero.CurrentATK -= GMpotion.AmountToAdd;
                hero.Armor -= GMpotion.AmountToAdd;
                GMpotion.DurationGM = GMpotion.DurationGM_backup;

                labelTextLog.Text = "...";
                GMpotion.active = false;
                getRidOfBubble();
            }
        }
        private void getRidOfBubble()
        {
            Graphics cleanGroundObject = board1.CreateGraphics();

            if (!(hero.xCoo >= 20 && hero.xCoo <= 40 && hero.yCoo >= 25 && hero.yCoo <= 34))
                cleanGroundObject.FillRectangle(yellowGreen, hero.left, hero.top, 25, 25);
            else
            {
                cleanGroundObject.FillRectangle(new SolidBrush(Color.DimGray), hero.left, hero.top, 25, 25);
                cleanGroundObject.FillRectangle(new SolidBrush(Color.SlateGray), hero.left, hero.top, 22, 22);
            }
            drawHero(direction);
        }

        /// <summary>
        /// +++++++++++ Attack-/Lootbutton ++++++++++++++
        /// </summary>
        private void buttonAttack_Click(object sender, EventArgs e) // Player Attack 
        {
            oncePerRound();

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (AllMonsters[i].xCoo - 1 <= hero.xCoo && hero.xCoo <= AllMonsters[i].xCoo + 1 && AllMonsters[i].yCoo - 1 <= hero.yCoo && hero.yCoo <= AllMonsters[i].yCoo + 1 && AllMonsters[i].Dead == false)
                {
                    statdmgMade += hero.CurrentATK; // hinzugefügt
                    AllMonsters[i].CurrentHP = AllMonsters[i].CurrentHP - hero.CurrentATK;

                    if (AllMonsters[i].CurrentHP < 0)
                    {
                        statMonstersKilled++;   // hinzugefügt
                        drawDeadMonster(AllMonsters[i]);
                        AllMonsters[i].Dead = true;
                    }
                    else
                    {
                        drawMonster(AllMonsters[i], AllMonsters[i].CurrentHP);
                    }
                        
                }
            } 
            
            MonsterAction();
            refreshStats();
        } 
        private void buttonLoot_Click(object sender, EventArgs e) // Player Loot 
        {
            oncePerRound();

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (AllMonsters[i].xCoo - 1 <= hero.xCoo && hero.xCoo <= AllMonsters[i].xCoo + 1 && AllMonsters[i].yCoo - 1 <= hero.yCoo && hero.yCoo <= AllMonsters[i].yCoo + 1)
                {
                    if (AllMonsters[i].Dead == true && AllMonsters[i].Looted == false)
                    {
                        hero.addExpPlayer(AllMonsters[i].RewardExpPoints);
                        hero.Gold += AllMonsters[i].RewardGold;

                        statGoldCollected += AllMonsters[i].RewardGold;

                        int amount = rand.Next(1, 3);
                        if (amount == 2 && rand.Next(0, 4) == 0)
                        {
                            amount = 1;
                        }

                        switch(rand.Next(1,6))
                        {
                            case 1:
                                HPpotion.potCount += amount;
                                lootTextLog(amount, HPpotion);
                                break;
                            case 2:
                                ATKpotion.potCount += amount;
                                lootTextLog(amount, HPpotion);
                                break;
                            case 3:
                                if (amount == 2) // Hard to get GM Potion
                                {
                                    GMpotion.potCount++;
                                    labelTextLog.Text = "You found " + GMpotion.Name + ".";
                                }
                                break;
                            case 4:
                                Apple.foodCount += amount;
                                lootTextLog(amount, Apple);
                                break;
                            case 5:
                                Steak.foodCount += amount;
                                lootTextLog(amount, Steak);
                                break;
                            case 6:
                                Beer.foodCount += amount;
                                lootTextLog(amount, Beer);
                                break;
                        }

                        AllMonsters[i].Looted = true;

                        MonsterAction();
                        refreshStats();
                    }
                }
            }
        }
        private void lootTextLog(int amount, Item item)
        {
            if (amount > 1)
                labelTextLog.Text = "You found " + item.NamePlural+".";
            else
                labelTextLog.Text = "You found " + item.Name+".";
            
        }

        /// <summary>
        /// ++++++++++++++ Demon Powers +++++++++++++++++
        /// </summary>
        private void buttonHireDemon_Click(object sender, EventArgs e)
        {
            if (hero.Gold >= 200)
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to hire the Demon Lords Powers for 200 Gold?", "Are you sure?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    pictureBox6.Visible = false;
                    hero.Gold -= 200;
                    labelGold.Text = hero.Gold.ToString();
                    buttonHireDemon.Enabled = false;

                }
            }
            else
            {
                MessageBox.Show("Don't ask for the Lords Powers without having 200 Gold to pay for his effort!", "You fool!");
            }
        }
        private void buttonDP1_Click(object sender, EventArgs e) // Gambling hell 
        {
            int goldBackup = hero.Gold;

            hero.Gold -= Convert.ToInt32(buttonDP1.Text);
            musicPlayer.Stop();

            Form_Gambling GamblingHell = new Form_Gambling(hero.Gold);
            GamblingHell.ShowDialog();

            hero.Gold = GamblingHell.myCurrentScore;

            hero.Gold = GamblingHell.myCurrentScore;
            if (hero.Gold - goldBackup > 0)
                statGoldCollected += hero.Gold - goldBackup;

            refreshStats();

            if (hero.MaxHP * 0.25 > hero.CurrentHP)
                musicPlayer = new SoundPlayer(Properties.Resources.LoZ_HyruleFieldTheme);
            else
                musicPlayer = new SoundPlayer(Properties.Resources.LoZ_GanondorfBattle);
            musicPlayer.PlayLooping();
        }

        /// <summary>
        /// +++++++++++++ Hero dead Event +++++++++++++++
        /// </summary>
        public void playerDead() // Player death 
        {
            statDeaths++; // hinzugefügt
            DialogResult dialogResult = MessageBox.Show("What a stupid death.\nTry again ...or die in shame.", "You are dead.", MessageBoxButtons.RetryCancel);
            characterStatsToolStripMenuItem_Click(new object(), new EventArgs());  //hinzugefügt
            if (dialogResult == DialogResult.Cancel)
            {
                System.Windows.Forms.Application.Exit();
                
            }
            else
            {
                Application.Restart();
            }
        }

        /// <summary>
        /// +++++++ Blinking icons and Fade out +++++++++
        /// </summary>
        public void lowHPalert(bool lowHP)
        {
            if (lowHP)
            {
                if (!battleTheme)
                {
                    musicPlayer = new SoundPlayer(Properties.Resources.LoZ_GanondorfBattle);
                    this.BackColor = Color.DarkRed;

                    musicPlayer.PlayLooping();
                    battleTheme = true;
                    mainTheme = false;
                }
                if (!timerLowHp.Enabled)
                    timerLowHp.Enabled = true;
            }
            else if (!lowHP)
            {
                if (!mainTheme)
                {
                    musicPlayer = new SoundPlayer(Properties.Resources.LoZ_HyruleFieldTheme);
                    this.BackColor = Color.ForestGreen;

                    musicPlayer.PlayLooping();
                    battleTheme = false;
                    mainTheme = true;

                    labelHPalert.Text = "DANGER";
                    labelHPalert.BackColor = Color.Silver;
                    labelHPalert.ForeColor = Color.DimGray;
                }

                timerLowHp.Enabled = false;
            }
        }
        private void timerLowHp_Tick(object sender, EventArgs e)
        {
            if (check)
            {
                labelHPalert.Text = "LOW HP";
                labelHPalert.BackColor = Color.Red;
                labelHPalert.ForeColor = Color.DarkRed;
                check = false;
            }
            else
            {
                labelHPalert.Text = "DANGER";
                labelHPalert.BackColor = Color.DarkRed;
                labelHPalert.ForeColor = Color.Red;
                check = true;
            }

        }
        private void timerFadeOut_Tick(object sender, EventArgs e)
        {
            if (fadeOut <= 7)
            {
                pictureBoxScroll.Width += 9;
                pictureBoxScroll.Height += 7;
                pictureBoxScroll.Left -= 4;
                pictureBoxScroll.Top -= 3;
            }
            if (fadeOut >= 42 && fadeOut <= 67)
            {
                pictureBoxScroll.Width -= 9;
                pictureBoxScroll.Height -= 7;
                pictureBoxScroll.Left += 4;
                pictureBoxScroll.Top += 3;
                drawGameBoard();
            }
            else if (fadeOut == 68)
            {
                timerFadeOut.Enabled = false;
                pictureBoxScroll.Visible = false;
                pictureBoxScroll.Enabled = false;
                drawGameBoard();
            }
            fadeOut++;
        }

        /// <summary>
        /// ++++++++++++++++++++++++++++++++++ Items +++++++++++++++++++++++++++++++++
        /// </summary>
        private void buttonHPpotion_Click(object sender, EventArgs e)
        {
            if (HPpotion.potCount > 0)
            {
                if (hero.healPlayer(HPpotion.AmountToHeal))
                {
                    HPpotion.potCount--;
                }
                else
                    MessageBox.Show("Your HP are at 100% already!");
            }
            refreshStats();
        }
        private void buttonATKpotion_Click(object sender, EventArgs e)
        {
            if (ATKpotion.potCount > 0 && ATKpotion.active == false)
            {
                ATKpotion.active = true;
                ATKpotion.DurationATK_backup = ATKpotion.DurationATK;
                ATKpotion.potCount--;

                hero.CurrentATK += ATKpotion.AmountToAdd;
                labelTextLog.Text = "You feel strong!";
            }
            else
                MessageBox.Show("You are still under the effect of Attack-Potion!\nRemaining rounds: " + ATKpotion.DurationATK);

            refreshStats();
            drawHero(direction);
        }
        private void buttonGMpotion_Click(object sender, EventArgs e)
        {
            if (GMpotion.potCount > 0 && GMpotion.active == false)
            {
                GMpotion.active = true;
                GMpotion.DurationGM_backup = GMpotion.DurationGM;
                GMpotion.potCount--;

                hero.CurrentATK += GMpotion.AmountToAdd;
                hero.Armor += GMpotion.AmountToAdd;

                labelTextLog.Text = "You feel unstoppable!";
            }
            else if(GMpotion.potCount >= 1)
                MessageBox.Show("You can still feel the power pouring through your veins!");

            refreshStats();
            drawHero(direction);
        }
        private void buttonApple_Click(object sender, EventArgs e)
        {
            if (Apple.foodCount > 0)
            {
                if (hero.healPlayer(Apple.AmountToHeal))
                {
                    hero.addExpPlayer(Apple.AmountAddExp);
                    Apple.foodCount--;
                }
                else
                    MessageBox.Show("You are at 100% HP already!");
            }
            buttonApple.Text = Apple.foodCount.ToString();
            refreshStats();
        }
        private void buttonSteak_Click(object sender, EventArgs e)
        {
            if (Steak.foodCount > 0)
            {
                if (hero.healPlayer(Apple.AmountToHeal))
                {
                    hero.addExpPlayer(Steak.AmountAddExp);
                    Steak.foodCount--;
                }
                else
                    MessageBox.Show("You are at 100% HP already!");
            }
            buttonSteak.Text = Steak.foodCount.ToString();
            refreshStats();
        }
        private void buttonBeer_Click(object sender, EventArgs e)
        {
            if (Beer.foodCount > 0)
            {
                if (hero.healPlayer(Apple.AmountToHeal))
                {
                    hero.addExpPlayer(Beer.AmountAddExp);
                    Beer.foodCount--;
                }
                else
                    MessageBox.Show("Don't drink too much!\nYou are at 100 % HP already!");
            }
            buttonBeer.Text = Beer.foodCount.ToString();
            refreshStats();
        }

        /// <summary>
        /// +++++++++++++++++++++++++++ Monster Functions ++++++++++++++++++++++++++++
        /// </summary>
        public void buildMonsters()
        {
            // chest 1
            AllMonsters.Add(new Monster(2, "Monster Squid", 10, 20, 20, 100, 100, 1, 29, 19));
            AllMonsters.Add(new Monster(3, "Monster Squid", 10, 20, 20, 100, 100, 1, 27, 18));
            AllMonsters.Add(new Monster(4, "Monster Squid", 10, 20, 20, 100, 100, 1, 28, 17));
            // chest 2
            AllMonsters.Add(new Monster(5, "Monster Squid", 10, 20, 20, 100, 100, 1, 47, 10));
            AllMonsters.Add(new Monster(6, "Monster Squid", 10, 20, 20, 100, 100, 1, 48, 9));
            // chest 3
            AllMonsters.Add(new Monster(7, "Monster Squid", 10, 20, 20, 100, 100, 1, 12, 30));
            AllMonsters.Add(new Monster(8, "Monster Squid", 10, 20, 20, 100, 100, 1, 8, 32));
            AllMonsters.Add(new Monster(9, "Monster Squid", 10, 20, 20, 100, 100, 1, 7, 28));
            AllMonsters.Add(new Monster(10, "Monster Squid", 10, 20, 20, 100, 100, 1, 9, 30));
            // chest 5
            AllMonsters.Add(new Monster(11, "Monster Squid", 10, 20, 20, 100, 100, 1, 2, 4));
            // chest stronghold
            AllMonsters.Add(new Monster(12, "Monster Squid", 10, 20, 20, 100, 100, 1, 25, 30));
            AllMonsters.Add(new Monster(13, "Monster Squid", 10, 20, 20, 100, 100, 1, 26, 34));
            AllMonsters.Add(new Monster(14, "Monster Squid", 10, 20, 20, 100, 100, 1, 33, 30));
            AllMonsters.Add(new Monster(15, "Monster Squid", 10, 20, 20, 100, 100, 1, 34, 31));
            // right plain
            AllMonsters.Add(new Monster(16, "Monster Squid", 10, 20, 20, 100, 100, 1, 46, 31));
            AllMonsters.Add(new Monster(17, "Monster Squid", 10, 20, 20, 100, 100, 1, 47, 32));

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                    drawMonster(AllMonsters[i], AllMonsters[i].CurrentHP);
            }
        }
        private void drawMonster(Monster monster, int hp)
        {
            Graphics monsterObject = board1.CreateGraphics();
            Brush darkRed = new SolidBrush(Color.DarkRed);
            Brush darkBlue = new SolidBrush(Color.DodgerBlue);
            Brush black = new SolidBrush(Color.Black);
            Brush white = new SolidBrush(Color.White);

            monster.left = monster.xCoo * 25;
            monster.top = monster.yCoo * 25;

            clearMonster(monster); // Clean Tile drawn under monster --> Makes HP bar work
           
            monsterObject.FillRectangle(darkBlue, monster.left + 2, monster.top + 1, hp/5, 2);     // HP bar
            monsterObject.FillRectangle(darkRed, monster.left + 5, monster.top + 5, 15, 15);   // main body
            monsterObject.FillRectangle(darkRed, monster.left + 5, monster.top + 15, 2, 8);    // feet
            monsterObject.FillRectangle(darkRed, monster.left + 8, monster.top + 15, 2, 8);    // *
            monsterObject.FillRectangle(darkRed, monster.left + 11, monster.top + 15, 3, 8);   // *
            monsterObject.FillRectangle(darkRed, monster.left + 15, monster.top + 15, 2, 8);   // *
            monsterObject.FillRectangle(darkRed, monster.left + 18, monster.top + 15, 2, 8);   // *
            monsterObject.FillRectangle(white, monster.left + 7, monster.top + 8, 4, 4);       // eye left
            monsterObject.FillRectangle(black, monster.left + 8, monster.top + 8, 2, 3);       // *
            monsterObject.FillRectangle(white, monster.left + 14, monster.top + 8, 4, 4);      // eye left
            monsterObject.FillRectangle(black, monster.left + 15, monster.top + 8, 2, 3);      // * *
            //monsterObject.FillRectangle(black, monster.left + 7, monster.top + 14, 12, 5);     // mouth
            monsterObject.FillRectangle(new SolidBrush(Color.Black), monster.left + 9, monster.top + 14, 8, 2);    // teeth
            monsterObject.FillRectangle(new SolidBrush(Color.Black), monster.left + 8, monster.top + 15, 2, 3);    // *
            monsterObject.FillRectangle(new SolidBrush(Color.Black), monster.left + 16, monster.top + 15, 2, 3);   // *
        }
        private void drawDeadMonster(Monster monster)
        {
            Graphics deadMonsterObject = board1.CreateGraphics();
            Brush darkRed = new SolidBrush(Color.DarkRed);
            Brush darkBlue = new SolidBrush(Color.Navy);
            Brush olive = new SolidBrush(Color.Olive);
            Brush black = new SolidBrush(Color.Black);
            Brush green = new SolidBrush(Color.Fuchsia);

            monster.left = monster.xCoo * 25;
            monster.top = monster.yCoo * 25;

            clearMonster(monster);

            deadMonsterObject.FillRectangle(darkRed, monster.left + 5, monster.top + 5, 15, 15);    // main body
            deadMonsterObject.FillRectangle(darkRed, monster.left + 5, monster.top + 15, 2, 8);     // feet
            deadMonsterObject.FillRectangle(darkRed, monster.left + 8, monster.top + 15, 2, 8);     // *
            deadMonsterObject.FillRectangle(darkRed, monster.left + 11, monster.top + 15, 3, 8);    // *
            deadMonsterObject.FillRectangle(darkRed, monster.left + 15, monster.top + 15, 2, 8);    // *
            deadMonsterObject.FillRectangle(darkRed, monster.left + 18, monster.top + 15, 2, 8);    // *
            deadMonsterObject.FillRectangle(black, monster.left + 8, monster.top + 5, 2, 7);        // eye cross left
            deadMonsterObject.FillRectangle(black, monster.left + 6, monster.top + 8, 6, 2);        // *
            deadMonsterObject.FillRectangle(black, monster.left + 15, monster.top + 5, 2, 7);       // eye cross left
            deadMonsterObject.FillRectangle(black, monster.left + 13, monster.top + 8, 6, 2);       // *
            deadMonsterObject.FillRectangle(black, monster.left + 7, monster.top + 14, 12, 5);      // mouth
            deadMonsterObject.FillRectangle(darkRed, monster.left + 9, monster.top + 16, 8, 1);     // teeth
            deadMonsterObject.FillEllipse(green, monster.left + 12, monster.top + 17, 4, 4);       // blood
            deadMonsterObject.FillEllipse(green, monster.left + 2, monster.top + 12, 3, 4);       // blood
            deadMonsterObject.FillEllipse(green, monster.left + 10, monster.top + 10, 3, 3);       // blood
            deadMonsterObject.FillEllipse(green, monster.left + 20, monster.top + 10, 3, 4);       // blood
            deadMonsterObject.FillEllipse(green, monster.left + 3, monster.top + 3, 4, 4);       // blood
        }

        /// <summary>
        /// +++++++++++++ Monster Actions +++++++++++++++
        /// </summary>
        public void MonsterAction()
        {
            int i = 0;

            for (i = 0; i < AllMonsters.Count; i++)
            {
                // Monsters next to Hero attack (<= 1 tile away)
                if (AllMonsters[i].xCoo - 1 <= hero.xCoo && hero.xCoo <= AllMonsters[i].xCoo + 1 &&
                AllMonsters[i].yCoo - 1 <= hero.yCoo && hero.yCoo <= AllMonsters[i].yCoo + 1 && AllMonsters[i].Dead == false)
                {
                    statdmgReceived += AllMonsters[i].CurrentDMG; // hinzugefügt
                    AllMonsters[i].attackHero(hero);
                    refreshStats();
                    
                        // Display Danger Symbol at 25% HP
                        if (hero.MaxHP*0.25 > hero.CurrentHP)
                            lowHPalert(true);
                        else
                            lowHPalert(false);                            

                    if (hero.CurrentHP < 1)
                    {
                        playerDead();
                    }

                }
                // Monsters near Hero move towards him (<= 6 tiles away)
                else if (AllMonsters[i].xCoo - 6 <= hero.xCoo && hero.xCoo <= AllMonsters[i].xCoo + 6 &&
                     AllMonsters[i].yCoo - 6 <= hero.yCoo && hero.yCoo <= AllMonsters[i].yCoo + 6 && AllMonsters[i].Dead == false)
                {

                    clearMonster(AllMonsters[i]);

                    // Random if monster moves or not
                    int monsterMayMove = rand.Next(1, 3);

                    if (monsterMayMove == 1)
                    {
                        if (AllMonsters[i].yCoo > hero.yCoo)
                        {
                            monsterMoveUp(AllMonsters[i]);
                        }
                        if (AllMonsters[i].yCoo < hero.yCoo)
                        {
                            monsterMoveDown(AllMonsters[i]);
                        }
                    }
                    if (monsterMayMove == 2)
                    {
                        if (AllMonsters[i].xCoo > hero.xCoo)
                        {
                            monsterMoveLeft(AllMonsters[i]);
                        }
                        if (AllMonsters[i].xCoo < hero.xCoo)
                        {
                            monsterMoveRight(AllMonsters[i]);
                        }
                    }
                }
                if (AllMonsters[i].Dead == false)
                    drawMonster(AllMonsters[i], AllMonsters[i].CurrentHP);
                else
                    drawDeadMonster(AllMonsters[i]);
            }
        }
        public void monsterMoveUp(Monster currentMonster)
        {
            bool taken = false;

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (currentMonster.yCoo - 1 == AllMonsters[i].yCoo && currentMonster.xCoo == (AllMonsters[i].xCoo))
                {
                    taken = true;
                }
            }

            if (boardTile[currentMonster.xCoo, currentMonster.yCoo - 1].solid == false && taken == false)
            {
                currentMonster.yCoo = currentMonster.yCoo - 1;
            }
        }
        public void monsterMoveDown(Monster currentMonster)
        {
            bool taken = false;

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (currentMonster.yCoo + 1 == AllMonsters[i].yCoo && currentMonster.xCoo == (AllMonsters[i].xCoo))
                {
                    taken = true;
                }
            }

            if (boardTile[currentMonster.xCoo, currentMonster.yCoo + 1].solid == false && taken == false)
            {
                currentMonster.yCoo = currentMonster.yCoo + 1;
            }
        }
        public void monsterMoveLeft(Monster currentMonster)
        {
            bool taken = false;

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (currentMonster.xCoo - 1 == AllMonsters[i].xCoo && currentMonster.yCoo == (AllMonsters[i].yCoo))
                {
                    taken = true;
                }
            }

            if (boardTile[currentMonster.xCoo - 1, currentMonster.yCoo].solid == false && taken == false)
            {
                currentMonster.xCoo = currentMonster.xCoo - 1;
            }
        }
        public void monsterMoveRight(Monster currentMonster)
        {
            bool taken = false;

            for (int i = 0; i < AllMonsters.Count; i++)
            {
                if (currentMonster.xCoo + 1 == AllMonsters[i].xCoo && currentMonster.yCoo == (AllMonsters[i].yCoo))
                {
                    taken = true;
                }
            }

            if (boardTile[currentMonster.xCoo + 1, currentMonster.yCoo].solid == false && taken == false)
            {
                currentMonster.xCoo = currentMonster.xCoo + 1;
            }
        }
        private void clearMonster(Monster monster) // Monster Footsteps 
        {
            Graphics clearTileObject = board1.CreateGraphics();
            Brush yellowGreen = new SolidBrush(Color.YellowGreen);
            Brush footstep = new SolidBrush(Color.OliveDrab);

            monster.left = monster.xCoo * 25;
            monster.top = monster.yCoo * 25;

            // Footprints nur auf Waldboden!
            if (!(monster.xCoo >= 20 && monster.xCoo <= 40 && monster.yCoo >= 25 && monster.yCoo <= 34))
            {
                clearTileObject.FillRectangle(yellowGreen, monster.left, monster.top, 25, 25);
                clearTileObject.FillEllipse(footstep, monster.left + 5, monster.top + 5, 3, 3);
                clearTileObject.FillEllipse(footstep, monster.left + 9, monster.top + 9, 5, 5);
                clearTileObject.FillEllipse(footstep, monster.left + 16, monster.top + 6, 3, 3);
                clearTileObject.FillEllipse(footstep, monster.left + 14, monster.top + 17, 3, 3);
                clearTileObject.FillEllipse(footstep, monster.left + 7, monster.top + 14, 3, 3);
            }
            else
            {
                clearTileObject.FillRectangle(new SolidBrush(Color.DimGray), monster.left, monster.top, 25, 25);
                clearTileObject.FillRectangle(new SolidBrush(Color.SlateGray), monster.left, monster.top, 22, 22);
            }
        }

        private void characterStatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime statTimeEnd = DateTime.Now;
            int timeSpanMin = (int)(statTimeEnd - statTimeStart).TotalMinutes;
            int timeSpanSec = (int)(statTimeEnd - statTimeStart).TotalSeconds;
            Statistics statistics = new Statistics(timeSpanMin, timeSpanSec, statSteps, statdmgMade, statdmgReceived, statMonstersKilled, statDeaths);
            statistics.Show();
            statSteps = 0;       
            statdmgMade = 0;         
            statdmgReceived = 0;     
            statMonstersKilled = 0;  
            statDeaths = 0;          
            statGoldCollected = 0;
    }
    }
}
