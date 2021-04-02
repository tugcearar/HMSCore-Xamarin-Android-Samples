using System.IO;
using Android.Content;
using Huawei.Agconnect.Config;

namespace HMS_HMapStyle.Utilities
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