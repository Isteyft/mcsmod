using MaiJiu.MCS.HH.Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace top.Isteyft.MCS.TianNan.Scene
{
    internal class SceneManeger : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.sceneLoaded += this.SceneLoaded;
        }
        public void SceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            string name = scene.name;
            Main.LogInfo($"当前场景为{name}");
            if (name == "F天南")
            {
                AllMapBase allMapBase = new UnityEngine.GameObject("Manager").AddComponent<AllMapBase>();
                allMapBase.gameObject.AddComponent<SceneBase>();
                SceneManager.MoveGameObjectToScene(allMapBase.gameObject, scene);
            }

        }
    }
}
