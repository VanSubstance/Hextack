using Assets.Scripts.UI;
using Assets.Scripts.UI.Manager;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Battle.Monster
{
    public class MonsterController : AbsPoolingContent
    {
        [SerializeField]
        private NavMeshAgent agent;
        private int Hp;

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
            UIInGameManager.Instance.ModifyCountMonster--;
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            UIInGameManager.Instance.ModifyCountMonster++;
            agent.speed = info.Spd;
            Hp = info.Hp;
            transform.position = info.InitPos;
            gameObject.SetActive(true);
            agent.SetDestination(Vector3.zero);
            return true;
        }

        /// <summary>
        /// HP 가감치 적용
        /// </summary>
        /// <param name="amountToApply"></param>
        public void ApplyHp(int amountToApply, bool isCrit)
        {
            if (amountToApply < 0)
            {
                // 데미지 텍스트 띄워주기
                amountToApply = (int)(amountToApply * (isCrit ? 1.5f : 1f));
                TextManager.Instance.GetNewComponent().Init(new TextController.Info()
                {
                    ScreenPos = ScreenPos,
                    TargetText = $"{Mathf.Abs(amountToApply)}",
                    TextColor = isCrit ? new Color(1, .8f, 0, 1) : Color.white,
                    Time = .5f,
                    SizeMultiplier = isCrit ? 1.4f : 1.3f
                });

                // 힐 이펙트 띄워주기
                EffectManager.Instance.ExecutNewEffect("Hit", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            }
            else
            {
                // 힐 텍스트 띄워주기
                TextManager.Instance.GetNewComponent().Init(new TextController.Info()
                {
                    ScreenPos = ScreenPos,
                    TargetText = $"+{Mathf.Abs(amountToApply)}",
                    TextColor = new Color(.5f, 1, .8f, 1),
                    Time = .5f,
                });

                // 힐 이펙트 띄워주기
                EffectManager.Instance.ExecutNewEffect("Heal", transform.position + (Vector3.up * 2) + Vector3.back, Color.white);
            }
            Hp += amountToApply;
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
            /// 시작 지점 = 절대 좌표
            /// </summary>
            public Vector2 InitPos;
        }
    }
}
