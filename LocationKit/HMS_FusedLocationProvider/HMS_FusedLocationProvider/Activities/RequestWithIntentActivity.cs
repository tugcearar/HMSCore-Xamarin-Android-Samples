
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using HMS_FusedLocationProvider.Helpers;

namespace HMS_FusedLocationProvider.Activities
{
    [Activity(Label = "RequestWithIntentActivity")]
    public class RequestWithIntentActivity : Activity
    {
        private static ListView lstView;
        private FusedLocationProviderClient fusedLocationProviderClient;
        private SettingsClient settingsClient;
        private LocationRequest locationRequest;
        private static Activity act;
        private Button stopButton, startButton;
        private PendingIntent pendingIntent;

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
            settingsClient = LocationServices.GetSettingsClient(this);
            locationRequest = new LocationRequest()
                .SetPriority(LocationRequest.PriorityHighAccuracy)
                .SetNumUpdates(10);
            locationRequest.SetFastestInterval(30000);
            locationRequest.SetInterval(30000);
            locationRequest.SetNeedAddress(true);
            locationRequest.SetLanguage("en");
            locationRequest.SetCountryCode("EN");
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = true;
            RemoveLocationUpdatesWithIntent();
            stopButton.Visibility = Android.Views.ViewStates.Gone;
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            startButton.Enabled = false;
            stopButton.Visibility = Android.Views.ViewStates.Visible;
            RequestLocationUpdatesWithIntent();
        }

        private PendingIntent CreatePendingIntent()
        {
            Intent intent = new Intent(this, typeof(LocationBroadcastReceiver));
            intent.SetAction(LocationBroadcastReceiver.ActionProcessLocation);
            return PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }
        private void RequestLocationUpdatesWithIntent()
        {
            LocationSettingsRequest.Builder builder = new LocationSettingsRequest.Builder();
            builder.AddLocationRequest(locationRequest);
            LocationSettingsRequest locationSettingsRequest = builder.Build();

            //checking device settings
            Task locationSettingsTask = settingsClient.CheckLocationSettings(locationSettingsRequest);
            pendingIntent = CreatePendingIntent();
            locationSettingsTask.AddOnSuccessListener(new OnSettingsSuccessListener(this, fusedLocationProviderClient, locationRequest, pendingIntent)).
                AddOnFailureListener(new OnSettingsFailureListener(this));
        }

        //This API is used to remove location updates for the designated PendingIntent object.
        private void RemoveLocationUpdatesWithIntent()
        {
            Task removeTask = fusedLocationProviderClient.RemoveLocationUpdates(pendingIntent);
            removeTask.AddOnSuccessListener(new RemoveLocationIntentOnSuccessListener(this))
                .AddOnFailureListener((new RemoveLocationIntentOnFailureListener(this)));
        }

        internal static void SetData(IList<HWLocation> locations)
        {
            if (lstView.Adapter != null && lstView.Adapter.Count > 0)
            {
                List<HWLocation> currentItems = ((LocationListViewAdapter)lstView.Adapter).GetAllItems();
                currentItems.AddRange(locations);
                lstView.Adapter = new LocationListViewAdapter(act, currentItems);
            }
            else
                lstView.Adapter = new LocationListViewAdapter(act, locations);
            ((LocationListViewAdapter)lstView.Adapter).NotifyDataSetChanged();
        }


        protected override void OnDestroy()
        {
            base.OnDestroy();
            RemoveLocationUpdatesWithIntent();
        }
    }
}