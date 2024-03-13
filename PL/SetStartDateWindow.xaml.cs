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
        /// <summary>
        /// dependency property for the start date of the project
        /// </summary>
        public DateTime StartDate
        {
            get { return (DateTime)GetValue(StartDateProperty); }
            set { SetValue(StartDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartDateProperty =
            DependencyProperty.Register("StartDate", typeof(DateTime), typeof(SetStartDateWindow), new PropertyMetadata(default));


        /// <summary>
        /// dependency proerty for the current date
        /// </summary>
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(SetStartDateWindow), new PropertyMetadata(default));

        /// <summary>
        /// getting data 
        /// </summary>
        public SetStartDateWindow()
        {
            InitializeComponent();
            StartDate = DateTime.Now;
            CurrentDate = s_bl.getClock();
        }
        /// <summary>
        /// set start date for the project and scheduld dates for the tasks
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void setDate_Click(object sender, RoutedEventArgs e)
        {
            
                s_bl.setStartAndEndDates(StartDate);

                Close();
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
