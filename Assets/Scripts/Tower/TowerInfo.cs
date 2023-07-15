using Assets.Scripts.Battle.Projectile;
using UnityEngine;
using Assets.Scripts.Dungeon;

namespace Assets.Scripts.Tower
{
    /// <summary>
    /// 타워 정보
    /// </summary>
    [CreateAssetMenu(fileName = "TowerInfo", menuName = "Scriptables/Tower/Info", order = int.MaxValue)]
    public class TowerInfo : ScriptableObject
    {
        /// <summary>
        /// 타워 타입
        /// </summary>
        public TowerType towerType;

        [HideInInspector]
        public string Code;
        public string Name, Desc;
        public int Tier;
        public string[] Materials;

        /// <summary>
        /// 투사체 정보
        /// </summary>
        public ProjectileInfo[] projectileInfo;

        [HideInInspector]
        public Vector3 Position;

        /// <summary>
        /// 현재 타워가 설치된 타일
        /// </summary>
        [HideInInspector]
        public TileController TileInstalled;

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
