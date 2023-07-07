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
        [SerializeField]
        private Transform ta;
        private void Update()
        {
            agent.SetDestination(ta.position);
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
        }
    }
}
