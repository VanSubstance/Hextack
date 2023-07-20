namespace Assets.Scripts.UI.Achievement
{
    public class AchievementInfo
    {
        /// <summary>
        /// 이름
        /// </summary>
        public string Title;

        /// <summary>
        /// 설명
        /// </summary>
        public string Desc;

        /// <summary>
        /// 대상 리소스
        /// </summary>
        public TargetResourceType TargetResource;

        /// <summary>
        /// 조건
        /// </summary>
        public System.Func<bool> ActionCondition;

        /// <summary>
        /// 달성 시 보상
        /// </summary>
        public System.Action ActionAchieve;

        /// <summary>
        /// 대상 리소스 타입
        /// </summary>
        public enum TargetResourceType
        {
            Tower,
            Stone,
            Monster
        }
    }
}
