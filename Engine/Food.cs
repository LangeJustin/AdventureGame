using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Food : Item
    {
        public int AmountToHeal;
        public int AmountAddExp;
        public int foodCount;

        public Food(int id, string name, string namePlural, int amountToHeal, int amountAddExp, int FoodCount) : base(id, name, namePlural)
        {
            AmountToHeal = amountToHeal;
            AmountAddExp = amountAddExp;
            foodCount = FoodCount;
        }
    }
}
