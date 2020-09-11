using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Agconnect.Config;

namespace HMS_Geofence.Utilities
{
    /// <summary>
    /// To read agconnect-services.json file.
    /// </summary>
    class HmsLazyInputStream : LazyInputStream
    {
        public HmsLazyInputStream(Context context):base(context)
        {

        }
        public override Stream Get(Context context)
        {
            return context.Assets.Open("agconnect-services.json");
        }
    }
}