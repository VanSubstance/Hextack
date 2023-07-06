using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class SwiperController : SingletonObject<SwiperController>, IEndDragHandler
    {
        [SerializeField]
        private SwiperContentController[] fragments;
        private ScrollRect scrollRect;
        private int currentIdx;
        private float normHorPos;
        private new void Awake()
        {
            base.Awake();
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((value) =>
            {
                normHorPos = value.x;
            });
        }

        /// <summary>
        /// 해당 프레그먼트로 이동
        /// </summary>
        /// <param name="targetIdx"></param>
        /// <param name="isWithAnimation">스무스 이동인가 ?</param>
        public void GoToFragment(int targetIdx, bool isWithAnimation)
        {
            if (isWithAnimation)
            {
                StartCoroutine(CrGoToFragment(targetIdx));
            }
            else
            {
                scrollRect.horizontalNormalizedPosition = (float)targetIdx / (fragments.Length - 1);
                currentIdx = targetIdx;
            }
            FooterController.Instance.Track(targetIdx);
        }

        private IEnumerator CrGoToFragment(int targetIdx)
        {
            float targetNorm = (float)targetIdx / (fragments.Length - 1);
            float curNorm = normHorPos;
            int frameLeft = 30;
            while (frameLeft-- > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                scrollRect.horizontalNormalizedPosition = curNorm = ((targetNorm - curNorm) / 3f) + curNorm;
            }
            scrollRect.horizontalNormalizedPosition = targetNorm;
            currentIdx = targetIdx;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            int tempIdx = 0;
            while (tempIdx < fragments.Length)
            {
                if ((tempIdx - .5f) / (fragments.Length - 1) <= normHorPos && normHorPos < (tempIdx + .5f) / (fragments.Length - 1))
                {
                    GoToFragment(tempIdx, true);
                    return;
                }
                tempIdx++;
            }
        }
    }
}
