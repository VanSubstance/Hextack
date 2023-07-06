using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Common.Pooling
{
    /// <summary>
    /// 오브젝트 풀링용 기본 컨트롤러
    /// </summary>
    public abstract class AbsPoolingController<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance;
        [SerializeField]
        private AbsPoolingContent componentPrefab;
        private Queue<AbsPoolingContent> q;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
            q = new Queue<AbsPoolingContent>();
            // 풀링: 10개만
            int ii = 0;
            AbsPoolingContent temp;
            while (ii++ < 10)
            {
                temp = Instantiate(componentPrefab, GetParent());
                temp.ConnectWithParent((content) =>
                {
                    q.Enqueue(content);
                });
                temp.gameObject.SetActive(false);
                q.Enqueue(temp);
            }
        }

        /// <summary>
        /// 신규 컨텐츠 컴포넌트 반환
        /// </summary>
        /// <returns></returns>
        public AbsPoolingContent GetNewComponent()
        {
            if (q.TryDequeue(out AbsPoolingContent res))
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
