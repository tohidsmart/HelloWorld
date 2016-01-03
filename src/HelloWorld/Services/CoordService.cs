using HelloWorld.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace HelloWorld.Services
{
    public class CoordService
    {

        public async Task<CoordinateServiceResult> Lookup(string locationName)
        {
            var result = new CoordinateServiceResult();
            //{
            //    Success = false,
            //    Message = "Failed to look up the Location"
            //};

            var bingKey = Startup.configuration["AppSettings:MapKey"];
            var encodedLocation = WebUtility.UrlEncode(locationName);
            var Url = $"http://dev.virtualearth.net/REST/v1/Locations?query={encodedLocation}&key={bingKey}";

            var httpClinet = new HttpClient();
            string webServiceMessage;
            JToken resources;
            string Json = await httpClinet.GetStringAsync(Url);

            if (EnsureCoordinateReceived(Json, out webServiceMessage, out resources, encodedLocation))
                SetCoordinateValues(resources, result);
            else
                result.Message = webServiceMessage;

            return result;
        }


        private bool EnsureCoordinateReceived(string Json, out string Message,out JToken resources, string encodedLocation)
        {
            var result = JObject.Parse(Json);
            Message = string.Empty;
            resources
                = result["resourceSets"][0]["resources"];
            if (!resources.HasValues)
            {
                Message = $"Could not find '{encodedLocation}' as a location";
                return false;
            }
            else if ((string)resources[0]["confidence"] != "High")
            {
                Message = $"Could not find a confident match for '{encodedLocation}' as a location";
                return false;
            }
            return true;
        }


        private void SetCoordinateValues(JToken resources, CoordinateServiceResult result)
        {
            var coords = resources[0]["geocodePoints"][0]["coordinates"];
            result.Latitude = (double)coords[0];
            result.Longitude = (double)coords[1];
            result.Success = true;
            result.Message = "Success";
        }

    }
}
