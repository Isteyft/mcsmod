using SkySwordKill.Next.DialogSystem;
using UnityEngine;

namespace top.Isteyft.MCS.YouZhou.Scene.walkMap
{
    /// <summary>
    /// WalkMap模式：WASD移动玩家，并根据距离显示入口点。
    /// </summary>
    public class WalkMapController : MonoBehaviour
    {
        private const float MoveSpeed = 3.5f;
        private const float RevealDistance = 0.9f;
        private const float MoveEdgePadding = 0.8f;
        private const float CameraEdgePadding = 1.2f;
        private const float MinCameraOrthographicSize = 2.5f;

        private bool initialized;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private Bounds mapLandBounds;
        private bool hasMapLandBounds;
        private Camera mainCamera;

        private void Update()
        {
            if (!EnsureInitialized())
            {
                return;
            }

            if (!DialogAnalysis.IsRunningEvent && !AllMapManage.instance.isPlayMove)
            {
                MoveByKeyboard();
            }
            else
            {
                AllMapManage.instance.MapPlayerController.SetSpeed(0);
            }

            RefreshEnterVisibilityAndNodeIndex();
        }

        private void LateUpdate()
        {
            if (!initialized)
            {
                return;
            }

            ClampCameraToMapLand();
        }

        private bool EnsureInitialized()
        {
            if (initialized)
            {
                return true;
            }

            if (AllMapManage.instance == null || AllMapManage.instance.mapIndex == null || AllMapManage.instance.MapPlayerController == null)
            {
                return false;
            }

            if (AllMapManage.instance.mapIndex.Count == 0)
            {
                return false;
            }

            BuildBounds();
            SetupCamera();
            HideAllEnters();
            DisableNodeClickMove();
            initialized = true;
            return true;
        }

        private void BuildBounds()
        {
            if (BuildBoundsFromMapLand())
            {
                return;
            }

            bool hasValue = false;
            foreach (BaseMapCompont mapComp in AllMapManage.instance.mapIndex.Values)
            {
                Vector3 pos = mapComp.transform.position;
                if (!hasValue)
                {
                    minX = pos.x;
                    maxX = pos.x;
                    minY = pos.y;
                    maxY = pos.y;
                    hasValue = true;
                    continue;
                }

                if (pos.x < minX) minX = pos.x;
                if (pos.x > maxX) maxX = pos.x;
                if (pos.y < minY) minY = pos.y;
                if (pos.y > maxY) maxY = pos.y;
            }

            minX -= MoveEdgePadding;
            maxX += MoveEdgePadding;
            minY -= MoveEdgePadding;
            maxY += MoveEdgePadding;
        }

        private bool BuildBoundsFromMapLand()
        {
            Transform mapLand = transform.Find("/AllMap/MapLand");
            if (mapLand == null)
            {
                return false;
            }

            Renderer renderer = mapLand.GetComponent<Renderer>();
            if (renderer != null)
            {
                Bounds b = renderer.bounds;
                mapLandBounds = b;
                hasMapLandBounds = true;
                minX = b.min.x + MoveEdgePadding;
                maxX = b.max.x - MoveEdgePadding;
                minY = b.min.y + MoveEdgePadding;
                maxY = b.max.y - MoveEdgePadding;
                return true;
            }

            Collider2D collider2D = mapLand.GetComponent<Collider2D>();
            if (collider2D != null)
            {
                Bounds b = collider2D.bounds;
                mapLandBounds = b;
                hasMapLandBounds = true;
                minX = b.min.x + MoveEdgePadding;
                maxX = b.max.x - MoveEdgePadding;
                minY = b.min.y + MoveEdgePadding;
                maxY = b.max.y - MoveEdgePadding;
                return true;
            }

            RectTransform rect = mapLand.GetComponent<RectTransform>();
            if (rect != null)
            {
                Vector3[] corners = new Vector3[4];
                rect.GetWorldCorners(corners);
                mapLandBounds = new Bounds((corners[0] + corners[2]) * 0.5f, corners[2] - corners[0]);
                hasMapLandBounds = true;
                minX = corners[0].x + MoveEdgePadding;
                maxX = corners[2].x - MoveEdgePadding;
                minY = corners[0].y + MoveEdgePadding;
                maxY = corners[2].y - MoveEdgePadding;
                return true;
            }

            return false;
        }

        private void SetupCamera()
        {
            mainCamera = Camera.main;
            if (mainCamera == null)
            {
                Transform cameraTransform = transform.Find("/Main Camera");
                if (cameraTransform != null)
                {
                    mainCamera = cameraTransform.GetComponent<Camera>();
                }
            }

            if (mainCamera == null || !mainCamera.orthographic || !hasMapLandBounds)
            {
                return;
            }

            float allowedHalfHeight = Mathf.Max(MinCameraOrthographicSize, mapLandBounds.extents.y - CameraEdgePadding);
            float allowedHalfWidthAsHeight = Mathf.Max(
                MinCameraOrthographicSize,
                (mapLandBounds.extents.x - CameraEdgePadding) / Mathf.Max(0.01f, mainCamera.aspect));

            float targetSize = Mathf.Min(allowedHalfHeight, allowedHalfWidthAsHeight);
            mainCamera.orthographicSize = Mathf.Max(MinCameraOrthographicSize, targetSize);
            ClampCameraToMapLand();
        }

        private void ClampCameraToMapLand()
        {
            if (mainCamera == null || !mainCamera.orthographic || !hasMapLandBounds)
            {
                return;
            }

            float cameraHalfHeight = mainCamera.orthographicSize;
            float cameraHalfWidth = cameraHalfHeight * mainCamera.aspect;

            float minCameraX = mapLandBounds.min.x + CameraEdgePadding + cameraHalfWidth;
            float maxCameraX = mapLandBounds.max.x - CameraEdgePadding - cameraHalfWidth;
            float minCameraY = mapLandBounds.min.y + CameraEdgePadding + cameraHalfHeight;
            float maxCameraY = mapLandBounds.max.y - CameraEdgePadding - cameraHalfHeight;

            if (minCameraX > maxCameraX)
            {
                float centerX = mapLandBounds.center.x;
                minCameraX = centerX;
                maxCameraX = centerX;
            }

            if (minCameraY > maxCameraY)
            {
                float centerY = mapLandBounds.center.y;
                minCameraY = centerY;
                maxCameraY = centerY;
            }

            Vector3 pos = mainCamera.transform.position;
            pos.x = Mathf.Clamp(pos.x, minCameraX, maxCameraX);
            pos.y = Mathf.Clamp(pos.y, minCameraY, maxCameraY);
            mainCamera.transform.position = pos;
        }

        private void HideAllEnters()
        {
            foreach (BaseMapCompont mapComp in AllMapManage.instance.mapIndex.Values)
            {
                GameObject enterObj = ResolveEnter(mapComp);
                if (enterObj != null)
                {
                    enterObj.SetActive(false);
                }
            }
        }

        private void DisableNodeClickMove()
        {
            foreach (BaseMapCompont mapComp in AllMapManage.instance.mapIndex.Values)
            {
                AllMapClick click = mapComp.GetComponent<AllMapClick>();
                if (click != null)
                {
                    click.enabled = false;
                }
            }
        }

        private void MoveByKeyboard()
        {
            float h = 0f;
            float v = 0f;

            if (Input.GetKey(KeyCode.A)) h -= 1f;
            if (Input.GetKey(KeyCode.D)) h += 1f;
            if (Input.GetKey(KeyCode.S)) v -= 1f;
            if (Input.GetKey(KeyCode.W)) v += 1f;

            Vector2 dir = new Vector2(h, v);
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            if (dir.sqrMagnitude < 0.001f)
            {
                playerController.SetSpeed(0);
                return;
            }

            dir.Normalize();
            Vector3 current = playerController.transform.position;
            Vector3 next = current + new Vector3(dir.x, dir.y, 0f) * MoveSpeed * Time.deltaTime;
            next.x = Mathf.Clamp(next.x, minX, maxX);
            next.y = Mathf.Clamp(next.y, minY, maxY);
            playerController.transform.position = next;
            playerController.SetSpeed(1);
        }

        private void RefreshEnterVisibilityAndNodeIndex()
        {
            MapPlayerController playerController = AllMapManage.instance.MapPlayerController;
            Vector3 playerPos = playerController.transform.position;
            AllMapComponent nearestComponent = null;
            float nearestDistance = float.MaxValue;

            foreach (BaseMapCompont mapComp in AllMapManage.instance.mapIndex.Values)
            {
                AllMapComponent component = mapComp as AllMapComponent;
                GameObject enterObj = ResolveEnter(mapComp);
                if (component == null || enterObj == null)
                {
                    continue;
                }

                float dis = Vector2.Distance(playerPos, component.transform.position);
                bool visible = dis <= RevealDistance;
                if (enterObj.activeSelf != visible)
                {
                    enterObj.SetActive(visible);
                }

                if (dis < nearestDistance)
                {
                    nearestDistance = dis;
                    nearestComponent = component;
                }
            }

            if (nearestComponent != null && nearestDistance <= RevealDistance)
            {
                nearestComponent.setAvatarNowMapIndex(nearestComponent.NodeIndex);
            }
        }

        private static GameObject ResolveEnter(BaseMapCompont mapComp)
        {
            AllMapComponent component = mapComp as AllMapComponent;
            if (component != null && component.enter != null)
            {
                return component.enter;
            }

            Transform enterTransform = mapComp.transform.Find("enter");
            if (enterTransform == null)
            {
                return null;
            }

            if (component != null)
            {
                component.enter = enterTransform.gameObject;
            }

            return enterTransform.gameObject;
        }
    }
}

