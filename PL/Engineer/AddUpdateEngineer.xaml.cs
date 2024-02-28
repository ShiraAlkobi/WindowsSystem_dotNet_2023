using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Windows.Shapes;
using BO;

namespace PL.Engineer
{
    /// <summary>
    /// Interaction logic for AddUpdateEngineer.xaml
    /// </summary>
    public partial class AddUpdateEngineer : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        //Using a DependencyProperty as the backing store for CurrentEngineer.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(AddUpdateEngineer), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the engineer that is being added or updated in this window
        /// </summary>
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        public IEnumerable<BO.TaskInEngineer> AllTasks
        {
            get { return (IEnumerable<BO.TaskInEngineer>)GetValue(AllTasksProperty); }
            set { SetValue(AllTasksProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllTasksProperty =
            DependencyProperty.Register("AllTasks", typeof(IEnumerable<BO.TaskInEngineer>), typeof(AddUpdateEngineer), new PropertyMetadata(null));
        public BO.ProjectStatus CurrentProjectStatusEngineer
        {
            get { return (BO.ProjectStatus)GetValue(CurrentProjectStatusEngineerProperty); }
            set { SetValue(CurrentProjectStatusEngineerProperty, value); }
        }
        ///Using a DependencyProperty as the backing store for CurrentProjectStatus.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProjectStatusEngineerProperty =
            DependencyProperty.Register("CurrentProjectStatusEngineer", typeof(BO.ProjectStatus), typeof(AddUpdateTask), new PropertyMetadata(null));
 


        /// <summary>
        /// a property for the experience of engineer - the level field 
        /// </summary>
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;

        /// <summary>
        /// ctor for the window
        /// </summary>
        /// <param name="id"> the id of the engineer - when updating the id will be the engineer to update id
        /// when adding the id will be 0 - default value
        /// </param>
        public AddUpdateEngineer(int id = 0)
        {
           
            InitializeComponent();
            CurrentProjectStatusEngineer = s_bl.getProjectStatus();
            AllTasks = (from item in s_bl.Task.ReadAll()
                            select new BO.TaskInEngineer() { Id = item.Id, Alias = item.Alias }).ToList();
            
            if (id == 0)
            {
                ///create an engineer with default values
                CurrentEngineer = new BO.Engineer();

            }
            else
            {
                try
                {
                    ///read the right engineer according to the given id
                    CurrentEngineer = s_bl.Engineer.Read(id);
  


                }///if an exception was thrown from the read function, catch it and show a message box which explains the exception
                catch (BO.BlDoesNotExistException e)
                {
                    MessageBox.Show($"Engineer with id: {id} does not exist", "Input Error!",
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
        private void AddEngineer_Click(object sender, RoutedEventArgs e)
        {
           
            ///create an new engineer according to the inserted values
            BO.Engineer newEngineer = new()
            {
                Id = CurrentEngineer.Id,
                Email = CurrentEngineer.Email,
                Cost = CurrentEngineer.Cost,
                Name = CurrentEngineer.Name,
                Level = CurrentEngineer.Level,
                Task=CurrentEngineer.Task,
            };
            try
            {
                ///add the new engineer
                s_bl.Engineer.Create(newEngineer);
                ///notify on the adding
                MessageBox.Show("Engineer added successfully");
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
        private void UpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentEngineer.Task != null)
            {
                BO.Task task = s_bl.Task.Read(CurrentEngineer.Task.Id);
                s_bl.Task.Update(new()
                {
                    Id = task.Id,
                    Alias = task.Alias,
                    Description = task.Description,
                    Dependencies = task.Dependencies,
                    Status = task.Status,
                    CreatedAtDate = task.CreatedAtDate,
                    ScheduledDate = task.ScheduledDate,
                    StartDate = DateTime.Now,
                    ForecastDate = task.ForecastDate,
                    RequiredEffortTime = task.RequiredEffortTime,
                    DeadlineDate = task.DeadlineDate,
                    CompleteDate = task.CompleteDate,
                    Deliverables = task.Deliverables!,
                    Remarks = task.Remarks!,
                    Engineer = task.Engineer,
                    Complexity = task.Complexity
                });
            }
            ///create an new engineer according to the inserted values
            BO.Engineer newEngineer = new()
            {
                Id = CurrentEngineer.Id,
                Email = CurrentEngineer.Email,
                Cost = CurrentEngineer.Cost,
                Name = CurrentEngineer.Name,
                Level = CurrentEngineer.Level,
                Task = CurrentEngineer.Task
            };
            try
            {
                ///update the engineer
                s_bl.Engineer.Update(newEngineer);
                ///notify on the updating
                MessageBox.Show("Engineer updated successfully");
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
    }
}
