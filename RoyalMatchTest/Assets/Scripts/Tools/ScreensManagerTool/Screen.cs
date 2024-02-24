using System;
using System.Collections;
using UnityEngine;
using Utils.Tools.ScreensManagerTool.Appearance;

namespace Utils.Tools.ScreensManagerTool
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] private ScreenAppearance _openingAppearance;
        [SerializeField] private ScreenAppearance _closingAppearance;
        [SerializeField] private ScreenType _screenType;
        public ScreenType screenType => _screenType;
        
        private GameObject _child;
        
        public event Action onOpening;
        public event Action onOpened;
        public event Action onClosing;
        public event Action onClosed;
        public event Action onActivated;
        public event Action onDeactivated;
        
        
        public bool isActive { get; private set; }
        
        internal IEnumerator Open()
        {
            // Should be activated before opening so screen can fade in
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(true);
            
            OnOpening();
            onOpening?.Invoke();
            yield return _openingAppearance.Play();
            OnOpened();
            onOpened?.Invoke();
        }

        internal void OpenInstant()
        {
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(true);
            
            OnOpening();
            onOpening?.Invoke();
            _openingAppearance.SetFinished();
            OnOpened();
            onOpened?.Invoke();
        }
        
        internal IEnumerator Close()
        {
            OnClosing();
            onClosing?.Invoke();
            yield return _closingAppearance.Play();
            OnClosed();
            onClosed?.Invoke();
            
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(false);
        }
        
        internal void CloseInstant()
        {
            OnClosing();
            onClosing?.Invoke();
            _closingAppearance.SetFinished();
            OnClosed();
            onClosed?.Invoke();
            
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(false);
        }
        
        internal void Activate()
        {
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(true);
            isActive = true;
            OnActivated();
            onActivated?.Invoke();
        }
        
        internal void Deactivate()
        {
            isActive = false;
            OnDeactivated();
            onDeactivated?.Invoke();
            _child ??= transform.GetChild(0).gameObject;
            _child.SetActive(false);
        }
        
        protected virtual void OnOpening() {}
        protected virtual void OnOpened() {}
        protected virtual void OnClosing() {}
        protected virtual void OnClosed() {}
        protected virtual void OnActivated() { }
        protected virtual void OnDeactivated() { }
    }
}
