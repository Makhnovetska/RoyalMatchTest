using System.Collections.Generic;
using Configs;
using UnityEngine;

namespace Controllers
{
    public class LocationController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer imgLocation;
        [SerializeField] private string imgLocationName;
        [SerializeField] private List<LocationItemController> locationItems;

        private int _locationId;
        public int itemsCount => locationItems.Count;

        private void Start()
        {
            imgLocation.sprite = Resources.Load<Sprite>($"Images/Locations/{imgLocationName}");
        }
        
        private void OnEnable()
        {
            LocationService.Instance.onItemUnlocked += OnItemUnlocked;
        }

        private void OnDisable()
        {
            LocationService.Instance.onItemUnlocked -= OnItemUnlocked;
        }

        public void Init(Location location)
        {
            _locationId = location.locationId;

            if (location.itemsCount != locationItems.Count)
            {
                Debug.LogError($"Location {location.locationName} has {location.itemsCount} items, but {locationItems.Count} items found in prefab");
                return;
            }

            for (int i = 0; i < location.itemsCount; i++)
            {
                locationItems[i].SetActive(PlayerPrefsService.Instance.IsItemUnlocked(location.locationId, i));
            }
        }

        public void Deinit()
        {
            _locationId = 0;
        }
        
        private void OnItemUnlocked(int locationId, int itemId)
        {
            if (locationId != _locationId)
                return;
            
            locationItems[itemId].SetActive(true);
        }
    }
}