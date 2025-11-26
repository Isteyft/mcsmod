using MaiJiu.MCS.HH.Scene;
using SkySwordKill.Next.DialogSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using top.Isteyft.MCS.YouZhou.Scene;
using top.Isteyft.MCS.YouZhou.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace top.Isteyft.MCS.YouZhou.GamePlayer
{
    public class YZSceneManeger : MonoBehaviour
    {
        public void Start()
        {
            SceneManager.sceneLoaded += this.SceneLoaded;
        }
        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            IsToolsMain.LogInfo("现在的场景是" + scene.name);
            if (scene.name == "F幽州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F雪剑域")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F中州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F衡州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F灞州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F颍州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F靖州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F渝州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
            else if (scene.name == "F雍州")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }
        }
    }
}
