using HMS_ActivityIdentification.Activities;
using HMS_ActivityIdentification.Helpers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hmf.Tasks;
using Com.Huawei.Hms.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using static HMS_ActivityIdentification.Helpers.Utility;

namespace HMS_ActivityIdentification
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        ActivityIdentificationService activityIdentificationService;
        private PendingIntent pendingIntent;
        private static TextView txtActivityType;
        private static ImageView imgActivityType;
        static ListView lstDetectedActivities;
        private static Activity act;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            activityIdentificationService = ActivityIdentification.GetService(this);
            txtActivityType = FindViewById<TextView>(Resource.Id.txtActivityType);
            imgActivityType = FindViewById<ImageView>(Resource.Id.imgActivityType);
            lstDetectedActivities = FindViewById<ListView>(Resource.Id.lstDetectedActivities);
            act = this;
            FindViewById<Button>(Resource.Id.btnConversion).Click += btnConversion_Click;
            RequestPermissions();
        }

        private void btnConversion_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent();
            intent.SetClass(this, typeof(ActivityConversionActivity));
            intent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(intent);
        }

        private PendingIntent CreatePendingIntent()
        {
            Intent intent = new Intent(this, typeof(ActivityIdBroadcastReceiver));
            intent.SetAction(ActivityIdBroadcastReceiver.ActionProcessActivity);
            return PendingIntent.GetBroadcast(this, 0, intent, PendingIntentFlags.UpdateCurrent);
        }

        private void RequestActivityUpdate()
        {
            if (pendingIntent != null)
                RemoveActivityUpdateRequest();

            pendingIntent = CreatePendingIntent();
            Task activityIdTask = activityIdentificationService.CreateActivityIdentificationUpdates(10000, pendingIntent);
            activityIdTask.AddOnSuccessListener(new RequestActivityUpdatesSuccessListener(this));
            activityIdTask.AddOnFailureListener(new RequestActivityUpdatesFailListener(this));
        }
        protected override void OnDestroy()
        {
            if (pendingIntent != null)
                RemoveActivityUpdateRequest();
            base.OnDestroy();
        }
        private void RemoveActivityUpdateRequest()
        {
            Task removeTask = activityIdentificationService.DeleteActivityIdentificationUpdates(pendingIntent);
            removeTask.AddOnSuccessListener(new DeleteActivityUpdatesSuccessListener(this));
            removeTask.AddOnFailureListener(new DeleteActivityUpdatesFailListener(this));
        }

        internal static void SetData(ActivityIdentificationResponse activityIdentificationResponse)
        {
            //Most Probable
            ActivityTypeRowModel itemDetail = activityIdentificationResponse.MostActivityIdentification.IdentificationActivity.ToActivityType();
            imgActivityType.SetImageResource(itemDetail.Image);
            txtActivityType.Text = itemDetail.Name.ToUpper() + " \n " + "Time: " + activityIdentificationResponse.Time.UnixTimeStampToDateTime() + " \n " + "Possibility: " + "%" + activityIdentificationResponse.MostActivityIdentification.Possibility;


            //Detected Activities
            List<ActivityIdentificationData> activityIdDatas = activityIdentificationResponse.ActivityIdentificationDatas.ToList();

            if (lstDetectedActivities.Adapter != null && lstDetectedActivities.Adapter.Count > 0)
            {
                var currentItems = ((DetectedActivityAdapter)lstDetectedActivities.Adapter).GetAllItems();
                currentItems.AddRange(activityIdDatas);
                lstDetectedActivities.Adapter = new DetectedActivityAdapter(act, currentItems);
            }
            else
                lstDetectedActivities.Adapter = new DetectedActivityAdapter(act, activityIdDatas);
            ((DetectedActivityAdapter)lstDetectedActivities.Adapter).NotifyDataSetChanged();
        }

        #region Permissions
        private void RequestPermissions()
        {
            if (Build.VERSION.SdkInt <= BuildVersionCodes.P)
            {
                if (ContextCompat.CheckSelfPermission(this, "com.huawei.hms.permission.ACTIVITY_RECOGNITION") != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this,
                        new String[]
                        {
                       "com.huawei.hms.permission.ACTIVITY_RECOGNITION"},
                        1);
                }
                else
                    RequestActivityUpdate();
            }
            else
            {
                if (ContextCompat.CheckSelfPermission(this, "android.permission.ACTIVITY_RECOGNITION") != (int)Permission.Granted)
                {
                    ActivityCompat.RequestPermissions(this,
                        new String[]
                        {
                       "android.permission.ACTIVITY_RECOGNITION"},
                        1);
                }
                else
                    RequestActivityUpdate();
            }
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == 1)
            {
                if ((grantResults.Length == 1) && (grantResults[0] == Permission.Granted))
                {
                    RequestActivityUpdate();
                }
                else
                {
                    //Activity Identification permission was NOT granted
                    //DENY
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[0]))
                        Snackbar.Make(FindViewById<LinearLayout>(Resource.Id.main_view), "You need to grant permission this app to access activity identification.", Snackbar.LengthLong).SetAction("Ask again", v => RequestPermissions()).Show();
                    else //DENY & DON'T ASK AGAIN
                        Toast.MakeText(this, "You need to grant permissions in settings.", ToastLength.Long).Show();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }



        #endregion
    }
}

