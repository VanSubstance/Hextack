using TMPro;
using UnityEngine;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContent : AbsPoolingContent<AchievementInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textDesc;
        private System.Func<object, bool> ActionCondition;
        private System.Action ActionAchiveve;
        public override void Clear()
        {

        }

        protected override bool InitExtra(AchievementInfo _info)
        {
            textDesc.text = _info.TextDesc;
            ActionCondition = _info.ActionCondition;
            ActionAchiveve = _info.ActionAchieve;
            return true;
        }

        public void UpdateCondition(object obj)
        {
            if (ActionCondition(obj))
            {
                ActionAchiveve?.Invoke();
            }
        }
    }
}
