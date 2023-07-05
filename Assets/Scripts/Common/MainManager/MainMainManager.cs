using Assets.Scripts.UI.Window;
using UnityEngine;

namespace Assets.Scripts.Common.MainManager
{
    public class MainMainManager : SingletonObject<MainMainManager>
    {
        [SerializeField]
        private SwiperController swiper;
        /// <summary>
        /// 시작 프레그먼트 번호
        /// </summary>
        [SerializeField]
        private int startIdx;

        private void Start()
        {
            //swiper.GoToFragment(startIdx, false);
        }
    }
}
