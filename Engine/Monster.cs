using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Monster
    {
        public int ID;
        public string Name;
        public int CurrentHP;
        public int CurrentDMG;

        public int RewardExpPoints;
        public int RewardGold;

        public int Level = 1;

        public bool Dead;
        public bool Looted;

        public int left;
        public int top;

        public int xCoo;
        public int yCoo;

        public Monster(int id, string name, int currentDMG, int rewardExpPoints, int rewardGold, int currentHP, int maxHP, int level, int x, int y)
        {
            ID = id;
            Name = name;
            CurrentDMG = currentDMG;
            CurrentHP = currentHP;
            RewardExpPoints = rewardExpPoints;
            RewardGold = rewardGold;
            Level = level;

            xCoo = x;
            yCoo = y;

            Dead = false;
            Looted = false;
        }
        public void attackHero(Player hero)
        {
            if (this.CurrentDMG - hero.Armor >= 0) // Maybe block full DMG amount
                hero.CurrentHP = hero.CurrentHP - (this.CurrentDMG - hero.Armor);
        }
    }
}
