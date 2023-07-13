using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Assets.Scripts.UI.Footer;

namespace Assets.Scripts.UI.Fragment
{
    /// <summary>
    /// 프레그먼트 컨트롤러
    /// </summary>
    public class FragmentContainer : SingletonObject<FragmentContainer>, IEndDragHandler
    {
        [SerializeField]
        private Transform contentParentTr;

        private AbsFragmentContent[] fragmentContentList;
        private ScrollRect scrollRect;
        private int currentIdx;
        private float normHorPos;

        private new void Awake()
        {
            base.Awake();
            fragmentContentList = contentParentTr.GetComponentsInChildren<AbsFragmentContent>();
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((value) =>
            {
                normHorPos = value.x;
            });
        }

        private void Start()
        {
            GoToFragment(1, false);
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
                scrollRect.horizontalNormalizedPosition = (float)targetIdx / (fragmentContentList.Length - 1);
                currentIdx = targetIdx;
            }
            FooterContainer.Instance.Track(targetIdx);
        }

        private IEnumerator CrGoToFragment(int targetIdx)
        {
            float targetNorm = (float)targetIdx / (fragmentContentList.Length - 1);
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
            while (tempIdx < fragmentContentList.Length)
            {
                if ((tempIdx - .5f) / (fragmentContentList.Length - 1) <= normHorPos && normHorPos < (tempIdx + .5f) / (fragmentContentList.Length - 1))
                {
                    GoToFragment(tempIdx, true);
                    return;
                }
                tempIdx++;
            }
        }

        public void Init(int param)
        {
            // 프레그먼트들 초기화
            fragmentContentList.All((frag) =>
            {
                frag?.Init();
                return true;
            });
            // 시작 프레그먼트로 이동
            GoToFragment(param, false);
        }
    }
}
