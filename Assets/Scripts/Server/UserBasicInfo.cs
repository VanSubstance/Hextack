using UnityEngine;

namespace Assets.Scripts.Server
{
    [CreateAssetMenu(fileName = "User Basic Info", menuName = "Scriptables/Server/User Basic Info", order = int.MaxValue)]
    public class UserBasicInfo : ScriptableObject
    {
        public string NickName;
        public int Rank, AmountGold, AmountArtifact;
        public UnitKeyList[] UnitStorageList;
        /// <summary>
        /// 덱 리스트: 최대 4개, 덱 당 기물 최대 5개
        /// </summary>
        public DeckCodeList[] DeckList;

        [System.Serializable]
        public class UnitKeyList
        {
            public string Code;
            public int Upgrade;
        }

        [System.Serializable]
        public class DeckCodeList
        {
            /// <summary>
            /// 코드 리스트: 무조건 5개
            /// </summary>
            public string[] Codes;
        }
    }
}
