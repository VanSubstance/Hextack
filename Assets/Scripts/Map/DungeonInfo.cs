using UnityEngine;

namespace Assets.Scripts.Map
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DungeonInfo", menuName = "Scriptables/Dungeon Info", order = int.MaxValue)]
    public class DungeonInfo : ScriptableObject
    {
        public string mapTitle, Desc;
        public string Code;
        public int radius, rounds, cntTileBan, cntUnitBan, cntUnitSummonAtOnce;
        public string[] unitCodeList;
        public Vector2[] EnteranceList;
    }
}
