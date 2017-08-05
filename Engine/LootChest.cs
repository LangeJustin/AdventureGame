using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class LootChest
    {
        Item[] lootInside = new Item[3];
        public int GoldInside = 0;

        public int xCoo;
        public int yCoo;

        public LootChest(Item a, Item b, Item c, int gold, int x, int y)
        {
            lootInside[0] = a;
            lootInside[1] = b;
            lootInside[2] = c;
            GoldInside = gold;
            xCoo = x;
            yCoo = y;
        }
        public LootChest(Item a, Item b, int gold, int x, int y)
        {
            lootInside[0] = a;
            lootInside[1] = b;
            lootInside[2] = null;
            GoldInside = gold;
            xCoo = x;
            yCoo = y;
        }
        public LootChest(Item a, int gold, int x, int y)
        {
            lootInside[0] = a;
            lootInside[1] = null;
            lootInside[2] = null;
            GoldInside = gold;
            xCoo = x;
            yCoo = y;
        }
    }
}
