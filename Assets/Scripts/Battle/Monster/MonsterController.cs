using Assets.Scripts.UI;
using Assets.Scripts.UI.Manager;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Battle.Monster
{
    public class MonsterController : AbsPoolingContent
    {
        [SerializeField]
        private NavMeshAgent agent;
        private int Hp;
        private bool IsBoss;
        private Queue<Vector3> destQ;
        private Coroutine CrDestinationCheck;

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
            UIInGameManager.Instance.CurrentCountMonster--;
            if (CrDestinationCheck != null)
            {
                ServerManager.Instance.StopCoroutine(CrDestinationCheck);
            }
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            destQ = new Queue<Vector3>();
            foreach (Vector3 v in info.Tracks)
            {
                destQ.Enqueue(v);
            }
            UIInGameManager.Instance.CurrentCountMonster++;
            agent.speed = info.Spd;
            Hp = info.Hp;
            IsBoss = info.IsBoss;
            transform.position = destQ.Dequeue();
            gameObject.SetActive(true);
            CrDestinationCheck = ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                // 다음 행선지로 이동해야 하는가?
                if (gameObject.activeSelf && agent.remainingDistance < 1f)
                {
                    // 지금 목표 도착
                    if (destQ.TryDequeue(out Vector3 nextPos))
                    {
                        // 다음 목표가 있다 = 이동
                        agent.SetDestination(nextPos);
                        return;
                    }
                    // 다음 목표가 없다 = 목숨 차감 후 파기 !
                    UIInGameManager.Instance.ApplyLife(IsBoss);
                    ReturnToPool();
                }
            }, null, null, .1f);
            return true;
        }

        /// <summary>
        /// HP 감소치 적용
        /// </summary>
        /// <param name="damage"></param>
        public void ApplyHp(int damage, bool isCrit)
        {
            // 데미지 텍스트 띄워주기
            damage = (int)(damage * (isCrit ? 1.5f : 1f));
            TextManager.Instance.ExecuteDamage(new TextController.Info()
            {
                ScreenPos = ScreenPos,
                TargetText = $"{Mathf.Abs(damage)}",
                TextColor = isCrit ? new Color(1, .8f, 0, 1) : Color.white,
                Time = .5f,
                SizeMultiplier = isCrit ? 1.4f : 1.3f
            });

            // 데미지 이펙트 띄워주기
            EffectManager.Instance.ExecutNewEffect("Hit", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            Hp -= damage;
            if (Hp <= 0)
            {
                // 죽음
                Debug.Log("몬스터 컷");
                ReturnToPool();
            }
        }

        public new class Info : AbsPoolingContent.Info
        {
            /// <summary>
            /// 최대 체력
            /// </summary>
            public int Hp;
            /// <summary>
            /// 1초에 이동하는 거리
            /// </summary>
            public float Spd;
            /// <summary>
            /// 경로 = 절대 좌표
            /// </summary>
            public List<Vector3> Tracks;
            /// <summary>
            /// 보스인지
            /// </summary>
            public bool IsBoss;
        }
    }
}
