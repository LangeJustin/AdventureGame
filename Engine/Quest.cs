﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Quest
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RewardExpPoints { get; set; }
        public int RewardGold { get; set; }

        public Quest(int id, string name, string description, int rewardExp, int rewardGold)
        {
            ID = id;
            Name = name;
            rewardExp = RewardExpPoints;
            rewardGold = RewardGold;
        }
    }
}
