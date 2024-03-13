using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for EngineerWindow.xaml
    /// </summary>
    public partial class EngineerWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        #region dependency properties        

        /// <summary>
        /// dependency property for the engineer that is being added or updated in this window
        /// </summary>
        public BO.Engineer CurrentEngineerUser
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerUserProperty); }
            set { SetValue(CurrentEngineerUserProperty, value); }
        }
        //Using a DependencyProperty as the backing store for CurrentEngineer.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerUserProperty =
            DependencyProperty.Register("CurrentEngineerUser", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the task assigned to the engineer in this window
        /// </summary>
        public BO.Task CurrentAssignedTask
        {
            get { return (BO.Task)GetValue(CurrentAssignedTaskProperty); }
            set { SetValue(CurrentAssignedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentAssignedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentAssignedTaskProperty =
            DependencyProperty.Register("CurrentAssignedTask", typeof(BO.Task), typeof(EngineerWindow), new PropertyMetadata(null));

        public BO.Task TaskDetails
        {
            get { return (BO.Task)GetValue(TaskDetailsProperty); }
            set { SetValue(TaskDetailsProperty, value); }
        }
        public static readonly DependencyProperty TaskDetailsProperty =
            DependencyProperty.Register("TaskDetails", typeof(BO.Task), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the tasks that are not yet completed or assigned to another engineer
        /// this is an observable collection so the updating can be faster and immidiate
        /// </summary>
        public ObservableCollection<BO.TaskInList> AvailableTasks
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(AvailableTasksProperty); }
            set { SetValue(AvailableTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableTasksProperty =
            DependencyProperty.Register("AvailableTasks", typeof(ObservableCollection<BO.TaskInList>), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the clock 
        /// </summary>
        public DateTime CurrentDate
        {
            get { return (DateTime)GetValue(CurrentDateProperty); }
            set { SetValue(CurrentDateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDateProperty =
            DependencyProperty.Register("CurrentDate", typeof(DateTime), typeof(EngineerWindow), new PropertyMetadata(null));

        #endregion

        public EngineerWindow(int id)
        {
            InitializeComponent();
            CurrentDate = s_bl.getClock();
            try
            {
                ///read the right engineer according to the given id
                CurrentEngineerUser = s_bl.Engineer.Read(id);
                if(CurrentEngineerUser.Task is not null)
                 CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
                else
                    CurrentAssignedTask=new BO.Task();

                AvailableTasks = new ObservableCollection<BO.TaskInList>
                                ((from item in s_bl.Task.ReadAll()
                                  where s_bl.Engineer.checkAssignedTask(item.Id) == true
                                  select item).ToList());

            }///if an exception was thrown from the read function, catch it and show a message box which explains the exception
            catch (BO.BlDoesNotExistException)
            {
                MessageBox.Show($"Engineer with id: {id} does not exist", "Input Error!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ///because there're several dependency properties, the data context needs to be the whole window
            this.DataContext = this;
        }

        #region help functions and event handlers

        /// <summary>
        /// when the engineer clicks on the button to complete a task, we update it in the data base
        /// then we update the list of tasks available to the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void taskCompleted_Click(object sender, RoutedEventArgs e)
        {
            try
            {                
                s_bl.Task.Update(new()
                {
                    Id = CurrentAssignedTask.Id,
                    Alias = CurrentAssignedTask.Alias,
                    Description = CurrentAssignedTask.Description,
                    Dependencies = CurrentAssignedTask.Dependencies,
                    Status = CurrentAssignedTask.Status,
                    CreatedAtDate = CurrentAssignedTask.CreatedAtDate,
                    ScheduledDate = CurrentAssignedTask.ScheduledDate,
                    StartDate = CurrentAssignedTask.StartDate,
                    ForecastDate = CurrentAssignedTask.ForecastDate,
                    RequiredEffortTime = CurrentAssignedTask.RequiredEffortTime,
                    CompleteDate = s_bl.getClock(),
                    Deliverables = CurrentAssignedTask.Deliverables!,
                    Remarks = CurrentAssignedTask.Remarks!,
                    Engineer = CurrentAssignedTask.Engineer,
                    Complexity = CurrentAssignedTask.Complexity
                });
                CurrentEngineerUser = s_bl.Engineer.Read(CurrentEngineerUser.Id);
                
                if (CurrentEngineerUser.Task is not null)
                    CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
                AvailableTasks = new ObservableCollection<BO.TaskInList>((from item in s_bl.Task.ReadAll()
                                                                          where s_bl.Engineer.checkAssignedTask(item.Id) == true
                                                                          select item).ToList());

            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message, "Input Error!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// when the engineer clicks on the button to start a task, we update it in the data base for the task and the engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void assignTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentAssignedTask = s_bl.Task.Read(((BO.TaskInList)tasksForEngineer.SelectedItem).Id);
                s_bl.Engineer.Update(new BO.Engineer
                {
                    Id = CurrentEngineerUser.Id,
                    Name = CurrentEngineerUser.Name,
                    Email = CurrentEngineerUser.Email,
                    Cost = CurrentEngineerUser.Cost,
                    Level = CurrentEngineerUser.Level,
                    Task = new BO.TaskInEngineer() { Id = CurrentAssignedTask.Id, Alias = CurrentAssignedTask.Alias }
                });

                s_bl.Task.Update(new()
                {
                    Id = CurrentAssignedTask.Id,
                    Alias = CurrentAssignedTask.Alias,
                    Description = CurrentAssignedTask.Description,
                    Dependencies = CurrentAssignedTask.Dependencies,
                    Status = CurrentAssignedTask.Status,
                    CreatedAtDate = CurrentAssignedTask.CreatedAtDate,
                    ScheduledDate = CurrentAssignedTask.ScheduledDate,
                    StartDate = s_bl.getClock(),
                    ForecastDate = CurrentAssignedTask.ForecastDate,
                    RequiredEffortTime = CurrentAssignedTask.RequiredEffortTime,
                    CompleteDate = CurrentAssignedTask.CompleteDate,
                    Deliverables = CurrentAssignedTask.Deliverables!,
                    Remarks = CurrentAssignedTask.Remarks!,
                    Engineer = new BO.EngineerInTask { Id = CurrentEngineerUser.Id, Name = CurrentEngineerUser.Name },
                    Complexity = CurrentAssignedTask.Complexity
                });
                CurrentEngineerUser = s_bl.Engineer.Read(CurrentEngineerUser.Id);
                if (CurrentEngineerUser.Task is not null)
                    CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
            }
            catch(Exception ) { MessageBox.Show("please choose a task"); }
        }
        
        /// <summary>
        /// when the engineer picked a task to start, the details in the window needs to change
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tasksForEngineer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasksForEngineer.SelectedItem is BO.TaskInEngineer selectedTask)
            {
                TaskDetails = s_bl.Task.Read(selectedTask.Id);
            }
        }

        /// <summary>
        /// gets the list of tasks can be assigned to the engineer when the comboBox is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tasksForEngineer_DropDownOpened(object sender, EventArgs e)
        {
            AvailableTasks = new ObservableCollection<BO.TaskInList>((from item in s_bl.Task.ReadAll()
                                                                      where s_bl.Engineer.checkAssignedTask(item.Id) == true
                                                                      select item).ToList());
        }

        /// <summary>
        /// open the pop up window when clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void viewBtn_Click(object sender, RoutedEventArgs e)
        {
            viewDetailsPopUp.IsOpen = true;
        }

        /// <summary>
        /// close the pop up window when clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseBtn_Click(object sender, RoutedEventArgs e)
        {
            viewDetailsPopUp.IsOpen = false;
        }

        /// <summary>
        /// open the pop up window when clicking
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ViewTask_Click(object sender, RoutedEventArgs e)
        {
            viewDetailsTaskPopUp.IsOpen = true;
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

        /// <summary>
        /// close the window when the x button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
