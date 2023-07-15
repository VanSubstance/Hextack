using UnityEngine;

namespace Assets.Scripts.UI.Window
{
    /// <summary>
    /// 윈도우 컨텐츠 컨트롤러
    /// </summary>
    /// <typeparam name="TParameter">사용할 변수 클래스</typeparam>
    public abstract class AbsWindowContent<TParameter> : MonoBehaviour
    {
        public abstract AbsWindowContent<TParameter> Init(TParameter parameter);

        public AbsWindowContent<TParameter> Close()
        {
            gameObject.SetActive(false);
            return this;
        }

        /// <summary>
        /// 닫을 때 추가로 실행할 기능
        /// </summary>
        /// <returns></returns>
        public abstract AbsWindowContent<TParameter> CloseExtra();
    }
}
