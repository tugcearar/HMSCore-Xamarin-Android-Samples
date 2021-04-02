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
using Huawei.Hmf.Tasks;
using Huawei.Hms.Location;
using HMS_FusedLocationProvider.Activities;

namespace HMS_FusedLocationProvider.Helpers
{
   
    public class OnCallbackSettingsSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private LocationRequest locationRequest;
        private RequestWithCallbackActivity locationActivity;
        private LocationCallback locationCallback;

        public OnCallbackSettingsSuccessListener(RequestWithCallbackActivity locationActivity, FusedLocationProviderClient fusedLocationProviderClient, LocationRequest locationRequest, LocationCallback callback)
        {
            this.locationActivity = locationActivity;
            this.fusedLocationProviderClient = fusedLocationProviderClient;
            this.locationRequest = locationRequest;
            this.locationCallback = callback;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Settings are checked.", ToastLength.Long).Show();
            fusedLocationProviderClient.RequestLocationUpdates(locationRequest, locationCallback, Looper.MainLooper)
                .AddOnSuccessListener(new RequestLocationCallbackOnSuccessListener(locationActivity))
                .AddOnFailureListener(new RequestLocationCallbackOnFailureListener(locationActivity)); ;
        }
    }

    public class OnCallbackSettingsFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithCallbackActivity locationActivity;

        public OnCallbackSettingsFailureListener(RequestWithCallbackActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class RequestLocationCallbackOnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private RequestWithCallbackActivity locationActivity;

        public RequestLocationCallbackOnSuccessListener(RequestWithCallbackActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Request Successful", ToastLength.Long).Show();
        }
    }

    public class RequestLocationCallbackOnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithCallbackActivity locationActivity;

        public RequestLocationCallbackOnFailureListener(RequestWithCallbackActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class RemoveLocationCallbackOnSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private RequestWithCallbackActivity locationActivity;

        public RemoveLocationCallbackOnSuccessListener(RequestWithCallbackActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(locationActivity, "Location Request Remove Successfully", ToastLength.Long).Show();
        }
    }

    public class RemoveLocationCallbackOnFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private RequestWithCallbackActivity locationActivity;

        public RemoveLocationCallbackOnFailureListener(RequestWithCallbackActivity locationActivity)
        {
            this.locationActivity = locationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(locationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }
}