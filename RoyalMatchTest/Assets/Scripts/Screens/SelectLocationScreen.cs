using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Screen = Utils.Tools.ScreensManagerTool.Screen;

public class SelectLocationScreen : Screen
{
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private VerticalLayoutGroup verticalLayoutGroup;

    protected override void OnActivated()
    {
        Init();
    }

    protected override void OnDeactivated()
    {
        Deinit();
    }
    
    private void Init()
    {
        LocationListConfig locationListConfig = ConfigsService.Instance.LocationListConfig;
        List<UIFaceLocation> faceLocations = UIFaceLocationFactory.Instance.Spawn(locationListConfig.locations.Count, scrollRect.content);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, GetContentHeight(locationListConfig.locations.Count, 500.0f));
        
        for (int i = 0; i < faceLocations.Count; i++)
            faceLocations[i].Init(locationListConfig.locations[i]);
    }
    
    private void Deinit()
    {
        UIFaceLocationFactory.Instance.DespawnAll();
    }
    
    private float GetContentHeight(int elementCount, float elementHeight)
    {
        return elementCount * (verticalLayoutGroup.spacing + elementHeight)
               + verticalLayoutGroup.padding.top
               + verticalLayoutGroup.padding.bottom;
    }
}
