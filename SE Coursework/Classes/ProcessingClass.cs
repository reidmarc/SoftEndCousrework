using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE_Coursework.Classes
{
    public class ProcessingClass
    {
        string headerCheck = string.Empty;

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
            MessageBox.Show("Processing SMS");
        }

        private void ProccessedEmail(ref string proText)
        {
            MessageBox.Show("Processing EMAIL");
        }

        private void ProccessedTweet(ref string proText)
        {
            MessageBox.Show("Processing TWEET");
        }



    }
}
