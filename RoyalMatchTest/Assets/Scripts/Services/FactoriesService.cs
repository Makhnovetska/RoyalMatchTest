using Factory;
using UnityEngine;

public class FactoriesService : MonoBehaviour
{
    [SerializeField] private UILocationFactory _uiLocationFactory;
    
    public UILocationFactory uiLocationFactory => _uiLocationFactory;
    
    public static FactoriesService Instance { get; private set; }

    private void Awake() => Instance = this;
}