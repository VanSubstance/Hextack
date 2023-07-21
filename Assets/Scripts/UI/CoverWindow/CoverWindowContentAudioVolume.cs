using UnityEngine;

namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 음소거 조절
    /// </summary>
    public class CoverWindowContentAudioVolume : AbsCoverWindowContent
    {
        [SerializeField]
        private GageController gage;

        public override AbsCoverWindowContent Init()
        {
            gage.Init(1, ServerData.Saving.Volume, null, isInteractable: true);
            return this;
        }

        public void OnChange(float value)
        {
            AudioListener.volume = ServerData.Saving.Volume = value;
        }

        protected override CoverWindowContentType GetContentType()
        {
            return CoverWindowContentType.AudioVolume;
        }

        protected override int GetHeight()
        {
            return 100;
        }
    }
}
