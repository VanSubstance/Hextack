using UnityEngine;

namespace Assets.Scripts.Tower
{
    [CreateAssetMenu(fileName = "TowerInfo", menuName = "Scriptables/Tower Info", order = int.MaxValue)]
    public class TowerInfo : ScriptableObject
    {
        public string Code, Name, Desc;
        public float AttackPerSecond;
        public int Damage, CountProjectilePerOnce;
    }
}
