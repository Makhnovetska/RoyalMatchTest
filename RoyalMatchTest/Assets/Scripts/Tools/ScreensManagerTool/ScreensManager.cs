using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utils.Tools.ScreensManagerTool.Blocker;

namespace Utils.Tools.ScreensManagerTool
{
    public class ScreensManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _screensParent;
        [SerializeField] private RectTransform _cacheParent;
        [SerializeField] private UIBlocker _blocker;
        [SerializeField] private string _screensResourcePath = "Prefabs/UI/Screens";
        
        private readonly HashSet<Screen> _screensPrefabs = new();
        private readonly List<Screen> _openedScreens = new();
        private readonly List<Screen> _closedScreens = new();
        
        private Coroutine _openCoroutine;
        private Coroutine _closeCoroutine;
        
        public Canvas canvas => _canvas;
        public UIBlocker blocker => _blocker;
        public Screen currentScreen => _openedScreens.Count > 0 ? _openedScreens.Last() : default;
        public IReadOnlyList<Screen> openedScreens => _openedScreens;
        
        public bool isOpening { get; private set; }
        public bool isClosing { get; private set; }
        public bool isInTransition => isOpening || isClosing;
        
        public event Action<Screen> onScreenOpening;
        public event Action<Screen> onScreenOpened;
        public event Action<Screen> onScreenClosing;
        public event Action<Screen> onScreenClosed;

        public static ScreensManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                Init();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public void Init()
        {
            LoadScreenPrefabs();
            Debug.Log("[ScreensManager] Loaded screens prefabs: " + _screensPrefabs.Count);
        }
        
        private void LoadScreenPrefabs()
        {
            Screen[] screens = Resources.LoadAll<Screen>(_screensResourcePath);
            foreach (Screen screen in screens)
            {
                _screensPrefabs.Add(screen);
            }
        }
        
        public void Open<T>(Action<T> onOpening = default, Action<T> onOpened = default) where T : Screen
        {
            T screenInstance = GetScreenInstance<T>();
            _openCoroutine = StartCoroutine(OpenScreen(screenInstance, onOpening, onOpened));
        }
        
        public void OpenInstant<T>() where T : Screen
        {
            T screen = GetScreenInstance<T>();
            isOpening = true;
            _blocker.Block();
            onScreenOpening?.Invoke(screen);
            screen.transform.SetAsLastSibling();
            screen.OpenInstant();
            screen.Activate();
            
            // Deactivate all previous screens if current screen is a full screen
            if (_openedScreens.Count > 0 && screen.screenType == ScreenType.Screen)
            {
                for (int i = _openedScreens.Count - 1; i >= 0; i--)
                {
                    if (_openedScreens[i].isActive) _openedScreens[i].Deactivate();
                }
            }
            
            _openedScreens.Add(screen);
            isOpening = false;
            _openCoroutine = default;
            _blocker.Unblock();
            onScreenOpened?.Invoke(screen);
        }
        
        public void CloseLast()
        {
            if (_openedScreens.Count == 0) return;
            Screen screen = _openedScreens.Last();
            _closeCoroutine = StartCoroutine(CloseScreen(screen));
        }
        
        public void CloseUntil<T>() where T : Screen
        {
            T screen = _openedScreens.LastOrDefault(n => n is T) as T;
            _closeCoroutine = StartCoroutine(CloseScreensUntil(screen));
        }

        public void CloseLastInstant()
        {
            if (_openedScreens.Count == 0) return;
            Screen screen = _openedScreens.Last();
            CloseInstant(screen);
        }

        public void BringToFront<T>(Action<T> onOpening, Action<T> onOpened) where T : Screen
        {
            T screen = _openedScreens.LastOrDefault(n => n is T) as T;
            if (screen == default) return;
            CloseInstant(screen);
            Open(onOpening, onOpened);
        }
        
        private void CloseInstant(Screen screen)
        {
            onScreenClosing?.Invoke(screen);
            _openedScreens.Remove(screen);
            for (int i = _openedScreens.Count - 1; i >= 0; i--)
            {
                if (!_openedScreens[i].isActive) _openedScreens[i].Activate();
                if (_openedScreens[i].screenType == ScreenType.Screen) break;
            }
            onScreenClosed?.Invoke(screen);
            CacheScreen(screen);
        }
        
        public void CloseAllInstant()
        {
            while (_openedScreens.Count > 0)
                CloseLastInstant();
        }

        public void TrimHistory()
        {
            while (_openedScreens.Count(screen => !screen.isActive) > 0)
            {
                Screen screen = _openedScreens.Last(screen => !screen.isActive);
                CloseInstant(screen);
            }
        }

        public void TrimHistoryUntil<T>()
        {
            while (_openedScreens.Count(screen => !screen.isActive) > 0)
            {
                Screen screen = _openedScreens.Last(screen => !screen.isActive);
                if (screen is T) break;
                CloseInstant(screen);
            }
        }
        
        public void CloseInstantUntil<T>() where T : Screen
        {
            while (_openedScreens.Count > 0 && _openedScreens.Last() is not T)
                CloseLastInstant();
        }
        
        public bool TryGetOpened<T>(out T screen) where T : Screen
        {
            screen = _openedScreens.LastOrDefault(screen => screen is T) as T;
            return screen != default;
        }
        
        private T GetScreenInstance<T>() where T : Screen
        {
            T instance = _closedScreens.FirstOrDefault(screen => screen is T) as T;
            RectTransform parent = default;
            if (instance == default)
            {
                T prefab = _screensPrefabs.First(screen => screen is T) as T;
                parent = _screensParent;
                instance = Instantiate(prefab, parent);
            }
            else
            {
                _closedScreens.Remove(instance);
            }
            
            parent = _screensParent;
            instance.transform.SetParent(parent);
            return instance;
        }
        
        private IEnumerator OpenScreen<T>(T screen, Action<T> onOpening, Action<T> onOpened) where T : Screen
        {
            isOpening = true;
            _blocker.Block();
            onScreenOpening?.Invoke(screen);
            screen.transform.SetAsLastSibling();
            onOpening?.Invoke(screen);
            yield return screen.Open();
            screen.Activate();
            // Deactivate all previous screens if current screen is a full screen
            if (_openedScreens.Count > 0 && screen.screenType == ScreenType.Screen)
            {
                for (int i = _openedScreens.Count - 1; i >= 0; i--)
                {
                    if (_openedScreens[i].isActive) _openedScreens[i].Deactivate();
                }
            }
            _openedScreens.Add(screen);
            isOpening = false;
            _openCoroutine = default;
            _blocker.Unblock();
            onOpened?.Invoke(screen);
            onScreenOpened?.Invoke(screen);
        }

        private IEnumerator CloseScreen(Screen screen)
        {
            isClosing = true;
            _blocker.Block();
            onScreenClosing?.Invoke(screen);
            _openedScreens.Remove(screen);
            
            for (int i = _openedScreens.Count - 1; i >= 0; i--)
            {
                if (!_openedScreens[i].isActive) _openedScreens[i].Activate();
                if (_openedScreens[i].screenType == ScreenType.Screen) break;
            }
            
            yield return screen.Close();
            isClosing = false;
            _closeCoroutine = default;
            onScreenClosed?.Invoke(screen);
            CacheScreen(screen);
            _blocker.Unblock();
        }

        private IEnumerator CloseScreensUntil(Screen screen)
        {
            for (int i = _openedScreens.Count - 1; i >= 0; i--)
            {
                if (screen != default && _openedScreens[i] == screen) break;
                yield return CloseScreen(_openedScreens[i]);
            }
        }

        private void CacheScreen(Screen screen)
        {
            if (screen.isActive)
            {
                screen.Deactivate();
            }

            screen.CloseInstant();
            screen.transform.SetParent(_cacheParent);
            _closedScreens.Add(screen);
        }
    }
}