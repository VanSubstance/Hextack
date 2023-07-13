using Assets.Scripts.Dungeon;
using Assets.Scripts.UI.Swiper.List;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Fragment.Section.DungeonSelection
{
    public class SectionDungeonSelectionContent : AbsListViewContent<DungeonInfo>
    {
        [SerializeField]
        private TextMeshProUGUI textTitle, textDesc, textMaxRound;
        [SerializeField]
        private Image image;

        private DungeonInfo info;

        public override void InitExtra(DungeonInfo _info)
        {
            info = _info;
            textTitle.text = info.mapTitle;
            textDesc.text = info.Desc;
            textMaxRound.text = $"총 {info.MonsterCodeList.Length} 라운드";
        }

        /// <summary>
        /// 던전 시작
        /// </summary>
        public void StartDungeon()
        {
            ServerManager.Instance.EnterDungeon(info.Code);
        }
    }
}
