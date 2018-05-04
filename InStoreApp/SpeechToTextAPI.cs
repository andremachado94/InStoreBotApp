using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Controls;
using Windows.Media.Capture;
using Windows.Storage.Streams;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.ApplicationModel;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Threading;
using Windows.Data.Json;

namespace InStoreApp
{

    class TimerState
    {
        public int counter = 0;
        public Timer tmr;
    }


    class SpeechToTextAPI
    {
        private MediaCapture _mediaCapture;
        private InMemoryRandomAccessStream _memoryBuffer;
        private readonly string DEFAULT_AUDIO_FILENAME = "output";
        private string _fileName;
        private Authentication auth;
        private string token;
        private TtsRecognitionSimpleResult lastTtsResult;
        private bool shouldStopRecording = false;
        private bool resultProcessed = false;

        private Button button_start;
        private TextBlock textBlock;

        public SpeechToTextAPI(Button button_start, TextBlock textBlock)
        {
            this.button_start = button_start;
            this.textBlock = textBlock;

            auth = new Authentication("a6881fd6f059434e8567557545d86c38");
            token = auth.GetAccessToken();
        }

        public async void Play()
        {
            MediaElement playbackMediaElement = new MediaElement();
            playbackMediaElement.SetSource(_memoryBuffer, "Wav");
            Debug.WriteLine("Size: " + _memoryBuffer.Size);
            var bytes = new byte[_memoryBuffer.Size];
            await _memoryBuffer.ReadAsync(bytes.AsBuffer(), (uint)_memoryBuffer.Size, Windows.Storage.Streams.InputStreamOptions.None);

            playbackMediaElement.Play();
        }

        private TtsRecognitionSimpleResult ParseTtsRecognitionSimpleResult(string json)
        {
            try
            {
                TtsRecognitionSimpleResult result = new TtsRecognitionSimpleResult();
                JsonObject jObject = JsonObject.Parse(json);
                result.RecognitionStatus = jObject.GetNamedString("RecognitionStatus", "");
                result.DisplayText = jObject.GetNamedString("DisplayText", "None");
                result.Duration = jObject.GetNamedNumber("Duration", 0);
                result.Offset = jObject.GetNamedNumber("Offset", 0);

                return result;

            }
            catch (Exception e)
            {

            }
            return null;
        }

        public bool IsRecording { get; set; }

        public async void Record()
        {
            if (IsRecording)
            {
                throw new InvalidOperationException("Sound recording already in progress.");
            }
            lastTtsResult = new TtsRecognitionSimpleResult();
            resultProcessed = false;
            _memoryBuffer = new InMemoryRandomAccessStream();
            //await DeleteExistingFile();
            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Audio
            };
            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(settings);
            await _mediaCapture.StartRecordToStreamAsync(MediaEncodingProfile.CreateWav(AudioEncodingQuality.Auto), _memoryBuffer);
            button_start.IsEnabled = false;
            IsRecording = true;

            TimerState s = new TimerState();
            TimerCallback timerDelegate = new TimerCallback(CheckStatus);
            Timer timer = new Timer(timerDelegate, s, 1000, 1000);
            s.tmr = timer;


            TimerState s1 = new TimerState();
            TimerCallback timerDelegate1 = new TimerCallback(CheckStopRecording);
            Timer timer1 = new Timer(timerDelegate1, s1, 1000, 40);
            s1.tmr = timer1;


        }

        private async void CheckStatus(Object state)
        {//do what you want here instead of what follows
            TimerState s = (TimerState)state;
            s.counter++;
            Debug.WriteLine("Timer iteration: " + s.counter);


            if (!IsRecording)
            {
                if (s.tmr != null)
                    s.tmr.Dispose();

                s.tmr = null;
            }
            else
            {
                await SttServiceCaller();

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                   () => {
                       textBlock.Text = lastTtsResult.DisplayText;
                   }
               );
            }
        }

        private void CheckStopRecording(Object state)
        {//do what you want here instead of what follows
            TimerState s = (TimerState)state;
            if (shouldStopRecording)
            {
                shouldStopRecording = false;
                StopRecording();

            }

        }

        public async void StopRecording()
        {
            if (_mediaCapture != null)
            {
                Debug.WriteLine("Stopping Recording");
                try
                {
                    await _mediaCapture.StopRecordAsync();

                    _mediaCapture = null;
                }
                catch (COMException)
                {
                    Debug.WriteLine("COM Exception - Only God knows why");
                }
            }

            IsRecording = false;

            //string result = ProcessRecordingResult();

            //await SaySomething(result);

            //Record();


            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                () => {
                    button_start.IsEnabled = true;
                }
            );

            //SaveAudioToFile();
        }

        private string ProcessRecordingResult()
        {
            return "Bom dia João, em que posso ajudá-lo";
        }

        private async void SaveAudioToFile()
        {
            IRandomAccessStream audioStream = _memoryBuffer.CloneStream();
            StorageFolder storageFolder = Package.Current.InstalledLocation;
            StorageFile storageFile = await storageFolder.CreateFileAsync(
                DEFAULT_AUDIO_FILENAME, CreationCollisionOption.GenerateUniqueName);
            this._fileName = storageFile.Name;
            using (IRandomAccessStream fileStream = await storageFile.OpenAsync(FileAccessMode.ReadWrite))
            {
                await RandomAccessStream.CopyAndCloseAsync(audioStream.GetInputStreamAt(0), fileStream.GetOutputStreamAt(0));
                await audioStream.FlushAsync();
                audioStream.Dispose();
            }
        }

        public async Task SttServiceCaller()
        {

            if (IsRecording && !shouldStopRecording)
            {

                string requestUri = "https://speech.platform.bing.com/speech/recognition/interactive/cognitiveservices/v1?language=pt-PT&format=simple";
                string host = @"speech.platform.bing.com";
                string contentType = @"audio/wav; codec=""audio/pcm""; samplerate=16000";

                /*
                 * Input your own audio file or use read from a microphone stream directly.
                 */
                string responseString;

                try
                {
                    HttpWebRequest request = null;
                    request = (HttpWebRequest)HttpWebRequest.Create(requestUri);
                    request.Accept = @"application/json;text/xml";
                    request.Method = "POST";
                    request.ContentType = contentType;
                    request.Headers["Authorization"] = "Bearer " + token;

                    Stream fs;
                    using (fs = _memoryBuffer.CloneStream().AsStream())
                    {

                        /*
                         * Open a request stream and write 1024 byte chunks in the stream one at a time.
                         */
                        byte[] buffer = null;
                        int bytesRead = 0;
                        using (Stream requestStream = await Task.Factory.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null))
                        {

                            Debug.WriteLine("Sending request o STT API");
                            /*
                             * Read 1024 raw bytes from the input audio file.
                             */
                            buffer = new Byte[checked((uint)Math.Min(1024, (int)fs.Length))];
                            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                requestStream.Write(buffer, 0, bytesRead);
                            }

                            // Flush
                            requestStream.Flush();
                        }

                        /*
                         * Get the response from the service.
                         */
                        Debug.WriteLine("Response:");
                        using (WebResponse response = await request.GetResponseAsync())
                        {
                            Debug.WriteLine(((HttpWebResponse)response).StatusCode);

                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseString = sr.ReadToEnd();
                            }




                            Debug.WriteLine(responseString);
                            TtsRecognitionSimpleResult result = ParseTtsRecognitionSimpleResult(responseString);

                            if (!result.RecognitionStatus.Equals("InitialSilenceTimeout"))
                            {
                                if (result.DisplayText.Equals(lastTtsResult.DisplayText))
                                {
                                    shouldStopRecording = true;
                                }
                            }
                            lastTtsResult = result;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                    Debug.WriteLine(ex.Message);
                }
            }
        }


    }
}
