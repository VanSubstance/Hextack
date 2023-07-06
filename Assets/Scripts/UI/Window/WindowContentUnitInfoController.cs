using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.Common.MainManager;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentUnitInfoController : MonoBehaviour, IInitiable<UnitInfo>
    {
        [SerializeField]
        private InfoController basicInfo;
        [SerializeField]
        private Button btnToggleEquip;
        private UnitInfo curInfo;
        public void Init(UnitInfo param)
        {
            basicInfo.Clear();
            curInfo = param;
            MainMainManager.Instance.IsTryingEquip = false;
            btnToggleEquip.image.color = Color.white;
            if (curInfo == null)
            {
                gameObject.SetActive(false);
                return;
            }
            basicInfo.Init(curInfo);
            gameObject.SetActive(true);
        }

        /// <summary>
        /// 장착 시도 상태 on/off
        /// </summary>
        public void ToggleTryingEquip()
        {
            if (MainMainManager.Instance.IsTryingEquip)
            {
                // 시도중임 = 시도 끔
                MainMainManager.Instance.IsTryingEquip = false;
                MainMainManager.Instance.CurrentSelectedUnitInfo = null;
                btnToggleEquip.image.color = Color.white;
            }
            else
            {
                // 시도중이 아님 = 시도 시작
                MainMainManager.Instance.IsTryingEquip = true;
                MainMainManager.Instance.CurrentSelectedUnitInfo = curInfo;
                btnToggleEquip.image.color = new Color(0, 1, .8f, 1);
            }
        }
    }
}
