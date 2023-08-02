using Assets.Scripts.UI.Footer;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Swiper
{
    public abstract class AbsSwiperContainer<TParameterForContent> : MonoBehaviour, IEndDragHandler
    {
        [SerializeField]
        protected Transform ContentParentTr;

        [HideInInspector]
        public List<AbsSwiperContent<TParameterForContent>> ContentList;
        private ScrollRect scrollRect;
        private float normHorPos;

        private void Awake()
        {
            ContentList = ContentParentTr.GetComponentsInChildren<AbsSwiperContent<TParameterForContent>>().ToList();
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((value) =>
            {
                normHorPos = value.x;
            });
        }

        private void Start()
        {
            Init();
        }

        /// <summary>
        /// 해당 프레그먼트로 이동
        /// </summary>
        /// <param name="targetIdx"></param>
        /// <param name="isWithAnimation">스무스 이동인가 ?</param>
        public void GoToContent(int targetIdx, bool isWithAnimation)
        {
            if (isWithAnimation)
            {
                StartCoroutine(CrGoToContent(targetIdx));
            }
            else
            {
                scrollRect.horizontalNormalizedPosition = (float)targetIdx / (ContentList.Count - 1);
            }
            FooterContainer.Instance.Track(targetIdx);
        }

        private IEnumerator CrGoToContent(int targetIdx)
        {
            float targetNorm = (float)targetIdx / (ContentList.Count - 1);
            float curNorm = normHorPos;
            while (Mathf.Abs(targetNorm - curNorm) > .01f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                scrollRect.horizontalNormalizedPosition = curNorm = ((targetNorm - curNorm) * Time.deltaTime * 10) + curNorm;
            }
            scrollRect.horizontalNormalizedPosition = targetNorm;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GoToContent(Mathf.FloorToInt(normHorPos * ContentList.Count), true);
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public abstract void Init();
    }
}
