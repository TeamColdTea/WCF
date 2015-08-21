using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using ColdTeaWCF.Source;
using System.ServiceModel.Activation;

namespace ColdTeaWCF
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Service1 : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public string MorphFaceByURL(string URL1, string URL2, double Parameter)
        {
            FaceMorph faceMorph = new FaceMorph();
            return faceMorph.MorphByURL(URL1,URL2,Parameter);  
        }

        public string MorphFaceLocal(string FileName1, string FileName2, double Parameter)
        {
            FaceMorph faceMorph = new FaceMorph();
            return faceMorph.MorphLocal(FileName1, FileName2, Parameter);
        }

        public double CalculateSimilar(string URL1,string URL2)
        {
            AzureFace azureFace = new AzureFace();
            return azureFace.GetSimilar(URL1,URL2);   
        }

        public string UploadImage(string base64String)
        {
            return Upload.UploadFile(base64String);
        }

    }
}
