using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Footer
{
    public class FooterContainer : SingletonObject<FooterContainer>
    {
        /// <summary>
        /// 현재 선택한 메뉴를 추적하는 포인터 이미지
        /// </summary>
        [SerializeField]
        private Image pointer;
        [SerializeField]
        private Transform footerBtnParentTr;
        private FooterContent[] menuBtnList;
        private List<Vector3> menus;

        private new void Awake()
        {
            base.Awake();
            menuBtnList = footerBtnParentTr.GetComponentsInChildren<FooterContent>();
        }

        /// <summary>
        ///  이동한 프레그먼트 메뉴로 포인터 이동
        /// </summary>
        /// <param name="idx"></param>
        public void Track(int idx)
        {
            if (menus == null)
            {
                menus = new List<Vector3>();
                foreach (Vector3 coor in menuBtnList.Select((btn) =>
                {
                    return btn.transform.position;
                }))
                {
                    menus.Add(coor);
                }
            }
            int ii = 0;
            menuBtnList.All((btn) =>
            {
                btn.IsSelected = idx == ii++;
                return true;
            });
            StartCoroutine(CrAnimate(menus[idx]));
        }

        private IEnumerator CrAnimate(Vector3 targetPos)
        {
            Vector3 tempPos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            while ((pointer.transform.position - targetPos).magnitude > .1f)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                pointer.transform.position = targetPos = ((pointer.transform.position - targetPos) * Time.deltaTime * 10) + targetPos;
            }
            pointer.transform.position = tempPos;
        }
    }
}
