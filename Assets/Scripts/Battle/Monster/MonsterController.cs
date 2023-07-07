using Assets.Scripts.Common.Pooling;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Battle.Monster
{
    public class MonsterController : AbsPoolingContent
    {
        [SerializeField]
        private NavMeshAgent agent;
        private void Update()
        {
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
            {
                return false;
            }
            agent.speed = info.Spd;
            transform.position = info.InitPos;
            gameObject.SetActive(true);
            agent.SetDestination(Vector3.zero);
            return true;
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
