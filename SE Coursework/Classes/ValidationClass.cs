using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE_Coursework.Classes
{
    public class ValidationClass
    {
        bool smsMessage = false;
        bool emailMessage = false;
        bool tweetMessage = false;


        // Default Constructor
        public ValidationClass()
        {

        }

        




        public bool MessageHeaderInputValidation(string inputText)
        {
            // Stores the first character of inputText as the string _inputText
            string _inputText = inputText[0].ToString();

            // Trims the input then stores it as the string numeric
            string numeric = inputText.Trim();

            // Stores a substring of numeric as the string subStringNumeric
            string subStringNumeric = numeric.Substring(1, 9);

            // Checks the length of the input
            if (!(numeric.Length).Equals(10))
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
            if (smsMessage.Equals(true))
            {

            }

            if (emailMessage.Equals(true))
            {

            }

            if (tweetMessage.Equals(true))
            {

            }

            return false;
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
