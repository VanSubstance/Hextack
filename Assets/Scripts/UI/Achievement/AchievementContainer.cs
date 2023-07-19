using Assets.Scripts.Tower;
using Assets.Scripts.UI.Swiper;
using UnityEngine;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContainer : AbsSwiperContainer<AchievementInfo>
    {
        [SerializeField]
        private AchievementContent contentPrefab;

        public override void Init()
        {
            // 각 업적 별로 하나씩 진행

            // 1티어 전부 모았는지
            AchievementContent newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "티끌모아 피클",
                Desc = "1티어 타워 종류별로 전부 모으기",
                ActionCondition = () =>
                {
                    // 지금 라이브 타워에 1티어가 전부 있는가 ?
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30골드 수령 딱
                }
            });

            // 2티어 전부 모였는지
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "피클모아 피자",
                Desc = "2티어 타워 종류별로 전부 모으기",
                ActionCondition = () =>
                {
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 40골드 수령 딱
                }
            });

            // 4티어 최초
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "4학년 입학",
                Desc = "4티어 타워 최초 설치",
                ActionCondition = () =>
                {
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 50골드 수령 딱
                }
            });

            // 같은 종류의 타워 4개 모으기
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "나는 ,,, 이게 좋아 ,,,",
                Desc = "같은 종류의 타워 4개 모으기",
                ActionCondition = () =>
                {
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30골드 수령 딱
                }
            });

            // 필드 위 몬스터가 30마리 이상으로 진입
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "한꺼번에 들어와라",
                Desc = "필드 위 몬스터가 30마리 이상으로 진입",
                ActionCondition = () =>
                {
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30골드 수령 딱
                }
            });

            // 골드량이 100 이상으로 진입
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "저축왕",
                Desc = "골드량이 100 이상으로 진입",
                ActionCondition = () =>
                {
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30골드 수령 딱
                }
            });

            Close();
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
