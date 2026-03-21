using BepInEx;
using Fungus;
using HarmonyLib;
using SkySwordKill.Next.DialogSystem;
using Spine.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;


namespace top.Isteyft.MCS.NewIndex
{
    [BepInDependency("skyswordkill.plugin.Next")]
    [BepInPlugin("top.Isteyft.MCS.NewIndex", "地标", "1.0.0")]
    public class Main: BaseUnityPlugin
    {
        public static Main I { get; private set; }
        private void Awake()
        {
            new Harmony("top.Isteyft.MCS.NewIndex").PatchAll();
            Main.I.Logger.LogInfo("秘境加载成功");
        }
    }

    [HarmonyPatch(typeof(AllMapShowLine), "Start")]
    public class AllMapShowLinePatch
    {
        [HarmonyPostfix]
        private static void AddExtraNodes(AllMapShowLine __instance)
        {
            Task task = __instance.gameObject.AddComponent<Task>();
            Action action = delegate ()
            {
                if (MapNodeManager.inst.transform.Find("LevelsWorld0/680") == null)
                {
                    GameObject gameObject = MapNodeManager.inst.transform.Find("LevelsWorld0/13").gameObject;
                    GameObject gameObject2 = UnityEngine.Object.Instantiate<GameObject>(gameObject);
                    gameObject2.name = "680";
                    gameObject2.transform.parent = MapNodeManager.inst.transform.Find("LevelsWorld0");
                    //gameObject2.transform.localPosition = new Vector3(5.17f, 0.20f, 0f);
                    gameObject2.transform.localPosition = new Vector3(-6.06f, -5.83f, 0f);
                    gameObject2.gameObject.SetActive(true);
                    gameObject2.GetComponent<MapComponent>().gameObject.SetActive(true);
                    List<MapComponent> value = Traverse.Create(__instance).Field("maps").GetValue<List<MapComponent>>();
                    UnityEngine.Object.Destroy(gameObject2.gameObject.GetComponent<AllMapNodeClick>());
                    gameObject2.transform.Find("ImageName/Canvas/Text").GetComponent<Text>().text = "秘境";
                    gameObject2.transform.Find("Level1Move").GetComponent<SpriteRenderer>().sprite = ResManager.inst.LoadSprite("NewUI/BG/16801");
                    gameObject2.transform.Find("Level1Move").localScale = new Vector3(1f, 1f, 0f);
                    gameObject2.transform.Find("Level1Move").localPosition = new Vector3(0.73f, -1.12f, 0f);
                    gameObject2.transform.Find("flowchat/enter/Canvas/Button").GetComponent<Button>().onClick.AddListener(delegate ()
                    {
                        DialogAnalysis.StartTestDialogEvent("HH_LoadScenes*S16801", null);
                    });
                    Block[] componentsInChildren = gameObject2.transform.Find("flowchat").GetComponentsInChildren<Block>();
                    componentsInChildren[0]._EventHandler = null;
                    componentsInChildren[0].commandList = null;
                    gameObject2.GetComponent<MapComponent>().NodeIndex = 680;
                    gameObject2.GetComponent<MapComponent>().NodeGroup = 680;
                    value.Add(gameObject2.GetComponent<MapComponent>());
                }
            };
            task.TaskInvoke(action, 1f);
        }
    }

    public class Task : MonoBehaviour
    {
        public Action action;

        public SkeletonAnimation skeletonAnimation;

        public void SetSpine(SkeletonAnimation skeletonAnimation, float time)
        {
            this.skeletonAnimation = skeletonAnimation;
            Invoke("SetSpine", time);
        }

        private void SetSpine()
        {
            ((SkeletonRenderer)skeletonAnimation).Initialize(true, false);
            ((Component)(object)skeletonAnimation).gameObject.SetActive(value: true);
        }

        private void StartInvoke()
        {
            action?.Invoke();
            UnityEngine.Object.Destroy(this);
        }

        public void TaskInvoke(Action action, float time)
        {
            this.action = action;
            Invoke("StartInvoke", time);
        }
    }

    [HarmonyPatch(typeof(BaseMapCompont), "Awake")]
    public class BaseMapCompontPatch
    {
        [HarmonyPrefix]
        public static bool BaseMapCompont_Patch(BaseMapCompont __instance)
        {
            int.TryParse(__instance.gameObject.name, out __instance.NodeIndex);
            __instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
            __instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            return false;

            //if (__instance.gameObject.name == "19(Clone)")
            //{
            //    __instance.NodeIndex = 16969;
            //    __instance.AllMapCastTimeJsonData = jsonData.instance.AllMapCastTimeJsonData;
            //    __instance.MapRandomJsonData = jsonData.instance.MapRandomJsonData;
            //    __instance.MapPositon = new Vector2(-1.5f, 13.5f);
            //    Main.Log("已新增九洲府");
            //    MapComponent mapComponent = __instance.gameObject.AddMissingComponent<MapComponent>();
            //    returnfalse;
            //}
            //return true;
        }
    }
}
