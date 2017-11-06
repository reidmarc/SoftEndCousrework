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
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        ValidationClass menuValidation = new ValidationClass();





        public MenuPage()
        {
            InitializeComponent(); 
        }
        

        #region Click Events

        private void manuallyInputButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            InputManuallyPage inputManPage = new InputManuallyPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(inputManPage);
        }

        private void autoInputButton_Click(object sender, RoutedEventArgs e)
        {
            // later
        }

        private void viewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the ViewMessages Page           
            ViewMessagesPage viewMessages = new ViewMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(viewMessages);

        }

        private void exportJson_Click(object sender, RoutedEventArgs e)
        {

        }


        #endregion
        
        #region Exit Button

        // Method which handles the 'Exit Application' button being clicked
        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method ExitApplicationValidation() from the Validation class.
            menuValidation.ExitApplicationValidation();
        }




        #endregion
                
    }
}
