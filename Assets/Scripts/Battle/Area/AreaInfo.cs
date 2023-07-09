using Assets.Scripts.Battle.Projectile;
using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    /// <summary>
    /// 장판 정보
    /// </summary>
    public class AreaInfo
    {
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
        public DamageEffectInfo[] damageEffects;

        /// <summary>
        /// 장판 목표 지점
        /// </summary>
        public Vector3 targetPos;
    }
}
