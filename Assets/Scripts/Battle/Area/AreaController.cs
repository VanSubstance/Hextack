using Assets.Scripts.Battle.Projectile;
using Assets.Scripts.Tower;
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
            if (CrEffects != null)
            {
                while (CrEffects.TryDequeue(out Coroutine cr))
                {
                    ServerManager.Instance.StopCoroutine(cr);
                }
                CrEffects = null;
            }
            info = null;
        }

        protected override bool InitExtra(AreaInfo _info)
        {
            info = _info;
            transform.position = new Vector3(_info.targetPos.x, .2f, _info.targetPos.z);
            // 각 효과 코루틴 실행
            CrEffects = new Queue<Coroutine>();
            DamageEffectInfo eff = info.damageEffect;
            // 각 데미지 효과 별 코루틴 실행
            CrEffects.Enqueue(ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                // 장판 내 적들에게 데미지 
                Collider[] cols;
                if ((cols = Physics.OverlapSphere(transform.position, (.5f + info.range) * (1 + (ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Range] * .05f)), GlobalDictionary.Layer.Monster)).Length == 0)
                {
                    return;
                }
                foreach (Collider col in cols)
                {
                    foreach (DamageEffectInfo.Token tk in eff.tokens)
                    {
                        switch (tk.damageEffectType)
                        {
                            case DamageEffectType.Damage:
                                col.GetComponent<Monster.MonsterController>().ApplyHp(
                                    (int)(
                                        tk.Amount
                                            * (1 + (.5f * ServerData.InGame.LevelUpgradeTower[info.towerType]))
                                            * (1 + (.05f * ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Damage]))
                                    ),
                                    Random.Range(0f, 1f) < GlobalStatus.InGame.RateCritical, info.towerType);
                                break;
                            case DamageEffectType.Speed:
                                col.GetComponent<Monster.MonsterController>().ApplySpeed(tk.Amount);
                                break;
                        }
                    }
                }
                cols = null;
            }, null, null, Mathf.Max(.2f, eff.Cooltime - (ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.AttackSpeed] * .02f))));
            EffectManager.Instance.ExecutNewEffect("Slow", transform.position, info.color, (.5f + info.range) * (1 + (ServerData.Saving.GoldUpgradeLevel[_info.towerType][TowerUpgradeType.Range] * .05f)), info.duration);

            // 장판 지속시간 체크 코루틴 실행
            if (info.duration > 0)
            {
                ServerManager.Instance.ExecuteWithDelay(() =>
                {
                    ReturnToPool();
                }, info.duration);
            }
            return true;
        }
    }
}
