using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;
using System.IO;

namespace ColdTeaWCF
{
    class AzureFace 
    {
        private static IFaceServiceClient faceServiceClient = new FaceServiceClient("f64a23b414764e85af459dee5e50537f");
        public double GetSimilar(string URL1,string URL2)
        {
            var test = new Test();
            var similarityResult = test.CalculateSimilarity(URL1,URL2);
            Task.WaitAll(similarityResult);

            Console.WriteLine();

            var detectionResult = test.DetectFaces();
            Task.WaitAll(detectionResult);        

            return similarityResult.Result.Confidence;
        }

        public class Test
        {
            public async Task<VerifyResult> CalculateSimilarity(string URL1,string URL2)
            {
                var face1 = await faceServiceClient.DetectAsync(URL1);
                var face2 = await faceServiceClient.DetectAsync(URL2);
                var result = await faceServiceClient.VerifyAsync(face1[0].FaceId, face2[0].FaceId);
                Console.WriteLine("Confidence: ");
                Console.WriteLine(result.Confidence);
                Console.WriteLine("Isidentical: ");
                Console.WriteLine(result.IsIdentical);
                return result;
            }

            public async Task<Face[]> DetectFaces()
            {
                var result = await faceServiceClient.DetectAsync("http://news.xinhuanet.com/world/2013-06/09/124835868_11n.jpg", true, true, true, true);
                for (int i = 0; i < result.Length; ++i)
                {
                    Console.WriteLine("Person" + (i + 1).ToString());
                    Console.WriteLine("Gender: " + result[i].Attributes.Gender);
                    Console.WriteLine("Age: " + result[i].Attributes.Age);
                }
                return result;
            }
        }
    }
}
