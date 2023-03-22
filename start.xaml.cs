using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace demo
{
    /// <summary>
    /// Logika interakcji dla klasy start.xaml
    /// </summary>
    public partial class start : Window
    {
        public start()
        {
            InitializeComponent();
        }

        private void graj_bt_Click(object sender, RoutedEventArgs e)
        {
            MainWindow win = new MainWindow();
            win.Show();

            
        }
    }
}
