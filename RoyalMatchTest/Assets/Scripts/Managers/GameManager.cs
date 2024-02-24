using UnityEngine;
using Utils.Tools.ScreensManagerTool;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        ScreensManager.Instance.Open<LocationScreen>();

        void OnLoadingScreen(LocationScreen locationScreen)
        {
            Location currentLocation = getCurrentLocation();
            locationScreen.Init(currentLocation);
        }
    }

    private Location getCurrentLocation()
    {
        int currentLocationId = PlayerPrefsService.Instance.GetCurrentLocationId();
        return ConfigsService.Instance.LocationListConfig.locations[currentLocationId];
    }
}