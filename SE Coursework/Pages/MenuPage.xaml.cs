//////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////// Page MenuPage ////////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

#region Usings

using SE_Coursework.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

#endregion

namespace SE_Coursework.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        #region Objects / Data Structures

        ValidationClass menuValidation = new ValidationClass();
        JsonClass jsonClass = new JsonClass();
        ProcessingClass menuProcessing = new ProcessingClass();
        
        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();

        List<string> hashTagList = new List<string>();
        List<string> mentionsList = new List<string>();
        List<string> sirList = new List<string>();

        #endregion

        #region Constructor

        public MenuPage()
        {
            InitializeComponent();
            RetrieveHashTags();
            RetrieveMentions();
            RetrieveSIR();
        }

        #endregion

        #region Updating ListBoxes

        #region Sorting List

        /// <summary>
        /// This method uses a bubble sort to sort the list passed in by the values
        /// </summary>
        /// <param name="list">Passed in to be sorted</param>
        /// <returns>returns a sorted list</returns>
        private List<string> BubbleSort(List<String> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - i; j++)
                {
                    string comparisonA = string.Empty;
                    string comparisonB = string.Empty;
                    int comparisonIntA = 0;
                    int comparisonIntB = 0;

                    // Retrieves the int value from the first comparison string
                    string[] splitStringArrayA = list[j].Trim().Split('-');

                    comparisonA = splitStringArrayA[0].Trim();

                    // Removes [ from string
                    if (comparisonA.Contains("["))
                    {
                        comparisonA = comparisonA.Replace("[", "");
                    }

                    // Removes ] from string
                    if (comparisonA.Contains("]"))
                    {
                        comparisonA = comparisonA.Replace("]", "");
                    }

                    Int32.TryParse(comparisonA, out comparisonIntA);


                    // Retrieves the int value from the second comparison string
                    string[] splitStringArrayB = list[j + 1].Trim().Split('-');

                    comparisonB = splitStringArrayB[0].Trim();

                    // Removes [ from string
                    if (comparisonB.Contains("["))
                    {
                        comparisonB = comparisonB.Replace("[", "");
                    }

                    // Removes ] from string
                    if (comparisonB.Contains("]"))
                    {
                        comparisonB = comparisonB.Replace("]", "");
                    }

                    Int32.TryParse(comparisonB, out comparisonIntB);

                    // Compares comparison a with comparison b and swaps is a is higher than b
                    if (comparisonIntA > comparisonIntB)
                    {
                        string temp = list[j];
                        list[j] = list[j + 1];
                        list[j + 1] = temp;
                    }
                }
            }
            return list;
        }

        #endregion

        #region Trending / Hashtag ListBox

        /// <summary>
        /// This method retrieves the hashtag values stored in the file hashtags.csv
        /// </summary>
        private void RetrieveHashTags()
        {
            using (var reader = new StreamReader(@".\hashtags.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line.Contains("#") && line.Count() < 50)
                    {
                        string lineString = line.ToString();

                        // Uses substrings to split the string into a key and a value
                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        // Stores the key and value in a dictionary
                        hashtagDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateHashTagListBox();
        }

        /// <summary>
        /// This method updates the hashtag list box with the values retrieved by the method RetrieveHashTags()
        /// </summary>
        private void UpdateHashTagListBox()
        {
            foreach (KeyValuePair<string, int> hashtag in hashtagDictionary)
            {
                hashTagList.Add(String.Format("[{0}] - {1}", hashtag.Value.ToString(), hashtag.Key));
            }

            hashTagList = BubbleSort(hashTagList);

            int hashtagCounter = hashTagList.Count;

            foreach (var entry in hashTagList)
            {
                if (hashtagCounter > 0)
                {
                    
                    trendingListBox.Items.Add(hashTagList[(hashtagCounter - 1)]);
                    

                    hashtagCounter = hashtagCounter - 1;
                }
            }
        }

        #endregion

        #region Mentions ListBox
        /// <summary>
        /// This method retrieves the mentions values stored in the file mentions.csv
        /// </summary>
        private void RetrieveMentions()
        {
            using (var reader = new StreamReader(@".\mentions.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line.Contains("@") && line.Count() < 20)
                    {
                        string lineString = line.ToString();

                        // Uses substrings to split the string into a key and a value
                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        // Stores the key and value in a dictionary
                        mentionsDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateMentionsListBox();
        }

        /// <summary>
        /// This method updates the mentions list box with the values retrieved by the method RetrieveMentions()
        /// </summary>
        private void UpdateMentionsListBox()
        { 
            foreach (KeyValuePair<string, int> mention in mentionsDictionary)
            {
                mentionsList.Add(String.Format("[{0}] - {1}", mention.Value.ToString(), mention.Key));
            }

            mentionsList = BubbleSort(mentionsList);

            int mentionsCounter = mentionsList.Count;

            foreach (var entry in mentionsList)
            {
                if (mentionsCounter > 0)
                {                    
                    mentionsListBox.Items.Add(mentionsList[(mentionsCounter - 1)]);                   

                    mentionsCounter = mentionsCounter - 1;
                }
            }
        }

        #endregion

        #region SIR ListBox

        /// <summary>
        /// This method retrieves the SIR values stored in the file sir.csv
        /// </summary>
        private void RetrieveSIR()
        {
            using (var reader = new StreamReader(@".\sir.csv"))
            {
                while (!reader.EndOfStream)
                {
                    // Reads each line in the .csv file
                    var line = reader.ReadLine();

                    if (line[3].ToString().Equals("-") && line[7].ToString().Equals("-"))
                    {   
                        sirList.Add(line.ToString());
                    }
                }
            }

            UpdateSIRListBox();
        }

        /// <summary>
        /// This method updates the SIR list box with the values retrieved by the method RetrieveSIR()
        /// </summary>
        private void UpdateSIRListBox()
        {  
            int sirCounter = sirList.Count;
            
            foreach (string entry in sirList)
            {
                if (sirCounter > 0)
                {
                    sirListBox.Items.Add(sirList[(sirCounter - 1)]);

                    sirCounter = sirCounter - 1;
                }
            }
        }

        #endregion

        #endregion

        #region Click Events

        /// <summary>
        /// This method handles what happens when the input messages button is clicked
        /// </summary>
        private void InputButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            InputMessagesPage inputManPage = new InputMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(inputManPage);
        }

        /// <summary>
        /// This method handles what happens when the view messages button is clicked
        /// </summary>
        private void ViewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the ViewMessages Page           
            ViewMessagesPage viewMessages = new ViewMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(viewMessages);
        }

        /// <summary>
        /// This method handles what happens when the Export Json file button is clicked
        /// </summary>      
        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
        {
            // Asks a confiormation question
            MessageBoxResult yesOrNo = MessageBox.Show("Would you like to delete the stored messages after exporting the JSON file?", "Exit Application", MessageBoxButton.YesNo);

            if (yesOrNo == MessageBoxResult.Yes)
            {
                ExportJsonFile();

                // Deletes the stored Json file after it has been exported to the desktop
                File.Delete(@".\EustonLeisureMessages.json");

                // Clears the storage files
                File.WriteAllText(@".\hashtags.csv", String.Empty);
                File.WriteAllText(@".\mentions.csv", String.Empty);
                File.WriteAllText(@".\sir.csv", String.Empty);

                // Clears the trending lists
                trendingListBox.Items.Clear();
                mentionsListBox.Items.Clear();
                sirListBox.Items.Clear();
            }
            else
            {
                ExportJsonFile();
            }
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// This method exports the Json file to the users desktop
        /// </summary>
        private void ExportJsonFile()
        {
            try
            {
                menuValidation.RetrieveStoredList();

                // Sets a string as the path for where to store the JSON file. 
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

        /// <summary>
        /// Method which handles the 'Exit Application' button being clicked
        /// </summary>        
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method ExitApplicationValidation() from the Validation class.
            menuValidation.ExitApplicationValidation();
        }

        #endregion                
    }
}
