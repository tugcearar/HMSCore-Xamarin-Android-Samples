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
using Huawei.Hms.Location;
using Java.Lang;
using HMS_FusedLocationProvider.Helpers;
using Huawei.Hmf.Tasks;

namespace HMS_FusedLocationProvider.Activities
{
    [Activity(Label = "RequestWithCallbackActivity")]
    public class RequestWithCallbackActivity : Activity
    {
        private static ListView lstView;
        private static Activity act;
        private FusedLocationProviderClient fusedLocationProviderClient;
        private SettingsClient locationSettings;
        private LocationRequest locationRequest;
        private LocationCallback locationCallback;
        private Button stopButton, startButton;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_location);
            startButton = FindViewById<Button>(Resource.Id.btnStartLocationRequest);
            stopButton = FindViewById<Button>(Resource.Id.btnStopLocationRequest);
            startButton.Click += StartButton_Click;
            stopButton.Click += StopButton_Click;
            act = this;
            lstView = FindViewById<ListView>(Resource.Id.lstLocations);

            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            locationSettings = LocationServices.GetSettingsClient(this);
            locationRequest = new LocationRequest().SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(100000);
            locationRequest.SetNeedAddress(true);
            locationRequest.SetLanguage("en");
            locationRequest.SetCountryCode("EN");
            if (locationCallback == null)
                locationCallback = new CustomLocationCallback(this);
        }

        internal void SetData(IList<HWLocation> locationList)
        {
            if (lstView.Adapter != null && lstView.Adapter.Count > 0)
            {
                var currentItems = ((LocationListViewAdapter)lstView.Adapter).GetAllItems();
                currentItems.AddRange(locationList);
                lstView.Adapter = new LocationListViewAdapter(act, currentItems);
            }
            else
                lstView.Adapter = new LocationListViewAdapter(act, locationList);
            ((LocationListViewAdapter)lstView.Adapter).NotifyDataSetChanged();
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            RemoveLocationUpdatesWithCallback();
            stopButton.Visibility = Android.Views.ViewStates.Gone;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            stopButton.Visibility = Android.Views.ViewStates.Visible;
            RequestLocationUpdatesWithCallback();
        }

        public void RequestLocationUpdatesWithCallback()
        {
            LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
            builder.AddLocationRequest(locationRequest);
            LocationSettingsRequest locationSettingsRequest = builder.Build();

            //checking device settings
            Task locationSettingsTask = locationSettings.CheckLocationSettings(locationSettingsRequest);

            locationSettingsTask.AddOnSuccessListener(new OnCallbackSettingsSuccessListener(this, fusedLocationProviderClient, locationRequest, locationCallback)).
                AddOnFailureListener(new OnCallbackSettingsFailureListener(this));
        }

        public void RemoveLocationUpdatesWithCallback()
        {
            Task removeTask = fusedLocationProviderClient.RemoveLocationUpdates(locationCallback);
            removeTask.AddOnSuccessListener(new RemoveLocationCallbackOnSuccessListener(this))
                .AddOnFailureListener((new RemoveLocationCallbackOnFailureListener(this)));
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveLocationUpdatesWithCallback();
        }


    }
}