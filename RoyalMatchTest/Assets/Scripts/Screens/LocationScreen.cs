using UnityEngine;
using UnityEngine.UI;
using Utils.Tools.ScreensManagerTool;
using Screen = Utils.Tools.ScreensManagerTool.Screen;

public class LocationScreen : Screen
{
    [SerializeField] private Image imgLocation;
    [SerializeField] private Button btnBack;
    [SerializeField] private Button btnPlaceFurniture;
    
    protected override void OnActivated()
    {
        btnBack.onClick.AddListener(OnBackBtnClick);
        btnPlaceFurniture.onClick.AddListener(OnPlaceFurnitureBtnClick);
    }
    
    protected override void OnDeactivated()
    {
        btnBack.onClick.RemoveListener(OnBackBtnClick);
        btnPlaceFurniture.onClick.RemoveListener(OnPlaceFurnitureBtnClick);
    }
    
    public void Init(Location location)
    {
        imgLocation.sprite = Resources.Load<Sprite>($"Images/Locations/{location.imgLocationName}");
    }
    
    private void OnBackBtnClick()
    {
        ScreensManager.Instance.Open<SelectLocationScreen>(onOpened: _ => ScreensManager.Instance.TrimHistory());
    }
    
    private void OnPlaceFurnitureBtnClick()
    {
        Debug.LogError("OnPlaceFurnitureBtnClick");
    }
}
