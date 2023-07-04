namespace Assets.Scripts.Unit
{
    /// <summary>
    /// 기물 능력
    /// </summary>
    [System.Serializable]
    public class AbilityInfo
    {
        /// <summary>
        /// 1회 적용인지
        /// </summary>
        public bool isOnce;

        /// <summary>
        /// 범위인지
        /// </summary>
        public bool isBound;

        /// <summary>
        /// 아군 대상인지
        /// </summary>
        public bool isForAlly;

        /// <summary>
        /// 효과
        /// </summary>
        public AbilityType type;

        /// <summary>
        /// 적용량
        /// </summary>
        public float amount;

        /// <summary>
        /// 1회당 필요한 시간 = 쿨타임
        /// </summary>
        public float secondForOnce;

        /// <summary>
        /// 공격 가능 효과인지
        /// </summary>
        public bool IsAttackable
        {
            get
            {
                return secondForOnce != 0;
            }
        }
    }
}
