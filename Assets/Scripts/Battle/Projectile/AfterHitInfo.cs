using UnityEngine;

namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 피격 후 효과 정보
    /// </summary>
    [System.Serializable]
    public class AfterHitInfo
    {
        /// <summary>
        /// 효과 타입
        /// </summary>
        public AfterHitType afterHitType;
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
        public DamageEffectInfo damageEffects;

        public Area.AreaInfo GetAreaInfo()
        {
            switch (afterHitType)
            {
                case AfterHitType.Area:
                    return new Area.AreaInfo()
                    {
                        color = color,
                        range = range,
                        duration = duration,
                        damageEffect = damageEffects,
                    };
                case AfterHitType.Explosive:
                    return new Area.AreaInfo()
                    {
                        color = color,
                        range = range,
                        duration = 1f,
                        damageEffect = damageEffects.CloneDisposal(),
                    };
            }
            return null;
        }
    }
}
