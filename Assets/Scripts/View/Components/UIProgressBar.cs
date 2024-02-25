using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIProgressBar: MonoBehaviour
{
    [SerializeField] private Image imgFilling;
    [SerializeField] private TextMeshProUGUI txtProgress;

    public void SetPercent(int progressPercent, string overrideProgressText = null )
    {
        imgFilling.fillAmount = (float)progressPercent / 100;
        txtProgress.text = overrideProgressText ?? $"{progressPercent}%";
    }
}
