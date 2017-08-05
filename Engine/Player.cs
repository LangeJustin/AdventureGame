using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public class Player
    {
        private int _CurrentHP;
        private int _MaxHP = 100;
        private int _CurrentATK;
        private int _Exp;
        private int _Level;
        private int _Armor;
        public int Gold = 0;

        public int left;
        public int top;

        public int xCoo;
        public int yCoo;

        public int MaxHP { get { return _MaxHP; } set { _MaxHP = value; } }
        public int CurrentHP
        {
            get
            {
                return _CurrentHP;
            }
            set
            {
                _CurrentHP = value;
                if (_CurrentHP > MaxHP)
                    _CurrentHP = _MaxHP;
                if (_CurrentHP < 0)
                    _CurrentHP = 0;
            }
        }
        public int CurrentATK { get { return _CurrentATK; } set { _CurrentATK = value; } }
        public int Armor{ get { return _Armor; } set { _Armor = value;} }
        public int Exp { get { return _Exp; } set { _Exp = value; } }
        public int Level { get { return _Level; } set { _Level = value; } }

        public Player(int currentHP, int currentATK, int maxHP, int armor, int gold, int exp, int level, int x, int y)
        {
            _CurrentHP = currentHP;
            _CurrentATK = currentATK;
            _Armor = armor;
            Gold = gold;
            _Exp = exp;
            _Level = level;
            xCoo = x;
            yCoo = y;
        }
        
        // Eventfunctions
        public bool healPlayer(int heal)
        {
            if (!(this.CurrentHP == MaxHP))
            {
                this.CurrentHP += heal;
                return true;
            }
            else
            {
                return false;
            }
        }
        public void addExpPlayer(int exp)
        {
            this.Exp += exp;
            if (Exp >= 100)
            {
                levelUp();
                Exp = Exp - 100;
            }
        }
        public void levelUp()
        {
            this.Level++;
            MaxHP += 10;
            CurrentATK += 2;
        }
    }
}
