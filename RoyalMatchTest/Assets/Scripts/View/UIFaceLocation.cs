using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.Tools.ScreensManagerTool;

public class UIFaceLocation : MonoBehaviour
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
        
        int locationIndex = ConfigsService.Instance.LocationListConfig.locations.IndexOf(location);
        _isLocationUnlocked = PlayerPrefsService.Instance.IsLocationUnlocked(locationIndex);
        
        txtLocationName.text = location.locationName;
        imgLocation.sprite = Resources.Load<Sprite>($"Images/Locations/{location.imgFaceLocationName}");
        imgLocation.material = _isLocationUnlocked ? null : grayScaleMaterial;
        ChangeColorDecoration(_isLocationUnlocked);
        
        uiProgressBar.gameObject.SetActive(_isLocationUnlocked);
        if (_isLocationUnlocked) 
            uiProgressBar.SetPercent(30);//TODO: temp value
    }
    
    private void ChangeColorDecoration( bool isLocationUnlocked )
    {
        foreach (Image img in imgChangeColorDecoration)
            img.color = isLocationUnlocked ? unlockedColor : lockedColor;
    }
    
    private void OnSelectLocation()
    {
        if (_isLocationUnlocked)
        {
            ScreensManager.Instance.Open<LocationScreen>( 
                screen => screen.Init(_location), 
                _ => ScreensManager.Instance.TrimHistory() );
        }
        else
        {
            Debug.Log("Location is locked");//TODO: show popup
        }
    }
}
