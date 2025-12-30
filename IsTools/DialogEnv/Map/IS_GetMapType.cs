using SkySwordKill.Next.DialogSystem;
using UnityEngine.SceneManagement;

namespace top.Isteyft.MCS.IsTools.DialogEnv.Map
{
    [DialogEnvQuery("IS_GetMapType")]
    public class IS_GetMapType : IDialogEnvQuery
    {
        public object Execute(DialogEnvQueryContext context)
        {
            return jsonData.instance.SceneNameJsonData[$"{SceneManager.GetActiveScene().name}"]["MapType"].I;
        }
    }
}
