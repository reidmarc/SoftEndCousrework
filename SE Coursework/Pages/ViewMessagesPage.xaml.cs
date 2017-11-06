using SE_Coursework.Classes;
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
    /// Interaction logic for ViewMessagesPage.xaml
    /// </summary>
    public partial class ViewMessagesPage : Page        
    {
        ValidationClass validation = new ValidationClass();
        JsonClass json = new JsonClass();



        public ViewMessagesPage()
        {
            InitializeComponent();
            DisplayNewMessage();
        }


        #region Click Events

        private void nextButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayNewMessage();
        }

        #endregion


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


        #region Private Methods

        private void DisplayNewMessage()
        {
            //MessageClass message = json.Deserialize();            
            
            //messageHeaderTxt.Text = message.Header;
            //messageBodyTxt.Text = message.MessageText;    
        }

        #endregion

    }
}
