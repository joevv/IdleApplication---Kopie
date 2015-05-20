using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdleApplication
{
    class Sound
    {
        private bool muteSound = false;

        public void soundSwitch()
        {
            if (!muteSound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Swoosh);
                player.Play();
            }
        }

        public void soundUpgrade()
        {
            if (!muteSound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Upgrade);
                player.Play();
            }
        }

        public void soundClick()
        {
            Console.WriteLine(muteSound);
            if (!muteSound)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer(Properties.Resources.Click);

            }

        }

        public void mute()
        {
            muteSound = true;
        }

        public void demute()
        {
            muteSound = false;
        }

        public bool getmute()
        {
            return muteSound;
        }
    }
}
