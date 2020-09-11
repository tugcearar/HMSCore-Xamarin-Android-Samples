using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Common;
using Com.Huawei.Hms.Location;
using HMS_Geofence.Models;

namespace HMS_Geofence.Helpers
{
    public class CreateGeoFailListener : Java.Lang.Object, IOnFailureListener
    {
        private Activity mainActivity;

        public CreateGeoFailListener(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(mainActivity, "Geofence request failed: " + GeofenceErrorCodes.GetErrorMessage((ex as ApiException).StatusCode), ToastLength.Long).Show();
        }
    }

    public class CreateGeoSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private Activity mainActivity;

        public CreateGeoSuccessListener(Activity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnSuccess(Java.Lang.Object data)
        {
            Toast.MakeText(mainActivity, "Geofence request successful", ToastLength.Long).Show();
        }
    }
}