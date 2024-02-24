using UnityEngine;

public class PlayerPrefsService : MonoBehaviour
{
    public static PlayerPrefsService Instance { get; private set; }

    private void Awake() => Instance = this;

    public void SetNewItem(int locationId, int itemId, int lastItemId)
    {
        if (itemId > lastItemId)
        {
            Debug.LogError( "Incorrect item id");
            return;
        }
        
        if (itemId == lastItemId)
            CompleteLocation(locationId);
        else 
            CompleteItem(itemId);
    }
    
    public bool IsLocationUnlocked(int locationId)
    {
        return GetCurrentLocationId() >= locationId;
    }

    public bool IsLocationCompleted(int locationId)
    {
        return GetCurrentLocationId() > locationId;
    }
    
    public int GetCurrentLocationId()
    {
        return PlayerPrefs.GetInt("CurrentLocationId", 0);
    }
    
    public int GetLastCompletedItemId()
    {
        return PlayerPrefs.GetInt("LastCompletedItemId");
    }
    
    private void CompleteItem(int itemId)
    {
        PlayerPrefs.SetInt("LastCompletedItemId", itemId);
        PlayerPrefs.Save();
    }
    
    private void CompleteLocation(int locationId)
    {
        PlayerPrefs.SetInt("CurrentLocationId", locationId);
        PlayerPrefs.SetInt("LastCompletedItemId", 0);
        PlayerPrefs.Save();
    }
}