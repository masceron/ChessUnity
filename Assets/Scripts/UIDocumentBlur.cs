using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System.Linq;
using ZLinq;

[RequireComponent(typeof(UIDocument))]
public class UIDocumentBlur : MonoBehaviour
{
    public enum BlurType
    {
        Kawase,
        DualKawase,
        Gaussian
    }

    [Header("References")]
    public Camera targetCamera;

    [Header("Shader References (REQUIRED for builds!)")]
    [Tooltip("Assign shaders here to prevent stripping in builds")]
    public Shader kawaseShader;
    public Shader dualKawaseShader;
    public Shader gaussianShader;

    [Header("Blur Settings")]
    public BlurType blurType = BlurType.DualKawase;

    [Tooltip("Class name to search for (without the dot)")]
    public string blurClassName = "blur";

    [Header("Kawase Settings")]
    [Range(1, 8)] public int kawaseIterations = 4;

    [Header("Dual Kawase Settings")]
    [Range(1, 6)] public int dualKawasePasses = 4;

    [Header("Gaussian Settings")]
    [Range(1, 8)] public int gaussianIterations = 3;
    [Range(0.1f, 10)] public float gaussianBlurSize = 3f;

    [Header("Common Settings")]
    [Range(1, 8)] public int downsample = 2;

    [Header("Performance")]
    public int updateInterval = 2;
    public bool onlyUpdateWhenVisible = true;

    private UIDocument _uiDocument;
    private readonly List<BlurPanelData> _panels = new List<BlurPanelData>();
    private RenderTexture _cameraRT;
    private RenderTexture _blurredFullRT;
    private Material _kawaseMaterial;
    private Material _dualKawaseMaterial;
    private Material _gaussianMaterial;
    private int _frameCounter;
    private bool _isInitialized;
    private bool _needsRefresh;

    private static readonly int BlurSizeId = Shader.PropertyToID("_BlurSize");
    private static readonly int OffsetId = Shader.PropertyToID("_Offset");

    private class BlurPanelData
    {
        public VisualElement Panel;
        public VisualElement Background;
        public Texture2D CroppedTexture;
    }

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        CreateMaterials();
    }

    private void CreateMaterials()
    {
        // Try assigned shader first, fallback to Shader.Find
        var kShader = kawaseShader ? kawaseShader : Shader.Find("Hidden/KawaseBlur");
        if (kShader)
        {
            _kawaseMaterial = new Material(kShader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }
        else
        {
            Debug.LogError("UIDocumentBlur: KawaseBlur shader not found!  Assign it in the inspector.");
        }

        var dkShader = dualKawaseShader ? dualKawaseShader : Shader.Find("Hidden/DualKawaseBlur");
        if (dkShader)
        {
            _dualKawaseMaterial = new Material(dkShader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }
        else
        {
            Debug.LogError("UIDocumentBlur: DualKawaseBlur shader not found! Assign it in the inspector.");
        }

        var gShader = gaussianShader ? gaussianShader : Shader.Find("Hidden/GaussianBlur");
        if (gShader)
        {
            _gaussianMaterial = new Material(gShader)
            {
                hideFlags = HideFlags.HideAndDontSave
            };
        }
        else
        {
            Debug.LogError("UIDocumentBlur:  GaussianBlur shader not found! Assign it in the inspector.");
        }
    }

    private RenderTexture CreateRT(int width, int height, int depth = 0)
    {
        var rt = new RenderTexture(width, height, depth, RenderTextureFormat.ARGB32)
        {
            wrapMode = TextureWrapMode.Clamp,
            filterMode = FilterMode.Bilinear
        };
        rt.Create();
        return rt;
    }

    private void Start()
    {
        if (!targetCamera)
            targetCamera = Camera.main;

        StartCoroutine(Initialize());
    }

    private System.Collections.IEnumerator Initialize()
    {
        yield return null;
        yield return null;
        yield return null;

        if (!_uiDocument)
        {
            Debug.LogError("UIDocumentBlur: UIDocument not found!");
            yield break;
        }

        if (!targetCamera)
        {
            Debug.LogError("UIDocumentBlur: No camera found!");
            yield break;
        }

        switch (blurType)
        {
            // Validate materials
            case BlurType.Kawase when !_kawaseMaterial:
                Debug.LogError("UIDocumentBlur:  Kawase material is null!  Check shader assignment.");
                yield break;
            case BlurType.DualKawase when !_dualKawaseMaterial:
                Debug.LogError("UIDocumentBlur: DualKawase material is null! Check shader assignment.");
                yield break;
            case BlurType.Gaussian when !_gaussianMaterial:
                Debug.LogError("UIDocumentBlur:  Gaussian material is null! Check shader assignment.");
                yield break;
            default:
                _needsRefresh = true;
                break;
        }
    }

    private void LateUpdate()
    {
        if (_needsRefresh)
        {
            _needsRefresh = false;
            FindAndSetupBlurPanels();
            _isInitialized = _panels.Count > 0;
            return;
        }

        if (!_isInitialized) return;

        _frameCounter++;
        if (_frameCounter % updateInterval != 0) return;

        UpdateBlur();
    }

    private void FindAndSetupBlurPanels()
    {
        foreach (var panel in _panels)
        {
            if (panel.CroppedTexture)
                Destroy(panel.CroppedTexture);

            if (panel.Background is { parent: not null })
                panel.Background.RemoveFromHierarchy();
        }
        _panels.Clear();

        var root = _uiDocument.rootVisualElement;

        var blurElements = root.Query<VisualElement>(className: blurClassName).ToList();
        foreach (var element in blurElements)
        {
            SetupBlurPanel(element, false);
        }
    }

    private void SetupBlurForChildren(VisualElement parent)
    {
        foreach (var child in parent.Children())
        {
            if (child.ClassListContains(blurClassName))
                continue;

            if (child.name == "blur-background-auto")
                continue;

            SetupBlurPanel(child, true);
        }
    }

    private void SetupBlurPanel(VisualElement panel, bool isChildBlur)
    {
        var existingBg = panel.Q<VisualElement>("blur-background-auto");
        existingBg?.RemoveFromHierarchy();

        var background = new VisualElement
        {
            name = "blur-background-auto",
            pickingMode = PickingMode.Ignore,
            style =
            {
                position = Position.Absolute,
                left = 0,
                top = 0,
                right = 0,
                bottom = 0,
                backgroundPositionX = BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(),
                backgroundPositionY = BackgroundPropertyHelper.ConvertScaleModeToBackgroundPosition(),
                backgroundRepeat = BackgroundPropertyHelper.ConvertScaleModeToBackgroundRepeat(),
                backgroundSize = BackgroundPropertyHelper.ConvertScaleModeToBackgroundSize()
            }
        };

        panel.Insert(0, background);
        panel.style.overflow = Overflow.Hidden;

        _panels.Add(new BlurPanelData
        {
            Panel = panel,
            Background = background,
            CroppedTexture = null
        });
    }

    private void UpdateBlur()
    {
        var anyVisible = Enumerable.Any(_panels, panelData => IsPanelVisible(panelData.Panel));

        if (onlyUpdateWhenVisible && !anyVisible) return;

        var fullWidth = targetCamera.pixelWidth;
        var fullHeight = targetCamera.pixelHeight;

        if (fullWidth <= 0 || fullHeight <= 0) return;

        var sourceRT = CaptureCameraOnly(fullWidth, fullHeight);

        var blurWidth = Mathf.Max(1, fullWidth / downsample);
        var blurHeight = Mathf.Max(1, fullHeight / downsample);
        EnsureRenderTexture(ref _blurredFullRT, blurWidth, blurHeight, 0);

        ApplyBlur(sourceRT, blurWidth, blurHeight);
        CropBlurToAllPanels(fullWidth, fullHeight);
    }

    private RenderTexture CaptureCameraOnly(int width, int height)
    {
        EnsureRenderTexture(ref _cameraRT, width, height, 24);

        var originalTarget = targetCamera.targetTexture;
        targetCamera.targetTexture = _cameraRT;
        targetCamera.Render();
        targetCamera.targetTexture = originalTarget;

        return _cameraRT;
    }

    private void EnsureRenderTexture(ref RenderTexture rt, int width, int height, int depth)
    {
        if (rt && rt.width == width && rt.height == height) return;
        if (rt)
        {
            rt.Release();
            Destroy(rt);
        }
        rt = CreateRT(width, height, depth);
    }

    private static bool IsPanelVisible(VisualElement panel)
    {
        if (panel == null) return false;
        if (panel.resolvedStyle.display == DisplayStyle.None) return false;
        if (panel.resolvedStyle.visibility == Visibility.Hidden) return false;
        if (panel.resolvedStyle.opacity < 0.01f) return false;

        var parent = panel.parent;
        while (parent != null)
        {
            if (parent.resolvedStyle.display == DisplayStyle.None) return false;
            if (parent.resolvedStyle.visibility == Visibility.Hidden) return false;
            parent = parent.parent;
        }

        return true;
    }

    private void ApplyBlur(RenderTexture source, int blurWidth, int blurHeight)
    {
        switch (blurType)
        {
            case BlurType.Kawase:
                ApplyKawaseBlur(source, blurWidth, blurHeight);
                break;
            case BlurType.DualKawase:
                ApplyDualKawaseBlur(source, blurWidth, blurHeight);
                break;
            case BlurType.Gaussian:
                ApplyGaussianBlur(source, blurWidth, blurHeight);
                break;
        }
    }

    private void CropBlurToAllPanels(int screenWidth, int screenHeight)
    {
        foreach (var panelData in _panels)
        {
            if (!IsPanelVisible(panelData.Panel)) continue;

            var panelRect = panelData.Panel.worldBound;

            if (float.IsNaN(panelRect.x) || float.IsNaN(panelRect.y) ||
                float.IsNaN(panelRect.width) || float.IsNaN(panelRect.height))
                continue;

            if (panelRect.width <= 0 || panelRect.height <= 0) continue;

            CropBlurToPanel(panelData, panelRect, screenWidth, screenHeight);
        }
    }

    private void ApplyKawaseBlur(RenderTexture source, int blurWidth, int blurHeight)
    {
        if (!_kawaseMaterial) return;

        var temp1 = RenderTexture.GetTemporary(blurWidth, blurHeight, 0, RenderTextureFormat.ARGB32);
        var temp2 = RenderTexture.GetTemporary(blurWidth, blurHeight, 0, RenderTextureFormat.ARGB32);
        temp1.wrapMode = TextureWrapMode.Clamp;
        temp2.wrapMode = TextureWrapMode.Clamp;

        Graphics.Blit(source, temp1);

        var currentSource = temp1;
        var currentDest = temp2;

        for (var i = 0; i < kawaseIterations; i++)
        {
            _kawaseMaterial.SetFloat(OffsetId, i + 0.5f);
            Graphics.Blit(currentSource, currentDest, _kawaseMaterial);
            (currentSource, currentDest) = (currentDest, currentSource);
        }

        Graphics.Blit(currentSource, _blurredFullRT);

        RenderTexture.ReleaseTemporary(temp1);
        RenderTexture.ReleaseTemporary(temp2);
    }

    private void ApplyDualKawaseBlur(RenderTexture source, int blurWidth, int blurHeight)
    {
        if (!_dualKawaseMaterial) return;

        var width = blurWidth;
        var height = blurHeight;

        var currentSource = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
        currentSource.wrapMode = TextureWrapMode.Clamp;
        currentSource.filterMode = FilterMode.Bilinear;

        Graphics.Blit(source, currentSource);

        var pyramid = new RenderTexture[dualKawasePasses];

        for (var i = 0; i < dualKawasePasses; i++)
        {
            width = Mathf.Max(1, width / 2);
            height = Mathf.Max(1, height / 2);

            pyramid[i] = RenderTexture.GetTemporary(width, height, 0, RenderTextureFormat.ARGB32);
            pyramid[i].wrapMode = TextureWrapMode.Clamp;
            pyramid[i].filterMode = FilterMode.Bilinear;

            _dualKawaseMaterial.SetFloat(OffsetId, 1f);
            Graphics.Blit(currentSource, pyramid[i], _dualKawaseMaterial, 0);

            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = pyramid[i];
        }

        for (var i = dualKawasePasses - 2; i >= 0; i--)
        {
            var dest = RenderTexture.GetTemporary(
                pyramid[i].width, pyramid[i].height, 0, RenderTextureFormat.ARGB32);
            dest.wrapMode = TextureWrapMode.Clamp;
            dest.filterMode = FilterMode.Bilinear;

            _dualKawaseMaterial.SetFloat(OffsetId, 0.5f);
            Graphics.Blit(currentSource, dest, _dualKawaseMaterial, 1);

            RenderTexture.ReleaseTemporary(currentSource);
            currentSource = dest;
        }

        Graphics.Blit(currentSource, _blurredFullRT);
        RenderTexture.ReleaseTemporary(currentSource);
    }

    private void ApplyGaussianBlur(RenderTexture source, int blurWidth, int blurHeight)
    {
        if (!_gaussianMaterial) return;

        var temp1 = RenderTexture.GetTemporary(blurWidth, blurHeight, 0, RenderTextureFormat.ARGB32);
        var temp2 = RenderTexture.GetTemporary(blurWidth, blurHeight, 0, RenderTextureFormat.ARGB32);
        temp1.wrapMode = TextureWrapMode.Clamp;
        temp2.wrapMode = TextureWrapMode.Clamp;

        Graphics.Blit(source, temp1);

        _gaussianMaterial.SetFloat(BlurSizeId, gaussianBlurSize);

        var currentSource = temp1;
        var currentDest = temp2;

        for (var i = 0; i < gaussianIterations; i++)
        {
            Graphics.Blit(currentSource, currentDest, _gaussianMaterial, 0);
            (currentSource, currentDest) = (currentDest, currentSource);

            Graphics.Blit(currentSource, currentDest, _gaussianMaterial, 1);
            (currentSource, currentDest) = (currentDest, currentSource);
        }

        Graphics.Blit(currentSource, _blurredFullRT);

        RenderTexture.ReleaseTemporary(temp1);
        RenderTexture.ReleaseTemporary(temp2);
    }

    private void CropBlurToPanel(BlurPanelData panelData, Rect panelRect, int screenWidth, int screenHeight)
    {
        if (screenWidth <= 0 || screenHeight <= 0) return;
        if (!_blurredFullRT) return;

        var panelX = Mathf.Max(0, panelRect.x);
        var panelY = Mathf.Max(0, panelRect.y);
        var panelRight = Mathf.Min(screenWidth, panelRect.xMax);
        var panelBottom = Mathf.Min(screenHeight, panelRect.yMax);

        var panelWidth = panelRight - panelX;
        var panelHeight = panelBottom - panelY;

        if (panelWidth <= 0 || panelHeight <= 0) return;

        var cropWidth = Mathf.Clamp(Mathf.RoundToInt(panelWidth / downsample), 1, _blurredFullRT.width);
        var cropHeight = Mathf.Clamp(Mathf.RoundToInt(panelHeight / downsample), 1, _blurredFullRT.height);

        if (!panelData.CroppedTexture ||
            panelData.CroppedTexture.width != cropWidth ||
            panelData.CroppedTexture.height != cropHeight)
        {
            if (panelData.CroppedTexture)
                Destroy(panelData.CroppedTexture);

            panelData.CroppedTexture = new Texture2D(cropWidth, cropHeight, TextureFormat.RGBA32, false)
                {
                    filterMode = FilterMode.Bilinear,
                    wrapMode = TextureWrapMode.Clamp
                };
        }

        var uvX = panelX / screenWidth;
        var uvY = panelY / screenHeight;
        var uvYFlipped = 1f - uvY - (panelHeight / screenHeight);

        var srcX = Mathf.FloorToInt(uvX * _blurredFullRT.width);
        var srcY = Mathf.FloorToInt(uvYFlipped * _blurredFullRT.height);

        srcX = Mathf.Clamp(srcX, 0, Mathf.Max(0, _blurredFullRT.width - cropWidth));
        srcY = Mathf.Clamp(srcY, 0, Mathf.Max(0, _blurredFullRT.height - cropHeight));

        if (srcX < 0 || srcY < 0 ||
            srcX + cropWidth > _blurredFullRT.width ||
            srcY + cropHeight > _blurredFullRT.height)
        {
            return;
        }

        var previous = RenderTexture.active;
        RenderTexture.active = _blurredFullRT;

        try
        {
            panelData.CroppedTexture.ReadPixels(new Rect(srcX, srcY, cropWidth, cropHeight), 0, 0, false);
            panelData.CroppedTexture.Apply(false);
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"UIDocumentBlur: ReadPixels failed - {e.Message}");
        }
        finally
        {
            RenderTexture.active = previous;
        }

        panelData.Background.style.backgroundImage = new StyleBackground(panelData.CroppedTexture);
    }

    public void RefreshPanels()
    {
        _needsRefresh = true;
    }

    private void OnDestroy()
    {
        if (_cameraRT) { _cameraRT.Release(); Destroy(_cameraRT); }
        if (_blurredFullRT) { _blurredFullRT.Release(); Destroy(_blurredFullRT); }
        if (_kawaseMaterial) Destroy(_kawaseMaterial);
        if (_dualKawaseMaterial) Destroy(_dualKawaseMaterial);
        if (_gaussianMaterial) Destroy(_gaussianMaterial);

        foreach (var panel in _panels.Where(panel => panel.CroppedTexture))
        {
            Destroy(panel.CroppedTexture);
        }
    }
}