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

        public BO.User CurrentUser
        {
            get { return (BO.User)GetValue(CurredntUserProperty); }
            set { SetValue(CurredntUserProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurredntUser.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurredntUserProperty =
            DependencyProperty.Register("CurredntUser", typeof(BO.User), typeof(EntryWindow), new PropertyMetadata(null));



        public EntryWindow()
        {
            InitializeComponent();

            CurrentUser = new BO.User();

                
            this.DataContext = this;
        }

        private void btn_AdminWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }

        private void btn_EngineerWindow_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(CurrentUser.Id).Show();
        }


        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Enter User Name")
            {
                textBox.Text = "";
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                textBox.Text = "Enter User Name";
            }
        }

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

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void btn_LogIn_Click(object sender, RoutedEventArgs e)
        {           
            int userId= Convert.ToInt32(CurrentUser.UserName);

            try
            {
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

    }
}
