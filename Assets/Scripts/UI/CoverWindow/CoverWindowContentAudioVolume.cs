namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 음소거 조절
    /// </summary>
    public class CoverWindowContentAudioVolume : AbsCoverWindowContent
    {

        public override AbsCoverWindowContent Init()
        {
            return this;
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
