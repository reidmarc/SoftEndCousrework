//////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////// Page InputMessagePage ////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

#region Usings

using SE_Coursework.Classes;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace SE_Coursework.Pages
{
    /// <summary>
    /// Interaction logic for InputManuallyPage.xaml
    /// </summary>
    public partial class InputMessagesPage : Page
    {
        #region Objects / Data Structure / Variables

        ValidationClass validation = new ValidationClass();
        JsonClass json = new JsonClass();
        ProcessingClass processing = new ProcessingClass();

        List<string> importList = new List<string>();

        bool IsTheDataImported = false;     

        private string processedText = string.Empty;

        int importCounter = 0;

        #endregion

        #region Constructor

        // Constructor
        public InputMessagesPage()
        {
            InitializeComponent();
            validation.RetrieveStoredList();
            processing.GetTextWords();
            processing.GetHashTags();
            processing.GetMentions();
            processing.GetSIR();
        }

        #endregion

        #region Click Events

        /// <summary>
        /// This click event method deals with how the message is formatted then previewed before being saved
        /// </summary>        
        private void ConvertButton_Click(object sender, RoutedEventArgs e)
        {           
            // Validates the message header
            if (validation.MessageHeaderInputValidation(messageHeaderTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the header incorrectly.");
                messageHeaderTxt.Focus();
                return;
            }

            // Validates the message body
            if (validation.MessageBodyInputValidation(messageBodyTxt.Text.Trim()).Equals(false))
            {
                MessageBox.Show("You have entered the body incorrectly.");
                messageBodyTxt.Focus();
                return;
            }
            
            // Enables the 'save button'
            saveButton.IsEnabled = true;   

            string text = validation.Text;
            string header = validation.Header;
            string subject = validation.Subject;


            processing.MessageProcessing(header, ref text, subject);

            // Splits the text up into different textboxes for the user to preview the information
            // Before saving
            convertedMessageHeaderTxt.Text = validation.Header;
            convertedMessageSenderTxt.Text = validation.Sender;
            convertedMessageSubjectTxt.Text = validation.Subject;
            convertedMessageBodyTxt.Text = text.Trim();
            processedText = text.Trim();  
        }

        /// <summary>
        /// This click event method deals with how the message is saved
        /// </summary>       
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {           
            // Sets the path as a string
            string path = @".\EustonLeisureMessages.json";

            // If the message is a Tweet, then the method SearchForHashTagsAndMentions() is called
            if (validation.Header.StartsWith("T"))
            {
                processing.SearchForHashTagsAndMentions(processedText);
            }

            // If the message is an Email, then the method SearchForSIR() is called
            if (validation.Header.StartsWith("E") && validation.Subject.StartsWith("SIR"))
            {
                processing.SearchForSIR(processedText);
            }

            // Adds the message to a list
            validation.AddMessageToList(processedText);

            // Converts the whole list of messages into JSON and stores it
            json.Serialize(validation.listOfMessages, path);

            // Sets the save button to IsEnabled = false
            saveButton.IsEnabled = false;

            validation.EndOfCycle();

            // Clears the textboxes
            convertedMessageHeaderTxt.Text = string.Empty;
            convertedMessageSenderTxt.Text = string.Empty;
            convertedMessageSubjectTxt.Text = string.Empty;
            convertedMessageBodyTxt.Text = string.Empty;
            messageHeaderTxt.Text = string.Empty;
            messageBodyTxt.Text = string.Empty;    
        }


        /// <summary>
        /// This click event method deals with the application importing infomation from a textfile
        /// When the user clicks the button
        /// </summary>       
        private void ImportFile_Click(object sender, RoutedEventArgs e)
        {
            if (IsTheDataImported.Equals(false))
            {
                using (var reader = new StreamReader(@".\testdata.txt"))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        importList.Add(line.ToString());
                    }
                }

                IsTheDataImported = true;
            }

            // Once a file is uploaded the content of the button changes to next message
            importFile.Content = "Next Message";                     

            SplitImportedData();
        }

        #endregion

        #region Private Method

        /// <summary>
        /// This method splits the data into 2 blocks of text from the list called importList
        /// It allows the user to cycle through the imported messages.
        /// </summary>
        private void SplitImportedData()
        { 
            // This IF statement stop an out of range exception
            if (importCounter < importList.Count)
            {
                string lineString = importList[importCounter];

                int firstCommaIndex = lineString.Trim().IndexOf(",");
                string headerString = lineString.Substring(0, firstCommaIndex);
                string bodyString = lineString.Substring(firstCommaIndex + 1);

                messageHeaderTxt.Text = headerString.Trim();
                messageBodyTxt.Text = bodyString.Trim();

                importCounter = importCounter + 1;
            }
            else
            {
                MessageBox.Show("There are no more messages to import.");
            }
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
        
    }
}
