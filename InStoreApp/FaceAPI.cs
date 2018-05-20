
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
        bool isPreviewing;
        private static string personGroupId = "myfriends";
        FaceServiceClient faceServiceClient;
        MediaCapture mediaCapture;



        public FaceAPI()
        {
            faceServiceClient = new FaceServiceClient("455142f3e9cc4acd8749ade9534d34b0", "https://northeurope.api.cognitive.microsoft.com/face/v1.0");
            mediaCapture = new MediaCapture();
        }

        public async Task start()
        {
            await mediaCapture.InitializeAsync();

        }

        public async Task stop()
        {
            mediaCapture.Dispose();
            mediaCapture = null;
        }

        public async Task<string> capturePhoto()
        {

            var stream = new InMemoryRandomAccessStream();
            await mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateJpeg(), stream);
            stream.Seek(0);
            var faces = await faceServiceClient.DetectAsync(stream.AsStream());
            
            var faceIds = faces.Select(face => face.FaceId).ToArray();

            if (faceIds.Length == 0) return null;

            var results = await faceServiceClient.IdentifyAsync(personGroupId, faceIds);


            foreach (var identifyResult in results)
            {
                Debug.WriteLine("Result of face: {0}", identifyResult.FaceId);
                if (identifyResult.Candidates.Length == 0)
                {
                    Debug.WriteLine("No one identified");
                    return "Teste";
                }
                else
                {
                    // Get top 1 among all candidates returned
                    var candidateId = identifyResult.Candidates[0].PersonId;
                    var person = await faceServiceClient.GetPersonAsync(personGroupId, candidateId);
                    Debug.WriteLine("Identified as {0}", person.Name);
                    return person.Name;
                }
            }
            return null;
        }
    }
}
