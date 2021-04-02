using System;
using System.Collections.Generic;

using Huawei.Hms.Location;

namespace HMS_ActivityIdentification.Helpers
{
    public static class Utility
    {

        public static List<int> AllActivities = new List<int>() { ActivityIdentificationData.Vehicle, ActivityIdentificationData.Bike, ActivityIdentificationData.Foot, ActivityIdentificationData.Still,  ActivityIdentificationData.Walking, ActivityIdentificationData.Running };

        
        public static string ToConversionTypeName(this int conversionType)
        {
            return conversionType == 0 ? "Enter" : "Exit";
        }

        //Convert the code for the detected activity type, into the model including corresponding string and icon
        public static ActivityTypeRowModel ToActivityType(this int activityType)
        {
            ActivityTypeRowModel model = new ActivityTypeRowModel();
            switch (activityType)
            {
                case 100:
                    model.Name = "Vehicle";
                    model.Image = Resource.Drawable.vehicle;
                    break;
                case 101:
                    model.Name = "Bike";
                    model.Image = Resource.Drawable.bicycle;
                    break;
                case 102:
                    model.Name = "On Foot";
                    model.Image = Resource.Drawable.foot;
                    break;
                case 103:
                    model.Name = "Still";
                    model.Image = Resource.Drawable.still;
                    break;
                case 104:
                    model.Name = "Others";
                    model.Image = Resource.Drawable.others;
                    break;
                case 105:
                    model.Name = "Tilting";
                    model.Image = Resource.Drawable.vehicle;
                    break;
                case 107:
                    model.Name = "Walking";
                    model.Image = Resource.Drawable.walking;
                    break;
                case 108:
                    model.Name = "Running";
                    model.Image = Resource.Drawable.running;
                    break;
                default:
                    break;
            }
            return model;
        }

        public static DateTime UnixTimeStampToDateTime(this long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
    }
    public class ActivityTypeRowModel
    {
        public string Name { get; internal set; }
        public int Image { get; internal set; }
    }
}