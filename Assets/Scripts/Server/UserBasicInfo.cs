using UnityEngine;

namespace Assets.Scripts.Server
{
    [CreateAssetMenu(fileName = "User Basic Info", menuName = "Scriptables/Server/User Basic Info", order = int.MaxValue)]
    public class UserBasicInfo : ScriptableObject
    {
        public int AmountGold, AmountArtifact;
    }
}
