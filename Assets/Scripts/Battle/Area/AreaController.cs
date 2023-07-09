using Assets.Scripts.Battle.Projectile;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle.Area
{
    public class AreaController : AbsPoolingContent<AreaInfo>
    {
        /// <summary>
        /// 반지름
        /// </summary>
        private AreaInfo info;
        private Queue<Coroutine> CrEffects;

        public float Range
        {
            get
            {
                return info.range;
            }
        }

        public override void Clear()
        {
            CrEffects = null;
            info = null;
        }

        protected override bool InitExtra(AreaInfo _info)
        {
            info = _info;
            transform.position = new Vector3(_info.targetPos.x, 0, _info.targetPos.z);
            // 각 효과 코루틴 실행
            CrEffects = new Queue<Coroutine>();
            foreach (DamageEffectInfo eff in info.damageEffects)
            {
                switch (eff.damageEffectType)
                {
                    case DamageEffectType.Damage:
                        CrEffects.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
                        {
                            // 장판 내 적들에게 데미지 계산
                            Collider[] cols;
                            if ((cols = Physics.OverlapSphere(transform.position, info.range, GlobalDictionary.Layer.Monster)).Length > 0)
                            {
                                foreach (Collider col in cols)
                                {
                                    col.GetComponent<Monster.MonsterController>().ApplyHp((int)eff.Amount, Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical);
                                }
                            }
                        }, null, null, eff.Cooltime));
                        break;
                    case DamageEffectType.Speed:
                        CrEffects.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
                        {
                            // 장판 내 적들 슬로우 부여
                            Collider[] cols;
                            if ((cols = Physics.OverlapSphere(transform.position, info.range, GlobalDictionary.Layer.Monster)).Length > 0)
                            {
                                foreach (Collider col in cols)
                                {
                                    col.GetComponent<Monster.MonsterController>().ApplySpeed(eff.Amount);
                                }
                            }
                        }, null, null, eff.Cooltime));
                        break;
                }
            }

            // 장판 지속시간 체크 코루틴 실행
            ServerManager.Instance.ExecuteWithDelay(() =>
            {
                while (CrEffects.TryDequeue(out Coroutine cr))
                {
                    ServerManager.Instance.StopCoroutine(cr);
                }
                ReturnToPool();
            }, info.duration);
            return true;
        }
    }
}
