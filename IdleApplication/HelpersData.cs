using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleApplication
{
    class HelpersData
    {
        private static int size = 7;
        private Helper[] helpers = new Helper[size];
        

        public HelpersData()
        {
            Helper h0 = new Helper(0, "Click Upgrade", 1, 10, 2.5, "clicker", addUpgrades(0));
            Helper h1 = new Helper(1, "simple Worker", 1, 25, 2.5, "worker", addUpgrades(1));
            Helper h2 = new Helper(2, "advanced Worker", 3, 50, 2.5, "worker", addUpgrades(2));
            Helper h3 = new Helper(3, "expert Worker", 7, 100, 2.5, "worker", addUpgrades(3));
            Helper h4 = new Helper(4, "pro Worker", 18, 250, 2.5, "worker", addUpgrades(4));
            Helper h5 = new Helper(5, "star Worker", 25, 450, 2.5, "worker", addUpgrades(5));
            Helper h6 = new Helper(6, "pro Worker", 37, 600, 2.5, "worker", addUpgrades(6));

            helpers[0] = h0;
            helpers[1] = h1;
            helpers[2] = h2;
            helpers[3] = h3;
            helpers[4] = h4;
            helpers[5] = h5;
            helpers[6] = h6;
        }

        public Helper[] getHelpers()
        {
            return helpers;
        }

        private Upgrade[] addUpgrades(int no)
        {
            Upgrade[] upgrades = new Upgrade[5];
            switch (no)
            {
                case 0:
                    upgrades[0] = new Upgrade("SubUp1_0", "critClick", 10, 50);
                    upgrades[1] = new Upgrade("SubUp2_0", "critClick", 5, 100);
                    upgrades[2] = new Upgrade("SubUp3_0", "critClick", 5, 150);
                    upgrades[3] = new Upgrade("SubUp4_0", "critClick", 5, 200);
                    upgrades[4] = new Upgrade("SubUp5_0", "critValue", 0.1, 350);
                    break;
                case 1:
                    upgrades[0] = new Upgrade("SubUp1_1", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_1", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_1", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_1", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_1", "critValue", 0.05, 600);
                    break;
                case 2:
                    upgrades[0] = new Upgrade("SubUp1_2", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_2", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_2", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_2", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_2", "critDps", 1, 600);
                    break;
                case 3:
                    upgrades[0] = new Upgrade("SubUp1_3", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_3", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_3", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_3", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_3", "critDps", 1, 600);
                    break;
                case 4:
                    upgrades[0] = new Upgrade("SubUp1_4", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_4", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_4", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_4", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_4", "critDps", 1, 600);
                    break;
                case 5:
                    upgrades[0] = new Upgrade("SubUp1_5", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_5", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_5", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_5", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_5", "critDps", 1, 600);
                    break;
                case 6:
                    upgrades[0] = new Upgrade("SubUp1_6", "critDps", 1, 50);
                    upgrades[1] = new Upgrade("SubUp2_6", "critDps", 1, 150);
                    upgrades[2] = new Upgrade("SubUp3_6", "critDps", 1, 300);
                    upgrades[3] = new Upgrade("SubUp4_6", "critDps", 1, 450);
                    upgrades[4] = new Upgrade("SubUp5_6", "critDps", 1, 600);
                    break;
            }

            return upgrades;
        }
    }
}
