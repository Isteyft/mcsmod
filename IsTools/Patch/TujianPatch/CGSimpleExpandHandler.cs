using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CGSimpleExpandHandler : MonoBehaviour, IPointerClickHandler
{
    private RectTransform imageRectTransform;
    private GameObject expandedImageGO;
    private bool isExpanded = false;
    private Camera mainCamera;
    private float originalOrthographicSize;
    private static CGSimpleExpandHandler instance; // 单例管理，确保只有一个展开的图片

    void Start()
    {
        imageRectTransform = transform as RectTransform;

        // 获取主相机
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = GameObject.FindGameObjectWithTag("MainCamera")?.GetComponent<Camera>();
        }

        if (mainCamera != null)
        {
            originalOrthographicSize = mainCamera.orthographicSize;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isExpanded)
        {
            // 如果当前已经是展开状态，则收缩
            Collapse();
        }
        else
        {
            // 如果当前不是展开状态，则展开
            // 首先确保没有其他展开的图片
            if (instance != null)
            {
                instance.Collapse();
            }

            Expand();
            instance = this; // 设置为当前实例
        }
    }

    private void Expand()
    {
        // 创建顶层Canvas
        Canvas topLevelCanvas = GetOrCreateTopLevelCanvas();

        // 复制当前图片到顶层Canvas
        expandedImageGO = new GameObject("ExpandedCGImage");
        expandedImageGO.transform.SetParent(topLevelCanvas.transform, false);

        // 复制图像组件（根据实际的图像类型进行复制）
        CopyImageComponent(expandedImageGO);

        // 添加点击处理器以便销毁 - 注意这里添加的是一个特殊处理器
        var clickHandler = expandedImageGO.AddComponent<ExpandedImageClickHandler>();
        clickHandler.parentHandler = this; // 引用当前处理器
        expandedImageGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // 设置全屏显示
        var expandedRectTransform = expandedImageGO.GetComponent<RectTransform>();
        expandedRectTransform.anchorMin = Vector2.zero;
        expandedRectTransform.anchorMax = Vector2.one;
        expandedRectTransform.offsetMin = Vector2.zero;
        expandedRectTransform.offsetMax = Vector2.zero;

        // 调整相机
        if (mainCamera != null && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = 5.4f;
        }

        isExpanded = true;
    }

    public void Collapse()
    {
        if (expandedImageGO != null)
        {
            // 销毁展开的图片
            GameObject.Destroy(expandedImageGO);
            expandedImageGO = null;
        }

        // 恢复相机
        if (mainCamera != null && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = originalOrthographicSize;
        }

        isExpanded = false;
        if (instance == this)
        {
            instance = null; // 清除单例引用
        }
    }

    private void CopyImageComponent(GameObject targetGO)
    {
        // 复制当前对象上的图像组件
        var sourceImage = GetComponent<UnityEngine.UI.Image>();
        if (sourceImage != null)
        {
            var targetImage = targetGO.AddComponent<UnityEngine.UI.Image>();
            targetImage.sprite = sourceImage.sprite;
            targetImage.color = sourceImage.color;
            targetImage.material = sourceImage.material;
            targetImage.type = sourceImage.type;
            targetImage.preserveAspect = sourceImage.preserveAspect;
        }
        else
        {
            // 如果是其他类型的图像组件，如RawImage等，可以在这里添加
            var sourceRawImage = GetComponent<UnityEngine.UI.RawImage>();
            if (sourceRawImage != null)
            {
                var targetRawImage = targetGO.AddComponent<UnityEngine.UI.RawImage>();
                targetRawImage.texture = sourceRawImage.texture;
                targetRawImage.color = sourceRawImage.color;
                targetRawImage.uvRect = sourceRawImage.uvRect;
            }
        }
    }

    // 获取或创建顶层Canvas
    private Canvas GetOrCreateTopLevelCanvas()
    {
        Canvas[] allCanvases = GameObject.FindObjectsOfType<Canvas>();
        int maxSortOrder = -1;
        Canvas targetCanvas = null;

        foreach (Canvas canvas in allCanvases)
        {
            if (canvas.sortingOrder > maxSortOrder &&
                (canvas.transform.parent == null || !canvas.transform.parent.GetComponent<Canvas>()))
            {
                maxSortOrder = canvas.sortingOrder;
                targetCanvas = canvas;
            }
        }

        if (targetCanvas != null)
        {
            return targetCanvas;
        }

        // 如果没找到合适的Canvas，创建一个新的
        GameObject canvasGO = new GameObject("CGExpandCanvas");
        Canvas newCanvas = canvasGO.AddComponent<Canvas>();
        newCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        newCanvas.sortingOrder = 10000; // 设置一个非常高的排序值

        canvasGO.AddComponent<CanvasScaler>();
        canvasGO.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        return newCanvas;
    }
}

// 专门用于处理展开图片点击的类
public class ExpandedImageClickHandler : MonoBehaviour, IPointerClickHandler
{
    public CGSimpleExpandHandler parentHandler; // 引用父处理器

    public void OnPointerClick(PointerEventData eventData)
    {
        if (parentHandler != null)
        {
            parentHandler.Collapse(); // 调用父处理器的Collapse方法
        }
        else
        {
            // 如果没有父处理器，直接销毁自己
            GameObject.Destroy(gameObject);
        }
    }
}