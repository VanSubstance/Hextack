namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 피격 후 효과 타입
    /// </summary>
    [System.Serializable]
    public enum AfterHitType
    {
        /// <summary>
        /// 효과 없음
        /// </summary>
        None,
        /// <summary>
        /// 폭발
        /// </summary>
        Explosive,
        /// <summary>
        /// 장판
        /// </summary>
        Area,
    }
}
