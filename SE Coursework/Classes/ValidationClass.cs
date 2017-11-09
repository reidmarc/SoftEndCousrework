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

        MessageClass messageToReturn;

        public string Header = string.Empty;
        public string Sender = string.Empty;
        public string Subject = string.Empty;
        public string Text = string.Empty;





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
                smsMessage = false;

                while (smsCheck)
                {
                    if (inputText.Length > 15)
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
                    }

                    if (smsCheck.Equals(true))
                    {
                        MessageBox.Show("The phone number entered, is not a valid phone number.");
                        return false;
                    }

                }

                SetPublicVariable();
                MessageBox.Show("SMS Converted");                
                return true;
            }

            // EMAIL
            if (emailMessage.Equals(true))
            {
                emailMessage = false;

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

                SetPublicVariable();
                MessageBox.Show("Email Converted");                
                return true;
            }

            // TWEET
            if (tweetMessage.Equals(true))
            {
                tweetMessage = false; 

                string[] splitProText = inputText.Trim().Split(' ');
                
                if (splitProText[0].StartsWith("@") && splitProText[0].Length < 16)
                {
                    sender = splitProText[0];
                    splitProText[0] = string.Empty;
                }
                else
                {
                    MessageBox.Show("You have entered an incorrect Twitter ID.\nPlease check and try again.");
                    return false;
                }


                // Concatenate all the elements into a StringBuilder.
                StringBuilder builder = new StringBuilder();
                foreach (string value in splitProText)
                {
                    builder.Append(value);
                    builder.Append(' ');
                }

                text = builder.ToString().Trim();               


                if (text.Length > 140)
                {
                    MessageBox.Show("The tweet text is more than 140 characters in length.");
                    return false;
                }

                SetPublicVariable();
                MessageBox.Show("Tweet Converted");                
                return true;

            }

            return false;

        }


        private void SetPublicVariable()
        {
            Header = header;
            Sender = sender;
            Subject = subject;
            Text = text;
        }
    
        public void EndOfCycle()
        {
            header = string.Empty;
            sender = string.Empty;
            text = string.Empty;
            subject = string.Empty;
            name = string.Empty;       

            Header = string.Empty;
            Sender = string.Empty;
            Subject = string.Empty;
            Text = string.Empty;
        }
    
    
        


        // Adds message to the list
        public void AddMessageToList(string inputText)
        {
            MessageClass message = new MessageClass()
            {
                Header = header,
                Sender = sender,
                Subject = subject,
                MessageText = inputText
            };


            listOfMessages.Add(message);
            messageToReturn = message;

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
