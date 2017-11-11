using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.ComponentModel.DataAnnotations;

namespace SE_Coursework.Classes
{
    public class ProcessingClass
    {
        Dictionary<string, string> textWordsDictionary = new Dictionary<string, string>();
        Dictionary<string, string> sirDictionary = new Dictionary<string, string>();
        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();        
        List<string> listQuarantine = new List<string>();

        


        string headerCheck = string.Empty;

        #region Construtor

        public ProcessingClass()
        {
            

        }

        #endregion

        #region Public Methods
        
        public void GetTextWords()
        {
            using (var reader = new StreamReader(@".\textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string lineString = line.ToString();


                    int firstSpaceIndex = lineString.Trim().IndexOf(",");
                    string keyString = lineString.Substring(0, firstSpaceIndex);
                    string valueString = lineString.Substring(firstSpaceIndex + 1);

                    textWordsDictionary.Add(keyString.Trim(), valueString.Trim());
                }

            }

        }





        

        public void MessageProcessing(string header, ref string text, string subject)
        {
            headerCheck = header[0].ToString();            

            if (headerCheck.Equals("S"))
            { 
                ProccessedSms(ref text);
            }

            if (headerCheck.Equals("E"))
            {
                ProccessedEmail(ref text, subject);
            }

            if (headerCheck.Equals("T"))
            {
                ProccessedTweet(ref text);
            }            
        }

        #endregion

        #region Private Methods

        private void ProccessedSms(ref string proText)
        {      
            TextSpeakAbbreviations(ref proText);

            MessageBox.Show("Processing SMS");
        }

        private void ProccessedEmail(ref string proText, string subject)
        {
            FindURL(ref proText);

            if (subject.Trim().StartsWith("SIR"))
            {
                MessageBox.Show("Found SIR");
                GetSportCentreCodeAndNatureOfIncident(proText);
            } 

            MessageBox.Show("Processing EMAIL");
        }

        private void ProccessedTweet(ref string proText)
        {
            TextSpeakAbbreviations(ref proText);   

            MessageBox.Show("Processing TWEET");
        }



        private void TextSpeakAbbreviations(ref string proText)
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

            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();
        }

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

            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();
        }

        private void GetSportCentreCodeAndNatureOfIncident(string proText)
        {
            string[] splitProText = proText.Split(' ');
            string[] natureOfIncident = {"Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };
            string incident = string.Empty;
            string code = string.Empty;


            if (!splitProText[3].Trim().Count().Equals(9))
            {
                MessageBox.Show("The Sport centre code you have entered is incorrect.");
            }
            else
            {
                code = splitProText[3];
            }


            foreach (string s in natureOfIncident)
            {
                if (s.StartsWith(splitProText[7]))
                {
                    incident = s;
                }
            }
           
            sirDictionary.Add(code, incident);       
        }

        private void AddHashTagToDictionaryToFile()
        {        
            using (var writer = new StreamWriter(@".\hashtags.csv"))
            {
                foreach (var pair in hashtagDictionary)
                {
                    writer.WriteLine("{0},{1}", pair.Key, pair.Value);
                }  
            }            
        }



        private void AddMentionsToDictionaryToFile()
        {  
            using (var writer = new StreamWriter(@".\mentions.csv"))
            {
                foreach (var pair in mentionsDictionary)
                {
                    writer.WriteLine("{0},{1}", pair.Key, pair.Value);
                }
            }           
        }


        public void GetHashTags()
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
        }

        public void GetMentions()
        {
            using (var reader = new StreamReader(@".\mentions.csv"))
            {
                
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

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

        public void SearchForHashTagsAndMentions(string proText)
        {            
            FindMentions(proText);
            FindHashTags(proText);
            AddHashTagToDictionaryToFile();
            AddMentionsToDictionaryToFile();            
        }

        #endregion  

    }
}
