using System;
using Controllers;
using Configs;
using UnityEngine;

public class LocationService : MonoBehaviour
{
    public static LocationService Instance { get; private set; }
    private void Awake() => Instance = this;

    private LocationController _loadedLocation;
    private Location _currentLocation;

    public event Action<int, int> onItemUnlocked;

    public void LoadLocation(Location location)
    {
        _currentLocation = location;
        _loadedLocation = Instantiate(_currentLocation.locationPrefab);
        _loadedLocation.Init(_currentLocation);
    }
    
    public void UnloadLocation()
    {
        _loadedLocation.Deinit();
        if (_loadedLocation != null)
            Destroy(_loadedLocation.gameObject);
    }
    
    public void UnlockItem(int locationId, int itemId)
    {
        if (_currentLocation.locationId != locationId)
            return;
        
        PlayerPrefsService.Instance.SetCompletedItem(locationId, itemId, _currentLocation.itemsCount);
        onItemUnlocked?.Invoke(locationId, itemId);
    }
}