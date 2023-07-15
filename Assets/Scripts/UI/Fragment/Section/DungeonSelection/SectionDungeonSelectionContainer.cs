using Assets.Scripts.Dungeon;
using Assets.Scripts.UI.Swiper.List;
using System.Collections.Generic;

namespace Assets.Scripts.UI.Fragment.Section
{
    public class SectionDungeonSelectionContainer : AbsListViewContainer<DungeonInfo>
    {
        public override void InitExtra()
        {
            foreach(KeyValuePair<string, DungeonInfo> info in ServerData.Dungeon.data)
            {
                GetNewContent(info.Value);
            }
        }
    }
}
