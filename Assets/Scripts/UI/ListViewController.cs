using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(ScrollRect))]
    public class ListViewController : MonoBehaviour
    {
        [SerializeField]
        private Transform componentPrefab;
        [SerializeField]
        private int sizeSpacing;

        private ScrollRect scrollRect;

        private VerticalLayoutGroup contentLayout;
        private Queue<Transform> q;

        private void Awake()
        {
            scrollRect = GetComponent<ScrollRect>();
            contentLayout = scrollRect.content.GetComponent<VerticalLayoutGroup>();
            contentLayout.spacing = sizeSpacing;
            q = new Queue<Transform>();
            // 풀링: 10개만
            int ii = 0;
            Transform temp;
            while (ii++ < 10)
            {
                temp = Instantiate(componentPrefab, scrollRect.content);
                temp.gameObject.SetActive(false);
                q.Enqueue(temp);
            }
        }

        /// <summary>
        /// 신규 컨텐츠 컴포넌트 반환
        /// </summary>
        /// <returns></returns>
        public T GetNewComponent<T>() where T : MonoBehaviour
        {
            Transform res;
            if (q.TryDequeue(out res))
            {
                return res.GetComponent<T>();
            }
            res = Instantiate(componentPrefab, scrollRect.content);
            res.gameObject.SetActive(false);
            return res.GetComponent<T>();
        }
    }
}
