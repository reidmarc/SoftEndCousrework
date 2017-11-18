//////////////////////////////////////////////////////////////////////////////////////////////////////
//////////////////////////////////////// Class ValidationClass ///////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

// Description: Class used to validate the information, input as phone number, email address, twitter ID's etc

#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

#endregion

namespace SE_Coursework.Classes
{
    public class ValidationClass
    {
        #region Objects / Data Structure / Variables

        JsonClass jsonClass = new JsonClass();

        public List<MessageClass> listOfMessages = new List<MessageClass>();

        bool smsMessage = false;
        bool emailMessage = false;
        bool tweetMessage = false;

        string header = string.Empty;
        string sender = string.Empty;
        string text = string.Empty;
        string subject = string.Empty;
        string name = string.Empty;

        public string Header = string.Empty;
        public string Sender = string.Empty;
        public string Subject = string.Empty;
        public string Text = string.Empty;        

        #endregion

        #region Constructor

        // Default Constructor
        public ValidationClass()
        {

        }

        #endregion

        #region Public Methods

        /// <summary>
        /// This method validates the message header, it does this by first trimming the input then storing it as header
        /// The method then checks that the header is 10 charcaters in length. The method then creates a substring of header to check that
        /// All the charcaters are numeric except for the first character. 
        /// The method retrieves the first character of header by using it's index of [0] then checks if that value is equal to either
        /// S, E or T. A boolean value is changed depending which value is matched with the forst character in header. This affects how the method
        /// MessageBodyInputValidation() validates its input.
        /// </summary>
        /// <param name="inputText">Passes in the message header, to be validated</param>
        /// <returns>A boolean value which returns true if the message header passes validation</returns>
        public bool MessageHeaderInputValidation(string inputText)
        {  
            // Trims the input then stores it as the string header
            header = inputText.Trim();

            // Checks the length of the input
            if (!(header.Length).Equals(10))
            {
                return false;
            }

            // Stores a substring of numeric as the string subStringNumeric
            string subStringNumeric = header.Substring(1, 9);           

            // Checks the substring is all numbers
            if (subStringNumeric.All(char.IsDigit).Equals(false))
            {
                return false;
            }

            // Stores the first character of inputText as the string _inputText
            string _inputText = inputText[0].ToString();


            if (_inputText.ToUpper().Equals("S"))
            {
                smsMessage = true;
                return true;
            }
            else if (_inputText.ToUpper().Equals("E"))
            {
                emailMessage = true;
                return true;
            }
            else if (_inputText.ToUpper().Equals("T"))
            {
                tweetMessage = true;
                return true;
            }
            else
            {
                MessageBox.Show("The header is incorrect. Please check and try again.");
            }

            return false;
        }

        /// <summary>
        /// This method validates the contents of the main body of input text based on which type of message is being input. The method MessageHeaderInputValidation()
        /// determines what type of message is being input then changes a boolean value based on the message type. In this method the boolean value that has been changed is
        /// found then the message body is validated based on which boolean has been changed.
        /// </summary>
        /// <param name="inputText">Passes in the message body, to be validated</param>
        /// <returns>A boolean value which returns true if the message being input passes the validation based on the which boolean value is set to true
        /// from the method MessageHeaderInputValidation()</returns>
        public bool MessageBodyInputValidation(string inputText)
        {   
            EmailAddressAttribute emailAddressCheck = new EmailAddressAttribute();
            PhoneAttribute phoneNumberCheck = new PhoneAttribute();

            // SMS Validation
            // This IF statement deals with the validation if the message has been identified as a SMS            
            if (smsMessage.Equals(true))
            {
                bool smsCheck = true;
                smsMessage = false;

                // while loop that extracts the phone number then uses the IsValid() from the PhoneAttribute class
                while (smsCheck)
                {
                    if (inputText.Length > 20)
                    {
                        for (int i = 20; i > 7; i--)
                        {
                            // Creates a substring then checks that value with IsValid()
                            sender = inputText.Trim().Substring(0, i);

                            // If the value is a valid phone number, inputText minus the phone number is set to the string text
                            if (phoneNumberCheck.IsValid(sender))
                            {
                                text = inputText.Trim().Substring(i);
                                smsCheck = false;
                                break;
                            }
                        }
                    }                   
                    else if (inputText.Length > 7 && inputText.Length < 21)
                    {
                        for (int i = inputText.Length; i > 7; i--)
                        {
                            // Creates a substring then checks that value with IsValid()
                            sender = inputText.Trim().Substring(0, i);

                            // If the value is a valid phone number, inputText minus the phone number is set to the string text
                            if (phoneNumberCheck.IsValid(sender))
                            {
                                text = inputText.Trim().Substring(i);
                                smsCheck = false;
                                break;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("The phone number entered, is not a valid phone number.");
                        return false;
                    }
                }

                // Checks the length of the SMS
                if (text.Length > 140)
                {
                    MessageBox.Show("The SMS is too long. It can only be upto a maximum of 140 characters");
                    return false;
                }

                SetPublicVariable();                                
                return true;
            }

            // EMAIL Validation
            // This IF statement deals with the validation if the message has been identified as a Email  
            if (emailMessage.Equals(true))
            {
                emailMessage = false;

                string[] splitProText = inputText.Trim().Split(' ');

                string[] nameArray = { };
                string[] subjectAndTextArray = { };

                string subjectAndText = string.Empty;
                bool emailAddressFound = false;

                for (int i = 0; i < splitProText.Length; i++)
                {
                    if (splitProText[i].Contains("@"))
                    {
                        // Checks if the email is a valid email address
                        if (emailAddressCheck.IsValid(splitProText[i]))
                        {
                            emailAddressFound = true;
                            sender = splitProText[i];
                            splitProText[i] = "";

                            nameArray = splitProText.Take(i).ToArray();
                            subjectAndTextArray = splitProText.Skip(i).ToArray();

                            break;                           
                        }                        
                    }                    
                }

                if (emailAddressFound.Equals(false))
                {
                    MessageBox.Show("You have entered an incorrect email address.");
                    return false;
                }

                // Concatenate all the elements into a StringBuilder.
                StringBuilder nameBuilder = new StringBuilder();
                foreach (string value in nameArray)
                {
                    nameBuilder.Append(value);
                    nameBuilder.Append(' ');
                }

                name = nameBuilder.ToString().Trim();

                // Concatenate all the elements into a StringBuilder.
                StringBuilder subjectAndTextBuilder = new StringBuilder();
                foreach (string value in subjectAndTextArray)
                {
                    subjectAndTextBuilder.Append(value);
                    subjectAndTextBuilder.Append(' ');
                }

                subjectAndText = subjectAndTextBuilder.ToString().Trim();

                if (subjectAndText.ToUpper().StartsWith("SIR"))
                {
                    // Creates substrings from newInputText
                    subject = subjectAndText.Trim().Substring(0, 12);
                    text = subjectAndText.Trim().Substring(12);
                }
                else
                {
                    // Creates substrings from newInputText
                    subject = subjectAndText.Trim().Substring(0, 20);
                    text = subjectAndText.Trim().Substring(20);
                }

                // Checks the email doesn't exceed the maximum length
                if (text.Length > 1048)
                {
                    MessageBox.Show("This email is longer than 1028 max characters");
                    return false;
                }

                SetPublicVariable();                         
                return true;
            }

            // TWEET Validation
            // This IF statement deals with the validation if the message has been identified as a Tweet  
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

                // Checks that the tweet text length is less than 140 characters
                if (text.Length > 140)
                {
                    MessageBox.Show("The tweet text is more than 140 characters in length.");
                    return false;
                }

                SetPublicVariable();                         
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets string values back to empty.
        /// </summary>
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
        
        /// <summary>
        /// Adds message to the list called listOfMessages
        /// </summary>
        /// <param name="inputText">Used to set the value for MessageText</param>
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
            header = sender = subject = text = string.Empty;
        }

        /// <summary>
        /// This method retrieves the values from the stored Json file as a list of Json objects
        /// </summary>
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

        #endregion

        #region Private Method

        /// <summary>
        /// This method sets the public variables from the private equivalent
        /// </summary>
        private void SetPublicVariable()
        {
            Header = header;
            Sender = sender;
            Subject = subject;
            Text = text;
        }

        #endregion

        #region User's Choice Validation Methods      

        /// <summary>
        /// Method which displays a messagebox asking the user if they are sure they want to exit the application
        /// And gives them the option of answering 'yes' or 'no'        
        /// </summary>
        public void ExitApplicationValidation()
        {
            MessageBoxResult yesOrNo = MessageBox.Show("Are you sure you want to exit the application?", "Exit Application", MessageBoxButton.YesNo);

            if (yesOrNo == MessageBoxResult.Yes)
            {
                File.WriteAllText(@".\hashtags.csv", String.Empty);
                File.WriteAllText(@".\mentions.csv", String.Empty);
                File.WriteAllText(@".\sir.csv", String.Empty);

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
