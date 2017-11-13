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
        
        Dictionary<string, int> mentionsDictionary = new Dictionary<string, int>();
        Dictionary<string, int> hashtagDictionary = new Dictionary<string, int>();

        List<string> sirList = new List<string>();
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
            headerCheck = header[0].ToString().ToUpper();            

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
                FormatSIR(ref proText);

               //MessageBox.Show("Found SIR");               
            } 

            //MessageBox.Show("Processing EMAIL");
        }

        private void ProccessedTweet(ref string proText)
        {
            TextSpeakAbbreviations(ref proText);   

            MessageBox.Show("Processing TWEET");
        }

        private void FormatSIR(ref string proText)
        {
            string[] splitProText = proText.Trim().Split(' ');

            
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
            if (splitProText[7].Equals("Staff") || splitProText[7].Equals("Device")     || splitProText[7].Equals("Customer") || splitProText[7].Equals("Customer") ||
                splitProText[7].Equals("Bomb")  || splitProText[7].Equals("Suspicious") || splitProText[7].Equals("Sport"))
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


            // Concatenate all the elements into a StringBuilder.
            StringBuilder builder = new StringBuilder();
            foreach (string value in splitProText)
            {
                builder.Append(value);
                builder.Append(' ');
            }

            proText = builder.ToString();

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
            string[] splitProText = proText.Trim().Split(' ');
            string[] natureOfIncident = {"Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };
            string incident = string.Empty;
            string code = string.Empty;


            if (!splitProText[3].ToString().Trim().Count().Equals(9))
            {
                
                MessageBox.Show("The Sport centre code you have entered is incorrect.");
                MessageBox.Show(splitProText[3].ToString());
            }
            else
            {
                MessageBox.Show(splitProText[3].ToString());
                code = splitProText[3];
            }


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





            string inputString = ($"[{code}] - [{incident}]");

            if (!sirList.Contains(inputString))
            {
                sirList.Add(inputString);
            }
            else
            {
                MessageBox.Show("The Sport Centre Code and Nature of Incident already exist on the SIR list.");
            }
        }

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

        public void GetSIR()
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
        }

        public void SearchForSIR(string proText)
        {
            GetSportCentreCodeAndNatureOfIncident(proText);
            AddSIRListToFile();
        }

        public void SearchForHashTagsAndMentions(string proText)
        {            
            FindMentions(proText);
            FindHashTags(proText);
            AddHashTagDictionaryToFile();
            AddMentionsDictionaryToFile();            
        }

        #endregion  

    }
}
