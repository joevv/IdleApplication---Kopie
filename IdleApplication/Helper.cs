using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleApplication
{
    class Helper : Object
    {
        private int helperNo;
        private bool isActive;
        private bool bought;
        private int level = 0;
        private string name;
        private double value;
        private double price;
        private double priceFactor;
        private string type;
        private Upgrade[] upgrades = new Upgrade[5];

        public Helper(int helperNo, string name, double value, double price, double priceFactor, string type, Upgrade[] Upgrades)
        {
            this.helperNo = helperNo;
            this.name = name;
            this.value = value;
            this.price = price;
            this.priceFactor = priceFactor;
            this.type = type;
            this.upgrades = Upgrades;
        }

        public Upgrade[] getUpgrades()
        {
            return this.upgrades;
        }

        public void setLevel(int lv)
        {
            this.level = lv;
        }

        public string getType()
        {
            return type;
        }

        public double getPriceFactor()
        {
            return priceFactor;
        }

        public double getPrice()
        {
            return price;
        }

        public void setPrice(double price)
        {
            this.price = price;
        }

        public void setName(string n)
        {
            name = n;
        }

        public string getName()
        {
            return name;
        }

        public void setBought(bool b)
        {
            if (b)
            {
                bought = true;
            }
            else
            {
                bought = false;
            }
        }

        public bool getBought()
        {
            return bought;
        }

        public double getValue()
        {
            return value;
        }

        public void setValue(double value)
        {
            this.value = value;
        }

        public void setIsActive(bool a)
        {
            if (a)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }

        public void incLevel()
        {
            level++;
        }

        public int getLevel()
        {
            return level;
        }

        public bool getIsActive()
        {
            return isActive;
        }

        public int getHelperNo()
        {
            return helperNo;
        }
    }
}
