using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            string name = scene.name;
            if (name == "F幽州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
                AllMapBase.RefreshMarksFromStaticData();
            }
            else if (name == "F雪剑域")  // 添加新的地图处理逻辑
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
                AllMapBase.RefreshMarksFromStaticData();
            }
        }

    }
}
