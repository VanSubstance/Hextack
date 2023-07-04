using UnityEngine;

namespace Assets.Scripts.Map
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "MapInfo", menuName = "Scriptables/Map Info", order = int.MaxValue)]
    public class MapInfo : ScriptableObject
    {
        public string mapTitle;
        public string Code;
        public int radius, rounds, cntTileBan, cntUnitBan, cntUnit;
        public string[] unitCodeList;
    }
}
