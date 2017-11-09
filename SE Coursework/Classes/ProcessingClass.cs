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
        List<string> listOfMentions = new List<string>();
        List<string> listOfHashTags = new List<string>();        
        List<string> listQuarantine = new List<string>();




        string headerCheck = string.Empty;

        #region Construtor

        public ProcessingClass()
        {
           

        }

        #endregion

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





        #region Message Processing

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
            }

            GetSportCentreCodeAndNatureOfIncident(proText);






            MessageBox.Show("Processing EMAIL");
        }

        private void ProccessedTweet(ref string proText)
        {
            TextSpeakAbbreviations(ref proText);
            FindMentions(proText);
            FindHashTags(proText);

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


            for (int i = 0; i < splitProText.Length; i++)
            {
                if (splitProText[i].StartsWith("#"))
                {
                    listOfHashTags.Add(splitProText[i]);
                    MessageBox.Show("Found a hashtag");
                }
            }
        }

        private void FindMentions(string proText)
        {
            string[] splitProText = proText.Split(' ');


            for (int i = 0; i < splitProText.Length; i++)
            {
                if (splitProText[i].StartsWith("@"))
                {
                    listOfMentions.Add(splitProText[i]);
                    MessageBox.Show("Found a mention");
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

        public List<string> GetMentionsList()
        {
            return listOfMentions;
        }

        public List<string> GetHashTagList()
        {
            return listOfHashTags;
        }



        #endregion

    }
}
