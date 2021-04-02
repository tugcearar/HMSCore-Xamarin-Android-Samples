using Huawei.Hms.Maps.Model;
using HMS_Geofence.Helpers;
using System.Collections.Generic;

namespace HMS_Geofence.Models
{
    public class GeofenceModel
    {
        public LatLng LatLng { get; set; }
        public string Id { get; set; }
        public int Radius { get; set; } = 30;
        public int Conversion { get;  set; }
        public long Timeout { get;  set; }
    }
}