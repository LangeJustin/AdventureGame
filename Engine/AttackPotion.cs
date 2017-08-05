using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class AttackPotion : Item
    {
        public int DurationATK;
        public int DurationATK_backup;
        public int AmountToAdd;

        public int potCount;

        public bool active = false;

        public AttackPotion(int id, string name, string namePlural, int durationATK, int amountToAdd, int PotCount) : base(id, name, namePlural)
        {
            DurationATK = durationATK;
            AmountToAdd = amountToAdd;
            potCount = PotCount;
        }
    }
}
