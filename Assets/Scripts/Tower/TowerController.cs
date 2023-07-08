using Assets.Scripts.Battle;
using Assets.Scripts.Battle.Monster;
using Assets.Scripts.Battle.Projectile;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Tower
{
    public class TowerController : AbsPoolingContent
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

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            towerInfo = ServerData.Tower.data[info.Code].Clone();
            // 메쉬 + 메테리얼 연결
            GetComponent<MeshCollider>().sharedMesh = GetComponent<MeshFilter>().mesh = GlobalDictionary.Mesh.Tower.data[towerInfo.Code];
            GetComponent<MeshRenderer>().materials = towerInfo.Materials.Select((code) => { return GlobalDictionary.Materials.data[code]; }).ToArray();
            transform.position = new Vector3(info.Position.x, 0, info.Position.z);
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
                        ProjectileManager.Instance.ExecuteNewProjectile(new ProjectileController.Info()
                        {
                            ActionEnd = (targetTr) =>
                            {
                                targetTr.GetComponent<MonsterController>().ApplyHp(prj.Damage, Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical);
                            },
                            color = prj.color,
                            targetTr = cols[idx].transform,
                            StartPos = transform.position,
                            Spd = prj.Spd,
                            TrailType = prj.trailType,
                        });
                        idx++;
                    }
                }, () => false, null, prj.Cooltime));
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
        }

        public new class Info : AbsPoolingContent.Info
        {
            public string Code;
            public Vector3 Position;
        }
    }
}
