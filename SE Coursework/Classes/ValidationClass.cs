using Newtonsoft.Json;
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
    public class ValidationClass
    {
        bool smsMessage = false;
        bool emailMessage = false;
        bool tweetMessage = false;


        string header = string.Empty;
        string sender = string.Empty;
        string text = string.Empty;
        string subject = string.Empty;
        string name = string.Empty;

        public List<MessageClass> listOfMessages = new List<MessageClass>();
        

        //public string Header { get; set; }
        //public string Sender { get; set; }
        //public string Subject { get; set; }
        //public string TweetText { get; set; }




        JsonClass jsonClass = new JsonClass();


        // Default Constructor
        public ValidationClass()
        {

        }    




        public bool MessageHeaderInputValidation(string inputText)
        {
            // Stores the first character of inputText as the string _inputText
            string _inputText = inputText[0].ToString();

            // Trims the input then stores it as the string header
            header = inputText.Trim();

            // Stores a substring of numeric as the string subStringNumeric
            string subStringNumeric = header.Substring(1, 9);

            // Checks the length of the input
            if (!(header.Length).Equals(10))
            {
                return false;
            }            

            // Checks the substring is all numbers
            if (subStringNumeric.All(char.IsDigit).Equals(false))
            {
                return false;
            }


            

            if (_inputText.ToUpper().Equals("S"))
            {
                smsMessage = true;
                return true;
            }

            if (_inputText.ToUpper().Equals("E"))
            {
                emailMessage = true;
                return true;
            }

            if (_inputText.ToUpper().Equals("T"))
            {
                tweetMessage = true;
                return true;
            }
            
            return false;
        }

        public bool MessageBodyInputValidation(string inputText)
        {
            bool smsCheck = true;
                        
            string emailPattern = @"[A-Za-z0-9_\-\+]+@[A-Za-z0-9\-]+\.([A-Za-z]{2,3})(?:\.[a-z]{2})?";
            

            EmailAddressAttribute emailAddressCheck = new EmailAddressAttribute();
            PhoneAttribute phoneNumberCheck = new PhoneAttribute();


            // SMS
            if (smsMessage.Equals(true))
            {
                //string smsPattern = @"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$";

                //Regex myRegex = new Regex(smsPattern, RegexOptions.None);

                //sender = myRegex.Replace(inputText, string.Empty);

                //MessageBox.Show(sender);

                

                while (smsCheck)
                {
                    for (int i = 15; i > 7; i--)
                    {
                        sender = inputText.Trim().Substring(0, i);

                        if (phoneNumberCheck.IsValid(sender))
                        {

                            text = inputText.Trim().Substring(i);


                            smsCheck = false;
                            break;
                        }

                    }

                    if (smsCheck.Equals(true))
                    {
                        MessageBox.Show("The phone number entered, is not a valid phone number.");
                        return false;
                    }

                }


                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////// MESSAGE PROCESSING GOES HERE //////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                MessageBox.Show("SMS Converted");

                return true;
            }

            // EMAIL
            if (emailMessage.Equals(true))
            {                
                Regex myRegex = new Regex(emailPattern, RegexOptions.None);

                Match myMatch = myRegex.Match(inputText);
                
                if (myMatch.Success)
                {
                    sender = myMatch.Value;

                    // Checks if the email is a valid email address
                    if (!emailAddressCheck.IsValid(sender))
                    {
                        MessageBox.Show("You have entered an incorrect email address.");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("You have entered an incorrect email address.");
                    return false;
                }

                // Removes the email address from the string and replaces it with a '|'
                string newInputText = myRegex.Replace(inputText, "|");

                // Splits the string in 2 based on the delimiter '|'
                string[] splitText = newInputText.Split('|');                               

                // Checks the email doesn't exceed the maximum length
                if (splitText[1].Length > 1048)
                {
                    MessageBox.Show("This email is longer than 1028 max characters");
                    return false;
                }

                // Creates the string name from the part of splitText before the '|'
                name = splitText[0];

                // Creates substrings from newInputText
                subject = splitText[1].Substring(0, 21);
                text = splitText[1].Substring(21);


                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////// MESSAGE PROCESSING GOES HERE //////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                MessageBox.Show("Email Coverted");

                return true;
            }

            // TWEET
            if (tweetMessage.Equals(true))
            {
                string tweetPattern = @"^((@\w+)\s)+";

                Regex myRegex = new Regex(tweetPattern, RegexOptions.None);

                Match myMatch = myRegex.Match(inputText);

                if (myMatch.Value.Length > 17)
                {
                    return false;
                }

                if (myMatch.Success)
                {
                    sender = myMatch.Value;
                }
                

                // Removes the Twitter ID  from the string, leaving the tweet.
                text = myRegex.Replace(inputText, string.Empty);


                if (text.Length > 140)
                {
                    return false;
                }

                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////// MESSAGE PROCESSING GOES HERE //////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                MessageBox.Show("Tweet Converted");


                return true;                

            }

            return false;           
            
        }


        // Adds message to the list
        public void AddMessageToList()
        {
            MessageClass message = new MessageClass()
            {
                Header = header,
                Sender = sender,
                Subject = subject,
                MessageText = text
            };


            listOfMessages.Add(message);

            if (header[0].ToString().Equals("T"))
            {
                MessageBox.Show("Tweet Saved.");
            }
            if (header[0].ToString().Equals("S"))
            {
                MessageBox.Show("SMS Saved.");
            }
            if (header[0].ToString().Equals("E"))
            {
                MessageBox.Show("Email Saved.");
            }


            header = sender = subject = text = string.Empty;
        }
        
        //// Adds SMS and Tweets to the list
        //private void AddMessageToList(string header, string sender, string text)
        //{
        //    MessageClass message = new MessageClass()
        //    {
        //        Header = header,
        //        Sender = sender,                
        //        MessageText = text
        //    };
        //    listOfMessages.Add(message);
            

        //    if (header[0].Equals("T"))
        //    {
        //        MessageBox.Show("Tweet Saved.");
        //    }

        //    if (header[0].Equals("S"))
        //    {
        //        MessageBox.Show("SMS Saved.");
        //    }
        //}


        //// Adds Email to the list
        //private void AddMessageToList(string header, string sender, string subject, string text)
        //{
        //    MessageClass message = new MessageClass()
        //    {
        //        Header = header,
        //        Sender = sender,
        //        Subject = subject,
        //        MessageText = text
        //    };

            
        //    listOfMessages.Add(message);

        //    if (header[0].Equals("T"))
        //    {
        //        MessageBox.Show("Tweet Saved.");
        //    }
        //    if (header[0].Equals("S"))
        //    {
        //        MessageBox.Show("SMS Saved.");
        //    }
        //    if (header[0].Equals("E"))
        //    {
        //        MessageBox.Show("Email Saved.");
        //    }
        //}


        public void RetrieveStoredList()
        {
            int counter = 0;

            try
            {
                // Returns the list that is stored as JSON             
                listOfMessages = jsonClass.Deserialize();

                counter = counter + 1;
            }
            catch (Exception ex)
            {
                if (counter > 0)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }    

        #region User's Choice Validation Methods

        // Method which displays a messagebox asking the user if they are sure they want to exit the application
        // And gives them the option of answering 'yes' or 'no'
        public void ExitApplicationValidation()
        {
            MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to exit the application?", "Exit Application", MessageBoxButton.YesNo);

            if (yesOrNo == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }

            else
            {
                return;
            }
        }

        #endregion
    }
}
