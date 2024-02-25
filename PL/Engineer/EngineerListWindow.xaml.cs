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
    /// Interaction logic for EngineerListWindow.xaml
    /// </summary>
    public partial class EngineerListWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();//giving us access to bl functions
        public BO.EngineerExperience Experience { get; set; } = BO.EngineerExperience.All;//experience field has defualt
        public IEnumerable<BO.Engineer> EngineerList//the list of engineers
        {
            get { return (IEnumerable<BO.Engineer>)GetValue(EngineerListProperty); }
            set { SetValue(EngineerListProperty, value); }
        }
        /// <summary>
        /// dependency propert that gets all engineers fields to the control list
        /// </summary>
        public static readonly DependencyProperty EngineerListProperty =
            DependencyProperty.Register("EngineerList", typeof(IEnumerable<BO.Engineer>), typeof(EngineerListWindow), new PropertyMetadata(null));
        /// <summary>
        /// the user can change the selection of engineers in the combobox to engineers with certain level
        /// the function will call readall again and the filter parameter will be the items the are the level as the one selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbExperienceSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EngineerList = (Experience == BO.EngineerExperience.All) ?
                s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!;
        }
        public EngineerListWindow()
        {
            InitializeComponent();
            EngineerList = s_bl?.Engineer.ReadAll()!;//rereading the engineerlist after updating or adding engineer
                                                     //because we want the list to be updated immidiatly

        }

        /// <summary>
        /// by clicking the add button a window will open with empty or default value to add an engineer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddEngineerWindow_Click(object sender, RoutedEventArgs e)
        {
            AddUpdateEngineer addUpdateEngineer=new AddUpdateEngineer();
            addUpdateEngineer.ShowDialog();
            EngineerList = (Experience == BO.EngineerExperience.All) ?
               s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!; ////rereading the engineerlist after updating or adding engineer
                                                     //because we want the list to be updated immidiatly
        }
        /// <summary>
        /// by double clicking an engineer, a window with its details will open to update
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateEngineerWindow_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            //create engineer with the details of the clicked engineer 
            BO.Engineer? engineer= (sender as ListView)?.SelectedItem as BO.Engineer;
            //create new window with id parameter from the clicked engineer
            AddUpdateEngineer addUpdateEngineer = new AddUpdateEngineer(engineer.Id);
            addUpdateEngineer.ShowDialog();//show the windo
            EngineerList = (Experience == BO.EngineerExperience.All) ?
                s_bl?.Engineer.ReadAll()! : s_bl?.Engineer.ReadAll(item => item.Level == Experience)!;//rereading the engineerlist after updating or adding engineer
                                                                                                      //because we want the list to be updated immidiatly
        }
    }
}
