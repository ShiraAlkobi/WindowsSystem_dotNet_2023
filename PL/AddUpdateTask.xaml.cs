using PL.Engineer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Printing.IndexedProperties;
using System.Text;
using System.Text.RegularExpressions;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for AddUpdateTask.xaml
    /// </summary>
    public partial class AddUpdateTask : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

       
        /// <summary>
        /// dependency property for the engineer that is being added or updated in this window
        /// </summary>
        public BO.Task CurrentTask
        {
            get { return (BO.Task)GetValue(CurrentTaskProperty); }
            set { SetValue(CurrentTaskProperty, value); }
        }
        ///Using a DependencyProperty as the backing store for CurrentTask.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentTaskProperty =
            DependencyProperty.Register("CurrentTask", typeof(BO.Task), typeof(AddUpdateTask), new PropertyMetadata(null));



        public IEnumerable<BO.TaskInList>? SelectedDependencies
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(selectedDependenciesProperty); }
            set { SetValue(selectedDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selectedDependenciesProperty =
            DependencyProperty.Register("MyProperty", typeof(IEnumerable<BO.TaskInList>), typeof(AddUpdateTask), new PropertyMetadata(null));



        public IEnumerable<BO.TaskInList> NotSelectedDependencies
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(NotSelectedDependenciesProperty); }
            set { SetValue(NotSelectedDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedDependencies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedDependenciesProperty =
            DependencyProperty.Register("NotSelectedDependencies", typeof(IEnumerable<BO.TaskInList>), typeof(AddUpdateTask), new PropertyMetadata(null));

        public IEnumerable<BO.EngineerInTask> AllEngineers
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(AllEngineersProperty); }
            set { SetValue(AllEngineersProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllEngineers.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllEngineersProperty =
            DependencyProperty.Register("AllEngineers", typeof(IEnumerable<BO.EngineerInTask>), typeof(AddUpdateTask), new PropertyMetadata(null));


        private BO.EngineerInTask EngineerAssigned;
        public BO.EngineerInTask engineerAssigned
        {
            get { return EngineerAssigned; }
            set
            {
                EngineerAssigned = value;
                UpdateAssignedEngineer(EngineerAssigned);
            }
        }

        private void UpdateAssignedEngineer(BO.EngineerInTask e)
        {
            if(CurrentTask is not null&& e is not null)
            {
                BO.EngineerInTask? t_engineer = (from item in AllEngineers
                                                 where item.Name == e.Name
                                                 select item).FirstOrDefault();
                if (t_engineer is not null) 
                {
                    CurrentTask.Engineer = t_engineer;
                }
            }
        }

        public BO.ProjectStatus CurrentProjectStatus
        {
            get { return (BO.ProjectStatus)GetValue(CurrentProjectStatusProperty); }
            set { SetValue(CurrentProjectStatusProperty, value); }
        }
        ///Using a DependencyProperty as the backing store for CurrentProjectStatus.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProjectStatusProperty =
            DependencyProperty.Register("CurrentProjectStatus", typeof(BO.ProjectStatus), typeof(AddUpdateTask), new PropertyMetadata(null));
        public DateTime today {  get; set; } =  DateTime.Today.Date;

        
        /// <summary>
        /// ctor for the window
        /// </summary>
        /// <param name="id"> the id of the engineer - when updating the id will be the engineer to update id
        /// when adding the id will be 0 - default value
        /// </param>
        public AddUpdateTask(int id = 0)
        {
            InitializeComponent();
            CurrentProjectStatus = s_bl.getProjectStatus();
            AllEngineers = (from item in s_bl.Engineer.ReadAll()
                            select new BO.EngineerInTask() { Id = item.Id, Name = item.Name }).ToList();
            
            
            if (id == 0)
            {
                ///create an engineer with default values
                CurrentTask = new BO.Task() { CreatedAtDate = DateTime.Now };
                SelectedDependencies = null;
                ///if we're in an add window, all of the tasks can be dependencies
                NotSelectedDependencies = (from item in s_bl.Task.ReadAll()
                                  select new BO.TaskInList() { Id = item.Id, Alias = item.Alias }).ToList();

            }
            else
            {
                try
                {
                    ///read the right task according to the given id
                    CurrentTask = s_bl.Task.Read(id);
                    SelectedDependencies = CurrentTask.Dependencies;
                    today = CurrentTask.CreatedAtDate;
                    ///if we're in an update window, in order to prevent circular dependency - 
                    ///the tasks list for dependencies have to contain only the ones that doesn't depend on the cuurent task
                    NotSelectedDependencies = (from item in s_bl.Task.ReadAll(t => s_bl.Task.getDependencies(t.Id).Any(a => a.Id == id) == false)
                                               where s_bl.Task.getDependencies(id).Any(b => b.Id == item.Id) == false
                                               where item.Id != id
                                               select new BO.TaskInList() { Id = item.Id, Alias = item.Alias }).ToList();

                }///if an exception was thrown from the read function, catch it and show a message box which explains the exception
                catch (BO.BlDoesNotExistException e)
                {
                    MessageBox.Show($"Task with id: {id} does not exist", "Input Error!",
                                                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            

            this.DataContext = this;
        }

        /// <summary>
        /// an event when clicking the add button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddTask_Click(object sender, RoutedEventArgs e)
        {
            ///create an new engineer according to the inserted values
            BO.Task newTask = new()
            {
                Id = CurrentTask.Id,
                Alias = CurrentTask.Alias,
                Description = CurrentTask.Description,
                Dependencies = CurrentTask.Dependencies,
                Status = CurrentTask.Status,
                CreatedAtDate = CurrentTask.CreatedAtDate,
                ScheduledDate = CurrentTask.ScheduledDate,
                StartDate = CurrentTask.StartDate,
                ForecastDate = CurrentTask.ForecastDate,
                RequiredEffortTime = CurrentTask.RequiredEffortTime,
                CompleteDate = CurrentTask.CompleteDate,
                Deliverables = CurrentTask.Deliverables,
                Remarks = CurrentTask.Remarks,
                Engineer = CurrentTask.Engineer,
                Complexity = CurrentTask.Complexity
            };
            try
            {
                ///add the new engineer
                s_bl.Task.Create(newTask);
                ///notify on the adding
                MessageBox.Show("Task added successfully");
                ///close the window
                Close();
            }///if an exception was thrown from the create function, catch it and show a message box which explains the exception
            catch (BO.BlInputCheckException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlAlreadyExistsException ex) { MessageBox.Show(ex.Message); }

        }
        /// <summary>
        /// an event when clicking the update button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateTask_Click(object sender, RoutedEventArgs e)
        {
            ///create an new engineer according to the inserted values
            BO.Task newTask = new()
            {
                Id = CurrentTask.Id,
                Alias = CurrentTask.Alias,
                Description = CurrentTask.Description,
                Dependencies = CurrentTask.Dependencies,
                Status = CurrentTask.Status,
                CreatedAtDate = CurrentTask.CreatedAtDate,
                ScheduledDate = CurrentTask.ScheduledDate,
                StartDate = CurrentTask.StartDate,
                ForecastDate = CurrentTask.ForecastDate,
                RequiredEffortTime = CurrentTask.RequiredEffortTime,
                CompleteDate = CurrentTask.CompleteDate,
                Deliverables = CurrentTask.Deliverables,
                Remarks = CurrentTask.Remarks,
                Engineer = CurrentTask.Engineer,
                Complexity = CurrentTask.Complexity
            };
            try
            {
                ///update the engineer
                s_bl.Task.Update(newTask);
                ///notify on the updating
                MessageBox.Show("Task updated successfully");
                ///close the window
                Close();
            }///if an exception was thrown from the update function, catch it and show a message box which explains the exception
            catch (BO.BlInputCheckException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlDoesNotExistException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlCanNotUpdate ex) { MessageBox.Show(ex.Message); }

        }

        private void CheckNumInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the input is a numeric character
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Mark the event as handled, preventing the character from being added to the TextBox
            }
        }
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void AddDependency_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                if (button.DataContext != null)
                {
                    var selectedObject = button.DataContext; // Access the object passed as datacontext
                    BO.TaskInList t = (BO.TaskInList)selectedObject;
                    CurrentTask.Dependencies.Add((BO.TaskInList)selectedObject);
                    NotSelectedDependencies = (from item in s_bl.Task.ReadAll(t => s_bl.Task.getDependencies(t.Id).Any(a => a.Id == CurrentTask.Id) == false)
                                               where s_bl.Task.getDependencies(CurrentTask.Id).Any(b => b.Id == item.Id) == false
                                               where item.Id != CurrentTask.Id
                                               select new BO.TaskInList() { Id = item.Id, Alias = item.Alias }).ToList();
                    s_bl.Task.UpdateDependencies(CurrentTask.Id, t.Id);
                    SelectedDependencies = CurrentTask.Dependencies;
                    ExistedDependencies.ItemsSource = SelectedDependencies;
                    RemainedDependencies.ItemsSource = NotSelectedDependencies;
                }
            }
        }

        private void RemoveDependency_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null)
            {
                if (button.DataContext != null)
                {
                    var selectedObject = button.DataContext; // Access the object passed as CommandParameter
                    BO.TaskInList t = (BO.TaskInList)selectedObject;
                    CurrentTask.Dependencies?.Remove((BO.TaskInList)selectedObject);
                    NotSelectedDependencies = (from item in s_bl.Task.ReadAll(t => s_bl.Task.getDependencies(t.Id).Any(a => a.Id == CurrentTask.Id) == false)
                                               where s_bl.Task.getDependencies(CurrentTask.Id).Any(b => b.Id == item.Id) == false
                                               where item.Id != CurrentTask.Id
                                               select new BO.TaskInList() { Id = item.Id, Alias = item.Alias }).ToList();
                    s_bl.Task.DeleteDependencies(CurrentTask.Id, t.Id);
                    SelectedDependencies = CurrentTask.Dependencies;
                    ExistedDependencies.ItemsSource = SelectedDependencies;
                    RemainedDependencies.ItemsSource = NotSelectedDependencies;
                }

            }
        }
    }

    
}
