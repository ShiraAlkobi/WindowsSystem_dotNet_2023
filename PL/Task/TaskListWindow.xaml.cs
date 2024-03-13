﻿using PL.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public BO.Status Status { get; set; } = BO.Status.Unscheduled;
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
        public IEnumerable<BO.EngineerInTask> Engineers
        {
            get { return (IEnumerable<BO.EngineerInTask>)GetValue(EngineersProperty); }
            set { SetValue(EngineersProperty, value); }
        }
        public BO.ProjectStatus ProjectStatusTaskList
        {
            get { return (BO.ProjectStatus)GetValue(ProjectStatusTaskListProperty); }
            set { SetValue(ProjectStatusTaskListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProjectStatus. 
        public static readonly DependencyProperty ProjectStatusTaskListProperty =
            DependencyProperty.Register("ProjectStatusTaskList", typeof(BO.ProjectStatus), typeof(MainWindow), new PropertyMetadata(null));

        // Using a DependencyProperty as the backing store for AllTasks.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EngineersProperty =
            DependencyProperty.Register("Engineers", typeof(IEnumerable<BO.EngineerInTask>), typeof(TaskListWindow), new PropertyMetadata(null));

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Content is BO.EngineerExperience selectedComplexity)
            {
                TaskList = (selectedComplexity == BO.EngineerExperience.All) ?
              s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Complexity == selectedComplexity)!;
            }
        }
        public TaskListWindow()
        {
            InitializeComponent();
            TaskList = s_bl?.Task.ReadAll()!;
            Engineers = from item in s_bl?.Engineer.ReadAll()
                        select new BO.EngineerInTask() { Id = item.Id, Name = item.Name };
            ProjectStatusTaskList = s_bl.getProjectStatus();
            this.DataContext = this;
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

       

 

        private void RadioButton_CheckedStatus(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Content is BO.Status selectedStatus)
            {
                TaskList = (selectedStatus == BO.Status.Unscheduled) ?
               s_bl?.Task.ReadAll()! : s_bl?.Task.ReadAll(item => item.Status == selectedStatus)!;
            }
        }

        private void RadioButton_CheckedEngineer(object sender, RoutedEventArgs e)
        {
            if (sender is RadioButton radioButton && radioButton.Content is BO.EngineerInTask selectedEngineer)
            {
                TaskList = from task in s_bl?.Task.ReadAll()
                           where (s_bl?.Task.Read(task.Id).Engineer?.Id == selectedEngineer.Id)
                           select task;
            }
        }

        private void resetFilter_Click(object sender, RoutedEventArgs e)
        {
            TaskList = s_bl?.Task.ReadAll();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}
