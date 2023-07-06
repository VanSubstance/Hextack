using UnityEngine;

namespace Assets.Scripts.UI.Fragment.Lobby
{
    public class LobbyFragmentController : SwiperFragmentController
    {
        [SerializeField]
        private LobbyGameModeSelectionController gameModeController;

        /// <summary>
        /// 로비 프레그먼트 컨텐츠 초기화
        /// </summary>
        public override void Init()
        {
            gameModeController.Init();
        }
    }
}
