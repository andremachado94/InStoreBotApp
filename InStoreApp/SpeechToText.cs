using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;


namespace InStoreApp
{
    class TimerState2
    {
        public int counter = 0;
        public Timer tmr;
    }

    class SpeechToText
    {
        //Authentication variables
        private Authentication auth;
        private string token;


        private InMemoryRandomAccessStream _memoryBuffer;
        private readonly string DEFAULT_AUDIO_FILENAME = "output";
        private TtsRecognitionSimpleResult ttsResult = new TtsRecognitionSimpleResult();
        private bool shouldStopRecording = false;
        private bool resultProcessed = false;
        MediaCapture _mediaCapture;

        private Task currentServiceCallerTask = null;

        private int serviceCallerCounter = 0;
        public bool ShouldCleanBuffers { get; set; }



        public bool IsRecording { get; set; }
        public bool IsRecoOver { get; set; }
        public bool IsServiceCallerTaskRunning { get; private set; }

        Timer timer;
        Timer timer1;


        public SpeechToText()
        {
            auth = new Authentication("a6881fd6f059434e8567557545d86c38");
            token = auth.GetAccessToken();
        }

        public async Task<TtsRecognitionSimpleResult> Start()
        {
            IsRecoOver = false;

            await StartRecording();
            StartTimers();

            int counter = 0;
            bool hasRes = true;

            while (!IsRecoOver)
            {
                await Task.Delay(250);
                counter++;

                if(counter * 250 >= 10000)
                {
                    hasRes = false;
                    break;
                }
            }

            await StopRecording();

            return hasRes? ttsResult : null;
        }

        private async Task StartRecording()
        {
            //ttsResult = new TtsRecognitionSimpleResult();
            _memoryBuffer = new InMemoryRandomAccessStream();

            MediaCaptureInitializationSettings settings = new MediaCaptureInitializationSettings
            {
                StreamingCaptureMode = StreamingCaptureMode.Audio
            };

            _mediaCapture = new MediaCapture();
            await _mediaCapture.InitializeAsync(settings);
            await _mediaCapture.StartRecordToStreamAsync(MediaEncodingProfile.CreateWav(AudioEncodingQuality.Medium), _memoryBuffer);

            IsRecording = true;

        }


        private void StartTimers()
        {
            TimerState2 s = new TimerState2();
            TimerCallback timerDelegate = new TimerCallback(CheckStatus);
            timer = new Timer(timerDelegate, s, 0, 3000);
            s.tmr = timer;


            TimerState2 s1 = new TimerState2();
            TimerCallback timerDelegate1 = new TimerCallback(CheckStopRecording);
            timer1 = new Timer(timerDelegate1, s1, 0, 40);
            s1.tmr = timer1;
        }


        public async Task StopRecording()
        {

            if (_mediaCapture != null)
            {
                //timer.Dispose();
                //timer1.Dispose();
                //Debug.WriteLine("Stopping Recording");
                try
                {
                    await _mediaCapture.StopRecordAsync();

                    await _memoryBuffer.FlushAsync();
                    Debug.WriteLine("Flushed");
                    _mediaCapture = null;
                    IsRecording = false;
                }
                catch (COMException)
                {
                    //Debug.WriteLine("COM Exception - Only God knows why");
                }
            }

            IsRecording = false;
            shouldStopRecording = false;
            resultProcessed = false;

            GC.Collect();

            //string result = ProcessRecordingResult();

            //await SaySomething(result);

            //Record();

            //SaveAudioToFile();
        }






        public async Task SttServiceCaller(int i)
        {
            IsServiceCallerTaskRunning = true;

            if (IsRecording && !shouldStopRecording && _memoryBuffer != null && _mediaCapture != null)
            {
                serviceCallerCounter++;

                //Debug.WriteLine("ServiceCaller - true");

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

                            //Debug.WriteLine("Sending request o STT API");
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

                            fs.Dispose();

                            // Debug.WriteLine("Sent!");

                        }

                        /*
                         * Get the response from the service.
                         */
                        //Debug.WriteLine("Waiting for Response:");
                        try
                        {

                            using (var response = await request.GetResponseAsync().WithTimeout(3000).ConfigureAwait(false))
                            {
                                //Debug.WriteLine("Hello!");

                                //Debug.WriteLine(((HttpWebResponse)response).StatusCode);

                                if (response != null)
                                {

                                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                    {
                                        responseString = sr.ReadToEnd();
                                    }


                                    if (responseString != null && responseString.Length > 0)
                                    {
                                        //Debug.WriteLine(responseString + "   -   " + i);
                                        TtsRecognitionSimpleResult result = ParseTtsRecognitionSimpleResult(responseString);

                                        if (!result.RecognitionStatus.Equals("InitialSilenceTimeout"))
                                        {
                                            if (result.DisplayText.Equals(ttsResult.DisplayText))
                                            {
                                                shouldStopRecording = true;
                                            }
                                        }
                                        else
                                        {
                                            //if (i % 6 == 0)
                                            //   await Restart();
                                            if (serviceCallerCounter % 2 == 0) ShouldCleanBuffers = true;

                                        }
                                        ttsResult = result;
                                        Debug.WriteLine("Result: " + result.DisplayText);
                                    }
                                }
                                else
                                {
                                    request.Abort();
                                }

                            }

                        }
                        catch (WebException e)
                        {
                            // await Restart();
                            IsServiceCallerTaskRunning = false;
                            //Debug.WriteLine("ServiceCaller - false");
                            Debug.WriteLine(e.ToString());


                        }
                        //Debug.WriteLine("Response received");
                    }
                }
                catch (Exception ex)
                {
                    //await Restart();
                    IsServiceCallerTaskRunning = false;
                    //Debug.WriteLine("ServiceCaller - false");
                    Debug.WriteLine(ex.ToString());
                   
                }
            }
            IsServiceCallerTaskRunning = false;
            //Debug.WriteLine("ServiceCaller - false");
        }

        /*
        private async Task Restart()
        {
            await StopRecording();
            IsRecording = true;
            await StartRecording();
            //StartTimers();
        }
        */

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


        private async void CheckStatus(Object state)
        {//do what you want here instead of what follows
            TimerState2 s = (TimerState2)state;
            s.counter++;
            //Debug.WriteLine("Timer iteration: " + s.counter);


            if (!IsRecording)
            {
                //Debug.WriteLine("In timer: Is not recording");
                if (s.tmr != null)
                    s.tmr.Dispose();

                s.tmr = null;
            }
            else
            {
                if (!IsServiceCallerTaskRunning)
                    await SttServiceCaller(s.counter);
            }

        }

        private void CheckStopRecording(Object state)
        {//do what you want here instead of what follows
            TimerState2 s = (TimerState2)state;

            if (ShouldCleanBuffers)
            {
                ShouldCleanBuffers = false;
                //Debug.WriteLine("Flushing Memory buffer");
                //_memoryBuffer.FlushAsync();
            }

            if (shouldStopRecording)
            {
                shouldStopRecording = false;
                IsRecoOver = true;
                //StopRecording();

                if (s.tmr != null)
                    s.tmr.Dispose();

                s.tmr = null;

            }

        }

    }


}
