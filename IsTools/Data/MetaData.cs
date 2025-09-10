using Newtonsoft.Json.Linq;
using SkySwordKill.NextModEditor.Mod.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Data
{
    public class MetaData
    {
        public static Dictionary<int, ModSeidMeta> BuffSeidMetas { get; set; }
        public static Dictionary<int, ModSeidMeta> SkillSeidMetas { get; set; }
        public static Dictionary<int, ModSeidMeta> ItemEquipSeidMetas { get; set; }
        public static Dictionary<int, ModSeidMeta> ItemUseSeidMetas { get; set; }
        public static Dictionary<int, ModSeidMeta> StaticSkillSeidMeta { get; set; }

        public static List<ModBuffDataTriggerType> BuffTriggerType { get; set; }
        public static void Init()
        {
            BuffSeidMetas = JObject.Parse(File.ReadAllText(Jsonpath + "/Meta/BuffSeidMeta.json")).ToObject<Dictionary<int, ModSeidMeta>>();
            SkillSeidMetas = JObject.Parse(File.ReadAllText(Jsonpath + "/Meta/SkillSeidMeta.json")).ToObject<Dictionary<int, ModSeidMeta>>();
            //ItemEquipSeidMetas = JObject.Parse(File.ReadAllText(Jsonpath + "/Meta/ItemEquipSeidMeta.json")).ToObject<Dictionary<int, ModSeidMeta>>();
            ItemUseSeidMetas = JObject.Parse(File.ReadAllText(Jsonpath + "/Meta/ItemUseSeidMeta.json")).ToObject<Dictionary<int, ModSeidMeta>>();
            StaticSkillSeidMeta = JObject.Parse(File.ReadAllText(Jsonpath + "/Meta/StaticSkillSeidMeta.json")).ToObject<Dictionary<int, ModSeidMeta>>();
            BuffTriggerType = JArray.Parse(File.ReadAllText(Jsonpath + "/Meta/BuffTriggerType.json")).ToObject<List<ModBuffDataTriggerType>>();
        }
        public static string Jsonpath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
    }
}
