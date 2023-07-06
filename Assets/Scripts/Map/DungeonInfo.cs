using UnityEngine;

namespace Assets.Scripts.Map
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "DungeonInfo", menuName = "Scriptables/Dungeon Info", order = int.MaxValue)]
    public class DungeonInfo : ScriptableObject
    {
        public string mapTitle;
        public string Code;
        public int radius, rounds, cntTileBan, cntUnitBan, cntUnit;
        public string[] unitCodeList;
    }
}
