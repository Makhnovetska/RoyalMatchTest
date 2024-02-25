using System.Collections;
using UnityEngine;

namespace Utils.Tools.ScreensManagerTool.Appearance.Appearances
{
    public class FadingAppearance : ScreenAppearance
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _duration = 1.0f;
        [SerializeField] private float _fromAlpha = 0.0f;
        [SerializeField] private float _toAlpha = 1.0f;
        
        internal override IEnumerator Play()
        {
            float time = 0.0f;
            
            while (true)
            {
                time += Time.deltaTime;
                float progress = Mathf.Clamp01(time / _duration);
                _canvasGroup.alpha = Mathf.Lerp(_fromAlpha, _toAlpha, progress);
                if (progress >= 1.0f) break;
                yield return null;
            }
        }

        internal override void SetFinished()
        {
            _canvasGroup.alpha = _toAlpha;
        }
    }
}