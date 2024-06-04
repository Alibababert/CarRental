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

namespace CarRental.View
{
    /// <summary>
    /// Interaction logic for RentCar.xaml
    /// </summary>
    public partial class RentCar : Window
    {
        public RentCar()
        {
            InitializeComponent();

        }


        public void OnStartRentingClicked(object sender, RoutedEventArgs e)
        {
            var dataContext = this.DataContext as ViewModels.RentCarViewModel;
            dataContext.OnStartRentingClicked(sender);
        }
    }
}
