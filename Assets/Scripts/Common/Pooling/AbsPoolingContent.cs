using UnityEngine;

namespace Assets.Scripts.Common.Pooling
{
    /// <summary>
    /// 풀링 컨텐츠 기본형
    /// </summary>
    public abstract class AbsPoolingContent : MonoBehaviour
    {
        private System.Action<AbsPoolingContent> ActionReturnToPool;
        private bool isConnected = false;
        public void ConnectWithParent(System.Action<AbsPoolingContent> _ActionReturnToPool)
        {
            ActionReturnToPool = _ActionReturnToPool;
            isConnected = true;
        }

        /// <summary>
        /// 사용 시작 함수
        /// </summary>
        public AbsPoolingContent Init(Info info)
        {
            if (!isConnected || !InitExtra(info))
            {
                Debug.Log("Not Connected.");
                ReturnToPool();
                return null;
            }
            return this;
        }


        /// <summary>
        /// 사용 시작 추가 실행
        /// </summary>
        protected abstract bool InitExtra(Info _info);

        public void ReturnToPool()
        {
            Clear();
            ActionReturnToPool?.Invoke(this);
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 풀에 반납할 때 추가 실행
        /// </summary>
        public abstract void Clear();

        public class Info
        {

        }
    }
}
