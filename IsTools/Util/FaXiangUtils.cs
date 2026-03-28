using SkySwordKill.NextMoreCommand.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Util
{
    public class FaXiangUtils
    {
        // 获取玩家或者npc的具体位置
        public static UnityEngine.GameObject GetAvatarSklPosition(bool isPlayer)
        {
            UnityEngine.GameObject avatar10;
            if (isPlayer) {
                avatar10 = UnityEngine.GameObject.Find("Avatar_10");
            }
            else
            {
                avatar10 = UnityEngine.GameObject.Find("Avatar_11");
            }

            if (avatar10 == null)
            {
                IsToolsMain.Error("不在战斗");
                return null;
            }
            return avatar10;
        }
        // 获取玩家骨骼
        public static UnityEngine.GameObject GetAvatarSkl(bool isPlayer)
        {
            UnityEngine.GameObject avatar10 = GetAvatarSklPosition(isPlayer);

            UnityEngine.GameObject nowPlayer = avatar10.transform.Find("Avater50_1(Clone)")?.gameObject
                ?? avatar10.transform.Find("Avater51_1(Clone)")?.gameObject;
            if (nowPlayer == null)
            {
                IsToolsMain.Error("没找到玩家或者npc骨骼所在位置，或npc、玩家不为普通玩家模型");
                return null;
            }
            return nowPlayer;
        }
        // 获取原版预制体
        public static UnityEngine.GameObject GetFaXiangPrefab(int i)
        {
            string path = $"Effect/Prefab/gameEntity/Avater/Avater{i}/Avater{i}_1";
            UnityEngine.GameObject prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>(path);
            if (prefab == null)
            {
                // 获取npc性别
                var npcSex = i.ToNpcNewId().NPCJson()["SexType"].I;
                // 骨骼模型
                int num = (npcSex == 1) ? 50 : 51;
                // 载入npc预制体
                prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>(string.Format($"Effect/Prefab/gameEntity/Avater/Avater{num}/Avater{num}_1"));
            }
            return prefab;
        }
        //判断id是否有预制体，没有就为普通npc
        public static bool isAvatarSkl(int i)
        {
            string path = $"Effect/Prefab/gameEntity/Avater/Avater{i}/Avater{i}_1";
            UnityEngine.GameObject prefab = UnityEngine.Resources.Load<UnityEngine.GameObject>(path);
            if (prefab == null)
            {
                return true;
            }
            return false;
        }
        // 切换到玩家
        public static void usePlayer(bool isPlayer = true)
        {
            GetAvatarSkl(isPlayer).transform.SetAsFirstSibling();
        }
        // 位置初始化
        public static void InitFaXiangPosition(UnityEngine.GameObject avatarInstance, bool isPlayer, int mode = 0)
        {
            UnityEngine.GameObject nowPlayer = GetAvatarSkl(isPlayer);
            // 设置父元素位置
            avatarInstance.transform.SetParent(GetAvatarSklPosition(isPlayer).transform);
            // 先复制目标位置
            avatarInstance.transform.position = nowPlayer.transform.position;
            if (mode == 0)
            {
                if (isPlayer)
                {
                    //avatarInstance.transform.position = new UnityEngine.Vector3(
                    //    nowPlayer.transform.position.x - 1,
                    //    nowPlayer.transform.position.y + 0.5f,
                    //    nowPlayer.transform.position.z
                    //);
                    avatarInstance.transform.localPosition = new UnityEngine.Vector3(
                        0f,
                        0.75f,
                        -1.5f
                    );
                }
                else
                {
                    avatarInstance.transform.localPosition = new UnityEngine.Vector3(
                        3f,
                        0.75f,
                        -1.5f
                    );
                    avatarInstance.transform.rotation = UnityEngine.Quaternion.Euler(
                        nowPlayer.transform.rotation.eulerAngles.x,
                        nowPlayer.transform.rotation.eulerAngles.y,
                        nowPlayer.transform.rotation.eulerAngles.z
                    );
                }
            } 
            else
            {
                if (isPlayer)
                {
                    avatarInstance.transform.localPosition = new UnityEngine.Vector3(
                        1.5f,
                        -0.75f,
                        3f
                    );
                }
                else
                {
                    avatarInstance.transform.localPosition = new UnityEngine.Vector3(
                        -1.5f,
                        -0.75f,
                        3f
                    );
                    avatarInstance.transform.rotation = UnityEngine.Quaternion.Euler(
                        nowPlayer.transform.rotation.eulerAngles.x,
                        nowPlayer.transform.rotation.eulerAngles.y,
                        nowPlayer.transform.rotation.eulerAngles.z
                    );
                }
                avatarInstance.transform.localScale = new UnityEngine.Vector3(0.35f, 0.35f, 0.35f);
            }
        }
        // 对于玩家模型特殊处理
        public static void InitAvatarSklFace(UnityEngine.GameObject avatarInstance, int i, int mode = 0)
        {
            try
            {
                UnityEngine.Transform spineTransform = avatarInstance.transform.Find("Spine GameObject (hero-pro)");

                if (spineTransform != null)
                {
                    PlayerSetRandomFace playerFaceComponent = spineTransform.GetComponentInChildren<PlayerSetRandomFace>(true);
                    if (playerFaceComponent != null)
                    {

                        playerFaceComponent.randomAvatar(i);
                    }
                    else
                    {
                        IsToolsMain.Error("PlayerSetRandomFace没有找到");
                    }
                }
                else
                {
                    IsToolsMain.Error("hero-pro没有找到");
                }
                if (mode == 1) avatarInstance.transform.localScale = new UnityEngine.Vector3(1.0f, 1.0f, 1.0f);
            }
            catch (Exception e)
            {
                IsToolsMain.Error(e);
            }
        }

        /// <summary>
        /// 移动指定GameObject的位置
        /// </summary>
        /// <param name="target">需要移动的游戏物体</param>
        /// <param name="x">X轴偏移量 (正值向右)</param>
        /// <param name="y">Y轴偏移量 (正值向上)</param>
        /// <param name="z">Z轴偏移量 (正值向前/远离摄像机)</param>
        public static void MoveGameObjectPosition(UnityEngine.GameObject target, float x, float y, float z)
        {
            if (target == null)
            {
                IsToolsMain.Error("目标GameObject为空，无法移动位置");
                return;
            }

            // 获取当前位置
            UnityEngine.Vector3 currentPos = target.transform.position;

            // 计算新位置
            UnityEngine.Vector3 newPos = new UnityEngine.Vector3(
                currentPos.x + x,
                currentPos.y + y,
                currentPos.z + z
            );

            // 应用新位置
            target.transform.position = newPos;
        }

        /// <summary>
        /// 更新指定GameObject的缩放大小
        /// </summary>
        /// <param name="target">需要更新的游戏物体</param>
        /// <param name="scale">统一缩放值 (例如 1.0f 为原大, 0.5f 为一半)</param>
        public static void UpdateGameObjectScale(UnityEngine.GameObject target, float scale)
        {
            if (target == null)
            {
                IsToolsMain.Error("目标GameObject为空，无法更新大小");
                return;
            }

            UnityEngine.Vector3 newScale = new UnityEngine.Vector3(scale, scale, scale);
            target.transform.localScale = newScale;
        }

        /// <summary>
        /// 更新指定GameObject的旋转角度
        /// </summary>
        /// <param name="target">需要更新的游戏物体</param>
        /// <param name="x">X轴角度 ( pitch / 俯仰 )</param>
        /// <param name="y">Y轴角度 ( yaw / 偏航 )</param>
        /// <param name="z">Z轴角度 ( roll / 翻滚 )</param>
        public static void UpdateGameObjectRotation(UnityEngine.GameObject target, float x, float y, float z)
        {
            if (target == null)
            {
                IsToolsMain.Error("目标GameObject为空，无法更新旋转");
                return;
            }

            UnityEngine.Vector3 newRotation = new UnityEngine.Vector3(x, y, z);
            target.transform.rotation = UnityEngine.Quaternion.Euler(newRotation);
        }
    }
}
