using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;
using System.Windows.Media;

namespace PL
{
    /// <summary>
    /// this file contains all of the convert classes used in the XAML files
    /// </summary>

    /// <summary>
    /// converts from Project's status to IsEnabled attribute
    /// we can make the Task required effort time textBox close for changes when the status is execution stage
    /// </summary>

    public class ProjectStatusToBoolConverter : IValueConverter
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ///if we are in plan stage of the project-you can update.else-not.
            int intValue = (int)value;
            if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
            {
                return true;
            }

            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class StatusToColorConverter : IValueConverter
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            ///if we are in plan stage of the project-you can update.else-not.
            BO.Status status = (BO.Status)value;
           

            if (status == BO.Status.Scheduled)
            {
                return Brushes.CornflowerBlue;
            }
            if (status == BO.Status.OnTrack)
            {
                return Brushes.Yellow;
            }
            if (status == BO.Status.Done)
            {
                return Brushes.LimeGreen;
            }
            else return Brushes.Red;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// converts from Project's status to IsEnabled attribute - just like the previos convertor but the logic is different
    /// we can make the Task start date textBox close for changes when the status is plan stage
    /// </summary>
    public class CompleteDateToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime? completeDate = (DateTime?)value;

            ///if the project's status is the plan stage, the date is not enabled to change
            if (completeDate == null)
            {
                return true;
            }
            ///if the project's status is the execution stage, the date is enabled to change
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AssignedTaskToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.TaskInEngineer? AssignedTask = (BO.TaskInEngineer)value;

            ///if the project's status is the plan stage, the date is not enabled to change
            if (AssignedTask != null)
            {
                return true;
            }
            ///if the project's status is the execution stage, the date is enabled to change
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class AssignedTaskToBoolConverterReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.TaskInEngineer? AssignedTask = (BO.TaskInEngineer)value;

            ///if the project's status is the plan stage, the date is not enabled to change
            if (AssignedTask == null)
            {
                return true;
            }
            ///if the project's status is the execution stage, the date is enabled to change
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class ProjectStatusToIsEnabledConverterReverse : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BO.ProjectStatus projectStatus = (BO.ProjectStatus)value;

            ///if the project's status is the plan stage, the date is not enabled to change
            if (projectStatus == BO.ProjectStatus.PlanStage)
            {
                return false;
            }
            ///if the project's status is the execution stage, the date is enabled to change
            else
            {
                return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    public class IntToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            ///if the id is 0 - we add an engineer, and the id can be inserted
            if (intValue == 0)
            {
                return true;
            }
            ///if the id is not 0 - we update an engineer, and the id can't be inserted
            else
            {
                return false;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// converts from Task id (int) to IsEnabled attribute
    /// we can make the Engineer Task's textBox close for changes when adding
    /// </summary>
    public class TaskToIsEnabledConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            ///if the id is 0 - we add an engineer, and the task can't be inserted
            if (intValue == 0)
            {
                return false;
            }
            ///if the id is not 0 - we update an engineer, and the task can be inserted
            else
            {
                return true;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// converts id to visibility attribute of the button for updating an engineer 
    /// </summary>
    public class IntToVisibilityConverterUpdate : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            ///if the id is 0 - we want to add, so the update button needs to be hidden
            if (intValue == 0)
            {
                return Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// converts id to visibility attribute of the button for adding an engineer 
    /// </summary>
    public class IntToVisibilityConverterAdd : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int intValue = (int)value;
            ///if the id is 0 - we want to add, so the add button needs to be visible
            if (intValue == 0)
            {
                return Visibility.Visible;
            }
            else
            {
                return Visibility.Collapsed;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    

}