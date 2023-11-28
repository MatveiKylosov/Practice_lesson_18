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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PrintManagementSystem_Kylosov
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Pages.PageWrite pg = new Pages.PageWrite();
        Pages.PageLog pl = new Pages.PageLog();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenLog(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(pl) ;
        }

        private void OpenWrite(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(pg);
        }
    }
}
