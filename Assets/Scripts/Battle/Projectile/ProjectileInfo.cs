using UnityEngine;
using Assets.Scripts.Monster;
using Assets.Scripts.Battle.Area;
using Assets.Scripts.Tower;

namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 정보
    /// </summary>
    [System.Serializable]
    public class ProjectileInfo
    {
        /// <summary>
        /// 투사체 타입; default = Bullet
        /// </summary>
        public ProjectileExecuteType executeType = ProjectileExecuteType.Bullet;

        /// <summary>
        /// 사용 시 소리
        /// </summary>
        public AudioClip SoundFire;

        /// <summary>
        /// 1회 당 발사하는 투사체 개수; 기본값 = 1
        /// </summary>
        public int CountPerOnce = 1;

        /// <summary>
        /// 투사체 속도; 기본값 = 4m/s
        /// </summary>
        public float Spd = 4;

        /// <summary>
        /// 사거리
        /// </summary>
        public float Range;

        /// <summary>
        /// 적용 효과 정보
        /// </summary>
        public DamageEffectInfo effectInfo;

        /// <summary>
        /// 궤적 타입; 기본값 = 직선
        /// </summary>
        public ProjectileTrailType TrailType;

        /// <summary>
        /// 도착 후 효과; 기본값 = null
        /// </summary>
        public AfterHitInfo afterHitInfo;

        /// <summary>
        /// 투사체 색; 기본값 = 흰색
        /// </summary>
        public Color color;

        [HideInInspector]
        public Vector3 StartPos, EndPos;
        [HideInInspector]
        public System.Action<MonsterController> ActionEnd;
        [HideInInspector]
        public MonsterController targetTr;
        /// <summary>
        /// 부모 타워 분류
        /// </summary>
        [HideInInspector]
        public TowerType towerType;

        public ProjectileInfo Clone()
        {
            return new()
            {
                executeType = executeType,
                color = color,
                CountPerOnce = CountPerOnce,
                Spd = Spd,
                Range = Range,
                TrailType = TrailType,
                effectInfo = effectInfo.Clone(),
                afterHitInfo = afterHitInfo,
            };
        }

        /// <summary>
        /// 장판 정보 추출
        /// </summary>
        /// <returns></returns>
        public AreaInfo GetAreaInfo()
        {
            return new AreaInfo()
            {
                color = color,
                damageEffect = effectInfo.Clone(),
                duration = -1,
                range = Range,
                towerType = towerType,
            };
        }
    }
}
