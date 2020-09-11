using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HMS_ActivityIdentification.Activities;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;

namespace HMS_ActivityIdentification.Helpers
{
    public class RequestActivityConversionSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
         private ActivityConversionActivity conversionActivity;
        public RequestActivityConversionSuccessListener(ActivityConversionActivity conversionActivity)
        {
            this.conversionActivity = conversionActivity;
        }
        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(conversionActivity, "Activity Conversion request successfully", ToastLength.Long).Show();
        }
    }

    public class RequestActivityConversionFailListener : Java.Lang.Object, IOnFailureListener
    {
         private ActivityConversionActivity conversionActivity;

        public RequestActivityConversionFailListener(ActivityConversionActivity conversionActivity)
        {
            this.conversionActivity = conversionActivity;
        }

        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(conversionActivity, "Activity Conversion request failed:" + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }

    public class DeleteActivityConversionSuccessListener : Java.Lang.Object, IOnSuccessListener
    {
         private ActivityConversionActivity conversionActivity;

        public DeleteActivityConversionSuccessListener(ActivityConversionActivity conversionActivity)
        {
            this.conversionActivity = conversionActivity;
        }

        public void OnSuccess(Java.Lang.Object p0)
        {
            Toast.MakeText(conversionActivity, "Activity Conversion request remove successfully", ToastLength.Long).Show();
        }
    }

    public class DeleteActivityConversionFailListener : Java.Lang.Object, IOnFailureListener
    {
        private ActivityConversionActivity conversionActivity;

        public DeleteActivityConversionFailListener(ActivityConversionActivity conversionActivity)
        {
            this.conversionActivity = conversionActivity;
        }
        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(conversionActivity, "Activity Conversion request removing failed: " + ex.Message.ToString(), ToastLength.Long).Show();
        }
    }
}