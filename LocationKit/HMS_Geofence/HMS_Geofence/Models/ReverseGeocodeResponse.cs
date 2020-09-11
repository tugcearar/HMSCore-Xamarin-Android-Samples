using System.Collections.Generic;

namespace HMS_Geofence.Models
{
    public class ReverseGeocodeResponse
    {
        public long ReturnCode { get; set; }
        public List<Site> Sites { get; set; }
        public string ReturnDesc { get; set; }
    }

    public class Site
    {
        public string FormatAddress { get; set; }
        public Address Address { get; set; }
        public Viewport Viewport { get; set; }
        public string Name { get; set; }
        public string SiteId { get; set; }
        public Location Location { get; set; }
        public Poi Poi { get; set; }
        public string MatchedLanguage { get; set; }
    }

    public class Address
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string SubLocality { get; set; }
        public string PostalCode { get; set; }
        public string Locality { get; set; }
        public string AdminArea { get; set; }
        public string SubAdminArea { get; set; }
        public string Thoroughfare { get; set; }
    }

    public class Location
    {
        public double Lng { get; set; }
        public double Lat { get; set; }
    }

    public class Poi
    {
        public List<string> HwPoiTypes { get; set; }
        public List<string> PoiTypes { get; set; }
        public long Rating { get; set; }
        public string InternationalPhone { get; set; }
        public OpeningHours OpeningHours { get; set; }
        public List<string> HwPoiTranslatedTypes { get; set; }
    }

    public class OpeningHours
    {
    }

    public class Viewport
    {
        public Location Southwest { get; set; }
        public Location Northeast { get; set; }
    }
}