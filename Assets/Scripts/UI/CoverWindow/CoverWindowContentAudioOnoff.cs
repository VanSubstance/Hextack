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

        public void ToggleMute(bool isOn)
        {
            if (!isInit) return;
            ServerData.Saving.IsMute = isOn;
            //AudioListener.pause = !isOn;
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
