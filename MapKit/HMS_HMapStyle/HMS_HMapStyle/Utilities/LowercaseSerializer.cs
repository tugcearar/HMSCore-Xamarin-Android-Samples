using Newtonsoft.Json.Serialization;

namespace HMS_HMapStyle.Utilities
{
    class LowercaseSerializer : DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}