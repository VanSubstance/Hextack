using Assets.Scripts.Battle.Projectile;
using Assets.Scripts.Tower;
using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    /// <summary>
    /// 장판 정보
    /// </summary>
    public class AreaInfo
    {
        /// <summary>
        /// 부모 타워 분류
        /// </summary>
        [HideInInspector]
        public TowerType towerType;

        /// <summary>
        /// 사용 시 소리
        /// </summary>
        public AudioClip SoundFire;
        /// <summary>
        /// 효과 색상
        /// </summary>
        public Color color;
        /// <summary>
        /// 반지름
        /// </summary>
        public float range;
        /// <summary>
        /// 지속 시간
        /// </summary>
        public float duration;
        /// <summary>
        /// 적용 효과 리스트
        /// </summary>
        public DamageEffectInfo damageEffect;

        /// <summary>
        /// 장판 목표 지점
        /// </summary>
        public Vector3 targetPos;
    }
}
