using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Azure.NotificationHubs;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Text;
using System.Globalization;

namespace WAAD.POC.ProductCatalog.Backend.Controllers
{
    public class RegisterDeviceController : ApiController
    {

        // POST api/RegisterDevice
        public void Post([FromBody] string value)
        {
			
			var connectionString = ConfigurationManager.AppSettings["NotificationHubConnectionString"];
			var notificationHubName = ConfigurationManager.AppSettings["NotificationHubName"];
			NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubName);
			// Decode from Base64
			string Base64DecodedChannelUri = Encoding.UTF8.GetString(Convert.FromBase64String(value));
            hub.DeleteRegistrationsByChannelAsync(Base64DecodedChannelUri).Wait();
            var result =  ManualRegistrationHelper(Base64DecodedChannelUri);
        }



        private static String WNS_REGISTRATION = // "<?xml version=\"1.0\" encoding=\"utf-8\"?><entry xmlns=\"http://www.w3.org/2005/Atom\"><content type=\"application/xml\"><WindowsRegistrationDescription xmlns:i=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns=\"http://schemas.microsoft.com/netservices/2010/10/servicebus/connect\">{0}<ChannelUri>{1}</ChannelUri></WindowsRegistrationDescription></content></entry>";
            @"<?xml version=""1.0"" encoding=""utf-8""?>
<entry xmlns=""http://www.w3.org/2005/Atom"">
  <content type=""application/xml"">
        <WindowsRegistrationDescription xmlns:i=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://schemas.microsoft.com/netservices/2010/10/servicebus/connect"">
            <Tags>Car</Tags>
            <PushVariables>{""userfirstname"":""John"",""userlastname"":""Smith""}</PushVariables>
            <ChannelUri>{channeluri}</ChannelUri>
        </WindowsRegistrationDescription>
  </content>
</entry>";

        private static string _apiVersion = "2015-04";

        
        private static string ManualRegistrationHelper(string channelUri)
        {
            var notificationUrl = String.Format("https://{0}/{1}/Registrations/?api-version={2}",ConfigurationManager.AppSettings["NotificationHubConnectionString"].Split(new string[]{"/"}, StringSplitOptions.RemoveEmptyEntries)[1],ConfigurationManager.AppSettings["NotificationHubName"],_apiVersion);
            HttpWebRequest httprequest = (HttpWebRequest)WebRequest.Create(notificationUrl);
            httprequest.Method = "POST";
            httprequest.ContentType = "application/atom+xml;type=entry;charset=utf-8";
            var sharedAccessKey = ConfigurationManager.AppSettings["NotificationHubConnectionString"].Split(new string[] { "SharedAccessKey=" }, StringSplitOptions.RemoveEmptyEntries).Last();
            httprequest.Headers["Authorization"] = createToken(notificationUrl, "DefaultFullSharedAccessSignature",sharedAccessKey);
            httprequest.Headers["x-ms-version"] = _apiVersion;
            string post = WNS_REGISTRATION.Replace("{channeluri}", channelUri);
            byte[] content = System.Text.Encoding.UTF8.GetBytes(post);
            using (var reqStream = httprequest.GetRequestStream())
            {
                reqStream.Write(content, 0, content.Length);
                var res = httprequest.GetResponse();
                return new StreamReader(res.GetResponseStream()).ReadToEnd();
            }
        }


        private static string createToken(string resourceUri, string keyName, string key)
        {
            TimeSpan sinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1);
            var expiry = Convert.ToString((int)sinceEpoch.TotalSeconds + 3600); //EXPIRES in 1h 
            string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
            HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
            var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));

            var sasToken = String.Format(CultureInfo.InvariantCulture,
            "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
                HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);

            return sasToken;
        }
    }
}
