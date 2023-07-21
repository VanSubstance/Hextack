using Assets.Scripts.UI.Manager;
using Assets.Scripts.UI.Swiper;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContent : AbsSwiperContent<AchievementInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc, textIsClear;
        private AchievementInfo.TargetResourceType resourceType;
        private System.Func<bool> ActionCondition;
        private System.Action ActionAchiveve;
        private bool isCleared;

        public AchievementInfo.TargetResourceType ResourceType
        {
            get { return resourceType; }
        }

        public void UpdateCondition()
        {
            if (!isCleared && ActionCondition())
            {
                isCleared = true;
                ActionAchiveve?.Invoke();
                textIsClear.gameObject.SetActive(true);
                UIInGameManager.Instance.TextInfo = $"업적을 완료하였습니다!\n[ {textTitle.text} ]";
            }
        }

        public override void Init(AchievementInfo _info)
        {
            isCleared = false;
            textTitle.text = _info.Title;
            textDesc.text = _info.Desc;
            ActionCondition = _info.ActionCondition;
            ActionAchiveve = _info.ActionAchieve;
            resourceType = _info.TargetResource;
            textIsClear.gameObject.SetActive(false);
        }
    }
}
