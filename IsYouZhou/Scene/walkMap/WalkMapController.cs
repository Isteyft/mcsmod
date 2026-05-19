using SkySwordKill.Next.DialogSystem;
using UnityEngine;

namespace top.Isteyft.MCS.JiuZhou.Scene.walkMap
{
    /// <summary>
    /// WalkMap模式：WASD移动玩家，并根据距离显示入口点。
    /// </summary>
    public class WalkMapController : MonoBehaviour
    {
        private const float MoveSpeed = 3.5f;
        private const float RevealDistance = 0.9f;
        private const float MoveEdgePadding = 0.8f;

        private bool initialized;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private Camera mainCamera;
        private CamaraFollow camaraFollow;

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

            if (camaraFollow != null && mainCamera != null && AllMapManage.instance?.MapPlayerController != null)
            {
                if (!Input.GetMouseButton(0))
                {
                    Vector3 playerPos = AllMapManage.instance.MapPlayerController.transform.position;
                    Vector3 camPos = mainCamera.transform.position;
                    camPos.x = playerPos.x;
                    camPos.y = playerPos.y;
                    Vector2 limited = camaraFollow.LimitPos(new Vector2(camPos.x, camPos.y));
                    camPos.x = limited.x;
                    camPos.y = limited.y;
                    mainCamera.transform.position = camPos;
                }
            }
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

            if (mainCamera == null)
            {
                return;
            }

            camaraFollow = mainCamera.GetComponent<CamaraFollow>();
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
            if (AllMapManage.instance.MapPlayerController.ShowType != MapPlayerShowType.遁术) return;
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

