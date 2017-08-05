using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Weapon : Item
    {
        public int minDMG { get; set; }
        public int maxDMG { get; set; }

        public Weapon(int id, string name, string namePlural, int minDMG2, int maxDMG2) : base(id, name, namePlural)
        {
            minDMG = minDMG2;
            maxDMG = maxDMG2;
        }
    }
}
