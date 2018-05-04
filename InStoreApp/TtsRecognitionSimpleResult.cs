using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InStoreApp
{
    class TtsRecognitionSimpleResult
    {

        public string RecognitionStatus { get; set; }
        public string DisplayText { get; set; }
        public double Offset { get; set; }
        public double Duration { get; set; }

        public TtsRecognitionSimpleResult() {
            RecognitionStatus = "";
            DisplayText = "";
            Offset = 0;
            Duration = 0;
        }

    }
}
