using Plugin.ExternalMaps;
using Plugin.ExternalMaps.Abstractions;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PostApp.Services
{
    public class LocationService
    {
        private IGeolocator locator;
        private IExternalMaps maps;
        public LocationService()
        {
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 75;

            maps = CrossExternalMaps.Current;
        }
        public bool IsLocationEnabled
        {
            get { return locator.IsGeolocationAvailable && locator.IsGeolocationEnabled; }
        }
        public bool IsLocationAvailable
        {
            get { return locator.IsGeolocationAvailable; }
        }
        public async Task<Position> GetLocation()
        {
            try
            {
                var position = await locator.GetPositionAsync(10000);
                return position;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<bool> NavigateTo(double latitude, double longitude, string name = "PostApp")
        {
            var res = await maps.NavigateTo(name, latitude, longitude);
            return res;
        }
    }
}
