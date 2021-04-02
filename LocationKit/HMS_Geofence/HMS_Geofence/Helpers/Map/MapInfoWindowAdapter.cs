using Android.App;
using Android.Views;
using Android.Widget;
using Huawei.Hms.Location;
using Huawei.Hms.Maps;
using Huawei.Hms.Maps.Model;
using Huawei.Hms.Site.Api.Model;
using HMS_Geofence.Models;
using System.Linq;

namespace HMS_Geofence.Helpers
{
    internal class MapInfoWindowAdapter : Java.Lang.Object, HuaweiMap.IInfoWindowAdapter
    {
        private MainActivity activity;
        private GeofenceModel selectedCoordinates;
        private View addressLayout;

        public MapInfoWindowAdapter(MainActivity currentActivity)
        {
            activity = currentActivity;
        }
        public View GetInfoContents(Marker marker)
        {
            return null;
        }

        public View GetInfoWindow(Marker marker)
        {
            if (marker == null)
                return null;

            //update everytime, drawcircle need it
            selectedCoordinates = new GeofenceModel { LatLng = new LatLng(marker.Position.Latitude, marker.Position.Longitude) };
            View mapInfoView = activity.LayoutInflater.Inflate(Resource.Layout.map_info_view, null);

            var radiusBar = activity.FindViewById<SeekBar>(Resource.Id.radiusBar);
            if (radiusBar.Visibility == Android.Views.ViewStates.Invisible)
            {
                radiusBar.Visibility = Android.Views.ViewStates.Visible;
                radiusBar.SetProgress(30, true);
            }

            activity.FindViewById<SeekBar>(Resource.Id.radiusBar)?.SetProgress(30, true);
            activity.DrawCircleOnMap(selectedCoordinates);
            Button button = mapInfoView.FindViewById<Button>(Resource.Id.btnInfoWindow);
            button.Click += btnInfoWindow_ClickAsync;

            return mapInfoView;
        }

        private async void btnInfoWindow_ClickAsync(object sender, System.EventArgs e)
        {
             addressLayout = activity.LayoutInflater.Inflate(Resource.Layout.reverse_alert_layout, null);
            //reverse geocode
            GeocodeManager geocodeManager = new GeocodeManager(activity);
            var addressResult = await geocodeManager.ReverseGeocode(selectedCoordinates.LatLng.Latitude, selectedCoordinates.LatLng.Longitude);

            if (addressResult.ReturnCode != 0)
                return;

            var address = addressResult.Sites.FirstOrDefault();

            //set ui components
            var txtAddress = addressLayout.FindViewById<TextView>(Resource.Id.txtAddress);
            var txtRadius = addressLayout.FindViewById<TextView>(Resource.Id.txtRadius);
            txtAddress.Text = address.FormatAddress;
            txtRadius.Text = selectedCoordinates.Radius.ToString();


            //create alert dialog
            AlertDialog.Builder builder = new AlertDialog.Builder(activity);
            builder.SetView(addressLayout);
            builder.SetTitle(address.Name);
            builder.SetPositiveButton("Save", (send, arg) =>
            {
                selectedCoordinates.Conversion = GetSelectedConversion();
                GeofenceManager geofenceManager = new GeofenceManager(activity);
                geofenceManager.AddGeofences(selectedCoordinates);
            });
            builder.SetNegativeButton("Cancel", (send, arg) => { builder.Dispose(); });
            AlertDialog alert = builder.Create();
            alert.Show();
        }

        ///Get selected conversion type from radio button group
        private int GetSelectedConversion()
        {
            var radioButtonId = (addressLayout.FindViewById<RadioGroup>(Resource.Id.radio_group)).CheckedRadioButtonId;
            switch (radioButtonId)
            {
                case Resource.Id.radio_enter:
                    return (int)Geofence.EnterGeofenceConversion;
                case Resource.Id.radio_exit:
                    return (int)Geofence.ExitGeofenceConversion;
                case Resource.Id.radio_dwell:
                    return (int)Geofence.DwellGeofenceConversion;
                case Resource.Id.radio_never_expire:
                    return 5;
                default:
                    return 5;
            }
        }

    }
}