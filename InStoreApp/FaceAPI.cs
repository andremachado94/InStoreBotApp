using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using Microsoft.ProjectOxford.Common.Contract;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System;
using Windows.Media;
using Windows.Media.Core;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using System.IO;
using System.Diagnostics;

namespace InStoreApp
{
    class FaceAPI
    {
        private MediaCapture mediaCapture;
        bool isPreviewing;
        private static FaceServiceClient faceServiceClient = new FaceServiceClient("455142f3e9cc4acd8749ade9534d34b0", "https://northeurope.api.cognitive.microsoft.com/face/v1.0");
        private static string personGroupId = "myfriends";


        public FaceAPI()
        {


        }

        public async Task capturePhoto()
        {
            if(mediaCapture == null)
            {
                mediaCapture = new MediaCapture();
                await mediaCapture.InitializeAsync();
                //mediaCapture.Failed += MediaCapture_Failed;

            }
            // Prepare and capture photo
            /*var lowLagCapture = await mediaCapture.PrepareLowLagPhotoCaptureAsync(ImageEncodingProperties.CreateUncompressed(MediaPixelFormat.Bgra8));

            var capturedPhoto = await lowLagCapture.CaptureAsync();
            var bitmap = capturedPhoto.Frame.SoftwareBitmap;
            */
            var stream = new InMemoryRandomAccessStream();
            await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), stream);
            stream.Seek(0);
            var faces = await faceServiceClient.DetectAsync(stream.AsStream());
            
            var faceIds = faces.Select(face => face.FaceId).ToArray();

            var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);
            foreach (var identifyResult in results)
            {
                Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                if (identifyResult.Candidates.Length == 0)
                {
                    Debug.WriteLine("No one identified");
                }
                else
                {
                    // Get top 1 among all candidates returned
                    var candidateId = identifyResult.Candidates[0].PersonId;
                    var person = await faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                    Debug.WriteLine("Identified as {0}", person.Name);
                }
            }
            //await lowLagCapture.FinishAsync();
        }
    }
}
