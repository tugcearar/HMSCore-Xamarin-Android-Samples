using Android.Locations;
using Android.Widget;
using Huawei.Hmf.Tasks;
using Huawei.Hms.Location;
using Huawei.Hms.Maps.Model;
using System;

namespace HMS_Geofence.Helpers
{
    public class LastLocationSuccess : Java.Lang.Object, IOnSuccessListener
    {
        private MainActivity mainActivity;

        public LastLocationSuccess(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnSuccess(Java.Lang.Object location)
        {
            Toast.MakeText(mainActivity, "LastLocation request successful", ToastLength.Long).Show();
            if (location != null)
            {
                MainActivity.CurrentPosition = new LatLng((location as Location).Latitude, (location as Location).Longitude);
                mainActivity.RepositionMapCamera((location as Location).Latitude, (location as Location).Longitude);
            }
        }

    }
    public class LastLocationFail : Java.Lang.Object, IOnFailureListener
    {
        private MainActivity mainActivity;

        public LastLocationFail(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {

            Toast.MakeText(mainActivity, "LastLocation request failed: " + ex.ToString(), ToastLength.Long).Show();
        }
    }


}