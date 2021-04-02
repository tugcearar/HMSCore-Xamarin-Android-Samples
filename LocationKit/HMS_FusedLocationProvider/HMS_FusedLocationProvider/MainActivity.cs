using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Android.Content;
using HMS_FusedLocationProvider.Activities;
using System.Threading.Tasks;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using System;
using Huawei.Hms.Location;
using HMS_FusedLocationProvider.Helpers;
using Huawei.Hmf.Tasks;
using Android.Support.Design.Widget;
using Task = Huawei.Hmf.Tasks.Task;

namespace HMS_FusedLocationProvider
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnSuccessListener, IOnFailureListener
    {
        private FusedLocationProviderClient fusedLocationProviderClient;
        private bool isLocationAvailable;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            var btnIntent = (Button)FindViewById(Resource.Id.btnIntent);
            var btnCallback = (Button)FindViewById(Resource.Id.btnCallback);
            var btnMock = (Button)FindViewById(Resource.Id.btnMock);
            btnIntent.Click += btnIntent_Click;
            btnCallback.Click += btnCallback_Click;
            btnMock.Click += btnMock_Click;
            RequestPermissions();
        }

        private void btnMock_Click(object sender, EventArgs e)
        {
            Intent mockAct = new Intent();
            mockAct.SetClass(this, typeof(MockLocationActivity));
            mockAct.SetFlags(ActivityFlags.ClearTop);
            StartActivity(mockAct);
        }

        private void btnCallback_Click(object sender, EventArgs e)
        {
            Intent locCallback = new Intent();
            locCallback.SetClass(this, typeof(RequestWithCallbackActivity));
            locCallback.SetFlags(ActivityFlags.ClearTop);
            StartActivity(locCallback);
        }

        private void btnIntent_Click(object sender, System.EventArgs e)
        {
            Intent locIntent = new Intent();
            locIntent.SetClass(this, typeof(RequestWithIntentActivity));
            locIntent.SetFlags(ActivityFlags.ClearTop);
            StartActivity(locIntent);
        }
        private void GetLastLocation()
        {
            Task lastLocation = fusedLocationProviderClient.LastLocation;
            lastLocation.AddOnSuccessListener(new LastLocationSuccess(this)).AddOnFailureListener(new LastLocationFailure(this));
        }
        private void RequestPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted ||
             ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this,
                    new String[]
                    {
                        Manifest.Permission.AccessCoarseLocation,
                        Manifest.Permission.AccessFineLocation},
                    1);
            }
            else
                GetLastLocation();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == 1)
            {
                // Check if the only required permission has been granted
                if ((grantResults.Length == 2) && (grantResults[0] == Permission.Granted) && (grantResults[1] == Permission.Granted))
                {
                    // Location permission has been granted, okay to retrieve the location of the device
                    GetLastLocation();
                }
                else
                {
                    //Location permission was NOT granted
                    //DENY
                    if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[0]) || ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[1]))
                        Snackbar.Make(FindViewById<LinearLayout>(Resource.Id.mainLayout), "You need to grant permission to use location services.", Snackbar.LengthLong).SetAction("Ask again", v => RequestPermissions()).Show();
                    else //DENY & DON'T ASK AGAIN
                        Toast.MakeText(this, "You need to grant location permissions in settings.", ToastLength.Long).Show();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }


        public void OnFailure(Java.Lang.Exception ex)
        {
            Toast.MakeText(this, ex.Message.ToString(), ToastLength.Long).Show();
        }

        public void OnSuccess(Java.Lang.Object obj)
        {
            var result = (LocationAvailability)obj;
            isLocationAvailable = result.IsLocationAvailable;
            if (isLocationAvailable)
            {
                GetLastLocation();
            }
        }
        private void IsLocationAvailable()
        {
            Huawei.Hmf.Tasks.Task locAvailability = fusedLocationProviderClient.LocationAvailability;
            locAvailability.AddOnSuccessListener(this)
                .AddOnFailureListener(this);
        }

    }
}