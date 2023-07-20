using Assets.Scripts.Battle;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.Swiper;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.UI.Achievement
{
    public class AchievementContainer : AbsSwiperContainer<AchievementInfo>
    {
        [SerializeField]
        private AchievementContent contentPrefab;
        public List<AchievementContent> Achievements;

        public override void Init()
        {
            Achievements = new List<AchievementContent>();
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
                    HashSet<string> codeSet = new HashSet<string>();
                    TowerManager.Instance.TowerLiveList.ForEach((tower) =>
                    {
                        if (tower.TowerInfo.Tier == 1)
                        {
                            codeSet.Add(tower.TowerInfo.Code);
                        }
                    });
                    if (codeSet.Count >= 6)
                    {
                        return true;
                    }
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 30;
                },
                TargetResource = AchievementInfo.TargetResourceType.Tower,
            });
            Achievements.Add(newAchiv);

            // 2티어 전부 모였는지
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "피클모아 피자",
                Desc = "2티어 타워 종류별로 전부 모으기",
                ActionCondition = () =>
                {
                    HashSet<string> codeSet = new HashSet<string>();
                    TowerManager.Instance.TowerLiveList.ForEach((tower) =>
                    {
                        if (tower.TowerInfo.Tier == 2)
                        {
                            codeSet.Add(tower.TowerInfo.Code);
                        }
                    });
                    if (codeSet.Count >= 6)
                    {
                        return true;
                    }
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 40석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 40;
                },
                TargetResource = AchievementInfo.TargetResourceType.Tower,
            });
            Achievements.Add(newAchiv);

            // 4티어 최초
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "4학년 입학",
                Desc = "4티어 타워 최초 설치",
                ActionCondition = () =>
                {
                    foreach (TowerController tower in TowerManager.Instance.TowerLiveList)
                    {
                        if (tower.TowerInfo.Tier == 4)
                        {
                            return true;
                        }
                    }
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 50석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 50;
                },
                TargetResource = AchievementInfo.TargetResourceType.Tower,
            });
            Achievements.Add(newAchiv);

            // 같은 종류의 타워 4개 모으기
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "나는 ,,, 이게 좋아 ,,,",
                Desc = "같은 종류의 타워 4개 모으기",
                ActionCondition = () =>
                {
                    Dictionary<string, int> countDict = new Dictionary<string, int>();
                    foreach (TowerController tower in TowerManager.Instance.TowerLiveList)
                    {
                        if (!countDict.ContainsKey(tower.Code))
                        {
                            countDict.Add(tower.Code, 1);
                        }
                        countDict[tower.Code]++;
                        if (countDict[tower.Code] >= 4) return true;
                    }
                    return false;
                },
                ActionAchieve = () =>
                {
                    // 30석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 30;
                },
                TargetResource = AchievementInfo.TargetResourceType.Tower,
            });
            Achievements.Add(newAchiv);

            // 필드 위 몬스터가 30마리 이상으로 진입
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "한꺼번에 들어와라",
                Desc = "필드 위 몬스터가 30마리 이상으로 진입",
                ActionCondition = () =>
                {
                    return ServerData.InGame.CountMonsterLive >= 30;
                },
                ActionAchieve = () =>
                {
                    // 30석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 30;
                },
                TargetResource = AchievementInfo.TargetResourceType.Monster,
            });
            Achievements.Add(newAchiv);

            // 석재량이 100 이상으로 진입
            newAchiv = Instantiate(contentPrefab, ContentParentTr).GetComponent<AchievementContent>();
            newAchiv.Init(new AchievementInfo()
            {
                Title = "피라미드라도 쌓을까",
                Desc = "석재량이 100 이상으로 진입",
                ActionCondition = () =>
                {
                    return ServerData.InGame.AmountStone >= 100;
                },
                ActionAchieve = () =>
                {
                    // 30석재 수령 딱
                    CommonInGameManager.Instance.AmountStone += 30;
                },
                TargetResource = AchievementInfo.TargetResourceType.Stone,
            });
            Achievements.Add(newAchiv);

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
