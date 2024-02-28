using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using PL.Engineer;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();//giving us access to bl functions


        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));


        public MainWindow()
        {
            InitializeComponent();
            CurrentDate = s_bl.ResetClock();
            this.DataContext = this;
        }
        /// <summary>
        /// by clicking on "handle engineer" button, a new window will appear with the engineers details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();//create new window and show
        }
        /// <summary>
        /// by clicking on "initialize date" button, this function will show a messege box asking 
        /// if the user is sure he wants to initailize, if he clicls yes, the data will be reset and initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Initialization_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult result= MessageBox.Show("Are you sure you want to initialize data? ","",MessageBoxButton.YesNo,MessageBoxImage.Question); 
            if (result == MessageBoxResult.Yes)
            {
                Factory.Get().ResetDB();
                Factory.Get().InitializeDB();
                MessageBox.Show("Data initialized!");
            }
            
        }

        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset data? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Factory.Get().ResetDB();
                MessageBox.Show("Data reset!");
            }
        }

        private void btn_GanttChart_Click(object sender, RoutedEventArgs e)
        {
            
        }


        private void btn_TaskList_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().Show();//create new window and show
        }

        private void btn_setStartDate_Click(object sender, RoutedEventArgs e)
        {
            new SetStartDateWindow().ShowDialog();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                DateTime? selectedDate = datePicker.SelectedDate;

                if (selectedDate is not null)
                {
                    DateTime start = selectedDate??DateTime.MinValue;
                    s_bl.setStartAndEndDates(start);
                }
            }

        }

        private void AddHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddHour();
            CurrentDate = s_bl.Clock;
        }
        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddDay();
            CurrentDate = s_bl.Clock;
        }
        private void AddMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddMonth();
            CurrentDate = s_bl.Clock;
        }
        private void AddYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddYear();
            CurrentDate = s_bl.Clock;
        }

    }
}