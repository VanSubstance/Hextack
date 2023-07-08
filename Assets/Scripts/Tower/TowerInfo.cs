using UnityEngine;

namespace Assets.Scripts.Tower
{
    /// <summary>
    /// 타워 정보
    /// </summary>
    [CreateAssetMenu(fileName = "TowerInfo", menuName = "Scriptables/Tower/Info", order = int.MaxValue)]
    public class TowerInfo : ScriptableObject
    {
        [HideInInspector]
        public string Code;
        public string Name, Desc;
        public float AttackPerSecond;
        public int Tier, Damage, CountProjectilePerOnce;
        public string[] Materials;

        public TowerInfo Clone()
        {
            return Instantiate(this);
        }

        public TowerInfo SetCode(string code)
        {
            Code = code;
            return this;
        }
    }
}
