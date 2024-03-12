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

namespace PL
{
    /// <summary>
    /// Interaction logic for SetStartDateWindow.xaml
    /// </summary>
    public partial class SetStartDateWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(SetStartDateWindow), new PropertyMetadata(default));


        public SetStartDateWindow()
        {
            InitializeComponent();
            StartDate = DateTime.Now;
        }

        private void setDate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("NOTE! You can only set the start date once.\n To confirm, press OK","",MessageBoxButton.OKCancel,MessageBoxImage.Warning);
            if (result == MessageBoxResult.OK)
            {
                s_bl.setStartAndEndDates(StartDate);

                Close();
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
