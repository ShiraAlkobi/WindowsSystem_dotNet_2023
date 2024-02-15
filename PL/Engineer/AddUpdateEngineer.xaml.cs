using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for AddUpdateEngineer.xaml
    /// </summary>
    public partial class AddUpdateEngineer : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        public BO.Engineer CurrentEngineer
        {
            get { return (BO.Engineer)GetValue(CurrentEngineerProperty); }
            set { SetValue(CurrentEngineerProperty, value); }
        }

        //Using a DependencyProperty as the backing store for CurrentEngineer.This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(AddUpdateEngineer), new PropertyMetadata(0));

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;

        public AddUpdateEngineer(int id = 0)
        {
            InitializeComponent();
            BO.Engineer t_engineer;
            if (id == 0)
            {
                t_engineer = new BO.Engineer();
            }
            else
            {
                try
                {
                    t_engineer = s_bl.Engineer.Read(id);
                }
                catch (BO.BlDoesNotExistException e)
                {
                    MessageBox.Show($"Engineer with id: {id} does not exist", "Input Error!",
                                                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }



        }
        public class intToVisibilityConverterUpdate : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int intValue = (int)value;
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
        public class intToVisibilityConverterAdd : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int intValue = (int)value;
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

        private void AddEngineer_Click(object sender, RoutedEventArgs e)
        {
            BO.Engineer newEngineer = new()
            {
                Id = CurrentEngineer.Id,
                Email = CurrentEngineer.Email,
                Cost = CurrentEngineer.Cost,
                Name = CurrentEngineer.Name,
                Level = CurrentEngineer.Level
            };
            try
            {
                s_bl.Engineer.Create(newEngineer);
                MessageBox.Show("Engineer added successfully");
                Close();
            }
            catch (BO.BlInputCheckException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlAlreadyExistsException ex) { MessageBox.Show(ex.Message); }

        }

        private void UpdateEngineer_Click(object sender, RoutedEventArgs e)
        {
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
                s_bl.Engineer.Update(newEngineer);
                MessageBox.Show("Engineer updated successfully");
                Close();
            }
            catch (BO.BlInputCheckException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlDoesNotExistException ex) { MessageBox.Show(ex.Message); }
            catch (BO.BlCanNotUpdate ex) { MessageBox.Show(ex.Message); }

        }
    }
}
