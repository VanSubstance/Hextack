using Assets.Scripts.Battle.Area;
using Assets.Scripts.Battle.Projectile;
using Assets.Scripts.Monster;
using Assets.Scripts.UI.Window;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerController : AbsPoolingContent<TowerInfo>
    {
        private TowerInfo towerInfo;
        public TowerInfo TowerInfo
        {
            get { return towerInfo; }
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
            towerInfo = ServerData.Tower.data[_info.Code].Clone();
            // 메쉬 + 메테리얼 연결
            /*GetComponent<MeshCollider>().sharedMesh = */
            GetComponent<MeshFilter>().mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            GetComponent<MeshRenderer>().materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
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
                    AreaInCase = AreaManager.Instance.GetNewContent(temp) as AreaController;
                    continue;
                }
                // 투사체 종류 별 코루틴 부여
                atkQ.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
                {
                    // 공격 대상 탐색
                    Collider[] cols;
                    if ((cols = Physics.OverlapSphere(transform.position, prj.Range, GlobalDictionary.Layer.Monster)).Length == 0)
                    {
                        return;
                    }
                    // 사거리 내 몬스터 존재
                    int idx = 0;
                    while (prj.CountPerOnce > idx)
                    {
                        ProjectileInfo tprj = prj.Clone();
                        tprj.StartPos = transform.position;
                        tprj.ActionEnd = (targetTr) =>
                        {
                            foreach (DamageEffectInfo.Token tk in tprj.effectInfo.tokens)
                            {
                                switch (tk.damageEffectType)
                                {
                                    case DamageEffectType.Damage:
                                        // 데미지 계산
                                        targetTr.ApplyHp((int)tk.Amount, Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical);
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
                }, () => false, null, prj.effectInfo.Cooltime));
            }
            return true;
        }

        private void OnMouseDown()
        {
            Debug.Log("다운");
        }

        private void OnMouseUp()
        {
            Debug.Log("업");
            WindowContainer.Instance.Open(towerInfo);
        }
    }
}
