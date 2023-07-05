using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window
{
    public class FooterController : SingletonObject<FooterController>
    {
        /// <summary>
        /// 현재 선택한 메뉴를 추적하는 포인터 이미지
        /// </summary>
        [SerializeField]
        private Image pointer;
        [SerializeField]
        private Transform contentsTf;
        private List<Vector3> menus;

        public void Track(int idx)
        {
            if (menus == null)
            {
                menus = new List<Vector3>();
                foreach (Vector3 coor in contentsTf.GetComponentsInChildren<FooterContentController>().Select((rect) =>
                {
                    return rect.transform.position;
                }))
                {
                    menus.Add(coor);
                }
            }
            StartCoroutine(CrAnimate(menus[idx]));
        }

        private IEnumerator CrAnimate(Vector3 targetPos)
        {
            Vector3 tempPos = new Vector3(targetPos.x, targetPos.y, targetPos.z);
            int frameLeft = 30;
            while (frameLeft-- > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                pointer.transform.position = targetPos = ((pointer.transform.position - targetPos) / 30f) + targetPos;
            }
            pointer.transform.position = tempPos;
        }
    }
}
