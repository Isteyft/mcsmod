using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Data
{
    public class FuLuItemData
    {
        [JsonProperty("符箓物品")]
        public int FuLuId { get; set; }

        [JsonProperty("符箓等级")]
        public int FuLuLevel { get; set; }

        [JsonProperty("制作材料")]
        public int[] FuLuItem { get; set; }
        
        [JsonProperty("材料数量")]
        public int[] FuLuItemNum { get; set; }

        [JsonProperty("制作时间")]
        public int FuLuTime { get; set; }

        [JsonProperty("制作经验")]
        public int FuLuExp { get; set; }

        public static List<FuLuItemData> FuLuData = new List<FuLuItemData>();
        public static FuLuItemData GetFuLuItemDataByFuLuId(int targetFuLuId)
        {
            return FuLuData.FirstOrDefault(data => data.FuLuId == targetFuLuId);
        }
    }
}
