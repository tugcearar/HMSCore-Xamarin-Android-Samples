using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Location;
using HMS_FusedLocationProvider.Activities;

namespace HMS_FusedLocationProvider.Helpers
{
    public class LastLocationSuccess : Java.Lang.Object, IOnSuccessListener
    {
        private MainActivity mainActivity;

        public LastLocationSuccess(MainActivity locationActivity)
        {
            this.mainActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object obj)
        {
            var lastLocation = (Location)obj;
            if (lastLocation != null)
            {
                var txtLat = mainActivity.FindViewById<TextView>(Resource.Id.txtLat);
                var txtLong = mainActivity.FindViewById<TextView>(Resource.Id.txtLng);
                txtLat.Text = lastLocation.Latitude.ToString();
                txtLong.Text = lastLocation.Longitude.ToString();
            }
        }
    }

    public class LastLocationFailure : Java.Lang.Object, IOnFailureListener
    {
        private MainActivity mainActivity;

        public LastLocationFailure(MainActivity locationActivity)
        {
            this.mainActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(mainActivity, "Getting last location failed:" + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class LastLocationWithAddressSuccess : Java.Lang.Object, IOnSuccessListener
    {
        private RequestWithIntentActivity requestWithIntentActivity;

        public LastLocationWithAddressSuccess(RequestWithIntentActivity locationActivity)
        {
            this.requestWithIntentActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object obj)
        {
            //var address = obj as HWLocation;
            //requestWithIntentActivity.FindViewById<TextView>(Resource.Id.txtAddress).Text = address.Street + address.PostalCode + address.City +address.CountryName;
        }
    }

    public class LastLocationWithAddressFailure : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithIntentActivity requestWithIntentActivity;

        public LastLocationWithAddressFailure(RequestWithIntentActivity locationActivity)
        {
            this.requestWithIntentActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(requestWithIntentActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class OnSettingsSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private LocationRequest locationRequest;
        private RequestWithIntentActivity locationActivity;
        private PendingIntent pendingIntent;

        public OnSettingsSuccessListener(RequestWithIntentActivity locationActivity, FusedLocationProviderClient fusedLocationProviderClient, LocationRequest locationRequest, PendingIntent pendingIntent)
        {
            this.locationActivity = locationActivity;
            this.fusedLocationProviderClient = fusedLocationProviderClient;
            this.locationRequest = locationRequest;
            this.pendingIntent = pendingIntent;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Settings are checked.", ToastLength.Long).Show();
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest, pendingIntent)
                .AddOnSuccessListener(new RequestLocationIntentOnSuccessListener(locationActivity))
                .AddOnFailureListener(new RequestLocationIntentOnFailureListener(locationActivity)); ;
        }
    }

    public class OnSettingsFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithIntentActivity locationActivity;

        public OnSettingsFailureListener(RequestWithIntentActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class RequestLocationIntentOnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private RequestWithIntentActivity locationActivity;

        public RequestLocationIntentOnSuccessListener(RequestWithIntentActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Request Successful", ToastLength.Long).Show();
        }
    }

    public class RequestLocationIntentOnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithIntentActivity locationActivity;

        public RequestLocationIntentOnFailureListener(RequestWithIntentActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class RemoveLocationIntentOnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private RequestWithIntentActivity locationActivity;

        public RemoveLocationIntentOnSuccessListener(RequestWithIntentActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Location Request Remove Successfully", ToastLength.Long).Show();
        }
    }

    public class RemoveLocationIntentOnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithIntentActivity locationActivity;

        public RemoveLocationIntentOnFailureListener(RequestWithIntentActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

}