using System;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.IO.Ports;
using System.ComponentModel;
using System.Diagnostics;
using Arduino;

namespace FindJenny
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool succeed = false, gameover = false;
        private SerialPort sp;
        private string ll, qm, w, sd = "";
        int winner = 0;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                Arduino.Arduino ard = new Arduino.Arduino("a", "Yes!");
                sp = ard.New();
                System.Threading.Thread.Sleep(500);
                sp.Open();
                sp.ReadTimeout = 5000;
                sp.DataReceived += this.GotSData;
                succeed = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK);
                this.Close();
            }

            ll = "/FindJenny;component/cards/lady_lawful.png";
            w = "/FindJenny;component/cards/wrong.png";
            qm = "/FindJenny;component/cards/question_mark.png";
            winner = GetRandom();

            lblText.Content = "FIND LADY LAWFUL!";
        }

        private int GetRandom()
        {
            Random r = new Random();
            while (true)
            {
                int r2 = r.Next(4, 8);
                if (r2 > 4 && r2 < 8 && r2 != winner) { return r2; }
            }
        }

        private void Wipe()
        {
            // Reset all
            BitmapImage q = new BitmapImage(new Uri(qm, UriKind.RelativeOrAbsolute));
            this.imgOne.Source = q;
            this.imgTwo.Source = q;
            this.imgThree.Source = q;
        }

        private void FlipCard()
        {
            Wipe();

            BitmapImage b;
            if (sd == winner.ToString())
            {
                b = new BitmapImage(new Uri(ll, UriKind.RelativeOrAbsolute));
                gameover = true;
            }
            else
            {
                b = new BitmapImage(new Uri(w, UriKind.RelativeOrAbsolute));
            }
            
            switch (sd)
            {
                case "7":
                    this.imgOne.Source = b;
                    break;
                case "6":
                    this.imgTwo.Source = b;
                    break;
                case "5":
                    this.imgThree.Source = b;
                    break;
            };

            if (gameover == true)
            {
                lblText.Content = "You found Lady Lawful! Press any button to play again.";
            }

            // Reset
            sd = "";
        }

        private void StartOver()
        {
            lblText.Content = "Lady Lawful is hiding.";
            Wipe();
            winner = GetRandom();
            gameover = false;
            lblText.Content = "FIND LADY LAWFUL!";
            sp.Write("p");
        }

        private void GotSData(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp1 = (SerialPort)sender;
            string temp = sp1.ReadLine();
            sd = temp.Substring(0, 1);
            if (gameover == false)
            {
                Application.Current.Dispatcher.Invoke((Action)(() => FlipCard()));
            }
            else
            {
                Application.Current.Dispatcher.Invoke((Action)(() => StartOver()));
            }

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (succeed == true)
            {
                if (sp.IsOpen) { sp.Close(); }
                sp.Dispose();
            }
        }
    }
}
