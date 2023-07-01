using Assets.Scripts.Map;
using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Choice
{
    public class ChoiceController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        private Image image;
        private UnitInfo info;
        private UnitController newUnit;
        private HexTileController targetTile;
        private void Awake()
        {
            image = transform.GetChild(0).GetComponent<Image>();
        }

        public void Init(UnitInfo _info)
        {
            info = _info;
            if (image == null)
            {
                image = transform.GetChild(0).GetComponent<Image>();
            }
            image.sprite = GlobalDictionary.Texture.Unit.data[_info.Code];
        }

        /// <summary>
        /// 마우스 위치 기준 현재 예상 설치 지점 추적 함수
        /// </summary>
        /// <param name="eventData"></param>
        public void OnDrag(PointerEventData eventData)
        {
            HexTileController curDetect;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit, 50f, GlobalDictionary.Layer.Map))
            {
                curDetect = hit.transform.GetComponent<HexTileController>();
                if (targetTile != null)
                {
                    // 이전에 감지된 애가 있음
                    if (!targetTile.Equals(curDetect))
                    {
                        // 다른 애 = 이전 미리보기 지워야 함
                        targetTile.ClearUnit();
                        if (!curDetect.IsPossessed)
                        {
                            newUnit.gameObject.SetActive(true);
                            targetTile = curDetect;
                            targetTile.PreviewUnit(newUnit);
                        }
                        else
                        {
                            targetTile = null;
                            newUnit.gameObject.SetActive(false);
                        }
                    }
                }
                else
                {
                    // 이전에 감지된 애가 없음 = 이번에 감지된 애 미리보기 가능하면 띄워주기
                    if (!curDetect.IsPossessed)
                    {
                        newUnit.gameObject.SetActive(true);
                        // 미리보기 on
                        targetTile = curDetect;
                        targetTile.PreviewUnit(newUnit);
                    }
                }
                return;
            }
            // 이번에 감지 안됨
            if (targetTile != null)
            {
                // 이전 감지된 애 있음 = 미리보기 꺼주기 + 해당 기물도 꺼주기
                targetTile.ClearUnit();
                targetTile = null;
                newUnit.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (newUnit == null)
            {
                if ((newUnit = GlobalStatus.GetUnitController()) == null)
                {
                    newUnit = Instantiate(GlobalDictionary.Prefab.Unit.Prefab, UnitManager.Instance.transform);
                }
                GlobalStatus.UnitsActive.Add(newUnit.Init(info.GetLiveInfo(), false));
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (newUnit != null && newUnit.gameObject.activeSelf)
            {
                targetTile.InstallUnit(newUnit);
                targetTile = null;
                newUnit = null;
                UIManager.Instance.FinishChoice();
            }
        }
    }
}
