using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace InStoreApp
{
    
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LanguagePage : Page
    {

        private BitmapImage languageIcon1 = new BitmapImage(new Uri("ms-appx:///Images/languages.png"));

        public LanguagePage()
        {
            this.InitializeComponent();
        }

        private void Language_Click(object sender, RoutedEventArgs e)
        {
            HyperlinkButton b = sender as HyperlinkButton;
            string languageCode = b.Tag.ToString();
            SetLanguage(languageCode);
            Debug.WriteLine("----------------------> Language : "+ languageCode);
            
        }

        private void SetLanguage(String l)
        {
            MainController.language = l;

            Frame rootFrame = Window.Current.Content as Frame;
            Page mainPage = rootFrame.Content as MainPage;
            var button = mainPage.FindName("changeLanguage") as Image;
            button.Source = languageIcon1;
            
            Frame.GoBack();
        }
    }
}
