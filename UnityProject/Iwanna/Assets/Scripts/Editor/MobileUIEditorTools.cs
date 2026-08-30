using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// 移动端触摸按钮布局编辑器工具：
// 1. 生成编辑画布 —— 在场景里生成可拖拽的 4 个按钮
// 2. 保存布局     —— 把按钮坐标写进 MobileLayoutConfig 资源
// 3. 删除编辑画布 —— 清理占位画布
// 使用流程：生成 -> 拖拽调整 -> 保存 -> 删除（或直接进 Play，画布会自动清理）
// </summary>
public static class MobileUIEditorTools
{
    private const string ConfigPath = "Assets/Resources/MobileLayoutConfig.asset";
    private const string CanvasName = "MobileTouchCanvas";

    // 按钮定义：id -> 物体名 / 贴图 / 组件
    private class ButtonSpec
    {
        public string id;
        public string goName;
        public string spritePath;
        public int moveDir;                       // 0 = 非移动按钮
        public TouchButton.ButtonType touchType;
    }

    private static readonly ButtonSpec[] Specs =
    {
        new ButtonSpec { id = "RightMove", goName = "RightMoveButton", spritePath = "UI/Androad/Right", moveDir = 1 },
        new ButtonSpec { id = "LeftMove",  goName = "LeftMoveButton",  spritePath = "UI/Androad/Left",  moveDir = -1 },
        new ButtonSpec { id = "Jump",      goName = "JumpButton",      spritePath = "UI/Androad/Jump",  touchType = TouchButton.ButtonType.Jump },
        new ButtonSpec { id = "Attack",    goName = "AttackButton",    spritePath = "UI/Androad/Fight", touchType = TouchButton.ButtonType.Shoot },
    };

    [MenuItem("Tools/MobileUI/1. 生成编辑画布")]
    public static void BuildEditCanvas()
    {
        if (FindEditCanvas() != null)
        {
            EditorUtility.DisplayDialog("MobileUI", "场景里已存在编辑画布。\n直接拖拽现有按钮调整，或先执行「3. 删除编辑画布」。", "知道了");
            return;
        }

        var cfg = GetConfig();

        var canvasGo = new GameObject(CanvasName, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        // DontSave：占位画布只用于编辑预览，不随场景/构建保存，避免污染正式场景
        canvasGo.hideFlags = HideFlags.DontSave;
        var canvas = canvasGo.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 100;

        var scaler = canvasGo.GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        scaler.matchWidthOrHeight = 0.5f;

        foreach (var spec in Specs)
        {
            var layout = cfg != null ? cfg.Get(spec.id) : null;
            if (layout == null) layout = DefaultLayout(spec.id);
            CreateEditButton(canvasGo.transform, spec, layout);
        }

        Selection.activeGameObject = canvasGo;
        EditorUtility.DisplayDialog("MobileUI", "已生成编辑画布。\n在 Scene 视图里直接拖拽 4 个按钮调整位置，\n调整好后执行「2. 保存布局」。", "知道了");
    }

    [MenuItem("Tools/MobileUI/2. 保存布局")]
    public static void SaveLayout()
    {
        var canvasGo = FindEditCanvas();
        if (canvasGo == null)
        {
            EditorUtility.DisplayDialog("MobileUI", "未找到编辑画布，请先执行「1. 生成编辑画布」。", "知道了");
            return;
        }

        var cfg = GetConfig(true);
        cfg.buttons = new MobileLayoutConfig.ButtonLayout[Specs.Length];
        for (int i = 0; i < Specs.Length; i++)
        {
            var spec = Specs[i];
            var child = canvasGo.transform.Find(spec.goName);
            if (child == null)
            {
                EditorUtility.DisplayDialog("MobileUI", "找不到按钮 " + spec.goName + "，保存中止。", "知道了");
                return;
            }

            var rt = (RectTransform)child;
            cfg.buttons[i] = new MobileLayoutConfig.ButtonLayout
            {
                id = spec.id,
                anchor = rt.anchorMin.x > 0.5f ? MobileLayoutConfig.Anchor.BottomRight : MobileLayoutConfig.Anchor.BottomLeft,
                anchoredPosition = rt.anchoredPosition,
                size = rt.sizeDelta
            };
        }

        EditorUtility.SetDirty(cfg);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("MobileUI", "布局已保存到 " + ConfigPath + "\n运行时将按此位置显示按钮。", "知道了");
    }

    [MenuItem("Tools/MobileUI/3. 删除编辑画布")]
    public static void RemoveEditCanvas()
    {
        var canvasGo = FindEditCanvas();
        if (canvasGo != null) Object.DestroyImmediate(canvasGo);
        else Debug.Log("MobileUI：没有编辑画布需要删除。");
    }

    //============================== 自动清理 =============================================
    [InitializeOnLoad]
    private static class AutoCleanup
    {
        static AutoCleanup()
        {
            EditorApplication.playModeStateChanged += OnPlayModeChanged;
        }

        // 进入 Play 前清掉编辑画布，避免与运行时自动生成的画布重叠
        private static void OnPlayModeChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) return;
            var canvasGo = FindEditCanvas();
            if (canvasGo != null) Object.DestroyImmediate(canvasGo);
        }
    }

    //============================== 工具方法 =============================================
    // 用场景根物体遍历查找：占位画布带 DontSave 标记，FindObjectsOfType 找不到它
    internal static GameObject FindEditCanvas()
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            var scene = SceneManager.GetSceneAt(i);
            if (!scene.IsValid() || !scene.isLoaded) continue;
            foreach (var root in scene.GetRootGameObjects())
            {
                if (root.name == CanvasName) return root;
            }
        }
        return null;
    }

    private static MobileLayoutConfig GetConfig(bool createIfMissing = false)
    {
        var cfg = AssetDatabase.LoadAssetAtPath<MobileLayoutConfig>(ConfigPath);
        if (cfg == null && createIfMissing)
        {
            EnsureResourcesFolder();
            cfg = ScriptableObject.CreateInstance<MobileLayoutConfig>();
            AssetDatabase.CreateAsset(cfg, ConfigPath);
            AssetDatabase.SaveAssets();
        }
        return cfg;
    }

    private static void EnsureResourcesFolder()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            AssetDatabase.CreateFolder("Assets", "Resources");
    }

    // 与 MobileInput 代码默认一致（margin=80/100，spacing=30，size=180x180）
    private static MobileLayoutConfig.ButtonLayout DefaultLayout(string id)
    {
        var margin = new Vector2(80f, 100f);
        const float spacing = 30f;
        var size = new Vector2(180f, 180f);

        switch (id)
        {
            case "RightMove":
                return new MobileLayoutConfig.ButtonLayout
                { id = id, anchor = MobileLayoutConfig.Anchor.BottomRight, anchoredPosition = new Vector2(-margin.x, margin.y), size = size };
            case "LeftMove":
                return new MobileLayoutConfig.ButtonLayout
                { id = id, anchor = MobileLayoutConfig.Anchor.BottomRight, anchoredPosition = new Vector2(-margin.x - spacing - size.x, margin.y), size = size };
            case "Jump":
                return new MobileLayoutConfig.ButtonLayout
                { id = id, anchor = MobileLayoutConfig.Anchor.BottomLeft, anchoredPosition = new Vector2(margin.x, margin.y), size = size };
            default: // Attack
                return new MobileLayoutConfig.ButtonLayout
                { id = id, anchor = MobileLayoutConfig.Anchor.BottomLeft, anchoredPosition = new Vector2(margin.x + spacing + size.x, margin.y), size = size };
        }
    }

    private static GameObject CreateEditButton(Transform parent, ButtonSpec spec, MobileLayoutConfig.ButtonLayout layout)
    {
        var go = new GameObject(spec.goName, typeof(RectTransform), typeof(Image), typeof(Button));
        go.transform.SetParent(parent, false);
        go.hideFlags = HideFlags.DontSave;

        var rt = (RectTransform)go.transform;
        bool bottomRight = layout.anchor == MobileLayoutConfig.Anchor.BottomRight;
        rt.anchorMin = bottomRight ? new Vector2(1f, 0f) : new Vector2(0f, 0f);
        rt.anchorMax = rt.anchorMin;
        rt.pivot = rt.anchorMin;
        rt.sizeDelta = layout.size;
        rt.anchoredPosition = layout.anchoredPosition;

        var img = go.GetComponent<Image>();
        img.sprite = Resources.Load<Sprite>(spec.spritePath);
        img.preserveAspect = true;

        var btn = go.GetComponent<Button>();
        btn.targetGraphic = img;

        if (spec.moveDir != 0)
        {
            var move = go.AddComponent<HoldMoveButton>();
            move.direction = spec.moveDir;
        }
        else
        {
            var touch = go.AddComponent<TouchButton>();
            touch.type = spec.touchType;
        }

        return go;
    }
}
