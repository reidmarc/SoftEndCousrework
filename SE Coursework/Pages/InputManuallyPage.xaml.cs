﻿using SE_Coursework.Classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SE_Coursework.Pages
{
    /// <summary>
    /// Interaction logic for InputManuallyPage.xaml
    /// </summary>
    public partial class InputManuallyPage : Page
    {
        ValidationClass validation = new ValidationClass();


        public InputManuallyPage()
        {
            InitializeComponent();
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {         

            if (validation.MessageHeaderInputValidation(messageHeaderTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the header incorrectly.");
                messageHeaderTxt.Focus();
                return;
            }

            if (validation.MessageBodyInputValidation(messageBodyTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the body incorrectly.");
                messageBodyTxt.Focus();
                return;
            }



        }



        #region Navigation Buttons

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            MenuPage menuPage = new MenuPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(menuPage);
        }

        // Method which handles the 'Exit Application' button being clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            

            // Calls the method ExitApplicationValidation() from the Validation class.
            validation.ExitApplicationValidation();
        }

        #endregion
        
    }
}
