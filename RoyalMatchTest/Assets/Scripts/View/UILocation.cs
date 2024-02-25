using System.Collections.Generic;
using Configs;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Tools.ScreensManagerTool;

public class UILocation : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLocationName;
    [SerializeField] private Image imgLocation;
    [SerializeField] private Button btnSelectLocation;
    [SerializeField] private UIProgressBar uiProgressBar;
    [SerializeField] private List<Image> imgChangeColorDecoration;
    [SerializeField] private Material grayScaleMaterial;
    
    private static Color unlockedColor = new Color(0.21f, 0.12f, 0.45f);
    private static Color lockedColor = new Color(0.27f, 0.26f, 0.27f);

    private Location _location;
    private bool _isLocationUnlocked;

    public void OnEnable()
    {
        btnSelectLocation.onClick.AddListener(OnSelectLocation);
    }
    
    public void OnDisable()
    {
        btnSelectLocation.onClick.RemoveListener(OnSelectLocation);
    }

    public void Init(Location location)
    {
        _location = location;
        
        _isLocationUnlocked = PlayerPrefsService.Instance.IsLocationUnlocked(_location.locationId);
        
        txtLocationName.text = location.locationName;
        imgLocation.sprite = Resources.Load<Sprite>($"Images/Locations/{location.imgUILocationName}");
        imgLocation.material = _isLocationUnlocked ? null : grayScaleMaterial;
        ChangeColorDecoration(_isLocationUnlocked);
        btnSelectLocation.interactable = _isLocationUnlocked;
        
        uiProgressBar.gameObject.SetActive(_isLocationUnlocked);
        if (_isLocationUnlocked) 
            uiProgressBar.SetPercent(GetProgressValue(), GetOverrideProgressText());
    }
    
    private int GetProgressValue()
    {
        if (PlayerPrefsService.Instance.IsLocationCompleted(_location.locationId))
            return 100;

        if (!PlayerPrefsService.Instance.IsLocationUnlocked(_location.locationId))
            return 0;

        if (_location.isFake)
            return 0;

        return PlayerPrefsService.Instance.GetCurrentItemId() * 100 / _location.itemsCount;
    }
    
    private string GetOverrideProgressText()
    {
        if (PlayerPrefsService.Instance.IsLocationCompleted(_location.locationId))
            return "Completed";

        if (_location.isFake)
            return "Coming soon";

        return null;
    }
    
    private void ChangeColorDecoration( bool isLocationUnlocked )
    {
        foreach (Image img in imgChangeColorDecoration)
            img.color = isLocationUnlocked ? unlockedColor : lockedColor;
    }
    
    private void OnSelectLocation()
    {
        if (_location.isFake)
            return;
        
        ScreensManager.Instance.Open<LocationScreen>( 
            screen => screen.Init(_location), 
            _ => ScreensManager.Instance.TrimHistory() );
    }
}
