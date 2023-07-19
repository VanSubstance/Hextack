using Assets.Scripts.Battle;
using Assets.Scripts.Tower;
using Assets.Scripts.UI.DamageText;
using Assets.Scripts.UI.Manager;
using Assets.Scripts.UI.Window;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Monster
{
    public class MonsterController : AbsPoolingContent<MonsterInfo>
    {
        [SerializeField]
        private NavMeshAgent agent;
        private float baseSpeed, rateSpeed;
        private bool IsBoss;
        private MonsterInfo info;
        private Queue<Transform> destQ;
        private Coroutine CrDestinationCheck, CrSpeedLack;

        /// <summary>
        /// 스크린 투영 좌표
        /// </summary>
        public Vector3 ScreenPos
        {
            get
            {
                return transform.position + (GlobalDictionary.VectorToScreen * 2);
            }
        }

        public override void Clear()
        {
            if (IsBoss)
            {
                ServerData.InGame.AccuGear++;
            }
            else
            {
                if (Random.Range(0f, 1f) < .01f)
                {
                    ServerData.InGame.AccuGear++;
                }
            }
            CommonInGameManager.Instance.AmountStone += IsBoss ? 30 : 1;
            ServerData.InGame.CountMonsterLive--;
            ServerData.InGame.CountMonsterKill++;
            if (CrDestinationCheck != null)
            {
                ServerManager.Instance.StopCoroutine(CrDestinationCheck);
            }
        }

        /// <summary>
        /// HP 감소치 적용
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyHp(int damage, bool isCrit, TowerType towerType)
        {
            if (info.Hp <= 0)
            {
                return;
            }
            // 크리티컬 데미지 연산
            damage = (int)(damage * (isCrit ? 1.5f : 1f));
            // 타워 타입 별 저항 연산
            if (info.TowerWeak.Contains(towerType))
            {
                // 데미지 추가
                damage = (int)(damage * 1.25f);
            }
            if (info.TowerResist.Contains(towerType))
            {
                // 데미지 경감
                damage = (int)(damage * .75f);
            }
            ServerData.InGame.AmountDealByCategory[towerType] += damage;

            // 데미지 텍스트 띄워주기
            TextManager.Instance.GetNewContent(new()
            {
                ScreenPos = ScreenPos,
                TargetText = $"{Mathf.Abs(damage)}",
                TextColor = isCrit ? new Color(1, .8f, 0, 1) : Color.white,
                Time = .5f,
                SizeMultiplier = isCrit ? 1.4f : 1.3f
            });

            // 데미지 이펙트 띄워주기
            EffectManager.Instance.ExecutNewEffect("Hit", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            info.Hp -= damage;
            if (info.Hp <= 0)
            {
                // 죽음
                ReturnToPool();
            }
        }

        /// <summary>
        /// 이동속도 감소율 적용
        /// </summary>
        /// <param name="rateSlow"></param>
        public void ApplySpeed(float rateSlow)
        {
            if (info.Hp <= 0)
            {
                return;
            }
            rateSpeed = Mathf.Max(rateSpeed, rateSlow);
            agent.speed = baseSpeed * (1 - rateSpeed);
            // 갱신된 속도로 신규 코루틴 시작
            if (CrSpeedLack != null)
            {
                // 기존 코루틴이 있다 = 파기
                ServerManager.Instance.StopCoroutine(CrSpeedLack);
            }
            // 다시 타이머 시작: 모든 슬로우는 [.25]초간 지속된다
            CrSpeedLack = ServerManager.Instance.ExecuteWithDelay(() =>
            {
                agent.speed = baseSpeed;
            }, .25f);
            // 슬로우 텍스트 띄우기
            TextManager.Instance.GetNewContent(new()
            {
                ScreenPos = ScreenPos + (Vector3.up * 1),
                TargetText = $"느려짐!",
                TextColor = Color.gray,
                Time = .5f,
                SizeMultiplier = 1.1f
            });
            // 슬로우 이펙트 띄우기
            //EffectManager.Instance.ExecutNewEffect("Hit", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
        }

        protected override bool InitExtra(MonsterInfo _info)
        {
            destQ = new Queue<Transform>();
            foreach (Transform v in _info.Tracks)
            {
                destQ.Enqueue(v);
            }
            info = _info.Clone();
            baseSpeed = agent.speed = _info.Spd;
            IsBoss = _info.CntMonsterSummoned == 1;
            transform.position = destQ.Dequeue().position;
            gameObject.SetActive(true);
            CrDestinationCheck = ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                // 다음 행선지로 이동해야 하는가?
                if (gameObject.activeSelf && agent.remainingDistance < 1f)
                {
                    // 지금 목표 도착
                    if (destQ.TryDequeue(out Transform nextPos))
                    {
                        // 다음 목표가 있다 = 이동
                        agent.SetDestination(nextPos.position);
                        return;
                    }
                    // 다음 목표가 없다 = 목숨 차감 후 파기 !
                    UIInGameManager.Instance.ApplyLife(IsBoss);
                    ReturnToPool();
                }
            }, null, null, .1f);
            return true;
        }

        private void OnMouseUp()
        {
            WindowContainer.Instance.Open(info);
        }
    }
}
