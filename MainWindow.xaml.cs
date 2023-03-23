using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace D8_porting_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int wizardcheck = 0;
        
        public MainWindow()
        {
            InitializeComponent();
            // Gör all initiering nedanför den här texten!
            CMUDS.loadFile("Rooms.txt");
            CMUDS.start();
            RoomInfo(CMUDS.player.PlayerNumber);
            image();
        }
        private void ApplicationKeyPress(object sender, KeyEventArgs e)
        {
            RoomInfo(CMUDS.player.PlayerNumber);
            string output = "Key pressed: ";
            output += e.Key.ToString();
            KeyPressDisplay.Text = output;
            com6.Text = CMUDS.test;
            if(wizardcheck == 1)
            {
                cleaninfo();
                wizarddialog();
            }
            if(e.Key == Key.Escape)
            {
                System.Windows.Application.Current.Shutdown();
            }
            else if(e.Key == Key.N)
            {
                
                CMUDS.n();
                RoomInfo(CMUDS.player.PlayerNumber);
                Error.Text = CMUDS.Error;
            }
            else if (e.Key == Key.S)
            {
                if (CMUDS.player.PlayerNumber == 0) { System.Windows.Application.Current.Shutdown(); }
                CMUDS.s();
                RoomInfo(CMUDS.player.PlayerNumber);
                Error.Text = CMUDS.Error;
            }
            else if (e.Key == Key.E)
            {

                CMUDS.e();
                RoomInfo(CMUDS.player.PlayerNumber);
                Error.Text = CMUDS.Error;
            }
            else if (e.Key == Key.W)
            {

                CMUDS.w();
                RoomInfo(CMUDS.player.PlayerNumber);
                Error.Text = CMUDS.Error;
            }
            else if (e.Key == Key.I)
            {
                cleaninfo();
                com1.Text = $"1: {CMUDS.objects[CMUDS.player.PlayerNumber][0].Place}";
                com2.Text = $"2: {CMUDS.objects[CMUDS.player.PlayerNumber][1].Place}";
                if (CMUDS.objects[CMUDS.player.PlayerNumber][0].Place == "wizard")
                {
                    wizardcheck = 1;
                }
            }
            else if (e.Key == Key.O)
            {
                Title.Text = "OOOOOH!";
                com3.Text = "Åååå,\nOoooh\nUuuuuh\nYyyyyyl!";
            }
            else if (e.Key == Key.D1)
            {
                if (CMUDS.objects[CMUDS.player.PlayerNumber][0].Place == "wizard" && wizardcheck == 1)
                {
                    CMUDS.wizardanswer = "hokus";
                    wizardcheck--;
                }
                else
                {
                    string Object = CMUDS.objects[CMUDS.player.PlayerNumber][0].Place;
                    CMUDS.Search(Object);
                    Error.Text = CMUDS.Error;
                }
            }
            else if (e.Key == Key.D2)
            {
                if (CMUDS.objects[CMUDS.player.PlayerNumber][0].Place == "wizard" && wizardcheck == 1)
                {
                    CMUDS.wizardanswer = "kadabra";
                    wizardcheck--;
                }
                else
                {
                    string Object = CMUDS.objects[CMUDS.player.PlayerNumber][1].Place;
                    CMUDS.Search(Object);
                    Error.Text = CMUDS.Error;
                }
            }
        }

        public void wizarddialog()
        {
            com1.Text = "hokus?";
            com2.Text = "kadabra?";
        }

        
        public void cleaninfo()
        {
            com1.Text = "";
            com2.Text = "";
            com3.Text = "";
            com4.Text = "";
            com5.Text = "";
            com6.Text = "";
    
        }

        public void image()
         {
             
            Uri uri = new Uri("vagskal.png",UriKind.RelativeOrAbsolute);
            BitmapFrame frame = BitmapFrame.Create(uri);

            image1.Source = frame;

         }
        public void RoomInfo(int roomnr)
        {
            Title.Text = CMUDS.Rooms[roomnr].Name;
            presentation.Text = CMUDS.Rooms[roomnr].Presentation;
            com1.Text = "n - go north";
            com2.Text = "e = go east";
            com3.Text = "s = go south";
            com4.Text = "w = go west";
            com5.Text = "i = search";
            com6.Text = CMUDS.test;
            invenotry.Text = "";
        }
    }
}
