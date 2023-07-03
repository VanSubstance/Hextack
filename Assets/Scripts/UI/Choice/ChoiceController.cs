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
                        // 이전 타일 != 지금 타일 -> 이전 미리보기 지워야 함
                        targetTile.ClearPreview();
                        if (!curDetect.IsPossessed)
                        {
                            // 현재 타일에 설치 가능
                            newUnit.gameObject.SetActive(true);
                            targetTile = curDetect;
                            targetTile.PreviewUnit(newUnit);
                        }
                        else
                        {
                            // 현재 타일에 설치 불가
                            newUnit.gameObject.SetActive(false);
                            if (curDetect.IsInstallable && curDetect.UnitCode != null)
                            {
                                // 지금 식별된 애 타일 위에 이미 설치된 기물이 있다
                                if (curDetect.UnitCode.Equals(newUnit.UnitCode))
                                {
                                    // 손에 든 기물 == 설치된 기물 -> 레벨업
                                    // 레벨업이 될거라는 느낌의 이펙트 줘야 함
                                    targetTile = curDetect;
                                    targetTile.PreviewUnit(newUnit);
                                    return;
                                }
                            }
                            // 식별된 애 위에 기물이 없다 or 손에 든 기물 != 설치된 기물 -> 지우기
                            targetTile = null;
                        }
                    }
                    else
                    {
                        // 이전 타일 = 지금 타일
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
                    else
                    {
                        newUnit.gameObject.SetActive(false);
                        if (curDetect.IsInstallable && curDetect.UnitCode != null)
                        {
                            // 지금 식별된 애 타일 위에 이미 설치된 기물이 있음
                            if (curDetect.UnitCode.Equals(newUnit.UnitCode))
                            {
                                // 손에 든 기물 == 설치된 기물 -> 레벨업
                                targetTile = curDetect;
                                targetTile.PreviewUnit(newUnit);
                                return;
                            }
                        }
                    }
                }
                return;
            }
            // 이번에 감지 안됨
            if (targetTile != null)
            {
                // 이전 감지된 애 있음 = 미리보기 꺼주기 + 해당 기물도 꺼주기
                targetTile.ClearPreview();
                targetTile = null;
                newUnit.gameObject.SetActive(false);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (newUnit != null)
            {
                newUnit.Clear();
            }
            newUnit = UnitManager.Instance.GetNewUnit().Init(info.GetLiveInfo(), false);
            UIManager.Instance.InitUnitInfo(info);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            UIManager.Instance.ClearUnitInfo();
            if (newUnit != null && targetTile != null)
            {
                targetTile.InstallUnit(newUnit);
                targetTile = null;
                newUnit = null;
                UIManager.Instance.FinishChoice();
            }
        }
    }
}
