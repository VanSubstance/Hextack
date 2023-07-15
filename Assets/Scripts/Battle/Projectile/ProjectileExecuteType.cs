namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 실행 타입
    /// </summary>
    [System.Serializable]
    public enum ProjectileExecuteType
    {
        /// <summary>
        /// 일반 탄환형
        /// </summary>
        Bullet,
        /// <summary>
        /// 즉발형
        /// </summary>
        Instant,
        /// <summary>
        /// 아우라형
        /// </summary>
        Aura,
        /// <summary>
        /// 레이저
        /// </summary>
        Lazer,
    }
}
