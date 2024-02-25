using Configs;
using UnityEngine;

public class ConfigsService : MonoBehaviour
{
    [SerializeField] private LocationListConfig locationListConfig;
    
    public LocationListConfig LocationListConfig => locationListConfig;
    
    public static ConfigsService Instance { get; private set; }

    private void Awake() => Instance = this;
}