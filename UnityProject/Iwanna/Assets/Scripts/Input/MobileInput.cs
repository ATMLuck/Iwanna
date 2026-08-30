using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 移动端触摸控制 UI：程序化搭建 Canvas + 虚拟按键，运行时自动创建并常驻。
/// 仅在移动平台且处于关卡场景时显示；PC / 编辑器默认隐藏，不影响 Windows 构建。
/// 布局：右半区左右移动按钮，左半区跳跃 + 攻击按钮。
/// </summary>
public class MobileInput : MonoBehaviour
{
    [Header("调试")]
    [Tooltip("在编辑器中强制显示触摸UI，用于预览布局")]
    public bool forceShowInEditor;

    [Header("布局参数")]
    [Tooltip("按钮尺寸（1920x1080 参考分辨率下）")]
    public Vector2 buttonSize = new Vector2(180f, 180f);
    [Tooltip("与屏幕边缘的边距")]
    public Vector2 margin = new Vector2(80f, 100f);
    [Tooltip("按钮间距")]
    public float spacing = 30f;

    private GameObject _canvasRoot;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateInstance()
    {
        if (FindObjectOfType<MobileInput>() != null) return;
        var go = new GameObject("MobileInput");
        DontDestroyOnLoad(go);
        go.AddComponent<MobileInput>();
    }

    private void Awake()
    {
        _canvasRoot = BuildUI();
        SceneManager.sceneLoaded += OnSceneLoaded;
        UpdateVisibility();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateVisibility();
    }

    private bool ShouldShow
    {
        get
        {
            #if UNITY_EDITOR
            return forceShowInEditor;
            #else
            return Application.platform == RuntimePlatform.Android
                || Application.platform == RuntimePlatform.IPhonePlayer;
            #endif
        }
    }

    private static bool IsLevelScene(string sceneName)
    {
        return sceneName.StartsWith("Level_") || sceneName.StartsWith("level_");
    }

    private void UpdateVisibility()
    {
        bool active = ShouldShow && IsLevelScene(SceneManager.GetActiveScene().name);
        if (_canvasRoot.activeSelf != active)
        {
            _canvasRoot.SetActive(active);
            if (!active) VirtualInput.Reset();
        }
    }

    //=============================== UI 搭建 =============================================
    private GameObject BuildUI()
    {
        EnsureEventSystem();

        var root = new GameObject("MobileTouchCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        root.transform.SetParent(transform, false);
        var canvas = root.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = root.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        // ---- 右半区：左右移动（右下角） ----
        CreateMoveButton(root.transform, "RightMoveButton", "UI/Androad/Right",
            new Vector2(-margin.x, margin.y), 1);
        CreateMoveButton(root.transform, "LeftMoveButton", "UI/Androad/Left",
            new Vector2(-margin.x - spacing - buttonSize.x, margin.y), -1);

        // ---- 左半区：跳跃 + 攻击（左下角） ----
        CreateTouchButton(root.transform, "JumpButton", "UI/Androad/Jump",
            new Vector2(margin.x, margin.y), TouchButton.ButtonType.Jump);
        CreateTouchButton(root.transform, "AttackButton", "UI/Androad/Fight",
            new Vector2(margin.x + spacing + buttonSize.x, margin.y), TouchButton.ButtonType.Shoot);

        return root;
    }

    // 与 UIManager 策略一致：保证常驻 EventSystem 存在（UIManager 也会做同样的事）
    private static void EnsureEventSystem()
    {
        var existing = FindObjectOfType<EventSystem>();
        if (existing != null)
        {
            if (existing.gameObject.scene.name != "DontDestroyOnLoad")
                DontDestroyOnLoad(existing.gameObject);
            return;
        }

        var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
        DontDestroyOnLoad(go);
    }

    private void CreateMoveButton(Transform parent, string name, string spritePath,
        Vector2 anchoredPosition, int direction)
    {
        var go = CreateButtonBase(parent, name, spritePath, anchoredPosition);
        var move = go.AddComponent<HoldMoveButton>();
        move.direction = direction;
    }

    private void CreateTouchButton(Transform parent, string name, string spritePath,
        Vector2 anchoredPosition, TouchButton.ButtonType type)
    {
        var go = CreateButtonBase(parent, name, spritePath, anchoredPosition);
        var touch = go.AddComponent<TouchButton>();
        touch.type = type;
    }

    // 底部锚点（左下角 cluster 用 (0,0)，右下角 cluster 用 (1,0)），pivot 跟随锚点
    private GameObject CreateButtonBase(Transform parent, string name, string spritePath,
        Vector2 anchoredPosition)
    {
        var go = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);

        var rt = (RectTransform)go.transform;
        rt.anchorMin = anchoredPosition.x < 0 ? new Vector2(1f, 0f) : new Vector2(0f, 0f);
        rt.anchorMax = rt.anchorMin;
        rt.pivot = rt.anchorMin;
        rt.sizeDelta = buttonSize;
        rt.anchoredPosition = anchoredPosition;

        var img = go.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>(spritePath);
        img.preserveAspect = true;

        var btn = go.GetComponent<Button>();
        btn.targetGraphic = img;
        btn.transition = Selectable.Transition.ColorTint;
        btn.colors = new ColorBlock
        {
            colorMultiplier = 1f,
            fadeDuration = 0.1f,
            normalColor = Color.white,
            highlightedColor = new Color(0.9f, 0.9f, 0.9f, 1f),
            pressedColor = new Color(0.7f, 0.7f, 0.7f, 1f),
            selectedColor = Color.white,
            disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f)
        };

        return go;
    }
}
