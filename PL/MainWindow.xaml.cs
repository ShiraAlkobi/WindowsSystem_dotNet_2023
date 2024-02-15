﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BlApi;
using PL.Engineer;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow()
        {
            InitializeComponent();            
        }
        /// <summary>
        /// by clicking on "handle engineer" button, a new window will appear with the engineers details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_EngineerList_Click(object sender, RoutedEventArgs e)
        {
            new EngineerListWindow().Show();//create new window and show
        }
        /// <summary>
        /// by clicking on "initialize date" button, this function will show a messege box asking 
        /// if the user is sure he wants to initailize, if he clicls yes, the data will be reset and initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_Initialization_Click(object sender, RoutedEventArgs e)
        {
           MessageBoxResult result= MessageBox.Show("Are you sure you want to initialize data? ","",MessageBoxButton.YesNo,MessageBoxImage.Question); 
            if (result == MessageBoxResult.Yes)
            {
                Factory.Get().ResetDB();
                Factory.Get().InitializeDB();
                MessageBox.Show("Data initialized!");
            }
            
        }
    }
}