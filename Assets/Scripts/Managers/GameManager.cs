using System.Collections.Generic;
using System.Linq;
using Configs;
using UnityEngine;
using Utils.Tools.ScreensManagerTool;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        Location? currentLocation = getCurrentLocation();
        
        if (currentLocation == null)
            ScreensManager.Instance.Open<SelectLocationScreen>();
        else
            ScreensManager.Instance.Open<LocationScreen>(OnLoadingScreen);

        void OnLoadingScreen(LocationScreen locationScreen)
        {
            locationScreen.Init(currentLocation.Value);
        }
    }

    private Location? getCurrentLocation()
    {
        int currentLocationId = PlayerPrefsService.Instance.GetCurrentLocationId();
        List<Location> locations = ConfigsService.Instance.LocationListConfig.locations;
        if (locations.Any(it => it.locationId == currentLocationId))
        {
            Location location = locations.First(it => it.locationId == currentLocationId);
            if (!location.isFake)
                return location;   
        }
        
        //get last not fake location
        foreach (Location location in locations.OrderByDescending(it => it.locationId))
        {
            if (location.locationId > currentLocationId || location.isFake)
                continue;
            
            return location;
        }

        return null;
    }
}