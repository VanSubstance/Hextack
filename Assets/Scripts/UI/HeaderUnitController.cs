using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    public class HeaderUnitController : MonoBehaviour, IPointerUpHandler, IPointerDownHandler
    {
        private UnitInfo info;
        private Image image;

        private void Awake()
        {
            image = GetComponent<Image>();
            gameObject.SetActive(false);
        }
        /// <summary>
        /// 해당 유닛 정보로 초기화
        /// </summary>
        /// <param name="_info"></param>
        public void Init(UnitInfo _info)
        {
            info = _info;
            image.sprite = GlobalDictionary.Texture.Unit.data[info.Code];
            gameObject.SetActive(true);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (info != null)
            {
                MainInGameManager.Instance.InitUnitInfo(info);
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (info != null)
            {
                MainInGameManager.Instance.ClearUnitInfo();
            }
        }
    }
}
