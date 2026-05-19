using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace top.Isteyft.MCS.JiuZhou.Scene.walkMap
{

    public class CamaraFollow : MonoBehaviour
    {
        public static CamaraFollow Inst;

        //private GameObject player;

        public Transform levo;

        public Transform desno;

        private Camera maincamera;

        private Vector3 firstMousePositon;

        private Vector3 firstCameraPositon;

        public bool follwPlayer;

        public static bool CanMove = true;

        private Dictionary<string, Func<bool>> BanMoveFunc = new Dictionary<string, Func<bool>>();

        private static float orthographicSize = 8f;

        public void Start()
        {
            Inst = this;
            maincamera = GetComponent<Camera>();
            //player = AllMapManage.instance.MapPlayerController.gameObject;
            Invoke("SetCameraToPlayer", 0.3f);
            //RegisterBanMove();
        }

        //public void RegisterBanMove()
        //{
        //    BanMoveFunc["UINPCLeftList"] = () => UINPCLeftList.Inst != null && UINPCLeftList.Inst.IsMouseInUI;
        //    BanMoveFunc["UINPCJiaoHu"] = () => UINPCJiaoHu.Inst.NowIsJiaoHu;
        //    BanMoveFunc["JiaoYiUIMag"] = () => JiaoYiUIMag.Inst != null;
        //}

        public void SetCameraToPlayer()
        {
            //base.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, base.transform.position.z);
        }

        private void Update()
        {
            CanMove = true;
            foreach (KeyValuePair<string, Func<bool>> item in BanMoveFunc)
            {
                if (item.Value != null && item.Value())
                {
                    CanMove = false;
                    break;
                }
            }

            if (!CanMove)
            {
                return;
            }

            if (Input.GetKeyUp(KeyCode.Space) || follwPlayer)
            {
                SetCameraToPlayer();
            }

            float num = levo.position.x + maincamera.orthographicSize * maincamera.aspect;
            float num2 = desno.position.x - maincamera.orthographicSize * maincamera.aspect;
            float min = levo.position.y + maincamera.orthographicSize;
            float max = desno.position.y - maincamera.orthographicSize;
            if (true)
            {
                if (SceneManager.GetActiveScene().name.StartsWith("Sea"))
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0f && maincamera.orthographicSize > 1f)
                    {
                        orthographicSize /= 1.03f;
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0f && maincamera.orthographicSize < 12f && maincamera.orthographicSize < num2 - num - 1f)
                    {
                        orthographicSize *= 1.03f;
                    }

                    maincamera.orthographicSize = orthographicSize;
                }
                else
                {
                    if (Input.GetAxis("Mouse ScrollWheel") > 0f && maincamera.orthographicSize > 1f)
                    {
                        maincamera.orthographicSize /= 1.03f;
                    }

                    if (Input.GetAxis("Mouse ScrollWheel") < 0f && maincamera.orthographicSize < 12f && maincamera.orthographicSize < num2 - num - 1f)
                    {
                        maincamera.orthographicSize *= 1.03f;
                    }
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                firstMousePositon = Input.mousePosition;
                firstCameraPositon = maincamera.transform.position;
            }

            if (Input.GetMouseButton(0))
            {
                float num3 = (Input.mousePosition.x - firstMousePositon.x) / (float)Screen.width * maincamera.aspect * maincamera.orthographicSize * 2f;
                float num4 = (Input.mousePosition.y - firstMousePositon.y) / (float)Screen.height * maincamera.orthographicSize * 2f;
                float x = firstCameraPositon.x - num3;
                float y = firstCameraPositon.y - num4;
                maincamera.transform.position = new Vector3(x, y, maincamera.transform.position.z);
            }

            float x2 = Mathf.Clamp(maincamera.transform.position.x, num, num2);
            float y2 = Mathf.Clamp(maincamera.transform.position.y, min, max);
            maincamera.transform.position = new Vector3(x2, y2, maincamera.transform.position.z);
        }

        public Vector2 LimitPos(Vector2 targetPos)
        {
            float min = levo.position.x + maincamera.orthographicSize * maincamera.aspect;
            float max = desno.position.x - maincamera.orthographicSize * maincamera.aspect;
            float min2 = levo.position.y + maincamera.orthographicSize;
            float max2 = desno.position.y - maincamera.orthographicSize;
            float x = Mathf.Clamp(targetPos.x, min, max);
            float y = Mathf.Clamp(targetPos.y, min2, max2);
            return new Vector2(x, y);
        }
    }
}