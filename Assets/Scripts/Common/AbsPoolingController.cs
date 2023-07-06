using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common
{
    /// <summary>
    /// 오브젝트 풀링용 기본 컨트롤러
    /// </summary>
    public abstract class AbsPoolingController<TObject> : MonoBehaviour where TObject : MonoBehaviour
    {
        [SerializeField]
        private TObject componentPrefab;
        private Queue<TObject> q;

        protected virtual void Awake()
        {
            q = new Queue<TObject>();
            // 풀링: 10개만
            int ii = 0;
            TObject temp;
            while (ii++ < 10)
            {
                temp = Instantiate(componentPrefab, GetParent());
                temp.gameObject.SetActive(false);
                q.Enqueue(temp);
            }
        }

        /// <summary>
        /// 신규 컨텐츠 컴포넌트 반환
        /// </summary>
        /// <returns></returns>
        public TObject GetNewComponent()
        {
            TObject res;
            if (q.TryDequeue(out res))
            {
                return res;
            }
            res = Instantiate(componentPrefab, GetParent());
            res.gameObject.SetActive(false);
            return res;
        }

        /// <summary>
        /// 오브젝트 풀링 부모 반환
        /// </summary>
        /// <returns></returns>
        public abstract Transform GetParent();
    }
}
