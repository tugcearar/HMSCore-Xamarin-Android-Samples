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

namespace HMS_Geofence.Models
{
    public class MyLocation
    {
        public double Lat { get; set; }
        public double Lng { get; set; }
    }
    public class ReverseGeocodeRequest
    {
        public MyLocation Location { get; set; }
    }
}