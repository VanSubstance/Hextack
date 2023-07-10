using UnityEngine;
using Assets.Scripts.Monster;
using System.Linq;

namespace Assets.Scripts.Dungeon
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DungeonInfo", menuName = "Scriptables/Dungeon/Info", order = int.MaxValue)]
    public class DungeonInfo : ScriptableObject
    {
        [HideInInspector]
        public string Code;
        public string mapTitle, Desc;
        /// <summary>
        /// 라운드당 시간 (초)
        /// </summary>
        public int TimeRound;

        /// <summary>
        /// 등장 몬스터들 (길이 = 라운드 수)
        /// </summary>
        [SerializeField]
        private MonsterTokenSerialized[] monsterCodeList;
        [HideInInspector]
        public MonsterToken[] MonsterCodeList;

        public DungeonInfo SetCode(string code)
        {
            Code = code;
            MonsterCodeList = monsterCodeList.Select((tk) => tk.Export()).ToArray();
            return this;
        }

        [System.Serializable]
        public class MonsterTokenSerialized
        {
            public string Code;
            public int IdxEnterance;
            public MonsterToken Export()
            {
                MonsterToken ret = CreateInstance<MonsterToken>();
                ret.Code = Code;
                ret.IdxEnterance = IdxEnterance;
                return ret;
            }
        }
    }
}
