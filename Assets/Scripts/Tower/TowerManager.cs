using Assets.Scripts.UI.Achievement;
using Assets.Scripts.UI.Manager;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerManager : AbsPoolingManager<TowerManager, TowerInfo>
    {
        private Dictionary<string, Queue<TowerController>> qForTowers = new Dictionary<string, Queue<TowerController>>();
        public List<TowerController> TowerLiveList = new List<TowerController>();
        private Coroutine CrSustainTowerLastClicked;
        public override Transform GetParent()
        {
            return transform;
        }
        public override int GetCountPoolForFirst()
        {
            return 10;
        }

        /// <summary>
        /// 설치된 타워 중 같은 타워 리스트 반환
        /// </summary>
        /// <param name="tower"></param>
        /// <returns></returns>
        public List<TowerController> FindSameTowers(TowerController tower)
        {
            return TowerLiveList.Where((towerC) =>
            {
                return towerC.Code.Equals(tower.Code);
            }).ToList();
        }

        /// <summary>
        /// 마지막으로 선택한 타워 유지시간 = 0.5초
        /// </summary>
        public void SustainTowerLastClicked()
        {
            if (CrSustainTowerLastClicked != null)
            {
                ServerManager.Instance.StopCoroutine(CrSustainTowerLastClicked);
                CrSustainTowerLastClicked = null;
            }
            CrSustainTowerLastClicked = ServerManager.Instance.ExecuteWithDelay(() =>
            {
                ServerData.InGame.LastTowerClicked = null;
            }, .5f);
        }

        public new TowerController GetNewContent(TowerInfo _info)
        {
            if (!qForTowers.ContainsKey(_info.Code))
            {
                qForTowers[_info.Code] = new Queue<TowerController>();
            }
            if (!qForTowers[_info.Code].TryDequeue(out TowerController res))
            {
                // 신규 인스턴스
                GameObject newInst = Instantiate(GlobalDictionary.Prefab.Tower.data[_info.Code], GetParent());
                newInst.transform.localScale = Vector3.one * .6f;
                res = newInst.AddComponent<TowerController>();
                Material[] ms = res.GetComponentInChildren<MeshRenderer>().materials;
                ms[0] = GlobalDictionary.Materials.data[_info.Tier == 4 ? "Purple" : _info.Tier == 3 ? "Yellow" : _info.Tier == 2 ? "Blue" : "White"];
                res.GetComponentInChildren<MeshRenderer>().materials = ms;
                res.GetComponent<BoxCollider>().center = Vector3.up;
                res.GetComponent<BoxCollider>().size = new Vector3(2, 2, 1.723f);
            }
            res.ConnectWithParent((content) =>
            {
                qForTowers[_info.Code].Enqueue(content as TowerController);
            });
            res.Init(_info);
            UIInGameManager.Instance.AchievementContainer.Achievements.ForEach((ach) =>
            {
                if (ach.ResourceType.Equals(AchievementInfo.TargetResourceType.Tower))
                {
                    ach.UpdateCondition();
                }
            });
            return res;
        }

        protected new void CreatePool()
        {
            // 풀링 생성 안함
        }

        public static Color GetColorByTowerType(TowerType type)
        {
            return type switch
            {
                TowerType.Machine => new Color(.35f, .395f, .452f),
                TowerType.Bio => new Color(1, .825f, .457f),
                TowerType.Magic => new Color(1, .458f, .877f),
                _ => Color.white,
            };
        }
    }
}
