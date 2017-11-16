//////////////////////////////////////////////////////////////////////////////////////////////////////
////////////////////////////////////////// Window MainWindow /////////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////
///////////////////////////////////// Code Written By: 03001588 //////////////////////////////////////
//////////////////////////////////////////////////////////////////////////////////////////////////////

#region Usings

using System.Windows;

#endregion

namespace SE_Coursework
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Sets the content of the window to the menu page on launch
            MainFrame.Content = new SE_Coursework.Pages.MenuPage();

            // Sets the size of the window to the si
            SetWindowSize();
        }

        /// <summary>
        /// This method sets the size of the window to dynamically adjust depending on the size of the page being viewed
        /// </summary>
        private void SetWindowSize()
        {   
            // This automatically resizes the height and the width relative to content displayed (i.e. pages)
            this.SizeToContent = SizeToContent.WidthAndHeight;
        }
    }
}
