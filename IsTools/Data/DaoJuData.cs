using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Data
{
    public class DaoJuData
    {
        [JsonProperty("道具物品")]
        public int DaoJuItem { get; set; }

        [JsonProperty("道具神通")]
        public int DaoJuSkill { get; set; }

        public static List<DaoJuData> data = new List<DaoJuData>();
    }
}
