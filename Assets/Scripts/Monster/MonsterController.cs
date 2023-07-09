namespace Assets.Scripts.Monster
{
    /// <summary>
    /// 몬스터 풀링 컨텐츠
    /// </summary>
    public class MonsterController : AbsPoolingContent<MonsterInfo>
    {
        public override void Clear()
        {
        }

        protected override bool InitExtra(MonsterInfo _info)
        {
            return true;
        }
    }
}
