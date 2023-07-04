using Assets.Scripts.Unit;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Window.Result
{
    public class UnitStatisticController : MonoBehaviour
    {
        [SerializeField]
        private Image image;
        [SerializeField]
        private TextMeshProUGUI textUsage, textDamage;
        [SerializeField]
        private GageController gage;

        //private void Awake()
        //{
        //    gameObject.SetActive(false);
        //}

        /// <summary>
        /// 유닛 데이터를 통한 초기화
        /// </summary>
        /// <param name="info"></param>
        /// <param name="maxDamage"></param>
        public void Init(UnitInfo info, int maxDamage)
        {
            image.sprite = GlobalDictionary.Texture.Unit.data[info.Code];
            gage.Init(maxDamage, info.AccuDamage, null, null, info.AbilityInfos[0].type.Equals(AbilityType.Heal) ? new Color(0, .9f, .6f, 1) : Color.red);
            gage.gameObject.SetActive(true);
            textUsage.text = $"{info.CountSummon}회 사용";
            textDamage.text = $"{string.Format("{0:N0}", info.AccuDamage)}";
            gameObject.SetActive(true);
        }
    }
}
