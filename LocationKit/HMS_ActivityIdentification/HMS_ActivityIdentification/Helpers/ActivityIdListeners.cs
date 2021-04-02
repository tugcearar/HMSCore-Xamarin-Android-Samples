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

namespace HMS_ActivityIdentification.Helpers
{
    public class RequestActivityUpdatesSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private MainActivity mainActivity;

        public RequestActivityUpdatesSuccessListener(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
           Toast.MakeText(mainActivity, "Activity Identification request successfully", ToastLength.Long).Show();
        }
    }

    public class RequestActivityUpdatesFailListener : Java.Lang.Object, IOnFailureListener
    {
        private MainActivity mainActivity;

        public RequestActivityUpdatesFailListener(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(mainActivity, "Activity Identification request failed:" + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class DeleteActivityUpdatesSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
        private MainActivity mainActivity;

        public DeleteActivityUpdatesSuccessListener(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(mainActivity, "Activity Identification request remove successfully", ToastLength.Long).Show();
        }
    }

    public class DeleteActivityUpdatesFailListener : Java.Lang.Object, IOnFailureListener
    {
        private MainActivity mainActivity;

        public DeleteActivityUpdatesFailListener(MainActivity mainActivity)
        {
            this.mainActivity = mainActivity;
        }
        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(mainActivity, "Activity Identification request removing failed: " + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }


}