using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LocationListConfig", menuName = "ScriptableObject/LocationListConfig", order = 0)]
public class LocationListConfig : ScriptableObject
{
    public List<Location> locations = new List<Location>();
}

[System.Serializable]
public struct Location
{
    public string locationName;
    public string imgFaceLocationName;
    public string imgLocationName;
}