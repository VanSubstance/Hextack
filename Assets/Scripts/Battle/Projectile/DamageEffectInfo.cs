namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 실제 공격 적용 시 효과
    /// </summary>
    [System.Serializable]
    public class DamageEffectInfo
    {
        /// <summary>
        /// 적용 효과 타입
        /// </summary>
        public DamageEffectType damageEffectType;
        /// <summary>
        /// 적용할 수치
        /// </summary>
        public float Amount;

        /// <summary>
        /// 쿨타임
        /// </summary>
        public float Cooltime;

        public DamageEffectInfo Clone()
        {
            return new() { damageEffectType = damageEffectType, Amount = Amount, Cooltime = Cooltime };
        }
    }
}
