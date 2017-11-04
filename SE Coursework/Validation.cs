using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SE_Coursework
{
    public class Validation
    {











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
