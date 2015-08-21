using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using ColdTeaWCF.Source;

namespace ColdTeaWCF
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service2
    {
        // To use HTTP GET, add [WebGet] attribute. (Default ResponseFormat is WebMessageFormat.Json)
        // To create an operation that returns XML,
        //     add [WebGet(ResponseFormat=WebMessageFormat.Xml)],
        //     and include the following line in the operation body:
        //         WebOperationContext.Current.OutgoingResponse.ContentType = "text/xml";
        //[OperationContract]
        //[WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        //public string DoWork(string a)
        //{
        //    // Add your operation implementation here
        //    return a + a;
        //}

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string MorphFaceByURL(string URL1, string URL2, double Parameter)
        {
            FaceMorph faceMorph = new FaceMorph();
            return faceMorph.MorphByURL(URL1, URL2, Parameter);
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public string MorphFaceLocal(string FileName1, string FileName2, double Parameter)
        {
            FaceMorph faceMorph = new FaceMorph();
            return faceMorph.MorphLocal(FileName1, FileName2, Parameter);
        }

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        public double CalculateSimilar(string URL1, string URL2)
        {
            AzureFace azureFace = new AzureFace();
            return azureFace.GetSimilar(URL1, URL2);
        }

        //[OperationContract]
        //[WebInvoke(UriTemplate = "UploadImage", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.WrappedRequest)]
        //public string UploadImage(string base64String)
        //{
        //    return Upload.UploadFile(base64String);
        //}

        // Add more operations here and mark them with [OperationContract]
    }
}
