using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Data
{
    public class LingShouData
    {
        [JsonProperty("灵兽物品")]
        public int LingShouItem { get; set; }

        [JsonProperty("灵兽神通")]
        public int LingShouSkill { get; set; }

        [JsonProperty("灵兽Buff")]
        public int[] LingShouBuff { get; set; }
        
        [JsonProperty("灵兽Buff数量")]
        public int[] LingShouBuffCount { get; set; }
        
        [JsonProperty("灵兽功法")]
        public int[] LingShouStaticSkill { get; set; }

        public static List<LingShouData> LSData = new List<LingShouData>();
    }
}
