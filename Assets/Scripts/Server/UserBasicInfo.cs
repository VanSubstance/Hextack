using UnityEngine;

namespace Assets.Scripts.Server
{
    [CreateAssetMenu(fileName = "User Basic Info", menuName = "Scriptables/Server/User Basic Info", order = int.MaxValue)]
    public class UserBasicInfo : ScriptableObject
    {
        public string NickName;
        public int Rank, AmountGold, AmountArtifact;
        public UnitUpgradeInfo[] unitPossessList;

        [System.Serializable]
        public class UnitUpgradeInfo
        {
            public string Code;
            public int Upgrade;
        }
    }
}
