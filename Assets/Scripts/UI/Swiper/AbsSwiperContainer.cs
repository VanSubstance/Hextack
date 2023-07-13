using Assets.Scripts.UI.Footer;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Swiper
{
    public abstract class AbsSwiperContainer<TParameterForContent> : MonoBehaviour, IEndDragHandler
    {
        [SerializeField]
        private Transform contentParentTr;

        public AbsSwiperContent<TParameterForContent>[] ContentList;
        private ScrollRect scrollRect;
        private int currentIdx;
        private float normHorPos;

        private void Awake()
        {
            ContentList = contentParentTr.GetComponentsInChildren<AbsSwiperContent<TParameterForContent>>();
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((value) =>
            {
                normHorPos = value.x;
            });
        }

        private void Start()
        {
            GoToContent(0, false);
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
                scrollRect.horizontalNormalizedPosition = (float)targetIdx / (ContentList.Length - 1);
                currentIdx = targetIdx;
            }
            FooterContainer.Instance.Track(targetIdx);
        }

        private IEnumerator CrGoToContent(int targetIdx)
        {
            float targetNorm = (float)targetIdx / (ContentList.Length - 1);
            float curNorm = normHorPos;
            while (Mathf.Abs(targetNorm - curNorm) > .1f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                scrollRect.horizontalNormalizedPosition = curNorm = ((targetNorm - curNorm) * Time.deltaTime * 10) + curNorm;
            }
            scrollRect.horizontalNormalizedPosition = targetNorm;
            currentIdx = targetIdx;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int tempIdx = 0;
            while (tempIdx < ContentList.Length)
            {
                if ((tempIdx - .5f) / (ContentList.Length - 1) <= normHorPos && normHorPos < (tempIdx + .5f) / (ContentList.Length - 1))
                {
                    GoToContent(tempIdx, true);
                    return;
                }
                tempIdx++;
            }
        }

        /// <summary>
        /// 초기화
        /// </summary>
        public abstract void Init();
    }
}
