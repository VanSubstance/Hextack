﻿using System.Collections;
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
        private FooterContentController[] menuBtnList;
        private List<Vector3> menus;

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
            int frameLeft = 30;
            while (frameLeft-- > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime);
                pointer.transform.position = targetPos = ((pointer.transform.position - targetPos) / 10f) + targetPos;
            }
            pointer.transform.position = tempPos;
        }
    }
}
