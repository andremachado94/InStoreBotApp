using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.Media.MediaProperties;
using Windows.Media.Audio;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http;
using System.Threading;
using System.Text;
using System.Runtime.Serialization;
using Windows.Data.Json;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Core;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InStoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {
        MainController mc;

        private BitmapImage languageIcon2 = new BitmapImage(new Uri("ms-appx:///Images/languages2.png"));
        private Timer timer;
        private Boolean IsFaceDetected { get; set; }
        private Boolean IsAppActive { get; set; }
        private FaceAPI fc;


        public MainPage()
        {
            this.InitializeComponent();
            mc = new MainController(this);
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(700, 850));
            ApplicationView.PreferredLaunchViewSize = new Size(700,850);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            myFrame.Navigate(typeof(InitialPage));
            
            fc = new FaceAPI();
            fc.start();
            start_face_timer();
            mc.Start();
        }
        
        private async void Button_Start(object sender, RoutedEventArgs e)
        {
            this.button_start.IsEnabled = false;
            await mc.Start();
            this.button_start.IsEnabled = true;
            myFrame.Navigate(typeof(ChatPage));
        }

        private async void Button_Stop(object sender, RoutedEventArgs e)
        {
            mc.Stop();
            this.button_start.IsEnabled = true;
        }

        private async void Button_Play(object sender, RoutedEventArgs e)
        {
            string res = await fc.capturePhoto();

        }
        private void Button_Quit(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(InitialPage));
        }
        private void Button_Call(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(ChatPage));

            //Get botAnswer textBlock inside chatpage
            Frame rootFrame = Window.Current.Content as Frame;
            Page mainPage = rootFrame.Content as MainPage;
            Frame frame = mainPage.FindName("myFrame") as Frame;
            Page chatpage = frame.Content as ChatPage;
            var botAnswer = chatpage.FindName("botAnswer") as TextBlock;
            botAnswer.Text = "O funcionário não deve demorar! \n";
        }
        
        private void Button_language(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(typeof(LanguagePage));
            this.changeLanguage.Source = languageIcon2;
        }

        private void start_face_timer()
        {
            TimerState2 s = new TimerState2();
            TimerCallback timerDelegate = new TimerCallback(CheckCameraStatus);
            timer = new Timer(timerDelegate, s, 10000, 2000);
            s.tmr = timer;
        }

        private async void CheckCameraStatus(Object state)
        {//do what you want here instead of what follows
            TimerState2 s = (TimerState2)state;
            s.counter++;
            //Debug.WriteLine("Timer iteration: " + s.counter);

            string res = await fc.capturePhoto();
            IsFaceDetected = (res == null) ? false : true; 

            if (IsAppActive)
            {
                if (!IsFaceDetected)
                {
                    //mc.Stop();
                    IsAppActive = false;
                    Debug.WriteLine("Stoping");
                    mc.SetFaceDetected(false);

                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            // Your UI update code goes here!
                            myFrame.Navigate(typeof(InitialPage));
                        }
                        );

                }
            }
            else
            {
                if (IsFaceDetected)
                {
                    IsAppActive = true;

                    //await mc.Start();
                    //this.button_start.IsEnabled = true;
                    await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                        () =>
                        {
                            // Your UI update code goes here!
                            myFrame.Navigate(typeof(ChatPage));
                        }
                        );
                   
                    mc.SetFaceDetected(true);
                    Debug.WriteLine("Starting");

                }
            }

        }


    }
}
