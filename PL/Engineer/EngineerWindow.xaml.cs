using System;
using System.Collections.Generic;
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

        //Using a DependencyProperty as the backing store for CurrentEngineer.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerUserProperty =
            DependencyProperty.Register("CurrentEngineerUser", typeof(BO.Engineer), typeof(EngineerWindow), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the engineer that is being added or updated in this window
        /// </summary>
        public BO.Engineer CurrentEngineerUser
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerUserProperty); }
            set { SetValue(CurrentEngineerUserProperty, value); }
        }


        public BO.Task CurrentAssignedTask
        {
            get { return (BO.Task)GetValue(CurrentAssignedTaskProperty); }
            set { SetValue(CurrentAssignedTaskProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentAssignedTask.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentAssignedTaskProperty =
            DependencyProperty.Register("CurrentAssignedTask", typeof(BO.Task), typeof(EngineerWindow), new PropertyMetadata(null));

        public static readonly DependencyProperty TaskDetailsProperty =
    DependencyProperty.Register("TaskDetails", typeof(BO.Task), typeof(EngineerWindow),new PropertyMetadata(null));

        public BO.Task TaskDetails
        {
            get { return (BO.Task)GetValue(TaskDetailsProperty); }
            set { SetValue(TaskDetailsProperty, value); }
        }
        public IEnumerable<BO.TaskInList> AvailableTasks
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(AvailableTasksProperty); }
            set { SetValue(AvailableTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AvailableTasksProperty =
            DependencyProperty.Register("AvailableTasks", typeof(IEnumerable<BO.TaskInList>), typeof(EngineerWindow), new PropertyMetadata(null));
        public EngineerWindow(int id)
        {
            InitializeComponent();

            try
            {

                ///read the right engineer according to the given id
                CurrentEngineerUser = s_bl.Engineer.Read(id);
                if(CurrentEngineerUser.Task is not null)
                 CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
                else
                    CurrentAssignedTask=new BO.Task();
                AvailableTasks = (from item in s_bl.Task.ReadAll()
                                  where s_bl.Engineer.checkAssignedTask(item.Id) == true
                                  select item).ToList();




            }///if an exception was thrown from the read function, catch it and show a message box which explains the exception
            catch (BO.BlDoesNotExistException e)
            {
                MessageBox.Show($"Engineer with id: {id} does not exist", "Input Error!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

            this.DataContext = this;

        }

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
                    DeadlineDate = CurrentAssignedTask.DeadlineDate,
                    CompleteDate = DateTime.Now,
                    Deliverables = CurrentAssignedTask.Deliverables!,
                    Remarks = CurrentAssignedTask.Remarks!,
                    Engineer = CurrentAssignedTask.Engineer,
                    Complexity = CurrentAssignedTask.Complexity
                });
                CurrentEngineerUser = s_bl.Engineer.Read(CurrentEngineerUser.Id);
                
                if (CurrentEngineerUser.Task is not null)
                    CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
                AvailableTasks = (from item in s_bl.Task.ReadAll()
                                  where s_bl.Engineer.checkAssignedTask(item.Id) == true
                                  select item).ToList();



            }
            catch (Exception t)
            {
                MessageBox.Show(t.Message, "Input Error!",
                                                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

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
                    StartDate = DateTime.Now,
                    ForecastDate = CurrentAssignedTask.ForecastDate,
                    RequiredEffortTime = CurrentAssignedTask.RequiredEffortTime,
                    DeadlineDate = CurrentAssignedTask.DeadlineDate,
                    CompleteDate = CurrentAssignedTask.CompleteDate,
                    Deliverables = CurrentAssignedTask.Deliverables!,
                    Remarks = CurrentAssignedTask.Remarks!,
                    Engineer = new BO.EngineerInTask { Id = CurrentEngineerUser.Id, Name = CurrentEngineerUser.Name },
                    Complexity = CurrentAssignedTask.Complexity
                });
                CurrentEngineerUser = s_bl.Engineer.Read(CurrentEngineerUser.Id);
                if (CurrentEngineerUser.Task is not null)
                    CurrentAssignedTask = s_bl.Task.Read(CurrentEngineerUser.Task.Id);
            }catch(Exception ) { MessageBox.Show("please choose a task"); }
        }         

        private void EditEngineer_Click(object sender, RoutedEventArgs e)
        {
            new AddUpdateEngineer(CurrentEngineerUser.Id).ShowDialog();
            CurrentEngineerUser = s_bl.Engineer.Read(CurrentEngineerUser.Id);

        }

        private void EditTask_Click(object sender, RoutedEventArgs e)
        {
            if(CurrentEngineerUser.Task is not null)
            new AddUpdateTask(CurrentEngineerUser.Task.Id).ShowDialog();
        }
        private void tasksForEngineer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tasksForEngineer.SelectedItem is BO.TaskInEngineer selectedTask)
            {
                TaskDetails = s_bl.Task.Read(selectedTask.Id);
            }
        }



    }
}
