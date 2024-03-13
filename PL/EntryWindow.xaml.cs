using PL.Engineer;
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

namespace PL
{
    /// <summary>
    /// Interaction logic for EntryWindow.xaml
    /// </summary>
    public partial class EntryWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();//giving us access to bl functions

        #region dependency properties

        /// <summary>
        /// dependency property for the task that is being added or updated in this window
        /// </summary>
        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurredntUserProperty); }
            set { SetValue(CurredntUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurredntUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurredntUserProperty =
            DependencyProperty.Register("CurredntUser", typeof(BO.User), typeof(EntryWindow), new PropertyMetadata(null));

        #endregion

        public EntryWindow()
        {
            InitializeComponent();
            CurrentUser = new BO.User();                           
            this.DataContext = this;
        }

        #region help functions and event handlers        

        /// <summary>
        /// event handler for got focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserName_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter User Name")
            {
                textBox.Text = "";
            }
        }

        /// <summary>
        /// event handler for lost focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter User Name";
            }
        }

        /// <summary>
        /// event handler for got focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter Password")
            {
                textBox.Text = "";
            }
        }

        /// <summary>
        /// event handler for got focus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter Password";
            }
        }

        /// <summary>
        /// prevents typing letters when only numbers are needed - in user name
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public bool ContainsOnlyLetters(string input)
        {
            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    return false; // Return false if any character is not a letter
                }
            }
            return true; // Return true if all characters are letters
        }

        /// <summary>
        /// returns whether the string contains only numbers
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        /// <summary>
        /// checking for correct user name and password when clicking logIn button - when inserting wrong input, messages will be shown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_LogIn_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (string.IsNullOrEmpty(CurrentUser.UserName))
                {
                    MessageBox.Show("Please insert user name", "", MessageBoxButton.OK);
                    return;
                }
                else if (string.IsNullOrEmpty(CurrentUser.Password))
                {
                    MessageBox.Show("Please insert password", "", MessageBoxButton.OK);
                    return;
                }

                if (!IsNumeric(CurrentUser.UserName))
                {
                    MessageBox.Show("User name have to contain only numbers", "", MessageBoxButton.OK);
                    return;
                }

                int userId = Convert.ToInt32(CurrentUser.UserName);

            
                BO.User t_user = s_bl.User.Read(userId);
                if (t_user.Password != CurrentUser.Password)
                {
                    MessageBox.Show("Password is wrong, try again", "", MessageBoxButton.OK);
                }
                else
                {
                    if (t_user.Position == BO.Position.Manager)
                    {
                        new MainWindow().ShowDialog();
                        Close();
                    }
                    else
                    {
                        if (s_bl.getProjectStatus() == BO.ProjectStatus.PlanStage)
                        {
                            MessageBox.Show("Engineer can not enter the system in this stage.", "", MessageBoxButton.OK, MessageBoxImage.Stop);
                        }
                        
                        else new EngineerWindow(t_user.Id).Show();
                    }
                }
            }
            ///user id was not found - means the user name is definately wrong
            catch (BO.BlDoesNotExistException)
            {
                MessageBox.Show("User Name or Password are wrong, try again", "", MessageBoxButton.OK);
            }            
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
