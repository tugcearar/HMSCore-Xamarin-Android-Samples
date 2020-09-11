using Com.Huawei.Hms.Location;

namespace HMS_FusedLocationProvider.Activities
{
    public class CustomLocationCallback : LocationCallback
    {
        private RequestWithCallbackActivity requestWithCallbackActivity;

        public CustomLocationCallback(RequestWithCallbackActivity requestWithCallbackActivity)
        {
            this.requestWithCallbackActivity = requestWithCallbackActivity;
        }

        public override void OnLocationAvailability(LocationAvailability p0)
        {
            base.OnLocationAvailability(p0);
        }
        public override void OnLocationResult(LocationResult locationResult)
        {
            base.OnLocationResult(locationResult);
            if (locationResult == null)
            {
                return;
            }
            else
            {
                requestWithCallbackActivity.SetData(locationResult.HWLocationList);
            }
        }
    }
}