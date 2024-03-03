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
        public string UserName
        {
            get { return (string)GetValue(userNameProperty); }
            set { SetValue(userNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty userNameProperty =
            DependencyProperty.Register("UserName", typeof(string), typeof(EntryWindow), new PropertyMetadata(null));

        public string Password
        {
            get { return (string)GetValue(passwordProperty); }
            set { SetValue(passwordProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty passwordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(EntryWindow), new PropertyMetadata(null));



        public EntryWindow()
        {
            InitializeComponent();
            UserName = "Enter User Name...";
            Password = "Enter Password...";

            this.DataContext = this;
        }

        private void btn_AdminWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().ShowDialog();
        }

        private void btn_EngineerWindow_Click(object sender, RoutedEventArgs e)
        {
            new EngineerWindow(12345678).ShowDialog();
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
            string user = UserName_text.Text;
            string password = Password_text.Text;
            int numPassword = 0;
            int.TryParse(password, out numPassword);
            bool flag = false;
            if (user == "Manager" && numPassword == 1)
            {
                new MainWindow().Show();
                Close();
            }
            else
            {
                
                
                    //if (!ContainsOnlyLetters(user))
                    //{
                    //    MessageBox.Show("The User Name must contain only letters! ", "User Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    UserName_text.Text = string.Empty;
                    //}
                    //if (!IsNumeric(password))
                    //{
                    //    MessageBox.Show("The Password must contain only numbers! ", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    Password_text.Text = string.Empty;
                    //}
                    //else
                    {
                        try
                        {
                            BO.Engineer t_enginer = s_bl.Engineer.Read(numPassword);
                            if (t_enginer.Name != user)
                            {
                                MessageBox.Show($"The user name - {user} does not exist in the project, please try again", "User Name Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                UserName_text.Text = string.Empty;
                            }
                            else flag = true;

                        }
                        catch (BO.BlDoesNotExistException ex)
                        {
                            MessageBox.Show("The Password is incorrect, please try again", "Password Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            Password_text.Text = string.Empty;
                        }
                    }
                }
                new EngineerWindow(numPassword).Show(); 
                Close();
            


        }
    }
}
