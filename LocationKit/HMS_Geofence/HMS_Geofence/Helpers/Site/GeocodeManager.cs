using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Huawei.Hms.Maps.Model;
using Huawei.Hms.Site.Api;
using Huawei.Hms.Site.Api.Model;
using HMS_Geofence.Models;
using HMS_Geofence.Utilities;
using Java.Lang;
using Newtonsoft.Json;

namespace HMS_Geofence.Helpers
{
    class GeocodeManager
    {
        public const string ApiKey = "CgB6e3x9NPSZfsfsG8oVcTDTEpTRfRm5A1rF/cT2+mjcQWCv93U9FICe6Do6XM+9w+7ZtPONmSxgxUpOC8HEUV//";
        private MainActivity activity;
        public GeocodeManager(MainActivity activity)
        {
            this.activity = activity;
        }

        public void NearbySearch(LatLng currentLocation, string searchText)
        {
            ISearchService searchService = SearchServiceFactory.Create(activity, Android.Net.Uri.Encode("CgB6e3x9NPSZfsfsG8oVcTDTEpTRfRm5A1rF/cT2+mjcQWCv93U9FICe6Do6XM+9w+7ZtPONmSxgxUpOC8HEUV//"));
            NearbySearchRequest nearbySearchRequest = new NearbySearchRequest();
            nearbySearchRequest.Query = searchText;
            nearbySearchRequest.Language = "en";
            if (currentLocation != null)
                nearbySearchRequest.Location = new Coordinate(currentLocation.Latitude, currentLocation.Longitude);
            nearbySearchRequest.Radius = (Integer)2000;
            nearbySearchRequest.PageIndex = (Integer)1;
            nearbySearchRequest.PageSize = (Integer)5;
            nearbySearchRequest.PoiType = LocationType.Address;
            searchService.NearbySearch(nearbySearchRequest, new QuerySuggestionResultListener(activity as MainActivity));
        }

        public async Task<ReverseGeocodeResponse> ReverseGeocode(double lat, double lng)
        {
            string result = "";
            using (var client = new HttpClient())
            {
                MyLocation location = new MyLocation();
                location.Lat = lat;
                location.Lng = lng;
                var root = new ReverseGeocodeRequest();
                root.Location = location;

                var settings = new JsonSerializerSettings();
                settings.ContractResolver = new LowercaseSerializer();
                var json = JsonConvert.SerializeObject(root, Formatting.Indented, settings);


                var data = new StringContent(json, Encoding.UTF8, "application/json");
                var url = "https://siteapi.cloud.huawei.com/mapApi/v1/siteService/reverseGeocode?key=" + Android.Net.Uri.Encode(ApiKey);
                var response = await client.PostAsync(url, data);
                result = response.Content.ReadAsStringAsync().Result;
            }

            return JsonConvert.DeserializeObject<ReverseGeocodeResponse>(result);
        }
    }


    public class QuerySuggestionResultListener : Java.Lang.Object, ISearchResultListener
    {
        private MainActivity context;

        public QuerySuggestionResultListener(MainActivity context)
        {
            this.context = context;
        }

        public void OnSearchError(SearchStatus status)
        {
            Toast.MakeText(context, "Error Code: " + status.ErrorCode + " Error Message: " + status.ErrorMessage, ToastLength.Long);

        }

        public void OnSearchResult(Java.Lang.Object results)
        {
            NearbySearchResponse nearbySearchResponse = (NearbySearchResponse)results;
            if (nearbySearchResponse != null && nearbySearchResponse.TotalCount > 0)
                context.SetSearchResultOnMap(nearbySearchResponse.Sites);
        }
    }
}