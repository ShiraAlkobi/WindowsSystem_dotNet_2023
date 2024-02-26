using PL.Engineer;
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
    /// Interaction logic for TaskListWindow.xaml
    /// </summary>
    public partial class TaskListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();//giving us access to bl functions
                                                             //public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;//experience field has defualt
        public IEnumerable<BO.TaskInList> TaskList//the list of engineers
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(TaskListProperty); }
            set { SetValue(TaskListProperty, value); }
        }
        public BO.EngineerExperience Complexity { get; set; } = BO.EngineerExperience.All;//experience field has defualt
        public BO.Status Status { get; set; } = BO.Status.Unschedualed;
        /// <summary>
        /// dependency propert that gets all engineers fields to the control list
        /// </summary>
        public static readonly DependencyProperty TaskListProperty =
            DependencyProperty.Register("TaskList", typeof(IEnumerable<BO.TaskInList>), typeof(TaskListWindow), new PropertyMetadata(null));
        ///// <summary>
        ///// the user can change the selection of task in the combobox to engineers with certain level
        ///// the function will call readall again and the filter parameter will be the items the are the level as the one selected
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        private void cbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Complexity == BO.EngineerExperience.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!;
        }
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
        }

        private void AddTaskWindow_Click(object sender, RoutedEventArgs e)
        {

            new AddUpdateTask().ShowDialog();
            TaskList = (Complexity == BO.EngineerExperience.All) ?
               s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!; ////rereading the engineerlist after updating or adding engineer
                                                                                                      //because we want the list to be updated immidiatlynew AddUpdateTask().ShowDialog();
        }

        private void UpdateTaskWindow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //create engineer with the details of the clicked engineer 
            BO.TaskInList? task = (sender as ListView)?.SelectedItem as BO.TaskInList;
            //create new window with id parameter from the clicked engineer
             new AddUpdateTask(task.Id).ShowDialog();//show the windo
            TaskList = (Complexity == BO.EngineerExperience.All) ?
                s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == Complexity)!;//rereading the engineerlist after updating or adding engineer
                                                                                                      //because we want the list to be updated immidiatly
        }

       

        private void status_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TaskList = (Status == BO.Status.Unschedualed) ?
               s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Status == Status)!;
        }
    }
}
