using System;
using System.Collections.Generic;
using Android.App;
using Android.Graphics;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hms.Location;
using Java.Sql;

namespace HMS_ActivityIdentification.Helpers
{
    public class ActivityConversionListViewAdapter : BaseAdapter<ActivityConversionData>
    {
        IList<ActivityConversionData> items;
        Activity context;
        public ActivityConversionListViewAdapter(Activity context, IList<ActivityConversionData> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ActivityConversionData this[int position]
        {
            get { return items[position]; }
        }
        public override int Count
        {
            get { return items.Count; }
        }
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            ActivityConversionData item = items[position];
            View view = convertView;
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.conversion_update_view, null);
            view.SetBackgroundColor(item.ConversionType == 0 ? Color.ParseColor("#72C98A") : Color.ParseColor("#EC6A6A"));
            ActivityTypeRowModel itemDetail = item.ActivityType.ToActivityType();
            view.FindViewById<TextView>(Resource.Id.txtActivityName).Text = itemDetail.Name;
            view.FindViewById<TextView>(Resource.Id.txtConversionType).Text = item.ConversionType.ToConversionTypeName();
            view.FindViewById<ImageView>(Resource.Id.imgActivity).SetImageResource(itemDetail.Image);
            return view;
        }

        public List<ActivityConversionData> GetAllItems()
        {
            var allItems = new List<ActivityConversionData>();
            for (int i = 0; i < this.Count; i++)
            {
                allItems.Add((ActivityConversionData)this.GetItem(i));
            }
            return allItems;
        }
    }
}