using Android.App;
using Android.Content;
using Android.Widget;
using Com.Huawei.Hms.Location;

namespace HMS_Geofence.Helpers
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.huawei.hms.geofence.ACTION_PROCESS_ACTIVITY" })]
    class GeofenceBroadcastReceiver : BroadcastReceiver
    {
        public static readonly string ActionGeofence = "com.huawei.hms.geofence.ACTION_PROCESS_ACTIVITY";
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                var action = intent.Action;
                if (action == ActionGeofence)
                {
                    GeofenceData geofenceData = GeofenceData.GetDataFromIntent(intent);
                    if (geofenceData != null)
                    {
                        Toast.MakeText(context, "Geofence triggered: " + geofenceData.ConvertingLocation.Latitude +"\n" + geofenceData.ConvertingLocation.Longitude + "\n" + geofenceData.Conversion.ToConversionName(), ToastLength.Long).Show();
                    }
                }
            }
        }
    }
}