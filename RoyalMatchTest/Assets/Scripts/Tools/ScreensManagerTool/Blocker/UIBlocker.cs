using UnityEngine;

namespace Utils.Tools.ScreensManagerTool.Blocker
{
    public class UIBlocker : MonoBehaviour
    {
        [SerializeField] private RectTransform _blockerParent;

        private int _blockerCount;
        public int blockerCount => _blockerCount;
        public bool isBlocked => _blockerCount > 0;

        private void Start()
        {
            RefreshBlocker();
        }

        public void Block()
        {
            _blockerCount++;
            RefreshBlocker();
        }
        
        public void Unblock()
        {
            _blockerCount = Mathf.Max(0, --_blockerCount);
            RefreshBlocker();
        }
        
        public void UnblockAll()
        {
            _blockerCount = 0;
            RefreshBlocker();
        }

        private void RefreshBlocker()
        {
            _blockerParent.gameObject.SetActive(_blockerCount > 0);
        }
    }
}