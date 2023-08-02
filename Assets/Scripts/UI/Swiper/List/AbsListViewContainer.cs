using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Swiper.List
{
    public abstract class AbsListViewContainer<TParam> : AbsSwiperContainer<TParam>
    {
        [SerializeField]
        private AbsListViewContent<TParam> contentPrefab;
        protected Queue<AbsListViewContent<TParam>> contentQueue;

        public override void Init()
        {
            contentQueue = new Queue<AbsListViewContent<TParam>>();
            InitExtra();
        }

        /// <summary>
        /// 신규 생성 및 초기화
        /// </summary>
        /// <param name="_info"></param>
        public void GetNewContent(TParam _info)
        {
            if (!contentQueue.TryDequeue(out AbsListViewContent<TParam> ret))
            {
                ret = Instantiate(contentPrefab, ContentParentTr);
            }
            ret.Init(_info);
            ContentList.Add(ret);
        }

        /// <summary>
        /// 추가 초기화
        /// </summary>
        public abstract void InitExtra();
    }
}
