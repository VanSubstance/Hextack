using UnityEngine;

namespace Assets.Scripts.UI.Swiper
{
    public abstract class AbsSwiperContent<TParameter>: MonoBehaviour
    {
        /// <summary>
        /// 초기화
        /// </summary>
        /// <param name="_info"></param>
        public abstract void Init(TParameter _info);
    }
}
