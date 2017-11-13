using SE_Coursework.Classes;
using System;
using System.Collections.Generic;
using System.IO;
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
        JsonClass json = new JsonClass();
        ProcessingClass processing = new ProcessingClass();

        List<string> importList = new List<string>();

        bool IsTheDataImported = false;

        int importCounter = 0;


        private string processedText = string.Empty;



        // Constructor
        public InputManuallyPage()
        {
            InitializeComponent();
            validation.RetrieveStoredList();
            processing.GetTextWords();
            processing.GetHashTags();
            processing.GetMentions();
            processing.GetSIR();
        }


        
        private void convertButton_Click(object sender, RoutedEventArgs e)
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
            
            saveButton.IsEnabled = true;   

            string text = validation.Text;
            string header = validation.Header;
            string subject = validation.Subject;


            processing.MessageProcessing(header, ref text, subject);


            convertedMessageHeaderTxt.Text = validation.Header;
            convertedMessageSenderTxt.Text = validation.Sender;
            convertedMessageSubjectTxt.Text = validation.Subject;
            convertedMessageBodyTxt.Text = text.Trim();
            processedText = text.Trim();  
        }


        private void saveButton_Click(object sender, RoutedEventArgs e)
        {            
            string path = @".\EustonLeisureMessages.json";

            if (validation.Header.StartsWith("T"))
            {
                processing.SearchForHashTagsAndMentions(processedText);
            }

            if (validation.Header.StartsWith("E"))
            {
                processing.SearchForSIR(processedText);
            }




            validation.AddMessageToList(processedText);


            // Converts the whole list of messages into JSON and stores it
            json.Serialize(validation.listOfMessages, path);

            saveButton.IsEnabled = false;

            validation.EndOfCycle();

            convertedMessageHeaderTxt.Text = string.Empty;
            convertedMessageSenderTxt.Text = string.Empty;
            convertedMessageSubjectTxt.Text = string.Empty;
            convertedMessageBodyTxt.Text = string.Empty;
            messageHeaderTxt.Text = string.Empty;
            messageBodyTxt.Text = string.Empty;    
        }



        private void importFile_Click(object sender, RoutedEventArgs e)
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


            importFile.Content = "Next Message";                     

            SplitImportedData();
        }
        
        private void SplitImportedData()
        {
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
