using UnityEngine;

namespace Assets.Scripts.UI.CoverWindow
{

    public abstract class AbsCoverWindowContent: MonoBehaviour
    {
        /// <summary>
        /// 컨텐츠 타입
        /// </summary>
        public CoverWindowContentType ContentType
        {
            get
            {
                return GetContentType();
            }
        }

        /// <summary>
        /// 컨텐츠 높이
        /// </summary>
        public int Height
        {
            get
            {
                return GetHeight();
            }
        }

        /// <summary>
        /// 해당 컨텐츠 타입 반환
        /// </summary>
        /// <returns></returns>
        protected abstract CoverWindowContentType GetContentType();
        
        /// <summary>
        /// 해당 컨텐츠 높이 반환
        /// </summary>
        /// <returns></returns>
        protected abstract int GetHeight();

        /// <summary>
        /// 초기화
        /// </summary>
        public abstract AbsCoverWindowContent Init();
    }
}
