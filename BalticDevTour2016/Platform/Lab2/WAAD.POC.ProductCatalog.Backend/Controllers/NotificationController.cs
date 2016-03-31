using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;

namespace WAAD.POC.ProductCatalog.Backend.Controllers
{
    public class NotificationController : ApiController
    {
        // POST api/notification
        public async Task<HttpResponseMessage> Get([FromUri]string message, [FromUri]string tag = "")
        {

            string payloadMessage = @"<toast launch=""1""><visual><binding template=""ToastGeneric"">
                <text id=""1"">{'Dear '+ $(userfirstname) +', Get {message} if you come visit us today ! Best Regards from our team.'}</text>
                </binding></visual>
                <actions>
                    <action activationType=""foreground"" content=""Yes I will !"" arguments=""Yes""/>
                    <action activationType=""background"" content=""No thanks."" arguments=""No""/>
                  </actions>
                </toast>";
            payloadMessage = payloadMessage.Replace("{message}", message);

            var connectionString = ConfigurationManager.AppSettings["NotificationHubConnectionString"];
            var notificationHubName = ConfigurationManager.AppSettings["NotificationHubName"];
            NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(connectionString, notificationHubName);
            var result = await hub.SendWindowsNativeNotificationAsync(payloadMessage, tag);


            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent("Success");
            return response;

        }

    }
}
