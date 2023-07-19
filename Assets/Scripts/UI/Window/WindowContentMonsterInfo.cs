using Assets.Scripts.Monster;
using TMPro;
using Assets.Scripts.Tower;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace Assets.Scripts.UI.Window
{
    public class WindowContentMonsterInfo : AbsWindowContent<MonsterInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc, textEffect;

        [SerializeField]
        private Image image;

        public override AbsWindowContent<MonsterInfo> CloseExtra()
        {
            ServerData.InGame.LastTowerClicked = null;
            return this;
        }

        public override AbsWindowContent<MonsterInfo> Init(MonsterInfo parameter)
        {
            image.sprite = GlobalDictionary.Texture.Monster.data[parameter.Code];
            textTitle.text = parameter.Name;
            textDesc.text = parameter.Desc;
            string eft = $"";

            // 기본 효과
            eft += $"전체 체력 : {parameter.MaxHp}\t기본 이동속도 : {parameter.MaxSpd}\n";
            // 저항
            eft += $"{string.Join(", ", parameter.TowerResist.Select((type) => TowerManager.Instance.TranslateTowerType(type)))} 세력의 타워에 대해 저항을 가집니다. (피해량 25% 감소)\n";
            eft += $"{string.Join(", ", parameter.TowerWeak.Select((type) => TowerManager.Instance.TranslateTowerType(type)))} 세력의 타워에 대해 약점을 가집니다. (피해량 25% 증가)\n";

            textEffect.text = eft;
            gameObject.SetActive(true);
            return this;
        }
    }
}
