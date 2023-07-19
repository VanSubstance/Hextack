namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 실제 공격 적용 시 효과
    /// </summary>
    [System.Serializable]
    public class DamageEffectInfo
    {
        public Token[] tokens;
        /// <summary>
        /// 쿨타임
        /// </summary>
        public float Cooltime;

        public DamageEffectInfo Clone()
        {
            return new() { tokens = tokens.Clone() as Token[], Cooltime = Cooltime };
        }

        /// <summary>
        /// 폭발용 일회성 효과
        /// </summary>
        /// <returns></returns>
        public DamageEffectInfo CloneDisposal()
        {
            return new()
            {
                tokens = tokens.Clone() as Token[],
                Cooltime = 2f,
            };
        }

        /// <summary>
        /// 효과 토큰
        /// </summary>
        [System.Serializable]
        public class Token
        {
            /// <summary>
            /// 적용 효과 타입
            /// </summary>
            public DamageEffectType damageEffectType;
            /// <summary>
            /// 적용할 수치
            /// </summary>
            public float Amount;
        }
    }
}
