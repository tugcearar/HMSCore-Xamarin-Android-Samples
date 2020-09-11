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
using Com.Huawei.Hms.Common;
using Com.Huawei.Hms.Location;
using Com.Huawei.Hms.Support.Api.Entity.Location.Mock;
using HMS_FusedLocationProvider.Helpers.MockLocation;

namespace HMS_FusedLocationProvider.Activities
{
    [Activity(Label = "MockLocationActivity")]
    public class MockLocationActivity : Activity
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private EditText latEntry, lngEntry;
        private Switch mockModeSwitch;
        public bool isMockModeFailure;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_mockMode);
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            FindViewById<Button>(Resource.Id.btnSetMockLocation).Click += btnSetMockLocation_Click;
            latEntry = FindViewById<EditText>(Resource.Id.latEntry);
            lngEntry = FindViewById<EditText>(Resource.Id.lngEntry);
            mockModeSwitch = FindViewById<Switch>(Resource.Id.mockModeSwitch);
            mockModeSwitch.CheckedChange += mockMode_CheckedChange;
        }

        private void mockMode_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
           
            var isSelected = e.IsChecked;
            fusedLocationProviderClient.SetMockMode(isSelected)
            .AddOnSuccessListener(new MockModeSuccessListener(this))
            .AddOnFailureListener(new MockModeFailureListener(this));
            if (isMockModeFailure)
            {
                mockModeSwitch.Checked = false;
                return;
            }
        }


        private void btnSetMockLocation_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(latEntry.Text) && !string.IsNullOrEmpty(lngEntry.Text))
                SetMockLocation();
            else
                Toast.MakeText(this, "Please enter both of the latitude and longitude values.", ToastLength.Long).Show();

        }

        private void SetMockLocation()
        {
            var mockLocation = new Location(LocationManager.GpsProvider);
            mockLocation.Longitude = Convert.ToDouble(latEntry.Text);
            mockLocation.Latitude = Convert.ToDouble(lngEntry.Text);
            var mockTask = fusedLocationProviderClient.SetMockLocation(mockLocation);
            mockTask.AddOnSuccessListener(new MockLocationSuccessListener(this));
            mockTask.AddOnFailureListener(new MockLocationFailureListener(this));
        }
    }
}