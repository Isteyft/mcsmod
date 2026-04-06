using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Scene.walkMap;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.Scene
{
    public class YZSceneManeger : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.sceneLoaded += this.SceneLoaded;
        }

        private static void CreateMapManagerForScene(UnityEngine.SceneManagement.Scene scene)
        {
            AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
            allMapBase.gameObject.AddComponent<SceneBase>();

            if (WalkMapModeConfig.IsWalkMapScene(scene.name))
            {
                allMapBase.gameObject.AddComponent<WalkMapController>();
            }

            SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            AllMapBase.RefreshMarksFromStaticData();
        }

        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            string name = scene.name;
            if (name == "F幽州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F雪剑域")  // 添加新的地图处理逻辑
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F中州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F衡州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F灞州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F颍州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F靖州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F渝州")
            {
                CreateMapManagerForScene(scene);
            }
            else if (name == "F雍州")
            {
                CreateMapManagerForScene(scene);
            }
        }

    }
}
