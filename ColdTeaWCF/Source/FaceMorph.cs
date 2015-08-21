using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Net;
using System.IO;

using Microsoft.ProjectOxford.Face;
using Microsoft.ProjectOxford.Face.Contract;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.Structure;
using System.Drawing;
using System.Configuration;

namespace ColdTeaWCF
{
    enum Position { 
        EyebrowLeftOuter,
        EyebrowLeftInner,
        EyebrowRightOuter,
        EyebrowRightInner,
        EyeLeftOuter,
        EyeLeftTop,
        EyeLeftInner,
        EyeLeftBottom,
        PupilLeft,
        EyeRightOuter,
        EyeRightTop,
        EyeRightInner,
        EyeRightBottom,
        PupilRight,
        NoseRootLeft,
        NoseLeftAlarTop,
        NoseLeftAlarOutTip,
        NoseTip,
        NoseRightAlarOutTip,
        NoseRightAlarTop,
        NoseRootRight,
        MouthLeft,
        UpperLipTop,
        MouthRight,
        UnderLipBottom,
        UpperLipBottom,
        UnderLipTop
    };

    class FaceMorph
    {
        static string picFolder = ConfigurationManager.AppSettings["ImgDirectory"];
        static string returnURL = ConfigurationManager.AppSettings["ReturnURL"];
        //Parameter -- 0.5
        
        
        public string MorphLocal(string FileName1, string FileName2, double Parameter)
        {
            string FilePath1 = picFolder + FileName1;
            string FilePath2 = picFolder + FileName2;
            return Morph(FilePath1,FilePath2, Parameter);
        }
        public string MorphByURL(string URL1, string URL2 , double Parameter)
        {
            string FilePath1 = picFolder + "Download1-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff") + ".jpg";
            string FilePath2 = picFolder + "Download2-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff") + ".jpg";
            if (!DownloadPicture(URL1, FilePath1, -1))
                return null;
            if (!DownloadPicture(URL2, FilePath2, -1))
                return null;

            return Morph(FilePath1, FilePath2, Parameter);
        }

        public string Morph(string FilePath1, string FilePath2, double Parameter)
        {
            //try
            {
                FaceRectangle[] obamaRect, kimRect;
                FaceLandmarks[] obamaLandmarks, kimLandmarks;

                runFaceAPI(FilePath1, out obamaRect, out obamaLandmarks);
                runFaceAPI(FilePath2, out kimRect, out kimLandmarks);

                PointF[] obamaLandmarkArr = convertLandmarkFormation(ref obamaLandmarks[0], ref obamaRect[0]);
                PointF[] kimLandmarkArr = convertLandmarkFormation(ref kimLandmarks[0], ref kimRect[0]);

                Rectangle obamaRectangle = convertRectangleFormation(obamaRect[0]);
                Rectangle kimRectangle = convertRectangleFormation(kimRect[0]);

                Image<Bgr, byte> obamaFace = new Image<Bgr, byte>(FilePath1).GetSubRect(obamaRectangle);
                Image<Bgr, byte> kimFace = new Image<Bgr, byte>(FilePath2).GetSubRect(kimRectangle);

                FaceIntegration faceIntegration = new FaceIntegration(
                    obamaFace,
                    kimFace,
                    obamaLandmarkArr,
                    kimLandmarkArr,
                    new Size(300, 300),
                    Parameter);
                Image<Bgr, byte> dstFace = faceIntegration.integrateFace();

                string fileout = "result-" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-ffff") + ".jpg";
                dstFace.Save(picFolder + fileout);
                return (returnURL + fileout);
            }
            //catch (Exception ex)
            //{
            //    return null;
            //}

        }
        private static void runFaceAPI(
            string _filePath, 
            out FaceRectangle[] _rects, 
            out FaceLandmarks[] _landmarks)
        {
            string key = "1cdb412565ae43879ea8133525e89040";
            FaceAPI faceAPI = new FaceAPI(key);
            var detectResult = faceAPI.detectFaces(_filePath);
            Task.WaitAll(detectResult);
            if (detectResult.Result)
            {
                _rects = faceAPI.getFaceRectangles();
                _landmarks = faceAPI.getFaceLandmarks();
            }
            else
            {
                _rects = new FaceRectangle[0];
                _landmarks = new FaceLandmarks[0];
            }
        }

        private static PointF[] convertLandmarkFormation(
            ref FaceLandmarks _landmarks,
            ref FaceRectangle _rectangle)
        {
            PointF[] retLandmarks = new PointF[27]{
                convertPointFormation(_landmarks.EyebrowLeftOuter, _rectangle),
                convertPointFormation(_landmarks.EyebrowLeftInner, _rectangle),
                convertPointFormation(_landmarks.EyebrowRightOuter, _rectangle),
                convertPointFormation(_landmarks.EyebrowRightInner, _rectangle),
                convertPointFormation(_landmarks.EyeLeftOuter, _rectangle),
                convertPointFormation(_landmarks.EyeLeftTop, _rectangle),
                convertPointFormation(_landmarks.EyeLeftInner, _rectangle),
                convertPointFormation(_landmarks.EyeLeftBottom, _rectangle),
                convertPointFormation(_landmarks.PupilLeft, _rectangle),
                convertPointFormation(_landmarks.EyeRightOuter, _rectangle),
                convertPointFormation(_landmarks.EyeRightTop, _rectangle),
                convertPointFormation(_landmarks.EyeRightInner, _rectangle),
                convertPointFormation(_landmarks.EyeRightBottom, _rectangle),
                convertPointFormation(_landmarks.PupilRight, _rectangle),
                convertPointFormation(_landmarks.NoseRootLeft, _rectangle),
                convertPointFormation(_landmarks.NoseLeftAlarTop, _rectangle),
                convertPointFormation(_landmarks.NoseLeftAlarOutTip, _rectangle),
                convertPointFormation(_landmarks.NoseTip, _rectangle),
                convertPointFormation(_landmarks.NoseRightAlarOutTip, _rectangle),
                convertPointFormation(_landmarks.NoseRightAlarTop, _rectangle),
                convertPointFormation(_landmarks.NoseRootRight, _rectangle),
                convertPointFormation(_landmarks.MouthLeft, _rectangle),
                convertPointFormation(_landmarks.UpperLipTop, _rectangle),
                convertPointFormation(_landmarks.MouthRight, _rectangle),
                convertPointFormation(_landmarks.UnderLipBottom, _rectangle),
                convertPointFormation(_landmarks.UpperLipBottom, _rectangle),
                convertPointFormation(_landmarks.UnderLipTop, _rectangle),
            };
            return retLandmarks;
        }

        private static PointF convertPointFormation(
            FeatureCoordinate _landmark,
            FaceRectangle _rectangle)
        {
            PointF retPoint = new PointF();
            retPoint.X = (float)((_landmark.X - _rectangle.Left) / _rectangle.Width);
            retPoint.Y = (float)((_landmark.Y - _rectangle.Top) / _rectangle.Height);
            return retPoint;
        }

        private static Rectangle convertRectangleFormation(
            FaceRectangle _rectangle)
        {
            return new Rectangle(
                _rectangle.Left,
                _rectangle.Top,
                _rectangle.Width,
                _rectangle.Height);
        }

        
        private bool DownloadPicture(string picUrl, string savePath,int timeOut)        
        { 
            bool value = false;            
            WebResponse response = null;            
            Stream stream = null;            
            try{                
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(picUrl);                
                if (timeOut != -1) request.Timeout = timeOut;                
                response = request.GetResponse();                stream = response.GetResponseStream(); 
                if (!response.ContentType.ToLower().StartsWith("text/"))                 
                    value = SaveBinaryFile(response, savePath);            
            }
            finally{
                if(stream!=null) 
                    stream.Close();               
                if (response != null) 
                    response.Close();
            }            
            return value;        
        }
        private static bool SaveBinaryFile(WebResponse response, string savePath)        
        {            
            bool value = false;            
            byte[] buffer = new byte[1024];            
            Stream outStream = null;            
            Stream inStream = null;           
            try{                
                if (File.Exists(savePath)) File.Delete(savePath);                
                outStream = System.IO.File.Create(savePath);                
                inStream = response.GetResponseStream();                
                int l;                
                do {       
                    l = inStream.Read(buffer, 0, buffer.Length); 
                    if (l > 0) outStream.Write(buffer, 0, l);                
                } while (l > 0);  
                value = true;            
            }
            finally{
                if(outStream!=null) outStream.Close();               
                if(inStream!=null) inStream.Close();            
            }
            return value;   
        }    

    }
}
