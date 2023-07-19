using Assets.Scripts.Battle;
using Assets.Scripts.Battle.Area;
using Assets.Scripts.Battle.Projectile;
using Assets.Scripts.Dungeon;
using Assets.Scripts.Monster;
using Assets.Scripts.UI.Window;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    [RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
    public class TowerController : AbsPoolingContent<TowerInfo>
    {
        private TowerInfo towerInfo;
        public TowerInfo TowerInfo
        {
            get { return towerInfo; }
        }
        public string Code
        {
            get
            {
                return towerInfo.Code;
            }
        }

        public float Range
        {
            get
            {
                return towerInfo.projectileInfo[0].Range;
            }
        }
        private Queue<Coroutine> atkQ;
        private AreaController AreaInCase;
        private TileController tileInstalled;
        public override void Clear()
        {
            if (atkQ != null)
            {
                while (atkQ.TryDequeue(out Coroutine atk))
                {
                    ServerManager.Instance.StopCoroutine(atk);
                }
            }
            if (AreaInCase != null)
            {
                AreaInCase.ReturnToPool();
            }
        }

        protected override bool InitExtra(TowerInfo _info)
        {
            tileInstalled = _info.TileInstalled;
            _info.TileInstalled = null;
            towerInfo = ServerData.Tower.data[_info.Code].Clone();
            // 메쉬 + 메테리얼 연결
            /*GetComponent<MeshCollider>().sharedMesh = */
            //GetComponent<MeshFilter>().mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            //GetComponent<MeshRenderer>().materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
            transform.position = new Vector3(_info.Position.x, 0, _info.Position.z);
            gameObject.SetActive(true);
            atkQ = new Queue<Coroutine>();
            AreaInCase = null;
            foreach (ProjectileInfo prj in towerInfo.projectileInfo)
            {
                if (prj.executeType.Equals(ProjectileExecuteType.Aura))
                {
                    // 아우라 = 투사체 없음
                    AreaInfo temp = prj.GetAreaInfo();
                    temp.targetPos = transform.position;
                    temp.towerType = towerInfo.towerType;
                    AreaInCase = AreaManager.Instance.GetNewContent(temp) as AreaController;
                    continue;
                }
                // 투사체 종류 별 코루틴 부여
                atkQ.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
                {
                    // 공격 대상 탐색
                    Collider[] cols;
                    if ((cols = Physics.OverlapSphere(transform.position, prj.Range * (1 + (ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Range] * .05f)), GlobalDictionary.Layer.Monster)).Length == 0)
                    {
                        return;
                    }
                    // 사거리 내 몬스터 존재
                    // 첫번째 대상 바라보기
                    int idx = 0;
                    Stare(cols[idx].transform.position);
                    while (prj.CountPerOnce > idx)
                    {
                        ProjectileInfo tprj = prj.Clone();
                        if (prj.executeType.Equals(ProjectileExecuteType.Lazer))
                        {
                            // 레이저 = 투사체 없음
                            // 이펙트 on
                            EffectManager.Instance.ExecutNewEffect("Lazer", transform.position, Color.yellow, 50, .5f, cols[idx].transform.position);
                            // 레이캐스트 = 걸리는 몬스터 전부 데미지
                            RaycastHit[] res;
                            if ((res = Physics.BoxCastAll(transform.position, Vector3.one, cols[idx].transform.position, Quaternion.identity, 100, GlobalDictionary.Layer.Monster)).Length > 0)
                            {
                                // 걸린 모든 몬스터에게 영향 부여
                                foreach (MonsterController targetTr in res.Select((ress) => { return ress.transform.GetComponent<MonsterController>(); }))
                                {
                                    foreach (DamageEffectInfo.Token tk in tprj.effectInfo.tokens)
                                    {
                                        switch (tk.damageEffectType)
                                        {
                                            case DamageEffectType.Damage:
                                                // 데미지 계산
                                                targetTr.ApplyHp(
                                                    (int)(
                                                        tk.Amount
                                                            * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[towerInfo.towerType]))
                                                            * (1 + (.05f * ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Damage]))
                                                    ),
                                                    Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical, towerInfo.towerType);
                                                break;
                                            case DamageEffectType.Speed:
                                                // 이동속도 저하 = 누적 X, Max(기존 슬로우, 신규 슬로우) 적용
                                                targetTr.ApplySpeed(tk.Amount);
                                                break;
                                        }
                                    }
                                }
                            }
                            idx++;
                            continue;
                        }
                        tprj.StartPos = transform.position;
                        tprj.ActionEnd = (targetTr) =>
                        {
                            foreach (DamageEffectInfo.Token tk in tprj.effectInfo.tokens)
                            {
                                switch (tk.damageEffectType)
                                {
                                    case DamageEffectType.Damage:
                                        // 데미지 계산
                                        targetTr.ApplyHp(
                                            (int)(
                                                tk.Amount
                                                    * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[towerInfo.towerType]))
                                                    * (1 + (.05f * ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Damage]))
                                            ),
                                            Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical, towerInfo.towerType);
                                        break;
                                    case DamageEffectType.Speed:
                                        // 이동속도 저하 = 누적 X, Max(기존 슬로우, 신규 슬로우) 적용
                                        targetTr.ApplySpeed(tk.Amount);
                                        break;
                                }
                            }
                        };
                        tprj.targetTr = cols[idx].transform.GetComponent<MonsterController>();
                        ProjectileManager.Instance.GetNewContent(tprj);
                        idx++;
                    }
                }, () => false, null, Mathf.Max(.2f, prj.effectInfo.Cooltime - (ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.AttackSpeed] * .02f))));
            }
            TowerManager.Instance.TowerLiveList.Add(this);
            return true;
        }

        /// <summary>
        /// 해당 좌표 방향 바라보기
        /// </summary>
        /// <param name="targetPos"></param>
        private void Stare(Vector3 targetPos)
        {
            transform.LookAt(targetPos);
        }

        /// <summary>
        /// 타워 파기
        /// </summary>
        public void CollabseTower()
        {
            tileInstalled.RemoveTower();
        }

        private void OnMouseDown()
        {
        }

        private void OnMouseUp()
        {
            if (towerInfo.Tier != 4 && ServerData.InGame.LastTowerClicked != null && ServerData.InGame.LastTowerClicked.Equals(this))
            {
                // 여기에 업그레이드 필요
                List<TowerController> temp;
                if ((temp = TowerManager.Instance.FindSameTowers(this)).Count >= 2)
                {
                    // 1. 합칠 애가 있는가 ? = 이 타워와 같은 타워가 이거 포함 2개 이상인가 ?
                }
                foreach (TowerController curT in temp)
                {
                    if (!curT.Equals(this))
                    {
                        // 얘랑 합친다
                        WindowContainer.Instance.Close();
                        // 2. 합치기 = 양쪽 다 파기 -> 목표 타일에 다음 티어 타워 설치
                        CollabseTower();
                        curT.CollabseTower();
                        tileInstalled.BuildTower(towerInfo.Tier + 1);
                        return;
                    }
                }
                return;
            }
            WindowContainer.Instance.Open(towerInfo);
            ServerData.InGame.LastTowerClicked = this;
            TowerManager.Instance.SustainTowerLastClicked();
        }
    }
}
