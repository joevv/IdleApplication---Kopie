using Microsoft.Win32;
using IdleApplication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IdleApplication
{
    public partial class MainWindow : Window
    {
        private double money = 0;
        private Helper[] helpers;
        private static Brush clr_bought = Brushes.Orange;
        private Thread t;
        private double value;
        private double critClick = 0;
        private double critDps = 0;
        private double clickValue = 1;
        private double critValue = 1;
        private DoubleAnimation AnimationMoney1 = new DoubleAnimation();
        private DoubleAnimation AnimationMoney2 = new DoubleAnimation();
        private DoubleAnimation AnimationButtonHeight = new DoubleAnimation();
        private DoubleAnimation AnimationButtonWidth = new DoubleAnimation();
        private DoubleAnimation AnimationCrit = new DoubleAnimation();
        SettingsScreen sScreen = new SettingsScreen();
        GameScreen gScreen = new GameScreen();
        StartScreen stScreen = new StartScreen();
        Sound s = new Sound();
        TimeSpan ts = new TimeSpan();

        public MainWindow()
        {
            InitializeComponent();
            this.Content = stScreen;
            gScreen.btn_manual.Click += this.btn_click;
            gScreen.btn_settings.Click += this.btn_settings_Click;
            stScreen.btn_NewGame.Click += this.btnStartNew_click;
            stScreen.btn_load.Click += this.btnLoad_click;
            sScreen.btn_return.Click += this.btnReturn_click;
            sScreen.btn_save.Click += this.btnSave_click;
            sScreen.btn_open.Click += this.btnLoad_click;
            sScreen.btn_sound_demute.Click += this.btnSoundClick;
            sScreen.btn_sound_mute.Click += this.btnSoundClick;
            sScreen.btn_restart.Click += this.btnRestart_click;
            InitializeHelpers();
            InitializeAnimation();
            ts = new TimeSpan(0, 0, 0, 0, (int)sScreen.sld_speed.Value*100);
            t = new Thread(this.incMoney);
            sScreen.sld_speed.ValueChanged += this.slider_changed;
            t.Start();
            this.Closing += new CancelEventHandler(OnClosing);
        }

        private void slider_changed(object sender, RoutedEventArgs e)
        {
            ts = new TimeSpan(0, 0, 0, 0, (int)sScreen.sld_speed.Value*100);
                sScreen.lbl_speedValue.Content = ((double)10 / (int)sScreen.sld_speed.Value);
            Console.WriteLine((int)sScreen.sld_speed.Value*100);
            Console.WriteLine(ts);
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            t.Abort();
        }

        private void InitializeAnimation()
        {
            AnimationMoney1.From = gScreen.lbl_money.FontSize;
            AnimationMoney1.To = gScreen.lbl_money.FontSize + 1;
            AnimationMoney1.Duration = new Duration(TimeSpan.FromSeconds(0.1));
            AnimationMoney1.AutoReverse = true;

            AnimationMoney2.From = 0;
            AnimationMoney2.To = 1;
            AnimationMoney2.Duration = new Duration(TimeSpan.FromSeconds(0.2));
            AnimationMoney2.AutoReverse = true;

            AnimationButtonHeight.From = gScreen.btn_manual.Height;
            AnimationButtonHeight.To = gScreen.btn_manual.Height + 1.5;
            AnimationButtonHeight.Duration = new Duration(TimeSpan.FromSeconds(0.05));
            AnimationButtonHeight.AutoReverse = true;

            AnimationButtonWidth.From = gScreen.btn_manual.Width;
            AnimationButtonWidth.To = gScreen.btn_manual.Width + 1.5;
            AnimationButtonWidth.Duration = new Duration(TimeSpan.FromSeconds(0.05));
            AnimationButtonWidth.AutoReverse = true;
        }

        private void incMoney()
        {
            while (true)
            {
                Thread.Sleep(ts);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    double added = 0;
                    foreach (Helper h in helpers)
                    {
                        if (h.getBought() == true && h.getType() == "worker")
                        {
                            added = added + h.getValue();
                        }
                    }
                    Random r = new Random();
                    int n = r.Next(0, 100);
                    if (n <= critDps)
                    {
                        money = money + added + added * critValue;
                        added = added + added * critValue;
                        gScreen.lbl_crit.BeginAnimation(Label.OpacityProperty, AnimationMoney2);
                    }
                    else
                    {
                        money = money + added;
                    }
                    if (added > 0)
                    {
                        updateMoney();
                        updateHelpers();
                        gScreen.lbl_addMoney.Content = "+" + Math.Round(added, 2) + "€";
                        gScreen.lbl_money.BeginAnimation(Button.FontSizeProperty, AnimationMoney1);
                        gScreen.lbl_addMoney.BeginAnimation(Button.OpacityProperty, AnimationMoney2);
                    }
                }));
            }
        }

        private void InitializeHelpers()
        {

            HelpersData hD = new HelpersData();

            this.helpers = hD.getHelpers();
        }

        private void btnSave_click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = "C:\\" + Environment.UserName;
            sfd.Filter = "Textdatei (*.txt) |*.txt";
            if (sfd.ShowDialog() == true)
            {
                string filepath = sfd.FileName;
                using (StreamWriter sw = new StreamWriter(filepath))
                {

                    sw.WriteLine(money);
                    foreach (Helper h in helpers)
                    {
                        if (h.getBought())
                        {
                            sw.WriteLine(h.getHelperNo());
                            sw.WriteLine(h.getName() + "$" + h.getLevel() + "$" + h.getPrice() + "$" + h.getValue() + "$");
                        }
                    }
                    sw.Close();
                }
            }
        }

        private void btnLoad_click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "C:\\" + Environment.UserName;
            ofd.Filter = "Textdatei (*.txt) |*.txt";
            ofd.ShowDialog();
            try
            {
                InitializeHelpers();
                using (StreamReader sr = new StreamReader(ofd.FileName))
                {
                    money = Convert.ToDouble(sr.ReadLine());

                    while (!sr.EndOfStream)
                    {
                        int no = Convert.ToInt32(sr.ReadLine());
                        string data = sr.ReadLine();
                        string name = data.Substring(0, data.IndexOf("$"));
                        data = data.Remove(0, name.Length + 1);
                        string lv = data.Substring(0, data.IndexOf("$"));
                        data = data.Remove(0, lv.Length + 1);
                        string pr = data.Substring(0, data.IndexOf("$"));
                        data = data.Remove(0, pr.Length + 1);
                        string val = data.Substring(0, data.IndexOf("$"));
                        data = data.Remove(0, val.Length + 1);
                        helpers[no].setBought(true);
                        helpers[no].setPrice(Convert.ToDouble(pr));
                        helpers[no].setLevel(Convert.ToInt32(lv));
                        helpers[no].setValue(Convert.ToDouble(val));
                        if (helpers[no].getType() == "clicker")
                        {
                            clickValue = 1 + Convert.ToDouble(val);
                        }
                    }
                }
                this.Content = gScreen;
                updateHelpers();
                updateMoney();
            }
            catch
            {

            }
        }
        private void btn_click(object sender, RoutedEventArgs e)
        {
            Random r = new Random();
            int n = r.Next(0, 100);
            Console.WriteLine("random = " + n + " critClick = " + critClick);
            if (n <= critClick)
            {
                money = money + (clickValue + (clickValue * critValue));
                gScreen.lbl_addMoney2.Content = "+" + (clickValue + (clickValue * 1)) + "€";
                gScreen.lbl_crit.BeginAnimation(Label.OpacityProperty, AnimationMoney2);
            }
            else
            {
                money = money + clickValue;
                gScreen.lbl_addMoney2.Content = "+" + clickValue + "€";
            }
            gScreen.lbl_addMoney2.Foreground = Brushes.PaleGreen;
            gScreen.lbl_money.BeginAnimation(Button.FontSizeProperty, AnimationMoney1);
            gScreen.lbl_addMoney2.BeginAnimation(Button.OpacityProperty, AnimationMoney2);
            s.soundClick();

            updateMoney();
            updateHelpers();
        }

        private string getShortMoney()
        {
            String sMoney = string.Format("{0:0.00}", money);

            if (money >= 1000)
            {
                sMoney = string.Format("{0:0.00}", money / 1000) + " k";
            }
            if (money >= 1000000)
            {
                sMoney = string.Format("{0:0.00}", money / 1000000) + " m";
            }
            if (money >= 1000000000)
            {
                sMoney = string.Format("{0:0.00}", money / 1000000) + " b";
            }
            if (money >= 1000000000000)
            {
                sMoney = string.Format("{0:0.00}", money / 1000000) + " t";
            }

            return sMoney;
        }

        private void updateMoney()
        {
            gScreen.lbl_money.Content = "Money: " + getShortMoney() + "€";
            gScreen.lbl_money.BeginAnimation(Button.FontSizeProperty, AnimationMoney1);
            value = 0;
            foreach (Helper h in helpers)
            {
                if (h.getBought() && h.getType() == "worker")
                {
                    value = value + h.getValue();

                    gScreen.lbl_value.Content = Math.Round(value, 2) + "€/s";
                }
            }
            gScreen.lbl_addMoney.Foreground = Brushes.PaleGreen;
        }

        private StackPanel getButtonPanel(Button b, Helper h)
        {
            StackPanel sp = new StackPanel();
            Label name = new Label();
            name.Content = h.getName().Replace("_", " ");
            Label level = new Label();
            switch (h.getType())
            {
                case "worker":
                    level.Content = "Lv. " + h.getLevel() + "   (+" + string.Format("{0:0.00}", h.getValue()) + "€/s)";
                    break;
                case "clicker":
                    level.Content = "Lv. " + h.getLevel() + "   (+" + string.Format("{0:0.00}", clickValue) + "€/Click)";
                    break;
            }

            Label price = new Label();
            price.Content = h.getPrice() + "€";
            name.FontSize = 16;

            WrapPanel wp = new WrapPanel();
            foreach (Upgrade u in h.getUpgrades())
            {
                if (u != null && h.getBought() == true)
                {
                    Button btn = new Button();
                    btn.Content = u.getName();
                    btn.Click += btnSubUp_click;
                    btn.Name = u.getName();
                    btn.Tag = u.getType() + "%%" + u.getValue() + "$$" + u.getPrice();
                    btn.ToolTip = u.getName() + "\r\n" + u.getPrice() + "€" + "\r\n" + "+" + u.getValue() + " " + u.getType();
                    wp.Children.Add(btn);
                }

                foreach (Button bt in wp.Children){
                    if(bt.Name == u.getName() && u.getBought() == true){
                        bt.IsEnabled = false;
                    }
                }
            }


            sp.Children.Add(name);
            sp.Children.Add(price);
            sp.Children.Add(level);
            sp.Children.Add(wp);
            return sp;
        }

        private Upgrade upgradeOf(Button b)
        {
            foreach (Upgrade u in helperOf(((Button)b.Parent)).getUpgrades())
            {
                if (u.getName() == b.Name)
                {
                    return u;
                }
            }
            return null;
        }

        private Helper helperOf(Button b)
        {
            try
            {
                foreach (Helper h in helpers)
                {
                    if (h.getName().Replace(" ", "_") == b.Name)
                    {
                        return h;
                    }
                }
            }
            catch
            {

            }
            return null;
        }

        private void updateHelpers()
        {
            foreach (Helper h in helpers)
            {
                if (money >= h.getPrice() * 0.75 && !h.getIsActive())
                {
                    addHelper(h, h.getHelperNo());
                }
            }
            foreach (Button b in gScreen.lbx_helpers.Items)
            {
                b.Content = getButtonPanel(b, helperOf(b));
                if (helperOf(b).getBought())
                {
                    b.Background = clr_bought;
                }
            }
            gScreen.btn_manual.Content = "Clicking (+" + clickValue + "€)";
        }

        private void addHelper(Helper h, int index)
        {
            h.setIsActive(true);
            Button b = new Button();
            b.Content = getButtonPanel(b, h);
            b.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Left;
            b.Width = gScreen.lbx_helpers.Width - 35;
            b.Focusable = false;
            b.BorderThickness = new Thickness(1);
            b.BorderBrush = new SolidColorBrush(Colors.Black);
            b.Cursor = Cursors.Hand;
            b.Name = h.getName().Replace(" ", "_");
            b.Background = new SolidColorBrush(Colors.DarkGray);
            b.Click += new RoutedEventHandler(helperClick);
            gScreen.lbx_helpers.Items.Add(b);
        }

        private void startMoneyDecAnimation(double value)
        {
            gScreen.lbl_money.BeginAnimation(Button.FontSizeProperty, AnimationMoney1);
            gScreen.lbl_addMoney.Content = "-" + value + "€";
            gScreen.lbl_addMoney.Foreground = Brushes.PaleVioletRed;
            gScreen.lbl_addMoney.BeginAnimation(Button.OpacityProperty, AnimationMoney2);
        }

        private void helperClick(object sender, RoutedEventArgs e)
        {
            try { 
                Helper h = helperOf((Button)e.Source);
                if (h.getBought() == false && money >= h.getPrice())
                {
                    h.setBought(true);
                    money = money - h.getPrice();
                    updateMoney();
                    h.incLevel();
                    if (h.getType() == "clicker")
                    {
                        clickValue++;
                        h.setValue(clickValue);
                        gScreen.btn_manual.Content = "Clicking (+" + clickValue + "€)";
                        startButtonAnimation();
                    }
                    else
                    {
                        gScreen.lbl_value.BeginAnimation(Label.FontSizeProperty, AnimationMoney1);
                    }

                    startMoneyDecAnimation(h.getPrice());
                    h.setPrice(h.getPrice() + h.getLevel() * h.getPriceFactor());
                    s.soundUpgrade();
                }
                else
                {
                    if (money >= h.getPrice())
                    {
                        money = money - h.getPrice();
                        updateMoney();
                        startMoneyDecAnimation(h.getPrice());
                        h.setPrice((h.getPrice() + h.getLevel() * h.getPriceFactor()));
                        h.incLevel();
                        switch (h.getType())
                        {
                            case "clicker":
                                clickValue++;
                                startButtonAnimation();
                                break;
                            case "worker":
                                h.setValue(h.getValue() * 1.2);
                                gScreen.lbl_value.BeginAnimation(Label.FontSizeProperty, AnimationMoney1);
                                break;
                        }
                    }
                }
                updateHelpers();
            }
            catch
            {

            }
        }

        private void startButtonAnimation()
        {
            gScreen.btn_manual.BeginAnimation(Button.HeightProperty, AnimationButtonHeight);
            gScreen.btn_manual.BeginAnimation(Button.WidthProperty, AnimationButtonWidth);
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            this.Content = sScreen;
            s.soundSwitch();
        }

        private void btnReturn_click(object sender, RoutedEventArgs e)
        {
            this.Content = gScreen;
            s.soundSwitch();
        }

        private void btnStartNew_click(object sender, RoutedEventArgs e)
        {
            this.Content = gScreen;
        }

        private void btnSoundClick(object sender, RoutedEventArgs e)
        {
            if (s.getmute())
            {
                sScreen.btn_sound_demute.Visibility = System.Windows.Visibility.Hidden;
                sScreen.btn_sound_mute.Visibility = System.Windows.Visibility.Visible;
                s.demute();
            }
            else
            {
                sScreen.btn_sound_demute.Visibility = System.Windows.Visibility.Visible;
                sScreen.btn_sound_mute.Visibility = System.Windows.Visibility.Hidden;
                s.mute();
            }
        }

        private void btnRestart_click(object sender, RoutedEventArgs e)
        {
            this.Content = stScreen;
            money = 0;
            clickValue = 1;
            value = 0;
            gScreen.lbl_value.Content = 0 + "€/s";
            InitializeHelpers();
            gScreen.lbx_helpers.Items.Clear();
            updateHelpers();
            updateMoney();
        }

        private void btnSubUp_click(object sender, RoutedEventArgs e)
        {
            string tag = ((Button)sender).Tag.ToString();
            double price = Convert.ToDouble(tag.Substring(tag.IndexOf("$$"), tag.Length - tag.IndexOf("$$")).Replace("$$", ""));
            string type = tag.Substring(0, tag.IndexOf("%%"));
            double value = Convert.ToDouble(tag.Substring(tag.IndexOf("%%"), tag.Length - (price.ToString().Length + type.Length + 2)).Replace("%%", ""));
            Console.WriteLine("Price: " + price + " Type: " + type + " Value: " + value);
            if (money >= price)
            {
                money = money - price;
                ((Button)sender).Background = clr_bought;
                startMoneyDecAnimation(price);
                foreach (Upgrade u in helperOf(((Button)((StackPanel)((WrapPanel)((Button)sender).Parent).Parent).Parent)).getUpgrades())
                {
                    if (u.getName() == ((Button)sender).Name)
                    {
                        u.setBought();
                    }
                }
                switch (type)
                {
                    case "critClick":
                        this.critClick += value;
                        break;
                    case "critDps":
                        this.critDps += value;
                        break;
                }
                updateHelpers();
                updateMoney();
            }
        }
    }
}