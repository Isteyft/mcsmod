using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.IsTools.Data
{
    public class AchievementData
    {
        [JsonProperty("成就ID")]
        public int Id { get; set; }

        [JsonProperty("成就标题")]
        public string AchievementTitle { get; set; }

        [JsonProperty("成就描述")]
        public string AchievementInfo { get; set; }

        [JsonProperty("成就介绍")]
        public string AchievementDesc { get; set; }

        public static List<AchievementData> data = new List<AchievementData>();
        public static List<int> successData = new List<int>();
        public static AchievementData GetAchievementId(int targetId)
        {
            return data.FirstOrDefault(data => data.Id == targetId);
        }

        public static bool IsSuccess(int targetId)
        {
            if (successData == null)
            {
                return false;
            }
            return successData.Contains(targetId);
        }
    }
}
