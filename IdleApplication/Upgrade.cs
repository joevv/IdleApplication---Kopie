using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleApplication
{
    class Upgrade
    {
        private string name;
        private string type;
        private double value;
        private double price;
        private bool bought = false;

        public Upgrade(string name, string type, double value, double price)
        {
            this.name = name;
            this.type = type;
            this.value = value;
            this.price = price;
        }

        public string getName(){
            return name;
        }

        public void setBought()
        {
            bought = true;
        }

        public bool getBought()
        {
            return bought;
        }

        public string getType()
        {
            return type;
        }

        public double getValue()
        {
            return value;
        }
        public double getPrice()
        {
            return price;
        }
    }
}
