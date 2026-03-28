using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.ISAchievement.Data
{
    public class AchievementData
    {
        [JsonProperty("成就ID")]
        public string Id { get; set; }
        [JsonProperty("成就标题")]
        public string AchievementTitle { get; set; }

        [JsonProperty("成就描述")]
        public string AchievementDesc { get; set; }

        public static List<AchievementData> data = new List<AchievementData>();
    }
}
