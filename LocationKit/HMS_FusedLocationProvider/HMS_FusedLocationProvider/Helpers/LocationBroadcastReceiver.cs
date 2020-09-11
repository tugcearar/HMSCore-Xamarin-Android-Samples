using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Locations;
using Com.Huawei.Hms.Location;
using HMS_FusedLocationProvider.Activities;

namespace HMS_FusedLocationProvider.Helpers
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "com.huawei.hms.location.ACTION_PROCESS_LOCATION" })]
    public class LocationBroadcastReceiver : BroadcastReceiver
    {
        public static readonly string ActionProcessLocation = "com.huawei.hms.fusedlocation.ACTION_PROCESS_LOCATION";
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                string action = intent.Action;
                if (ActionProcessLocation == action)
                {
                    if (LocationResult.HasResult(intent))
                    {
                        LocationResult locationResult = LocationResult.ExtractResult(intent);
                        if (locationResult != null)
                        {
                            RequestWithIntentActivity.SetData(locationResult.HWLocationList);
                        }
                    }
                }
            }
        }
    }
}