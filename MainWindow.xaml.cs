using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MySql.Data.MySqlClient;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using System.Diagnostics;

namespace demo
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string poziom = "taski_demo";
        public string gracz = "Tom";
        string adres_odbiorcy;
        public int czy_legit;
        public int ilosc_bledow = 0;
        public int ilosc_dobrych = 0;
        int pierwszy_raz;
        public string jaka_anim;
        bool w_grze;
        bool czy_pauza;
        public int punkty = 0;
        bool koniec;
        int p_odj = 75;
        int p_dod = 125;
        int ile_taski = 1;
        int taski_max;

        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public static int czas = 0;
        int sekundy = 0;
        int minuty = 0;
        int godz = 0;
        string zegar;        
        void timer_Tick(object sender, EventArgs e)
        {
            //czas_lb.Content = DateTime.Now.ToLongTimeString();
            sekundy += 1;
            if (sekundy == 60)
            {
                sekundy = 0;
                minuty += 1;
            }

            if(minuty == 60)
            {
                minuty = 0;
                godz += 1;
            }


            if (sekundy < 10)
            {
                
                if(minuty < 10)
                {
                    zegar = string.Format("{0}:0{1}:0{2}", godz, minuty, sekundy);
                }
                else
                {
                    zegar = string.Format("{0}:{1}:0{2}", godz, minuty, sekundy);
                }
            }
            else
            {
                if (minuty < 10)
                {
                    zegar = string.Format("{0}:0{1}:{2}", godz, minuty, sekundy);
                }
                else
                {
                    zegar = string.Format("{0}:{1}:{2}", godz, minuty, sekundy);
                }
            }                        

            czas_lb.Content = zegar;           
        }


        #region punktacja
        int punkty_tim_start = 0;
        DispatcherTimer punkty_tim = new DispatcherTimer();
        void punkty_tim_inic(bool on)
        {
            if (punkty_tim_start == 0)
            {
                punkty_tim_start = 1;
                punkty_tim.Interval = TimeSpan.FromSeconds(1);
                punkty_tim.Tick += punkty_tim_Tick;
            }

            if (on == true)
            {
                punkty_tim.Start();
            }
            else
            {
                punkty_tim.Stop();
                punkty_sekundy = 0;
            }
        }
        int punkty_sekundy = 0;
        void punkty_tim_Tick(object sender, EventArgs e)
        {
            punkty_sekundy++;
        }


        int dodatnie_punkty(int sek)
        {
            int p = 0;
            double p2 = 0;
            double p3 = 1;
            if(punkty_sekundy <= 25)
            {
                p = p_dod;
            }
            else if(punkty_sekundy <= 75)
            {
                p2 = (punkty_sekundy / 100.00);
                p3 = p3 - p2;
                p = Convert.ToInt32(p_dod * p3);
            }
            else
            {
                p = 10;
            }
            return p;
        }





        #endregion

        void restart()
        {
            ilosc_dobrych = 0;
            ilosc_bledow = 0;
            w_grze = false;
            punkty = 0;
            koniec = false;
            sekundy = 0;
            minuty = 0;
            ile_taski = 1;
            taski_lib.Items.Clear();
            poprawne_lb.Content = ilosc_dobrych.ToString();
            punkty_lb.Content = punkty.ToString();
            czas_lb.Content = string.Format("0:0{0}:0{1}", minuty, sekundy);
            pierwszy_raz = 1;
            koniec_grid.Margin = new Thickness(824, -1434, -830, 0);
            menu_grid.Margin = new Thickness(0, 0, -4, -3);
            Rectangle.Margin = new Thickness(-1546, 556, 1538, -587);
        }


        #region koniec
        int koniec_sekundy = 0;
        void koniectim_Tick(object sender, EventArgs e)
        {
            koniec_sekundy++;
            if(koniec_sekundy == 2)
            {
                wynik_lb.Content = string.Format("Twój wynik: {0}", punkty);
                czas_end_lb.Content = string.Format("Twój czas: {0}", zegar);
                koniec_grid.Margin = new Thickness(0, 0, 0, 0);
                koniectim_inic(false);
                napis_show();
            }
        }


        void koniec_pracy()
        {
            zegar_inic(false);
            w_grze = false;
            koniec = true;
            sciemnienie();
            koniectim_inic(true);            
        }

        int koniec_zegar_start = 0;
        DispatcherTimer koniectim = new DispatcherTimer();

        void koniectim_inic(bool on)
        {
            if (koniec_zegar_start == 0)
            {
                koniec_zegar_start = 1;
                koniectim.Interval = TimeSpan.FromSeconds(1);
                koniectim.Tick += koniectim_Tick;
            }

            if (on == true)
            {
                koniec_sekundy = 0;
                koniectim.Start();
            }
            else
            {
                koniectim.Stop();                
            }
        }
        void sciemnienie()
        {
            Rectangle.Margin = new Thickness(0, 0, 0, 0);
            Storyboard storyboard = new Storyboard();
            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0;
            doubleAnimationOpacity.To = 1;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.9));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, Rectangle.Name);
            storyboard.Begin(this);            
        }

        void napis_show()
        {
            Storyboard storyboard = new Storyboard();
            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0;
            doubleAnimationOpacity.To = 1;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.9));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, info_lb.Name);
            storyboard.Begin(this);
        }

        #endregion
        void pauza()
        {
            
            if(w_grze == true)
            {
                if (czy_pauza == false)
                {
                    czy_pauza = true;
                    grid_pauza.Margin = new Thickness(0, 0, -4, -3);
                    zegar_inic(false);

                }
                else if (czy_pauza == true)
                {
                    czy_pauza = false;
                    grid_pauza.Margin = new Thickness(1633, -563, -1641, 0);
                    zegar_inic(true);
                }
            }            
        }

        public MainWindow()
        {
            InitializeComponent();
            pierwszy_raz = 1;
            intro_grid.Margin = new Thickness(0, -14, -4, -3);            
            intro_me.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "intro/inertko.mp4");
            intro_me.Play();
            menu_grid.Margin = new Thickness(0, 0, -4, -3);
        }
        void inicjalizacja_gracza(string praca, string domena)
        {
            adres_odbiorcy = string.Format("{0}@{1}", gracz, domena);
            ranga_lb.Content = praca;
            adresodbiorcy_lb.Content = adres_odbiorcy;
        }
        MySqlConnection Polaczenie = new MySqlConnection("server=127.0.0.1;user=root;database=taski;");        
        MySqlCommand komenda;
        MySqlDataReader czytnik;
        string zapytanie = "";
        List<taski_demo> listataskow = new List<taski_demo>();
        taski_demo wybranytask = null;

        public class taski_demo
        {
            public int id { get; set; }
            public string nazwa { get; set; }
            public string email { get; set; }
            public string temat { get; set; }
            public string zawartosc { get; set; }
            public int good { get; set; }
            public string powod { get; set; }

            public taski_demo() { }

            public taski_demo(int id, string nazwa, string email, string temat, string zawartosc, int good, string powod)
            {
                this.id = id;
                this.nazwa = nazwa;
                this.email = email;
                this.temat = temat;
                this.zawartosc = zawartosc;
                this.good = good;
                this.powod = powod;
            }


        }
        int timer_start = 0;
        DispatcherTimer timer = new DispatcherTimer();
        void zegar_inic(bool on)
        {
            if(timer_start == 0)
            {
                timer_start = 1;
                timer.Interval = TimeSpan.FromSeconds(1);
                timer.Tick += timer_Tick;                
            }

            if(on == true)
            {
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        void uruchomienie_gry()
        {
            Inicjacja();
            inicjalizacja_gracza(dane.Default.ranga, dane.Default.domena);
            taski_max = taski_lib.Items.Count;
            zegar_inic(true);           
            losowanie();
            czy_pauza = false;
            w_grze = true;
        }
        public int sekundy_animacja = 0;
        int inic = 0;
        DispatcherTimer timanim = new DispatcherTimer();
        void zegar_animacje(bool x)
        {
            if(inic == 0)
            {
                sekundy_animacja = 0;                
                timanim.Interval = TimeSpan.FromSeconds(1);
                timanim.Tick += timanim_Tick;
                inic = 1;
            }
            
            if(x == true)
            {
                timanim.Start();
            }
            else
            {
                timanim.Stop();
                sekundy_animacja = 0;
            }
            
        }

        void timanim_Tick (object sender, EventArgs e)
        {
            sekundy_animacja += 1;
            if(sekundy_animacja == 1)
            {
                if(jaka_anim == "dobrze")
                {
                    animacja_dobrze_stop();
                }
                else if (jaka_anim == "zle")
                {
                    animacja_zle_stop();
                }
                losowanie();
                zegar_animacje(false);
            }
        }

        void Inicjacja()
        {
            try
            {
                string zapytanie = "";
                if (Polaczenie.State == ConnectionState.Closed)
                    Polaczenie.Open();

                zapytanie = string.Format("select * from taski_demo");
                komenda = new MySqlCommand(zapytanie, Polaczenie);
                czytnik = komenda.ExecuteReader();

                listataskow.Clear();
                int licznik = 1;
                if (czytnik.HasRows)
                {
                    while (czytnik.Read())
                    {
                        listataskow.Add(new taski_demo(czytnik.GetInt32("id"), czytnik["nazwa"].ToString(), czytnik["email"].ToString(), czytnik["temat"].ToString(), czytnik["zawartosc"].ToString(), czytnik.GetInt32("good"), czytnik["powod"].ToString()));
                        taski_lib.Items.Add(string.Format("{0}. {1}", licznik++, czytnik["nazwa"].ToString()));
                    }
                    czytnik.Close();
                }

            }
            catch (Exception ex)
            {
                string byk = string.Format("Błąd podczas pobierania danych:\n{0}", ex.Message);
                MessageBox.Show(byk, "Błąd");
            }
            finally
            {
                if (czytnik == null)
                {

                }
                else
                {
                    czytnik.Close();
                }
                Polaczenie.Close();
            }

        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            
        }
        string lokalizacja;

        private void taski_lib_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (taski_lib.SelectedIndex == -1)
            {
                adresnadawcy_lb.Content = "nadawca";
                temat_lb.Content = "temat";
                zawartosc_img.Source = null;
            }
            else
            {
                string llb = taski_lib.SelectedItem.ToString();
                string[] tab = llb.Split('.');
                taski_demo p = listataskow.FirstOrDefault(x => x.nazwa.Equals(tab[1].Trim()));
                wybranytask = p;

                if (p != null)
                {
                    lokalizacja = string.Format("pack://application:,,,/UWAGA!WIRUS;component/lv_demo/{0}.png", p.zawartosc);
                    adresnadawcy_lb.Content = p.email;
                    temat_lb.Content = p.temat;
                    czy_legit = p.good;
                    try
                    {
                        var converter = new ImageSourceConverter();

                        zawartosc_img.Source = (ImageSource)converter.ConvertFromString(lokalizacja);
                    }
                    catch
                    {

                    }


                }
            }
        }

        int obenca_liczba;
        int nowa_liczba;
        int stara_liczba;

        string obecny_task;
        string nowy_task;
        string stary_task;

        void losowanie()
        {
            if(ile_taski == taski_max)
            {
                koniec_pracy();
            }
            else
            {
                try
                {
                    if (taski_lib.SelectedItem == null)
                    {

                    }
                    else
                    {
                        stary_task = taski_lib.SelectedItem.ToString();
                    }

                    nowa_liczba = wylosowana_liczba(taski_lib.Items.Count - 1);

                    if (stary_task == null)
                    {

                    }
                    else
                    {
                        taski_lib.Items.Remove(stary_task);
                    }
                    taski_lib.SelectedIndex = nowa_liczba;
                    obecny_task = nowy_task;
                    stary_task = obecny_task;
                }
                catch
                {
                    losowanie();
                }

                punkty_tim_inic(true);
                ile_taski++;

            }




        }

        int wylosowana_liczba(int ilosc)
        {
            System.Random random = new System.Random();
            int num = random.Next(ilosc);
            return num;
        }

        void animacja_dobrze()
        {
            Rectangle.Margin = new Thickness(0, 0, 0, 0);
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 1200;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Image.WidthProperty));
            Storyboard.SetTargetName(doubleAnimation, dobrze.Name);
            storyboard.Begin(this);

            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = 0;
            doubleAnimation2.To = 700;
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation2);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(Image.HeightProperty));
            Storyboard.SetTargetName(doubleAnimation2, dobrze.Name);
            storyboard.Begin(this);

            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0;
            doubleAnimationOpacity.To = 0.8;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, Rectangle.Name);
            storyboard.Begin(this);

        }

        void animacja_zle()
        {
            Rectangle.Margin = new Thickness(0, 0, 0, 0);
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 0;
            doubleAnimation.To = 1200;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Image.WidthProperty));
            Storyboard.SetTargetName(doubleAnimation, Zle.Name);
            storyboard.Begin(this);

            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = 0;
            doubleAnimation2.To = 700;
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation2);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(Image.HeightProperty));
            Storyboard.SetTargetName(doubleAnimation2, Zle.Name);
            storyboard.Begin(this);

            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0;
            doubleAnimationOpacity.To = 0.8;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, Rectangle.Name);
            storyboard.Begin(this);
        }

        void animacja_dobrze_stop()
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 1200;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3)); 
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Image.WidthProperty));
            Storyboard.SetTargetName(doubleAnimation, dobrze.Name);
            storyboard.Begin(this);

            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = 700;
            doubleAnimation2.To = 0;
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation2);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(Image.HeightProperty));
            Storyboard.SetTargetName(doubleAnimation2, dobrze.Name);
            storyboard.Begin(this);

            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0.8;
            doubleAnimationOpacity.To = 0;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, Rectangle.Name);
            storyboard.Begin(this);
            Rectangle.Margin = new Thickness(-1546, 556, 1538, -587);
        }

        void animacja_zle_stop()
        { 
            Storyboard storyboard = new Storyboard();
            DoubleAnimation doubleAnimation = new DoubleAnimation();
            doubleAnimation.From = 1200;
            doubleAnimation.To = 0;
            doubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath(Image.WidthProperty));
            Storyboard.SetTargetName(doubleAnimation, Zle.Name);
            storyboard.Begin(this);

            Storyboard storyboard2 = new Storyboard();
            DoubleAnimation doubleAnimation2 = new DoubleAnimation();
            doubleAnimation2.From = 700;
            doubleAnimation2.To = 0;
            doubleAnimation2.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimation2);
            Storyboard.SetTargetProperty(doubleAnimation2, new PropertyPath(Image.HeightProperty));
            Storyboard.SetTargetName(doubleAnimation2, Zle.Name);
            storyboard.Begin(this);

            Storyboard storyboardOpacity = new Storyboard();
            DoubleAnimation doubleAnimationOpacity = new DoubleAnimation();
            doubleAnimationOpacity.From = 0.8;
            doubleAnimationOpacity.To = 0;
            doubleAnimationOpacity.Duration = new Duration(TimeSpan.FromSeconds(0.3));
            storyboard.Children.Add(doubleAnimationOpacity);
            Storyboard.SetTargetProperty(doubleAnimationOpacity, new PropertyPath(Rectangle.OpacityProperty));
            Storyboard.SetTargetName(doubleAnimationOpacity, Rectangle.Name);
            storyboard.Begin(this);

            Rectangle.Margin = new Thickness(-1546, 556, 1538, -587);
        }
        private void ok_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (czy_legit == 1)
            {
                animacja_dobrze();
                jaka_anim = "dobrze";
                zegar_animacje(true);
                ilosc_dobrych++;
                poprawne_lb.Content = ilosc_dobrych.ToString();
                punkty = punkty + dodatnie_punkty(punkty_sekundy);
                punkty_tim_inic(false);
                punkty_lb.Content = punkty.ToString();
            }
            else
            {
                ilosc_bledow = ilosc_bledow + 1;
                animacja_zle();
                jaka_anim = "zle";
                zegar_animacje(true);
                if(punkty < p_odj)
                {
                    punkty = 0;
                }
                else
                {
                    punkty = punkty - p_odj;                    
                }
                punkty_tim_inic(false);
                punkty_lb.Content = punkty.ToString();
            }
            
        }
        private void bad_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (czy_legit == 0)
            {
                animacja_dobrze();
                jaka_anim = "dobrze";
                zegar_animacje(true);
                ilosc_dobrych++;
                poprawne_lb.Content = ilosc_dobrych.ToString();
                punkty = punkty + dodatnie_punkty(punkty_sekundy);
                punkty_tim_inic(false);
                punkty_lb.Content = punkty.ToString();
            }
            else
            {
                ilosc_bledow = ilosc_bledow + 1;
                animacja_zle();
                jaka_anim = "zle";
                zegar_animacje(true);
                if (punkty < p_odj)
                {
                    punkty = 0;
                }
                else
                {
                    punkty = punkty - p_odj;
                }
                punkty_tim_inic(false);
                punkty_lb.Content = punkty.ToString();
            }
        }

        int n;
        private void lb2_lib_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            obenca_liczba = taski_lib.SelectedIndex;            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            uruchomienie_gry();            
            menu_grid.Margin = new Thickness(-1459, -705, 0, 0);

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Button_Click(null, null);
        }

       

        private void wyjdz_bt_MouseDown(object sender, MouseButtonEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                pauza();
            }

            if (e.Key == Key.Space && koniec == true)
            {
                restart();
            }

            if (e.Key == Key.Space && czy_pauza == true)
            {
                czy_pauza = false;
                grid_pauza.Margin = new Thickness(1633, -563, -1641, 0);
                restart();
            }
        }

        private void samo_img_MouseDown(object sender, MouseButtonEventArgs e)
        {
            samouk_grid.Margin = new Thickness(0, 0, -4, -3);
        }

       

        private void back_lb_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            samouk_grid.Margin = new Thickness(-1146, -1634, 1140, 1605);
        }

        private void intro_me_MediaEnded(object sender, RoutedEventArgs e)
        {
            intro_grid.Margin = new Thickness(745, 1002, -751, -1031);
        }
    }
}
