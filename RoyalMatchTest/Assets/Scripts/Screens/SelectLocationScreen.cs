using System.Collections.Generic;
using System.Linq;
using Configs;
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
        List<UILocation> uiLocations = FactoriesService.Instance.uiLocationFactory.Spawn(locationListConfig.locations.Count, scrollRect.content);
        scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, GetContentHeight(locationListConfig.locations.Count, 500.0f));
        List<Location> orderedLocations = locationListConfig.locations.OrderBy(it => it.locationId).ToList();
        
        for (int i = 0; i < uiLocations.Count; i++)
            uiLocations[i].Init(orderedLocations[i]);
    }
    
    private void Deinit()
    {
        FactoriesService.Instance.uiLocationFactory.DespawnAll();
    }
    
    private float GetContentHeight(int elementCount, float elementHeight)
    {
        return elementCount * elementHeight
               + (elementCount - 1) * verticalLayoutGroup.spacing
               + verticalLayoutGroup.padding.top
               + verticalLayoutGroup.padding.bottom;
    }
}
