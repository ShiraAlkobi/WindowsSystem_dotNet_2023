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

        // Using a DependencyProperty as the backing store for CurrentEngineer. This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentEngineerProperty =
            DependencyProperty.Register("CurrentEngineer", typeof(BO.Engineer), typeof(AddUpdateEngineer), new PropertyMetadata(0));

        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;

        public AddUpdateEngineer(int id=0)
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
                catch(BO.BlDoesNotExistException e) { MessageBox.Show($"Engineer with id: {id} does not exist","Input Error!",
                                                        MessageBoxButton.OK,MessageBoxImage.Error); }
            }



        }
    }
}
