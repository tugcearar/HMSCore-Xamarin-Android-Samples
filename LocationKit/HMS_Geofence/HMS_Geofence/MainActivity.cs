using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using System;
using Android.Support.V4.Content;
using Android;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.Design.Widget;
using HMS_Geofence.Helpers;
using Android.Graphics;
using HMS_Geofence.Models;
using System.Collections.Generic;
using AlertDialog = Android.Support.V7.App.AlertDialog;
using Android.Views;
using Huawei.Hms.Maps;
using Huawei.Hms.Maps.Model;
using Huawei.Agconnect.Config;
using Huawei.Hms.Location;
using Huawei.Hmf.Tasks;
using Google.Android.Material.Snackbar;

namespace HMS_Geofence
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback
    {
        MapFragment mapFragment;
        HuaweiMap hMap;
        Marker marker;
        Circle circle;
        SeekBar radiusBar;

        FusedLocationProviderClient fusedLocationProviderClient;
        GeofenceModel selectedCoordinates;
        List<Marker> searchMarkers;
        private View search_view;
        private AlertDialog alert;

        public static LatLng CurrentPosition { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            AGConnectServicesConfig config = AGConnectServicesConfig.FromContext(ApplicationContext);
            fusedLocationProviderClient = LocationServices.GetFusedLocationProviderClient(this);
            mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapfragment);
            mapFragment.GetMapAsync(this);


            FindViewById<Button>(Resource.Id.btnGeoWithAddress).Click += btnGeoWithAddress_Click;
            FindViewById<Button>(Resource.Id.btnClearMap).Click += btnClearMap_Click;
            radiusBar = FindViewById<SeekBar>(Resource.Id.radiusBar);
            radiusBar.ProgressChanged += radiusBar_ProgressChanged; ;
            RequestPermissions();
        }

        #region Events


        private void btnClearMap_Click(object sender, EventArgs e)
        {
            hMap.Clear();
            circle?.Remove();
            marker?.Remove();
            circle = null;
            marker = null;
            radiusBar.Visibility = Android.Views.ViewStates.Invisible;
        }

        private void radiusBar_ProgressChanged(object sender, SeekBar.ProgressChangedEventArgs e)
        {
            selectedCoordinates.Radius = e.Progress;
            DrawCircleOnMap(selectedCoordinates);
        }

        private void btnGeoWithAddress_Click(object sender, EventArgs e)
        {
            search_view = base.LayoutInflater.Inflate(Resource.Layout.search_alert_layout, null);
            AlertDialog.Builder builder = new AlertDialog.Builder(this);
            builder.SetView(search_view);
            builder.SetTitle("Search Location");
            builder.SetNegativeButton("Cancel", (send, arg) => { builder.Dispose(); });
            search_view.FindViewById<Button>(Resource.Id.btnSearch).Click += btnSearchClicked;
            alert = builder.Create();
            alert.Show();
        }

        private void btnSearchClicked(object sender, EventArgs e)
        {
            string searchText = search_view.FindViewById<TextView>(Resource.Id.txtSearch).Text;
            GeocodeManager geocodeManager = new GeocodeManager(this);
            geocodeManager.NearbySearch(CurrentPosition, searchText);
        }
        #endregion

        #region Permissions
        private void RequestPermissions()
        {
            if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) != (int)Permission.Granted ||
             ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) != (int)Permission.Granted ||
             ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) != (int)Permission.Granted ||
             ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) != (int)Permission.Granted ||
             ContextCompat.CheckSelfPermission(this, Manifest.Permission.Internet) != (int)Permission.Granted)
            {
                ActivityCompat.RequestPermissions(this,
                    new System.String[]
                    {
                        Manifest.Permission.AccessCoarseLocation,
                        Manifest.Permission.AccessFineLocation,
                        Manifest.Permission.WriteExternalStorage,
                        Manifest.Permission.ReadExternalStorage,
                        Manifest.Permission.Internet
                    },
                    100);
            }
            else
                GetCurrentPosition();

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            if (requestCode == 100)
            {
                foreach (var item in permissions)
                {
                    if (ContextCompat.CheckSelfPermission(this, item) == Permission.Denied)
                    {
                        if (ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[0]) || ActivityCompat.ShouldShowRequestPermissionRationale(this, permissions[1]))
                            Snackbar.Make(FindViewById<RelativeLayout>(Resource.Id.mainLayout), "You need to grant permission to use location services.", Snackbar.LengthLong).SetAction("Ask again", v => RequestPermissions()).Show();
                        else
                            Toast.MakeText(this, "You need to grant location permissions in settings.", ToastLength.Long).Show();
                    }
                    else
                        GetCurrentPosition();
                }
            }
            else
            {
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
        }
        #endregion

        private void GetCurrentPosition()
        {
            Task locationTask = fusedLocationProviderClient.LastLocation;
            locationTask.AddOnSuccessListener(new LastLocationSuccess(this));
            locationTask.AddOnFailureListener(new LastLocationFail(this));
        }
        public void OnMapReady(HuaweiMap map)
        {
            hMap = map;
            hMap.MapType = HuaweiMap.MapTypeNormal; 
            hMap.UiSettings.MyLocationButtonEnabled = true;
            hMap.UiSettings.CompassEnabled = true;
            hMap.UiSettings.ZoomControlsEnabled = true;
            hMap.UiSettings.ZoomGesturesEnabled = true;
            hMap.MyLocationEnabled = true;
            hMap.MapClick += HMap_MapClick;

            if (selectedCoordinates == null)
                selectedCoordinates = new GeofenceModel { LatLng = CurrentPosition, Radius = 30 };
        }
        private void HMap_MapClick(object sender, HuaweiMap.MapClickEventArgs e)
        {
            selectedCoordinates.LatLng = e.P0;

            if (circle != null)
            {
                circle.Remove();
                circle = null;
            }

            AddMarkerOnMap();

            //DrawCircleOnMap(selectedCoordinates);
        }
        internal void SetSearchResultOnMap(IList<Huawei.Hms.Site.Api.Model.Site> sites)
        {
            hMap.Clear();
            if (searchMarkers != null && searchMarkers.Count > 0)
                foreach (var item in searchMarkers)
                    item.Remove();
            searchMarkers = new List<Marker>();
            //foreach cause an issue, not added markers properly on map
            for (int i = 0; i < sites.Count; i++)
            {
                MarkerOptions marker1Options = new MarkerOptions()
                        .InvokePosition(new LatLng(sites[i].Location.Lat, sites[i].Location.Lng))
                        .InvokeTitle(sites[i].Name).Clusterable(true);
                hMap.SetInfoWindowAdapter(new MapInfoWindowAdapter(this));
                var marker1 = hMap.AddMarker(marker1Options);
                searchMarkers.Add(marker1);
                RepositionMapCamera(sites[i].Location.Lat, sites[i].Location.Lng);
            }
            hMap.SetMarkersClustering(true);
            alert.Dismiss();
        }
        void AddMarkerOnMap()
        {
            if (marker != null) marker.Remove();
            var markerOption = new MarkerOptions()
                .InvokeTitle("Şu an buradasınız")
                .InvokePosition(selectedCoordinates.LatLng);

            hMap.SetInfoWindowAdapter(new MapInfoWindowAdapter(this));
            marker = hMap.AddMarker(markerOption);
            bool isInfoWindowShown = marker.IsInfoWindowShown;
            if (isInfoWindowShown)
                marker.HideInfoWindow();
            else
                marker.ShowInfoWindow();
        }
        public void RepositionMapCamera(double lat, double lng)
        {
            var cameraPosition = new CameraPosition.Builder();
            cameraPosition.Target(new LatLng(lat, lng));
            cameraPosition.Zoom(1000);
            cameraPosition.Bearing(45);
            cameraPosition.Tilt(20);
            CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition.Build());
            hMap.MoveCamera(cameraUpdate);
        }
        public void DrawCircleOnMap(GeofenceModel geoModel)
        {
            this.selectedCoordinates = geoModel;
            if (circle != null)
            {
                circle.Remove();
                circle = null;
            }
            CircleOptions circleOptions = new CircleOptions()
          .InvokeCenter(geoModel.LatLng)
          .InvokeRadius(geoModel.Radius)
          .InvokeFillColor(Color.Argb(50, 0, 14, 84))
          .InvokeStrokeColor(Color.Yellow)
          .InvokeStrokeWidth(15);
            circle = hMap.AddCircle(circleOptions);
        }
    }
}