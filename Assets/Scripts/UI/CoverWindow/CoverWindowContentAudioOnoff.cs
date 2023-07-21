namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 음소거 조절
    /// </summary>
    public class CoverWindowContentAudioOnoff : AbsCoverWindowContent
    {
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
