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
        public UnitInfo Clone()
        {
            return Instantiate(this);
        }
        public UnitToken GetLiveInfo()
        {
            UnitToken res = CreateInstance<UnitToken>();
            res.Title = Code;
            return res;
        }
    }
}
