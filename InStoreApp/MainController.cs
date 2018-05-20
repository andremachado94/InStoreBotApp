using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStoreApp
{
    class MainController
    {
        public const int STATE_NOT_INITIALIZED = 0;
        public const int STATE_IDLE = 1;
        public const int STATE_SPEAKING = 2;
        public const int STATE_LISTENING = 3;


        public bool IsRecordingSound { get; set; }
        public bool IsSpeaking { get; set; }
        public bool IsWaitingLUIS { get; set; }
        public bool IsWaitingTts { get; set; }
        public bool IsWaitingStt { get; set; }
        public bool IsFaceDetected { get; set; }


        public int currentState = STATE_NOT_INITIALIZED;
        public int previousState = STATE_NOT_INITIALIZED;


        private MainPage mainPage;
        private SpeechToText stt;
        private BotConnector bot;


        public MainController(MainPage mainPage)
        {
            this.mainPage = mainPage;
            SetState(STATE_IDLE);
            bot = new BotConnector();
        }

        public async Task Start()
        {
            GC.Collect();
            await bot.startConversation();
            await StartVoiceRec();
        }

        public void SetFaceDetected(bool val)
        {
            IsFaceDetected = val;
        }

        private async Task StartVoiceRec()
        {
            stt = new SpeechToText();
            while (true)
            {
                if (IsFaceDetected)
                {
                    Debug.WriteLine("Starting VoiceRec");
                    TtsRecognitionSimpleResult res = await VoiceRec();
                    if (res != null)
                    {
                        string response = await bot.sendBotMessage(res.DisplayText);
                        Debug.WriteLine("STT result: " + res.DisplayText + "\n" + "Bot response: " + response + "\n");
                        await TextToSpeech.TTSbegin(response);
                    }
                }
                else
                {
                    await Task.Delay(250);
                }

            }
        }

        private async Task<TtsRecognitionSimpleResult> VoiceRec()
        {
            SetState(STATE_LISTENING);
            TtsRecognitionSimpleResult res = await stt.Start();
            return res;
        }

        public void SetState(int state)
        {
            previousState = currentState;
            currentState = state;
        }

        public async void Stop()
        {
            IsFaceDetected = false;
            await stt.StopRecording();
            stt = null;
            GC.Collect();
        }

    }
}
