﻿using System;
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


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InStoreApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class MainPage : Page
    {

        private SpeechToTextAPI stt;

        public MainPage()
        {
            this.InitializeComponent();
           // stt = new SpeechToTextAPI(this.button_start, this.textBlock);
        }

        /*
        private async Task SaySomething(string s)
        {
             
             MediaElement mediaElement = new MediaElement();
             var synth = new Windows.Media.SpeechSynthesis.SpeechSynthesizer();
             Windows.Media.SpeechSynthesis.SpeechSynthesisStream stream = await synth.SynthesizeTextToStreamAsync(s);
             mediaElement.SetSource(stream, stream.ContentType);
             mediaElement.Play();
            

        }
        */
        private async void Button_Start(object sender, RoutedEventArgs e)
        {
           // stt.Record();
        }

        private async void Button_Stop(object sender, RoutedEventArgs e)
        {
          //  stt.StopRecording();
        }

        private async void Button_Play(object sender, RoutedEventArgs e)
        {
          //  stt.Play();
        }
    }
}
