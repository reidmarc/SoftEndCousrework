using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace SE_Coursework.Classes
{
    public class ProcessingClass
    {
        Dictionary<string, string> textWordsDictionary = new Dictionary<string, string>();
        List<string> listOfMentions = new List<string>();
        List<string> listOfHashTags = new List<string>();



        string headerCheck = string.Empty;

        #region Construtor

        public ProcessingClass()
        {
           

        }

        #endregion

        private void GetTextWords()
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

        public void MessageProcessing(string header, ref string text)
        {
            headerCheck = header[0].ToString();            

            if (headerCheck.Equals("S"))
            { 
                ProccessedSms(ref text);
            }

            if (headerCheck.Equals("E"))
            {
                ProccessedEmail(ref text);
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

        private void ProccessedEmail(ref string proText)
        {
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
            GetTextWords();

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

        }

        private void FindMentions(string proText)
        {
            // Needs updated to pick up ALL mentions not just the first

            string[] splitProText = proText.Split(' ');


            for (int i = 0; i < splitProText.Length; i++)
            {
                if (splitProText[i].StartsWith("@"))
                {
                    listOfMentions.Add(splitProText[i]);
                }
            }











            //string tweetPattern = @"^((@\w+)\s)+";

            //Regex myRegex = new Regex(tweetPattern, RegexOptions.None);

            //Match myMatch = myRegex.Match(proText);

            //if (myMatch.Success)
            //{
            //    listOfMentions.Add(myMatch.Value);
            //}
        }

        #endregion

    }
}
