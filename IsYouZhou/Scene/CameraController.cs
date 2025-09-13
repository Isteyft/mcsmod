using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 3f;
    [SerializeField] private float minZoom = 0.5f;
    [SerializeField] private float maxZoom = 6f;

    [Header("Drag Settings")]
    [SerializeField] private float dragSpeed = 1f;
    [SerializeField] private Vector2 xBounds = new Vector2(-12.6f, 12.8f); // 固定X轴边界
    [SerializeField] private Vector2 yBounds = new Vector2(-6.8f, 6.8f); // 固定Y轴边界

    private Camera _camera;
    private Vector3 _dragOrigin;
    private bool _isDragging;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        HandleZoom();
        HandleDrag();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            _camera.orthographicSize -= scroll * zoomSpeed;
            _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize, minZoom, maxZoom);

            // 缩放后立即修正位置，确保不会超出边界
            ClampCameraPosition();
        }
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(2))
        {
            _dragOrigin = _camera.ScreenToWorldPoint(Input.mousePosition);
            _isDragging = true;
        }

        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(2))
        {
            _isDragging = false;
        }

        if (_isDragging)
        {
            Vector3 currentPos = _camera.ScreenToWorldPoint(Input.mousePosition);
            Vector3 difference = _dragOrigin - currentPos;

            // 直接应用位移，不限制（会在ClampCameraPosition中处理）
            transform.position += difference * dragSpeed;

            // 立即修正位置确保不超出边界
            ClampCameraPosition();
        }
    }

    private void ClampCameraPosition()
    {
        // 计算相机视野范围
        float orthoSize = _camera.orthographicSize;
        float aspect = _camera.aspect;
        float cameraWidth = orthoSize * aspect;
        float cameraHeight = orthoSize;

        // 计算实际允许的相机位置范围
        float minX = xBounds.x + cameraWidth;
        float maxX = xBounds.y - cameraWidth;
        float minY = yBounds.x + cameraHeight;
        float maxY = yBounds.y - cameraHeight;

        // 确保边界有效（当相机视野大于地图时）
        minX = Mathf.Min(minX, maxX);
        minY = Mathf.Min(minY, maxY);

        // 限制相机位置
        Vector3 clampedPosition = transform.position;
        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 center = new Vector3(
            (xBounds.x + xBounds.y) / 2,
            (yBounds.x + yBounds.y) / 2,
            0
        );
        Vector3 size = new Vector3(
            xBounds.y - xBounds.x,
            yBounds.y - yBounds.x,
            0.1f
        );
        Gizmos.DrawWireCube(center, size);
    }
}