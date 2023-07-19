namespace Assets.Scripts.UI.Achievement
{
    public class AchievementInfo
    {
        /// <summary>
        /// 설명
        /// </summary>
        public string TextDesc;

        /// <summary>
        /// 조건
        /// </summary>
        public System.Func<object, bool> ActionCondition;

        /// <summary>
        /// 달성 시 보상
        /// </summary>
        public System.Action ActionAchieve;

    }
}
