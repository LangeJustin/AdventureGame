using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class GodModePotion : Item
    {
        public int DurationGM;
        public int DurationGM_backup;
        public int AmountToAdd;

        public int potCount;

        public bool active = false;

        public GodModePotion(int id, string name, string namePlural, int durationGM, int amountToAdd, int PotCount) : base(id, name, namePlural)
        {
            DurationGM = durationGM;
            AmountToAdd = amountToAdd;
            potCount = PotCount;
        }
    }
}
