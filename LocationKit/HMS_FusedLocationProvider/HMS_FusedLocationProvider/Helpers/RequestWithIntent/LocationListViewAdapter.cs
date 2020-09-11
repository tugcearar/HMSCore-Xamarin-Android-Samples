using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hms.Location;

namespace HMS_FusedLocationProvider.Helpers
{
    class LocationListViewAdapter : BaseAdapter<HWLocation>
    {
        IList<HWLocation> items;
        Activity context;
        public LocationListViewAdapter(Activity context, IList<HWLocation> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override HWLocation this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;
            if (view == null) // no view to re-use, create new
                view = context.LayoutInflater.Inflate(Resource.Layout.customview, null);
            view.FindViewById<TextView>(Resource.Id.txtLat2).Text = item.Latitude.ToString();
            view.FindViewById<TextView>(Resource.Id.txtLng2).Text = item.Longitude.ToString();
            view.FindViewById<TextView>(Resource.Id.txtTime).Text = item.Time.UnixTimeStampToDateTime().ToString();
            view.FindViewById<TextView>(Resource.Id.txtAccuracy).Text = item.Accuracy.ToString();
            view.FindViewById<TextView>(Resource.Id.txtProvider).Text = item.Provider.ToString();
            view.FindViewById<TextView>(Resource.Id.txtAltitude).Text = item.Altitude.ToString();
            view.FindViewById<TextView>(Resource.Id.txtAddress).Text = string.Format("{0} {1} {2} {3} {4}", item?.Street, item?.PostalCode, item?.County, item?.State, item.CountryName);
            return view;
        }

        public List<HWLocation> GetAllItems()
        {
            var allItems = new List<HWLocation>();
            for (int i = 0; i < this.Count; i++)
            {
                allItems.Add((HWLocation)this.GetItem(i));
            }
            return allItems;
        }
    }
}