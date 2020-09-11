using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Huawei.Hms.Location;

namespace HMS_Geofence.Helpers
{
    static class MiscExtensions
    {
        public static string ToConversionName(this int conversionType)
        {
            switch (conversionType)
            {
                case Geofence.EnterGeofenceConversion:
                    return "Enter";
                case Geofence.ExitGeofenceConversion:
                    return "Exit";
                case Geofence.DwellGeofenceConversion:
                    return "Currently in there";
                case 5:
                    return "Timeless";
                default:
                    return "";
            }
        }
    }
}