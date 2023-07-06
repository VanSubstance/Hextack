using UnityEngine;
using Assets.Scripts.Unit;

namespace Assets.Scripts.UI.Fragment.Storage
{
    public class StorageFragmentController : SwiperFragmentController
    {
        [SerializeField]
        private StorageSectionController storageSection;
        [SerializeField]
        private DeckSectionController deckSection;

        /// <summary>
        /// 창고 프레그먼트 컨텐츠 초기화
        /// </summary>
        public override void Init()
        {
            storageSection.Init();
            deckSection.Init();
        }
    }
}
