namespace Assets.Scripts.Unit
{
    [System.Serializable]
    public enum AbilityType
    {
        /// <summary>
        /// 도발
        /// </summary>
        Provoke,

        /// <summary>
        /// 사거리 내 전부 적용
        /// </summary>
        Bounds,

        /// <summary>
        /// 아군이 대상
        /// </summary>
        TargetAlly,

        /// <summary>
        /// 공격 속도
        /// </summary>
        AttackSpeed,

        /// <summary>
        /// 치명타율
        /// </summary>
        RateCritical,
    }
}
