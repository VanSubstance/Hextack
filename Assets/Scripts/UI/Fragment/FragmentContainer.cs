using Assets.Scripts.UI.Swiper;
using System.Linq;

namespace Assets.Scripts.UI.Fragment
{
    /// <summary>
    /// 프레그먼트 컨트롤러
    /// </summary>
    public class FragmentContainer : AbsSwiperContainer<object>
    {
        public override void Init()
        {
            // 프레그먼트들 초기화
            ContentList.All((frag) =>
            {
                frag?.Init(null);
                return true;
            });
            // 시작 프레그먼트로 이동
            GoToContent(1, false);
        }
    }
}
