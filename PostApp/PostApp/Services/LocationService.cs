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
        public LocationService()
        {
            locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 75;
        }
        public async Task<Position> GetLocation()
        {
            try
            {
                var position = await locator.GetPositionAsync(10000);
                
                Debug.WriteLine("Position Status: {0}", position.Timestamp);
                Debug.WriteLine("Position Latitude: {0}", position.Latitude);
                Debug.WriteLine("Position Longitude: {0}", position.Longitude);
                return position;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }
    }
}
