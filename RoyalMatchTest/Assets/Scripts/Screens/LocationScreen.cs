using Configs;
using UnityEngine;
using UnityEngine.UI;
using Utils.Tools.ScreensManagerTool;
using Screen = Utils.Tools.ScreensManagerTool.Screen;

public class LocationScreen : Screen
{
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnPlaceFurniture;
    
    private Location _location;
    private LocationService _locationService;
    
    private int currentItemId => PlayerPrefsService.Instance.GetCurrentItemId();

    protected void Awake()
    {
        _locationService = LocationService.Instance;
    }

    protected override void OnActivated()
    {
        btnBack.onClick.AddListener(OnBackBtnClick);
        btnPlaceFurniture.onClick.AddListener(OnPlaceFurnitureBtnClick);
        
        _locationService.onItemUnlocked += onItemUnlocked;
    }
    
    protected override void OnDeactivated()
    {
        btnBack.onClick.RemoveListener(OnBackBtnClick);
        btnPlaceFurniture.onClick.RemoveListener(OnPlaceFurnitureBtnClick);
        
        _locationService.onItemUnlocked -= onItemUnlocked;
    }

    public void Init(Location location)
    {
        _location = location;

        _locationService.LoadLocation(_location);
        UpdateBtnPlaceFurniture();
    }
    
    private void UpdateBtnPlaceFurniture()
    {
        bool allFurniturePlaced = PlayerPrefsService.Instance
            .IsAllFurniturePlaced(_location.locationId, _location.itemsCount);
        btnPlaceFurniture.interactable = !allFurniturePlaced;
    }

    protected override void OnClosed()
    {
        _locationService.UnloadLocation();
        Resources.UnloadUnusedAssets();
    }

    private void onItemUnlocked(int locationId, int itemId)
    {
        if (locationId != _location.locationId)
            return;

        UpdateBtnPlaceFurniture();
    }
    
    private void OnBackBtnClick()
    {
        ScreensManager.Instance.Open<SelectLocationScreen>(
            onOpened: _ => ScreensManager.Instance.TrimHistory());
    }
    
    private void OnPlaceFurnitureBtnClick()
    {
        _locationService.UnlockItem(_location.locationId, currentItemId);
    }
}
