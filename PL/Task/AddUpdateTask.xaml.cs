﻿using BO;
using PL.Engineer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        #region dependency properties

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

        /// <summary>
        /// dependency property for the tasks that are selected as the task's dependencies
        /// this is an observable collection so the updating can be faster and immidiate
        /// </summary>
        public ObservableCollection<BO.TaskInList>? SelectedDependencies
        {
            get { return (ObservableCollection<BO.TaskInList>)GetValue(selectedDependenciesProperty); }
            set { SetValue(selectedDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty selectedDependenciesProperty =
            DependencyProperty.Register("SelectedDependencies", typeof(ObservableCollection<BO.TaskInList>), typeof(AddUpdateTask), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the tasks that are not selected as the task's dependencies - or can't be selected because of circular dependency
        /// this is an observable collection so the updating can be faster and immidiate
        /// </summary>
        public IEnumerable<BO.TaskInList> NotSelectedDependencies
        {
            get { return (IEnumerable<BO.TaskInList>)GetValue(NotSelectedDependenciesProperty); }
            set { SetValue(NotSelectedDependenciesProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NotSelectedDependencies.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NotSelectedDependenciesProperty =
            DependencyProperty.Register("NotSelectedDependencies", typeof(IEnumerable<BO.TaskInList>), typeof(AddUpdateTask), new PropertyMetadata(null));

        /// <summary>
        /// dependency property for the project's status
        /// </summary>
        public BO.ProjectStatus CurrentProjectStatusTask
        {
            get { return (BO.ProjectStatus)GetValue(CurrentProjectStatusTaskProperty); }
            set { SetValue(CurrentProjectStatusTaskProperty, value); }
        }
        ///Using a DependencyProperty as the backing store for CurrentProjectStatus.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentProjectStatusTaskProperty =
            DependencyProperty.Register("CurrentProjectStatusTask", typeof(BO.ProjectStatus), typeof(AddUpdateTask), new PropertyMetadata(null));

        #endregion

        /// <summary>
        /// ctor for the window
        /// </summary>
        /// <param name="id"> the id of the engineer - when updating the id will be the engineer to update id
        /// when adding the id will be 0 - default value
        /// </param>
        public AddUpdateTask(int id = 0)
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception) { MessageBox.Show("Can't Open Window, Error"); return; }
            CurrentProjectStatusTask = s_bl.getProjectStatus();

            if (id == 0)
            {
                ///create an engineer with default values
                CurrentTask = new BO.Task() { CreatedAtDate = s_bl.getClock() };
                SelectedDependencies = null;
                ///if we're in an add window, all of the tasks can be dependencies
                NotSelectedDependencies = s_bl.Task.ReadAll();
            }
            else
            {
                try
                {
                    ///read the right task according to the given id
                    CurrentTask = s_bl.Task.Read(id);
                    SelectedDependencies = new ObservableCollection<TaskInList>(CurrentTask?.Dependencies);

                    ///if we're in an update window, in order to prevent circular dependency - 
                    ///the tasks list for dependencies have to contain only the ones that doesn't depend on the cuurent task
                    NotSelectedDependencies = (from item in s_bl.Task.ReadAll(t => t.Id != id)
                                               where s_bl.Task.getDependencies(id).Any(b => b.Id == item.Id) == false
                                               where s_bl.Task.CircularDependency(item.Id, id)
                                               select item).ToList();
                   
                }///if an exception was thrown from the read function, catch it and show a message box which explains the exception
                catch (BO.BlDoesNotExistException e)
                {
                    MessageBox.Show($"Task with id: {id} does not exist", "Input Error!",
                                                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            ///because there're several dependency properties, the data context needs to be the whole window
            this.DataContext = this;
        }

        #region help functions and event handlers

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
                int id = s_bl.Task.Create(newTask);
                CurrentTask= s_bl.Task.Read(id);
                if (SelectedDependencies is not null)
                {
                    foreach (var item in SelectedDependencies)
                    {
                        s_bl.Task.UpdateDependencies(item.Id, id);
                    }
                }
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
                CreatedAtDate = s_bl.getClock(),
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

        /// <summary>
        /// Check if the input is a numeric character - preventing from writing letters where only numbers are needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckNumInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsNumeric(e.Text))
            {
                e.Handled = true; // Mark the event as handled, preventing the character from being added to the TextBox
            }
        }

        /// <summary>
        /// returns whether the text contains only numbers
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        /// <summary>
        /// when clicking the plus button, a dependency is added so update the data base and the collections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDependency_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            if (button != null && button.DataContext is BO.TaskInList selectedObject)
            {
                if(CurrentTask.Id==0)
                {
                    if (SelectedDependencies is null)
                        SelectedDependencies = new ObservableCollection<TaskInList>();

                    SelectedDependencies.Add(selectedObject); 
                    NotSelectedDependencies = NotSelectedDependencies.Where(item => item.Id != selectedObject.Id).ToList();
                    return;
                }
                if(CurrentTask.Dependencies is null)
                {
                    CurrentTask.Dependencies = new List<TaskInList>();
                }
                CurrentTask.Dependencies.Add(selectedObject);
                s_bl.Task.UpdateDependencies(selectedObject.Id, CurrentTask.Id);
                NotSelectedDependencies = NotSelectedDependencies.Where(item => item.Id != selectedObject.Id).ToList();
                SelectedDependencies = new ObservableCollection<TaskInList>(CurrentTask?.Dependencies);
            }
        }

        /// <summary>
        /// when clicking the minus button, a dependency is removed, so update the data base and the collections
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDependency_Click(object sender, RoutedEventArgs e)
        {


            Button button = (Button)sender;
            if (button != null && button.DataContext is BO.TaskInList selectedObject)
            {
                CurrentTask.Dependencies.Remove(selectedObject);
                s_bl.Task.DeleteDependencies(selectedObject.Id, CurrentTask.Id);
                NotSelectedDependencies = NotSelectedDependencies.Concat(new[] { selectedObject }).ToList();
                SelectedDependencies = new ObservableCollection<TaskInList>(CurrentTask?.Dependencies);
            }
        }

        ///<summary>
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