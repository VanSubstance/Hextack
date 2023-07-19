using Assets.Scripts.UI.Swiper;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContent : AbsSwiperContent<AchievementInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc, textIsClear;
        private System.Func<bool> ActionCondition;
        private System.Action ActionAchiveve;
        private bool isCleared;

        public void UpdateCondition()
        {
            if (!isCleared && ActionCondition())
            {
                isCleared = true;
                ActionAchiveve?.Invoke();
                textIsClear.gameObject.SetActive(true);
            }
        }

        public override void Init(AchievementInfo _info)
        {
            isCleared = false;
            textTitle.text = _info.Title;
            textDesc.text = _info.Desc;
            ActionCondition = _info.ActionCondition;
            ActionAchiveve = _info.ActionAchieve;
            textIsClear.gameObject.SetActive(false);
        }
    }
}
