
using System.Collections.Generic;
using System.Linq;
using HMS_ActivityIdentification.Helpers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Location;

namespace HMS_ActivityIdentification.Activities
{
    [Activity(Label = "ActivityConversionActivity")]
    public class ActivityConversionActivity : Activity
    {
        ActivityIdentificationService identificationService;
        private PendingIntent pendingIntent;
        private static Activity act;
        static ListView lstHistory;
     
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_activityconversion);
            identificationService = ActivityIdentification.GetService(this);
            lstHistory = FindViewById<ListView>(Resource.Id.lstHistory);
            act = this;
            RequestActivityConversion();
        }

        private PendingIntent CreatePendingIntent()
        {
            Intent intent = new Intent(this, typeof(ActivityIdBroadcastReceiver));
            intent.SetAction(ActivityIdBroadcastReceiver.ActionProcessActivity);
            return PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        private void RequestActivityConversion()
        {
            if (pendingIntent != null)
                RemoveActivityConversionRequest();

            pendingIntent = CreatePendingIntent();

            var infoList = new List<ActivityConversionInfo>();
            foreach (var item in Utility.AllActivities)
            {
                infoList.Add(new ActivityConversionInfo(item, 1));
                infoList.Add(new ActivityConversionInfo(item, 0));
            }

            ActivityConversionRequest request = new ActivityConversionRequest(infoList);

            Task activityIdTask = identificationService.CreateActivityConversionUpdates(request, pendingIntent);
            activityIdTask.AddOnSuccessListener(new RequestActivityConversionSuccessListener(this));
            activityIdTask.AddOnFailureListener(new RequestActivityConversionFailListener(this));
        }

        private void RemoveActivityConversionRequest()
        {
            Task removeTask = identificationService.DeleteActivityConversionUpdates(pendingIntent);
            removeTask.AddOnSuccessListener(new DeleteActivityConversionSuccessListener(this));
            removeTask.AddOnFailureListener(new DeleteActivityConversionFailListener(this));
        }

        internal static void SetData(List<ActivityConversionData> activityConversionDatas)
        {
            if (lstHistory.Adapter != null && lstHistory.Adapter.Count > 0)
            {
                var currentItems = ((ActivityConversionListViewAdapter)lstHistory.Adapter).GetAllItems();
                currentItems.AddRange(activityConversionDatas);
                lstHistory.Adapter = new ActivityConversionListViewAdapter(act, currentItems);
            }
            else
                lstHistory.Adapter = new ActivityConversionListViewAdapter(act, activityConversionDatas);
            ((ActivityConversionListViewAdapter)lstHistory.Adapter).NotifyDataSetChanged();
        }

        protected override void OnDestroy()
        {
            if (pendingIntent != null)
                RemoveActivityConversionRequest();
            base.OnDestroy();
        }
    }
}