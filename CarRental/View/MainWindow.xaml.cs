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

namespace CarRental.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            GlobalValues.basDygnsHyra = 500;
            GlobalValues.baskmPris = 10;
        }

        void OnRentClick(object sender, RoutedEventArgs e)
        {
            var mRentCar = new RentCar();
            
            mRentCar.Show();
        }

        void OnReturnClick(object sender, RoutedEventArgs e)
        {
            var mReturnCar = new ReturnCar();

            mReturnCar.Show();
        }
    }
}
