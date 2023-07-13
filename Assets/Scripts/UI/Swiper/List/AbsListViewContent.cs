namespace Assets.Scripts.UI.Swiper.List
{
    public abstract class AbsListViewContent<TInfo> : AbsSwiperContent<TInfo>
    {
        private System.Action<AbsListViewContent<TInfo>> ActionReturnToPool;

        public override void Init(TInfo _info)
        {
            InitExtra(_info);
            gameObject.SetActive(true);
        }

        public void ConnectWithParent(System.Action<AbsListViewContent<TInfo>> _ActionReturnToPool)
        {
            ActionReturnToPool = _ActionReturnToPool;
        }

        /// <summary>
        /// 풀에 반납
        /// </summary>
        public void ReturnToPool()
        {
            ActionReturnToPool(this);
            gameObject.SetActive(false);
        }

        public abstract void InitExtra(TInfo _info);
    }
}
