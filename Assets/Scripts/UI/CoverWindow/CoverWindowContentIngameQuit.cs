namespace Assets.Scripts.UI.CoverWindow
{
    /// <summary>
    /// 음소거 조절
    /// </summary>
    public class CoverWindowContentIngameQuit : AbsCoverWindowContent
    {

        public override AbsCoverWindowContent Init()
        {
            return this;
        }
        /// <summary>
        /// 인게임 > 아웃게임
        /// </summary>
        public void QuitIngame()
        {
            ServerManager.Instance.ExitNormal();
        }

        protected override CoverWindowContentType GetContentType()
        {
            return CoverWindowContentType.IngameQuit;
        }

        protected override int GetHeight()
        {
            return 120;
        }
    }
}
