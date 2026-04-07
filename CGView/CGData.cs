using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace top.Isteyft.MCS.CGView
{
    public class CGData
    {
        [JsonProperty("CGId")]
        public int CGId { get; set; }

        [JsonProperty("CG名字")]
        public string CGName { get; set; }

        public static List<CGData> data = new List<CGData>();
        public static List<int> successData = new List<int>();
        public static CGData GetCGId(int targetId)
        {
            return data.FirstOrDefault(data => data.CGId == targetId);
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
