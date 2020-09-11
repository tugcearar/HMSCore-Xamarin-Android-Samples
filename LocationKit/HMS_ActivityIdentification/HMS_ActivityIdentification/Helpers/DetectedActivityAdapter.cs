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
    public class DetectedActivityAdapter : BaseAdapter<ActivityIdentificationData>
    {
        IList<ActivityIdentificationData> items;
        Activity context;
        public DetectedActivityAdapter(Activity context, IList<ActivityIdentificationData> items)
            : base()
        {
            this.context = context;
            this.items = items;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override ActivityIdentificationData this[int position]
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
            if (view == null)
                view = context.LayoutInflater.Inflate(Resource.Layout.detected_activity_view, null);
            var itemDetail = item.IdentificationActivity.ToActivityType();
            view.FindViewById<TextView>(Resource.Id.txtDetectedActivity).Text = itemDetail.Name + "- Possibility: %" + item.Possibility;
            return view;
        }

        public List<ActivityIdentificationData> GetAllItems()
        {
            var allItems = new List<ActivityIdentificationData>();
            for (int i = 0; i < this.Count; i++)
            {
                allItems.Add((ActivityIdentificationData)this.GetItem(i));
            }
            return allItems;
        }
    }
}