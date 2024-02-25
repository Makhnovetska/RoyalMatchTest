using System.Linq;
using UnityEngine;

public class PlayerPrefsService : MonoBehaviour
{
    public static PlayerPrefsService Instance { get; private set; }
    
    private static string CurrentLocationIdKey = "CurrentLocationId";
    private static string CurrentItemIdKey = "CurrentItemId";

    private void Awake() => Instance = this;

    public void SetCompletedItem(int locationId, int itemId, int itemCount)
    {
        if (itemId >= itemCount)
        {
            Debug.LogError( "Incorrect item id");
            return;
        }

        if (locationId != GetCurrentLocationId())
        {
            Debug.LogError( "Incorrect location id");
            return;
        }
        
        if (GetCurrentItemId() >= itemCount - 1)
            CompleteLocation(locationId);
        else 
            CompleteItem(itemId);
    }
    
    public int GetCurrentLocationId()
    {
        return PlayerPrefs.GetInt(CurrentLocationIdKey, ConfigsService.Instance.LocationListConfig.locations.Min(it => it.locationId));
    }
    
    public int GetCurrentItemId()
    {
        return PlayerPrefs.GetInt(CurrentItemIdKey, 0);
    }
    
    public bool IsAllFurniturePlaced(int locationId, int itemCount)
    {
        if (locationId < GetCurrentLocationId())
            return true;

        return GetCurrentItemId() >= itemCount;
    }
    
    public bool IsLocationUnlocked(int locationId)
    {
        return GetCurrentLocationId() >= locationId;
    }

    public bool IsLocationCompleted(int locationId)
    {
        return GetCurrentLocationId() > locationId;
    }
    
    public bool IsItemUnlocked(int locationId, int itemId)
    {
        return GetCurrentLocationId() > locationId || GetCurrentItemId() > itemId;
    }
    
    private void CompleteItem(int itemId)
    {
        PlayerPrefs.SetInt(CurrentItemIdKey, itemId + 1);
        PlayerPrefs.Save();
    }
    
    private void CompleteLocation(int locationId)
    {
        int nextLocationId = ConfigsService.Instance.LocationListConfig.locations
            .Select( it => it.locationId)
            .Where(it => it > locationId)
            .Min();

        PlayerPrefs.SetInt(CurrentLocationIdKey, nextLocationId);
        PlayerPrefs.SetInt(CurrentItemIdKey, 0);
        PlayerPrefs.Save();
    }
}