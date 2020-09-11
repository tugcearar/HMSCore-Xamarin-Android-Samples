using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Com.Huawei.Hms.Maps;
using Com.Huawei.Agconnect.Config;
using Android.Support.Design.Widget;
using Android.Views;
using Com.Huawei.Hms.Maps.Model;
using Android.Support.V4.App;
using Android.Widget;
using System;
using System.IO;

namespace HMS_HMapStyle
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnMapReadyCallback, BottomNavigationView.IOnNavigationItemSelectedListener
    {
        private HuaweiMap hMap;
        private MapFragment mapFragment;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            BottomNavigationView navigation = FindViewById<BottomNavigationView>(Resource.Id.navigation);
            navigation.SetOnNavigationItemSelectedListener(this);
            AGConnectServicesConfig config = AGConnectServicesConfig.FromContext(this);
            SetMap();
        }
        private void SetMap()
        {
            mapFragment = (MapFragment)FragmentManager.FindFragmentById(Resource.Id.mapfragment);
            mapFragment.GetMapAsync(this);
        }
        public void OnMapReady(HuaweiMap map)
        {
            hMap = map;
            hMap.UiSettings.CompassEnabled = true;
            hMap.UiSettings.MyLocationButtonEnabled = true;
            hMap.MapType = HuaweiMap.MapTypeNormal;
            hMap.MoveCamera(CameraUpdateFactory.NewLatLngZoom(new LatLng(48.893478, 2.334595), 10));

            StreamReader strm = new StreamReader(Assets.Open("mapstyle_greeen.json"));
            string response = strm.ReadToEnd();
            var isLoad = hMap.SetMapStyle(MapStyleOptions.LoadAssetResouceStyle(response));
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            try
            {
                StreamReader strm = new StreamReader(Assets.Open("mapstyle_light.json"));
                switch (item.ItemId)
                {
                    case Resource.Id.navigation_map_1:
                        strm = new StreamReader(Assets.Open("mapstyle_light.json"));
                        break;
                    case Resource.Id.navigation_map_2:
                        strm = new StreamReader(Assets.Open("mapstyle_night.json"));
                        break;
                    default:
                        strm = new StreamReader(Assets.Open("mapstyle_light.json"));
                        break;
                }
                string response = strm.ReadToEnd();
                var isLoad = hMap.SetMapStyle(MapStyleOptions.LoadAssetResouceStyle(response));
                if (isLoad)
                    return true;
                else
                {
                    Toast.MakeText(this, "Sorry, selected theme not loading right now", ToastLength.Long);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "Sorry, selected theme not loading right now", ToastLength.Long);
                return false;
            }

        }

      
    }
}