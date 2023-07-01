using UnityEngine;

namespace Assets.Scripts.Unit
{
    [System.Serializable]
    [CreateAssetMenu(fileName = "UnitInfo", menuName = "Scriptables/Unit Info", order = int.MaxValue)]
    public class UnitInfo : ScriptableObject
    {
        public string Code;
        public string Title;
        public int Hp, Damage, Range;
        public float Spd;
        public bool IsAttackable;
        public Direction[] Bounds;
        [System.Serializable]
        public enum Direction
        {
            C12, C2, C4, C6, C8, C10,
        }
        public UnitInfo Clone()
        {
            return Instantiate(this);
        }
        public UnitLiveInfo GetLiveInfo()
        {
            UnitLiveInfo res = CreateInstance<UnitLiveInfo>();
            res.title = Code;
            return res;
        }
    }
}
