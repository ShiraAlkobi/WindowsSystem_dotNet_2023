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

        #region dependency properties

        /// <summary>
        /// dependency property for project status
        /// </summary>
        public BO.ProjectStatus ProjectStatus
        {
            get { return (BO.ProjectStatus)GetValue(ProjectStatusProperty); }
            set { SetValue(ProjectStatusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatus. 
        public static readonly DependencyProperty ProjectStatusProperty =
            DependencyProperty.Register("ProjectStatus", typeof(BO.ProjectStatus), typeof(MainWindow), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the current date
        /// </summary>
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(MainWindow), new PropertyMetadata(null));
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            CurrentDate = s_bl.getClock();//getting the time from data base
            ProjectStatus = s_bl.getProjectStatus();//getting the project status from data base
            this.DataContext = this;
        }

        #region help functions and event handlers

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
            MessageBoxResult result = MessageBox.Show("Are you sure you want to initialize data? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Factory.Get().ResetDB();//delete data
                Factory.Get().InitializeDB();//initialize data
                CurrentDate = s_bl.ResetClock();//reset time
                ProjectStatus = s_bl.getProjectStatus();//getting the prokject status
                MessageBox.Show("Data initialized!");
            }
            
        }
        /// <summary>
        /// deleting data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Reset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to reset data? ", "", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Factory.Get().ResetDB();
                CurrentDate = s_bl.ResetClock();
                ProjectStatus = s_bl.getProjectStatus();
                MessageBox.Show("Data reset!");
            }
   
        }
        /// <summary>
        /// show gantt window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_GanttChart_Click(object sender, RoutedEventArgs e)
        {
            new GanttWindow().ShowDialog();
        }

        /// <summary>
        /// show task list window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_TaskList_Click(object sender, RoutedEventArgs e)
        {
            new TaskListWindow().ShowDialog();//create new window and show
        }

        /// <summary>
        /// setting schedule window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_setStartDate_Click(object sender, RoutedEventArgs e)
        {
            new SetStartDateWindow().ShowDialog();
            ProjectStatus = s_bl.getProjectStatus();
        }


        /// <summary>
        /// add hour to time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddHour_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddHour();
            CurrentDate = s_bl.getClock();
            if (CurrentDate >= s_bl.getStartDate())//if we reached the start date of the project- 
                                                   //change project status from plan stage to execution stage
                if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
                {
                    s_bl.changeStatus();
                    MessageBox.Show("Project Started!");

                }
            ProjectStatus = s_bl.getProjectStatus();//read the project status to the dependency property
        }

        /// <summary>
        /// add day to time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDay_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddDay();
            CurrentDate = s_bl.getClock();
            if (CurrentDate >= s_bl.getStartDate())
                if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
                {
                    s_bl.changeStatus();
                    MessageBox.Show("Project Started!");

                }
            ProjectStatus = s_bl.getProjectStatus();
        }

        /// <summary>
        /// add month to time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddMonth_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddMonth();
            CurrentDate = s_bl.getClock();
            if (CurrentDate >= s_bl.getStartDate())
                if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
                {
                    s_bl.changeStatus();
                    MessageBox.Show("Project Started!");

                }
            ProjectStatus = s_bl.getProjectStatus();
        }

        /// <summary>
        /// add year to time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddYear_Click(object sender, RoutedEventArgs e)
        {
            s_bl.AddYear();
            CurrentDate = s_bl.getClock();
            if (CurrentDate >= s_bl.getStartDate())
                if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
                {
                    s_bl.changeStatus();
                    MessageBox.Show("Project Started!");

                }
            ProjectStatus = s_bl.getProjectStatus();
        }

        /// <summary>
        /// reset clock
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_ResetClock_Click(object sender, RoutedEventArgs e)
        {
            s_bl.ResetClock();
            CurrentDate = s_bl.getClock();
            if (CurrentDate < s_bl.getStartDate())//if the reset time is before the projectstart date- change status to plan stage
                s_bl.setStatus();
            ProjectStatus = s_bl.getProjectStatus();

        }

        /// <summary>
        /// close the window when the x button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// enables the window to move according to mouse moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        #endregion
    }
}