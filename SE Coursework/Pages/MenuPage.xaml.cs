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
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        #region Object

        ValidationClass menuValidation = new ValidationClass();
        JsonClass jsonClass = new JsonClass();
        ProcessingClass menuProcessing = new ProcessingClass();

        Dictionary<string, string> sirDictionary = new Dictionary<string, string>();
        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();


        List<string> hashTagList = new List<string>();


        #endregion

        #region Constructor

        public MenuPage()
        {
            InitializeComponent();
            RetrieveHashTags();

        }      

        #endregion

        private void RetrieveHashTags()
        {
            using (var reader = new StreamReader(@".\hashtags.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.Contains("#") && line.Count() < 50)
                    {
                        string lineString = line.ToString();


                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        hashtagDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateHashTagListBox();
        }

        private void UpdateHashTagListBox()
        {            
            foreach (KeyValuePair<string, int> hashtag in hashtagDictionary)
            {
                hashTagList.Add(String.Format("[{0}] - {1}", hashtag.Value.ToString(), hashtag.Key));
            }

            hashTagList.Sort();

            if (hashTagList.Count() > 0)
            {
                for (int i = 5; i > 0; i--)
                {
                    trendingListBox.Items.Add(hashTagList[i]);
                }
            }

            
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
            MessageBox.Show("Feature not yet implmented.");
        }

        private void viewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the ViewMessages Page           
            ViewMessagesPage viewMessages = new ViewMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(viewMessages);

        }

        private void exportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menuValidation.RetrieveStoredList();

                // Sets a string as the path for where to store the JSON file.
                //string path = @"C:\Users\reidm\Desktop\EustonLeisureMessages.json";
                //string path = @"C:\Users\03001588\Desktop\EustonLeisureMessages.json"; 

                string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "/EustonLeisureMessages.json";


                jsonClass.Serialize(menuValidation.listOfMessages, path);

                MessageBox.Show("JSON Exported");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
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
