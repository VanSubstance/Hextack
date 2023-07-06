using UnityEngine;

namespace Assets.Scripts.UI.Fragment
{
    /// <summary>
    /// 스와이퍼용 프레그먼트 부모
    /// </summary>
    public abstract class SwiperFragmentController : MonoBehaviour, IInitiable
    {
        /// <summary>
        /// 프레그먼트 초기화
        /// </summary>
        public abstract void Init();
    }
}
