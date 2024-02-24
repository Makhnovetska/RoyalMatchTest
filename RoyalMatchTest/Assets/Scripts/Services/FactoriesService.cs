using UnityEngine;

public class FactoriesService : MonoBehaviour
{
    [SerializeField] private UIFaceLocationFactory uiFaceLocationFactory;
    
    public UIFaceLocationFactory UIFaceLocationFactory => uiFaceLocationFactory;
    
    public static FactoriesService Instance { get; private set; }

    private void Awake() => Instance = this;
}