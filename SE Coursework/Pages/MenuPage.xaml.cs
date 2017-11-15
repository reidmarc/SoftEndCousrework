using SE_Coursework.Classes;
using System;
using System.Collections;
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



                    string[] splitStringArrayA = list[j].Trim().Split('-');

                    comparisonA = splitStringArrayA[0].Trim();

                    if (comparisonA.Contains("["))
                    {
                        comparisonA = comparisonA.Replace("[", "");
                    }

                    if (comparisonA.Contains("]"))
                    {
                        comparisonA = comparisonA.Replace("]", "");
                    }

                    Int32.TryParse(comparisonA, out comparisonIntA);



                    string[] splitStringArrayB = list[j + 1].Trim().Split('-');

                    comparisonB = splitStringArrayB[0].Trim();


                    if (comparisonB.Contains("["))
                    {
                        comparisonB = comparisonB.Replace("[", "");
                    }

                    if (comparisonB.Contains("]"))
                    {
                        comparisonB = comparisonB.Replace("]", "");
                    }

                    Int32.TryParse(comparisonB, out comparisonIntB);


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

       private void RetrieveMentions()
       {
            using (var reader = new StreamReader(@".\mentions.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line.Contains("@") && line.Count() < 20)
                    {
                        string lineString = line.ToString();


                        int firstSpaceIndex = lineString.Trim().IndexOf(",");
                        string keyString = lineString.Substring(0, firstSpaceIndex);
                        string valueString = lineString.Substring(firstSpaceIndex + 1);

                        Int32.TryParse(valueString, out int valueInt);

                        mentionsDictionary.Add(keyString.Trim(), valueInt);
                    }
                }
            }

            UpdateMentionsListBox();
       }


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


        private void RetrieveSIR()
        {
            using (var reader = new StreamReader(@".\sir.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (line[3].ToString().Equals("-") && line[7].ToString().Equals("-"))
                    {   
                        sirList.Add(line.ToString());
                    }
                }
            }

            UpdateSIRListBox();
        }


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

        private void ManuallyInputButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the InputManually page
            InputMessagesPage inputManPage = new InputMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(inputManPage);
        }        

        private void ViewMessagesButton_Click(object sender, RoutedEventArgs e)
        {
            // Instantiate an object of the ViewMessages Page           
            ViewMessagesPage viewMessages = new ViewMessagesPage();

            // Navigates to the InputManually page
            NavigationService.Navigate(viewMessages);

        }

        private void ExportJsonButton_Click(object sender, RoutedEventArgs e)
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

        // Method which handles the 'Exit Application' button being clicked
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            // Calls the method ExitApplicationValidation() from the Validation class.
            menuValidation.ExitApplicationValidation();
        }




        #endregion
                
    }
}
