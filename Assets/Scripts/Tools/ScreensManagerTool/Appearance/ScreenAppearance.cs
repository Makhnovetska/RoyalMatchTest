using System.Collections;
using UnityEngine;

namespace Utils.Tools.ScreensManagerTool.Appearance
{
    public abstract class ScreenAppearance : MonoBehaviour
    {
        internal abstract IEnumerator Play();
        internal abstract void SetFinished();
    }
}