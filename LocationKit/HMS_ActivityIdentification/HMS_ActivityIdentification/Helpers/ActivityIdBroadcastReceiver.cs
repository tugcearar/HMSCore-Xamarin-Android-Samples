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
using Huawei.Hms.Location;

namespace HMS_ActivityIdentification.Helpers
{
    [BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { "Huawei.hms.activityId.ACTION_PROCESS_ACTIVITY" })]
    public class ActivityIdBroadcastReceiver : BroadcastReceiver
    {
        public static readonly string ActionProcessActivity = "Huawei.hms.location.ACTION_PROCESS_ACTIVITY";
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent != null)
            {
                string action = intent.Action;
                if (ActionProcessActivity == action)
                {
                    if (ActivityIdentificationResponse.ContainDataFromIntent(intent))
                    {
                        ActivityIdentificationResponse activityIdResult = ActivityIdentificationResponse.GetDataFromIntent(intent);
                        if (activityIdResult != null)
                        {
                            MainActivity.SetData(activityIdResult);
                        }
                    }

                    if (ActivityConversionResponse.ContainDataFromIntent(intent))
                    {
                        ActivityConversionResponse activityConversionResponse = ActivityConversionResponse.GetDataFromIntent(intent);
                        if (activityConversionResponse != null)
                        {
                            List<ActivityConversionData> activityConversionDatas = activityConversionResponse.ActivityConversionDatas.ToList();
                            ActivityConversionActivity.SetData(activityConversionDatas);
                        }
                    }
                }
            }
        }
    }
}