using Assets.Scripts.UI.Swiper;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContainer : AbsSwiperContainer<AchievementInfo>
    {
        public override void Init()
        {
            Close();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
