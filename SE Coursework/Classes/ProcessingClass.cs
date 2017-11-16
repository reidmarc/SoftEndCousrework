//////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////// Class ProcessingClass /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

// Description: Class used to process the main block of text provided, depending on the message type.

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.DataAnnotations;

#endregion

namespace SE_Coursework.Classes
{
    public class ProcessingClass
    {
        #region Data Structures

        Dictionary<string, string> textWordsDictionary = new Dictionary<string, string>();        
        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();

        List<string> sirList = new List<string>();
        List<string> listQuarantine = new List<string>();         

        #endregion

        #region Construtor

        // Default Constructor
        public ProcessingClass()
        {            

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method calls either ProcessedSms, ProcessedEmail or ProcessedTweet based on the first character
        /// Of the string header
        /// </summary>
        /// <param name="header">Passed in to be able to identify each of the different message types</param>
        /// <param name="text">Passed in to be passed on, so it can be processed by the appropriate methods</param>
        /// <param name="subject">Passed in to go on to ProcessedEmail</param>
        public void MessageProcessing(string header, ref string text, string subject)
        {
            string headerCheck = string.Empty;

            // Takes the first char of the string header, converts the char to a string then converts it to uppercase
            headerCheck = header[0].ToString().ToUpper();

            if (headerCheck.Equals("S"))
            {
                ProcessedSms(ref text);
            }
            else if (headerCheck.Equals("E"))
            {
                ProcessedEmail(ref text, subject);
            }
            else if (headerCheck.Equals("T"))
            {
                ProcessedTweet(ref text);
            }
            else
            {
                MessageBox.Show("There is a problem with the header enetered.");
            }
        }

        /// <summary>
        /// This method retrieves the list of words and abbreviations from the textwords.csv file as a string per line
        /// The method uses substrings to split each string at the first comma. Storing before the comma string as the key
        /// And after the comma string as the value in a Dictionary.
        /// </summary>
        public void GetTextWords()
        {
            using (var textWordReader = new StreamReader(@".\textwords.csv"))
            {
                while (!textWordReader.EndOfStream)
                {
                    var line = textWordReader.ReadLine();
                    string lineString = line.ToString();

                    int firstCommaIndex = lineString.Trim().IndexOf(",");
                    string keyString = lineString.Substring(0, firstCommaIndex);
                    string valueString = lineString.Substring(firstCommaIndex + 1);

                    textWordsDictionary.Add(keyString.Trim(), valueString.Trim());
                }
            }
        }

        /// <summary>
        /// This method retrieves hashtag values stored in hashtags.csv file as a string per line
        /// Then checks that each line contains a '#' and is less than 51 chars in length
        /// The method uses substrings to split each string at the first comma. Storing the before the comma string as the key
        /// And converts the after the comma string to an int then stores it as the value in a Dictionary.
        /// </summary>
        public void GetHashTags()
        {
            using (var hashTagReader = new StreamReader(@".\hashtags.csv"))
            {
                while (!hashTagReader.EndOfStream)
                {
                    var line = hashTagReader.ReadLine();

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
        }

        /// <summary>
        /// This method retrieves mention values stored in mentions.csv file as a string per line
        /// Then checks that each line contains a '@' and is less than 16 chars in length
        /// The method uses substrings to split each string at the first comma. Storing the before the comma string as the key
        /// And converts the after the comma string to an int then stores it as the value in a Dictionary.
        /// </summary>
        public void GetMentions()
        {
            using (var mentionsReader = new StreamReader(@".\mentions.csv"))
            {
                while (!mentionsReader.EndOfStream)
                {
                    var line = mentionsReader.ReadLine();

                    if (line.Contains("@") && line.Count() < 16)
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
        }

        /// <summary>
        /// This method retrieves sir values stored in sir.csv file as a string per line
        /// Checks that the sir code is formatted correctly then stores the string in a List
        /// </summary>
        public void GetSIR()
        {
            using (var sirReader = new StreamReader(@".\sir.csv"))
            {
                while (!sirReader.EndOfStream)
                {
                    var line = sirReader.ReadLine();

                    if (line[3].ToString().Equals("-") && line[7].ToString().Equals("-"))
                    {
                        sirList.Add(line.ToString());
                    }
                }
            }
        }

        /// <summary>
        /// This method calls the methods GetSportCentreCodeAndNatureOfIncident() and AddSIRListToFile()
        /// </summary>
        /// <param name="proText">Passes the string proText on to the method GetSportCentreCodeAndNatureOfIncident()</param>
        public void SearchForSIR(string proText)
        {
            GetSportCentreCodeAndNatureOfIncident(proText);
            AddSIRListToFile();
        }

        /// <summary>
        /// This method calls the methods FindMentions(), FindHashTags(), AddHashTagDictionaryToFile() and AddMentionsDictionaryToFile();
        /// </summary>
        /// <param name="proText">Passes the string proText on to the methods FindMentions() and FindHashTags()</param>
        public void SearchForHashTagsAndMentions(string proText)
        {
            FindMentions(proText);
            FindHashTags(proText);
            AddHashTagDictionaryToFile();
            AddMentionsDictionaryToFile();
        }

        #endregion

        #region Private Methods

        #region SMS Related

        /// <summary>
        /// This method calls the method FindAndReplaceTextSpeakAbbreviations passing the ref to the string proText
        /// </summary>
        /// <param name="proText">Passes the string proText to the method called</param>
        private void ProcessedSms(ref string proText)
        {      
            FindAndReplaceTextSpeakAbbreviations(ref proText);            
        }

        #endregion

        #region Email Related

        /// <summary>
        /// This methoid calls the method FindURL passing on the string proText. If the message subject starts with
        /// 'SIR' then the method calls the method FormatSIR and passes on the string proText.
        /// </summary>
        /// <param name="proText">Passed on to potentially two methods</param>
        /// <param name="subject">Used to determine if the method FormatSIR needs to be called on not</param>
        private void ProcessedEmail(ref string proText, string subject)
        {
            FindURL(ref proText);

            if (subject.Trim().StartsWith("SIR"))
            {
                FormatSIR(ref proText);                            
            }             
        }

        /// <summary>
        /// This method takes the string proText and formats the the string to match what is asked for in the
        /// Coursework brief with the Sports centre code on the first line then the nature of incident on the second line
        /// and any additional text on the third line onwards.
        /// </summary>
        /// <param name="proText">Passes in proText so that the method can format the contents of the string</param>
        private void FormatSIR(ref string proText)
        {
            // Splits the string into a string array
            string[] splitProText = proText.Trim().Split(' ');

            // Finds the word Nature in the string then replaces it with \nNature so that a new line is created before Nature
            if (splitProText[4].Equals("Nature"))
            {
                splitProText[4] = $"\n{splitProText[4]}";
            }

            // Formats for Nature of Incident with 1 words
            if (splitProText[7].Equals("Raid") || splitProText[7].Equals("Terrorism"))
            {
                if (splitProText.Length > 8)
                {
                    splitProText[8] = $"\n{splitProText[8]}";
                }
            }

            // Formats for Nature of Incident with 2 words
            if (splitProText[7].Equals("Staff") || splitProText[7].Equals("Device") || splitProText[7].Equals("Customer") || splitProText[7].Equals("Customer") ||
                splitProText[7].Equals("Bomb") || splitProText[7].Equals("Suspicious") || splitProText[7].Equals("Sport"))
            {
                if (splitProText.Length > 9)
                {
                    splitProText[9] = $"\n{splitProText[9]}";
                }
            }

            // Formats for Nature of Incident with 3 words
            if (splitProText[7].Equals("Personal") || splitProText[7].Equals("Theft"))
            {
                if (splitProText.Length > 10)
                {
                    splitProText[10] = $"\n{splitProText[10]}";
                }
            }

            // Appends all the values stored in the string array
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();
        }

        /// <summary>
        /// This method splits the string proText and stores it in a string array then searches through  
        /// the array looking for any URL's, when it finds one
        /// the method adds the URL to a quarantine list then replaces the URL with
        /// "<URL Quarantined>" then the method builds a single string from the array
        /// </summary>
        /// <param name="proText">Passed in, to be searched through for a URL</param>
        private void FindURL(ref string proText)
        {
            UrlAttribute url = new UrlAttribute();

            string[] splitProText = proText.Split(' ');

            for (int i = 0; i < splitProText.Length; i++)
            {
                if (url.IsValid(splitProText[i]) || splitProText[i].StartsWith("www."))
                {
                    listQuarantine.Add(splitProText[i]);
                    splitProText[i] = "'<URL Quarantined>'";
                }
            }

            // Appends all the values stored in the string array
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();
        }

        /// <summary>
        /// This method takes the string proText and extracts the sport centre code and nature of incident from it
        /// Then if the formats the 2 values together in the format that I want it to be displayed on the menu page
        /// </summary>
        /// <param name="proText">Passed in to extract the string incident and string code from</param>
        private void GetSportCentreCodeAndNatureOfIncident(string proText)
        {
            // splits the string proText by spaces and stores each word in a string array
            string[] splitProText = proText.Trim().Split(' ');
            string[] natureOfIncident = { "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };

            string incident = string.Empty;
            string code = string.Empty;

            // Checks the array index 3 to confirm the length of the value stored is 9 characters long
            if (!splitProText[3].ToString().Trim().Count().Equals(9))
            {
                MessageBox.Show("The Sport centre code you have entered is incorrect.");
                return;
            }
            else
            {
                code = splitProText[3];
            }

            // As there are 2 incidents that start with 'Staff' this IF statement
            // deals with confirming which incident has happened
            if (splitProText[8].Equals("Staff"))
            {
                if (splitProText[9].Equals("Attack"))
                {
                    incident = "Staff Attack";
                }
                else
                {
                    incident = "Staff Abuse";
                }
            }
            else
            {
                foreach (string s in natureOfIncident)
                {
                    if (s.StartsWith(splitProText[7]))
                    {
                        incident = s;
                        break;
                    }
                }
            }

            // Formats the string as intended
            string inputString = ($"[{code}] - [{incident}]");

            // Adds the string to the sirList, only if the string does not already exist on the list
            if (!sirList.Contains(inputString))
            {
                sirList.Add(inputString);
            }
            else
            {
                MessageBox.Show("The Sport Centre Code and Nature of Incident already exist on the SIR list.");
                return;
            }
        }

        #endregion

        #region Tweet Related

        /// <summary>
        /// This methoid calls the method FindAndReplaceTextSpeakAbbreviations passing on the string proText.
        /// </summary>
        /// <param name="proText"></param>
        private void ProcessedTweet(ref string proText)
        {
            FindAndReplaceTextSpeakAbbreviations(ref proText);
        }

        /// <summary>
        /// This method splits the string proText and stores it in a string array then searches through  
        /// the array looking for any '#' hashtags, when it finds one it checks if that entry already exists
        /// if it does, then the value for that hashtag is increased by 1. If it doesn't already exist
        /// the method adds the hashtag as a key with a value of 1 to the hashtagDictionary
        /// </summary>
        /// <param name="proText">Passed in, to be searched through for a '#' hashtags</param>       
        private void FindHashTags(string proText)
        {
            string[] splitProText = proText.Split(' ');
            bool hashtagExists = false;

            for (int i = 0; i < splitProText.Length; i++)
            {
                hashtagExists = false;

                if (splitProText[i].StartsWith("#"))
                {
                    for (int q = 0; q < hashtagDictionary.Count; q++)
                    {
                        var hashtagElement = hashtagDictionary.ElementAt(q);

                        if (hashtagElement.Key.Equals(splitProText[i]))
                        {
                            hashtagDictionary[splitProText[i]] = hashtagDictionary[splitProText[i]] + 1;
                            MessageBox.Show("Updated a hashtag entry");
                            hashtagExists = true;
                        }
                    }

                    if (hashtagExists.Equals(false))
                    {
                        hashtagDictionary.Add(splitProText[i], 1);
                        MessageBox.Show("Found a hashtag");
                    }
                }
            }
        }

        /// <summary>
        /// This method splits the string proText and stores it in a string array then searches through  
        /// the array looking for any '@' mentions, when it finds one it checks if that entry already exists
        /// if it does, then the value for that mention is increased by 1. If it doesn't already exist
        /// the method adds the mention as a key with a value of 1 to the mentionsDictionary
        /// </summary>
        /// <param name="proText">Passed in, to be searched through for a '@' mentions</param>       
        private void FindMentions(string proText)
        {
            string[] splitProText = proText.Split(' ');
            bool mentionExists = false;


            for (int i = 0; i < splitProText.Length; i++)
            {
                mentionExists = false;

                if (splitProText[i].StartsWith("@"))
                {

                    for (int w = 0; w < mentionsDictionary.Count; w++)
                    {
                        var mentionElement = mentionsDictionary.ElementAt(w);

                        if (mentionElement.Key.Equals(splitProText[i]))
                        {
                            mentionsDictionary[splitProText[i]] = mentionsDictionary[splitProText[i]] + 1;
                            MessageBox.Show("Updated a mention entry");
                            mentionExists = true;
                        }
                    }

                    if (mentionExists.Equals(false))
                    {
                        mentionsDictionary.Add(splitProText[i], 1);
                        MessageBox.Show("Found a mention");
                    }
                }
            }
        }


        #endregion

        /// <summary>
        /// This methods takes the string proText, splits it by the spaces and stores the values in a string array
        /// Then loops through each index in the array looking for an abbreviation that matches a key stored in the textWordsDictionary
        /// If it finds a match it removes the key and replaces it with a string that is made up from the key and the value enclosed in <>
        /// Then the method takes all the values in the array and appends them togther into a single string
        /// </summary>
        /// <param name="proText"></param>
        private void FindAndReplaceTextSpeakAbbreviations(ref string proText)
        {   
            string[] splitProText = proText.Split(' ');

            for (int i = 0; i < splitProText.Length; i++)
            {
                foreach (KeyValuePair<string, string> abbreviation in textWordsDictionary)
                {
                    if (abbreviation.Key.Equals(splitProText[i]))
                    {
                        splitProText[i] = ($"{abbreviation.Key.Trim()} <{abbreviation.Value.Trim()}>");
                    }
                }
            }

            // Appends all the values stored in the string array
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();
        }

        #region Add Dictionary / List to file

        /// <summary>
        /// This method stores in hashtags.csv, every key and value that is in the Dictionary hashtagDictionary as a string per line
        /// </summary>
        private void AddHashTagDictionaryToFile()
        {        
            using (var writer = new StreamWriter(@".\hashtags.csv"))
            {
                foreach (var pair in hashtagDictionary)
                {
                    writer.WriteLine($"{pair.Key},{pair.Value}");
                }  
            }            
        }


        /// <summary>
        /// This method stores in mentions.csv, every key and value that is in the Dictionary mentionsDictionary as a string per line
        /// </summary>
        private void AddMentionsDictionaryToFile()
        {  
            using (var writer = new StreamWriter(@".\mentions.csv"))
            {
                foreach (var pair in mentionsDictionary)
                {
                    writer.WriteLine($"{pair.Key},{pair.Value}");
                }
            }           
        }

        /// <summary>
        /// This method stores in sir.csv, every value that is in the List sirList
        /// </summary>
        private void AddSIRListToFile()
        {
            using (var writer = new StreamWriter(@".\sir.csv"))
            {
                foreach (var entry in sirList)
                {
                    writer.WriteLine($"{entry}");
                }
            }
        }

        #endregion

        #endregion
    }
}
