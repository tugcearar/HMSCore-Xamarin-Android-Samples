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
using HMS_FusedLocationProvider.Activities;

namespace HMS_FusedLocationProvider.Helpers.MockLocation
{
    public class MockLocationSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private MockLocationActivity mockLocationActivity;

        public MockLocationSuccessListener(MockLocationActivity mockLocationActivity)
        {
            this.mockLocationActivity = mockLocationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(mockLocationActivity, "Mock location is set successfully.", ToastLength.Long).Show();

        }
    }
    public class MockLocationFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private MockLocationActivity mockLocationActivity;

        public MockLocationFailureListener(MockLocationActivity mockLocationActivity)
        {
            this.mockLocationActivity = mockLocationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(mockLocationActivity, ex.Message.ToString(), ToastLength.Long).Show();
        }
    }
    public class MockModeSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private MockLocationActivity mockLocationActivity;

        public MockModeSuccessListener(MockLocationActivity mockLocationActivity)
        {
            this.mockLocationActivity = mockLocationActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            mockLocationActivity.isMockModeFailure = false;
            Toast.MakeText(mockLocationActivity, "Mock mode is enabled: " +p0, ToastLength.Long).Show();

        }
    }
    public class MockModeFailureListener : Java.Lang.Object, IOnFailureListener
    {
        private MockLocationActivity mockLocationActivity;

        public MockModeFailureListener(MockLocationActivity mockLocationActivity)
        {
            this.mockLocationActivity = mockLocationActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            mockLocationActivity.isMockModeFailure = true;
            if ((ex as ApiException).StatusCode == 10803)
            {
                Toast.MakeText(mockLocationActivity, "You need to set the mock location app in the developer options first.", ToastLength.Long).Show();

            }
            else
                Toast.MakeText(mockLocationActivity, "Mock Mode cannot be enabled. " + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }
}