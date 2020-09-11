using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Location;
using HMS_Geofence.Models;

namespace HMS_Geofence.Helpers
{
    class GeofenceManager
    {
        public MainActivity activity;
        public GeofenceManager(MainActivity activity)
        {
            this.activity = activity;
        }

        private PendingIntent CreatePendingIntent()
        {
            Intent intent = new Intent(activity, typeof(GeofenceBroadcastReceiver));
            intent.SetAction(GeofenceBroadcastReceiver.ActionGeofence);
            return PendingIntent.GetBroadcast(activity, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        public void AddGeofences(GeofenceModel geofenceModel)
        {
            //Set parameters
            geofenceModel.Id = Guid.NewGuid().ToString();
            if (geofenceModel.Conversion == 5) //Expiration value that indicates the geofence should never expire.
                geofenceModel.Timeout = Geofence.GeofenceNeverExpire;
            else
                geofenceModel.Timeout = 10000;

            List<IGeofence> geofenceList = new List<IGeofence>();

            //Geofence Service
            GeofenceService geofenceService = LocationServices.GetGeofenceService(activity);
            PendingIntent pendingIntent = CreatePendingIntent();
            GeofenceBuilder somewhereBuilder = new GeofenceBuilder()
                 .SetUniqueId(geofenceModel.Id)
                 .SetValidContinueTime(geofenceModel.Timeout)
                 .SetRoundArea(geofenceModel.LatLng.Latitude, geofenceModel.LatLng.Longitude, geofenceModel.Radius)
                 .SetDwellDelayTime(10000)
                 .SetConversions(geofenceModel.Conversion); ;

            //Create geofence request
            geofenceList.Add(somewhereBuilder.Build());
            GeofenceRequest geofenceRequest = new GeofenceRequest.Builder()
                .CreateGeofenceList(geofenceList)
                .Build();

            //Register geofence
            //Task geoTask = geofenceService.CreateGeofenceList(geofenceRequest, pendingIntent);
            //geoTask.AddOnSuccessListener(new CreateGeoSuccessListener(activity));
            //geoTask.AddOnFailureListener(new CreateGeoFailListener(activity));
        }
    }
}