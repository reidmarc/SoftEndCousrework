//////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////// Page ViewMessagesPage ////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

#region Usings

using SE_Coursework.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace SE_Coursework.Pages
{
    /// <summary>
    /// Interaction logic for ViewMessagesPage.xaml
    /// </summary>
    public partial class ViewMessagesPage : Page        
    {
        #region Objects / Data Structure / Variable

        ValidationClass validation = new ValidationClass();
        JsonClass json = new JsonClass();

        List<MessageClass> listOfMessages = new List<MessageClass>();

        int displayCounter = 0;

        #endregion

        #region Constructor

        public ViewMessagesPage()
        {
            InitializeComponent();
            RetrieveStoredList();
            DisplayInitialMessage();            
        }

        #endregion

        #region Click Events

        /// <summary>
        /// Method which calls the method DisplayNextMessage(), when clicked
        /// </summary>       
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayNextMessage();            
        }

        /// <summary>
        /// Method which calls the method DisplayPreviousMessage(), when clicked
        /// </summary>      
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayPreviousMessage();
        }

        #endregion

        #region Navigation Buttons

        /// <summary>
        /// Method which handles the 'Back' button being clicked
        /// </summary>
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            MenuPage menuPage = new MenuPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(menuPage);
        }

        /// <summary>
        /// Method which handles the 'Exit Application' button being clicked
        /// </summary>      
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method ExitApplicationValidation() from the Validation class.
            validation.ExitApplicationValidation();
        }

        #endregion

        #region Private Methods
        
        /// <summary>
        /// This method displays the first message that has been stored in the Json file.
        /// </summary>
        private void DisplayInitialMessage()
        {
            if (displayCounter < (listOfMessages.Count - 1))
            {    
                messageHeaderTxt.Text = listOfMessages[displayCounter].Header;
                messageSenderTxt.Text = listOfMessages[displayCounter].Sender;
                messageSubjectTxt.Text = listOfMessages[displayCounter].Subject;
                messageBodyTxt.Text = listOfMessages[displayCounter].MessageText;
            }
            else
            {
                MessageBox.Show("There are no more messages in the list to view.");
            }

        }

        /// <summary>
        /// This method displays the next message that has been stored in the Json file.
        /// </summary>
        private void DisplayNextMessage()
        {
            if (displayCounter < (listOfMessages.Count - 1))
            {
                displayCounter = displayCounter + 1;

                messageHeaderTxt.Text = listOfMessages[displayCounter].Header;
                messageSenderTxt.Text = listOfMessages[displayCounter].Sender;
                messageSubjectTxt.Text = listOfMessages[displayCounter].Subject;
                messageBodyTxt.Text = listOfMessages[displayCounter].MessageText;
            }
            else
            {
                MessageBox.Show("There are no more messages in the list to view.");
            }
        }

        /// <summary>
        /// This method displays the previous message that has been stored in the Json file.
        /// </summary>
        private void DisplayPreviousMessage()
        {            
            if (displayCounter > 0)
            {
                displayCounter = displayCounter - 1;

                messageHeaderTxt.Text = listOfMessages[displayCounter].Header;
                messageSenderTxt.Text = listOfMessages[displayCounter].Sender;
                messageSubjectTxt.Text = listOfMessages[displayCounter].Subject;
                messageBodyTxt.Text = listOfMessages[displayCounter].MessageText;
            }
            else
            {
                MessageBox.Show("You are at the start of the list.");
            }            
        }

        /// <summary>
        /// This method retrieves the values from the stored Json file as a list of Json objects
        /// </summary>
        public void RetrieveStoredList()
        {
            int counter = 0;

            try
            {
                // Returns the list that is stored as JSON             
                listOfMessages = json.Deserialize();

                counter = counter + 1;                
            }
            catch (Exception ex)
            {
                if (counter > 0)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        #endregion       
    }
}
