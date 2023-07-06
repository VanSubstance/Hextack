﻿using Assets.Scripts.UI.Window;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common.MainManager;

namespace Assets.Scripts.Unit
{
    public class UnitStorageController : MonoBehaviour
    {
        private UnitInfo unitInfo;
        private Image image;
        private int idxInDeck;
        public UnitInfo Info
        {
            get
            {
                return unitInfo;
            }
        }

        /// <summary>
        /// 장착 시 실행되는 람다
        /// </summary>
        [HideInInspector]
        public System.Action<int> ActionOnEquip;

        /// <summary>
        /// 간단 정보창이 열려야하는지 판별 람다
        /// </summary>
        [HideInInspector]
        public System.Func<bool> ActionIfSimpleInfo;

        private void Awake()
        {
            image = GetComponent<Image>();
            idxInDeck = -1;
            gameObject.SetActive(false);
        }

        public UnitStorageController Init(UnitInfo _unitInfo, Transform parent = null)
        {
            unitInfo = _unitInfo;
            image.sprite = GlobalDictionary.Texture.Unit.data[unitInfo.Code];
            if (parent != null)
            {
                transform.SetParent(parent);
            }
            else
            {
                idxInDeck = transform.GetSiblingIndex();
            }
            gameObject.SetActive(true);
            return this;
        }

        public void OnClick()
        {
            if (MainMainManager.Instance.IsTryingEquip)
            {
                ActionOnEquip(idxInDeck);
                WindowController.Instance.Close();
            }
            else
            {
                // 정보 띄워주기
                if (ActionIfSimpleInfo != null ? ActionIfSimpleInfo() : false)
                {
                    WindowController.Instance.OpenUnitInfoSimple(unitInfo);

                }
                else
                {
                    WindowController.Instance.OpenUnitInfo(unitInfo);
                }
            }
        }
    }
}
