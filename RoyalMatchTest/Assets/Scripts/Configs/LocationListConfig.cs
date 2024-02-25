using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "LocationListConfig", menuName = "ScriptableObject/LocationListConfig", order = 0)]
    public class LocationListConfig : ScriptableObject
    {
        public List<Location> locations = new List<Location>();
    }

    [System.Serializable]
    public struct Location
    {
        public int locationId;
        public string locationName;
        public string imgUILocationName;
        public bool isFake;
        public LocationController locationPrefab;

        public int itemsCount => locationPrefab != null ? locationPrefab.itemsCount : 0;
    }
}
