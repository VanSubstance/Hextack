using Assets.Scripts.Battle.Monster;
using Assets.Scripts.Battle.Projectile;
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
        private Queue<Coroutine> atkQ;
        public override void Clear()
        {
            while (atkQ.TryDequeue(out Coroutine atk))
            {
                ServerManager.Instance.StopCoroutine(atk);
            }
        }

        private void OnMouseDown()
        {
            Debug.Log("다운");
        }

        private void OnMouseUp()
        {
            Debug.Log("업");
        }

        protected override bool InitExtra(TowerInfo _info)
        {
            towerInfo = ServerData.Tower.data[_info.Code].Clone();
            // 메쉬 + 메테리얼 연결
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            GetComponent<MeshRenderer>().materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
            transform.position = new Vector3(_info.Position.x, 0, _info.Position.z);
            gameObject.SetActive(true);
            atkQ = new Queue<Coroutine>();
            foreach (ProjectileInfo prj in towerInfo.projectileInfo)
            {
                // 투사체 종류 별 코루틴 부여
                atkQ.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
                {
                    // 공격 대상 탐색
                    Collider[] cols;
                    if ((cols = Physics.OverlapSphere(transform.position, 1 + prj.Range, GlobalDictionary.Layer.Monster)).Length == 0)
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
                            targetTr.GetComponent<MonsterController>().ApplyHp((int)tprj.effectInfo.Amount, Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical);
                        };
                        tprj.targetTr = cols[idx].transform;
                        ProjectileManager.Instance.GetNewContent(tprj);
                        idx++;
                    }
                }, () => false, null, prj.effectInfo.Cooltime));
            }
            return true;
        }
    }
}
