using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 음소거 조절
    /// </summary>
    public class CoverWindowContentAudioOnoff : AbsCoverWindowContent
    {
        [SerializeField]
        private Toggle toggle;
        private bool isInit;

        public override AbsCoverWindowContent Init()
        {
            isInit = false;
            toggle.isOn = ServerData.Saving.IsMute;
            isInit = true;
            return this;
        }

        public void ToggleMute()
        {
            if (!isInit) return;
            if (ServerData.Saving.IsMute)
            {
                // 음소거 풀기
                toggle.isOn = false;
                ServerData.Saving.IsMute = false;
            }
            else
            {
                // 음소거 켜기
                toggle.isOn = true;
                ServerData.Saving.IsMute = true;
            }
        }

        protected override CoverWindowContentType GetContentType()
        {
            return CoverWindowContentType.AudioOnoff;
        }

        protected override int GetHeight()
        {
            return 100;
        }
    }
}
