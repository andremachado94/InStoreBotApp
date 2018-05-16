using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.SpeechSynthesis;
using Windows.UI.Xaml.Controls;

namespace InStoreApp
{
    public static class TextToSpeech
    {

        public async static Task TTSbegin(string message)
        {
            MediaElement media = new MediaElement();
            var cortana = new SpeechSynthesizer();
            VoiceInformation v = SpeechSynthesizer.AllVoices[0];

            cortana.Voice = v;
            foreach(VoiceInformation vo in SpeechSynthesizer.AllVoices)
            {
                if (vo.Language.Equals("pt-PT"))
                {
                    cortana.Voice = vo;
                    break;
                }
            }
            SpeechSynthesisStream synthesisStream = await cortana.SynthesizeTextToStreamAsync(message);
            media.AutoPlay = true;
            media.SetSource(synthesisStream, synthesisStream.ContentType);
            media.Play();
        }
    }
}