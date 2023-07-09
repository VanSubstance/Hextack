using UnityEngine;
using Assets.Scripts.Monster;

namespace Assets.Scripts.Battle.Projectile
{
    /// <summary>
    /// 투사체 컨트롤러
    /// 시작 -> 도착 위치가 정해져있다
    /// 도착 시 기능 실행
    /// </summary>
    public class ProjectileController : AbsPoolingContent<ProjectileInfo>
    {
        private TrailRenderer trail;
        private MeshRenderer meshRenderer;
        private Rigidbody rigid;
        private Color color
        {
            set
            {
                trail.startColor = value;
                trail.endColor = value;
                meshRenderer.materials[0].color = value;
            }
        }
        private System.Action<MonsterController> actionEnd;
        private Vector3 endPos, startPos;
        private MonsterController targetTr;
        private float spd;
        private ProjectileTrailType trailType;
        private ProjectileExecuteType executeType;

        private float distort
        {
            get
            {
                switch (trailType)
                {
                    case ProjectileTrailType.Lighting:
                        return .2f;
                }
                return 0;
            }
        }

        private void Awake()
        {
            trail = GetComponent<TrailRenderer>();
            meshRenderer = GetComponent<MeshRenderer>();
            rigid = GetComponent<Rigidbody>();
            gameObject.SetActive(false);
        }

        /// <summary>
        /// 도착 시 종료
        /// </summary>
        private void FixedUpdate()
        {
            if (targetTr != null)
            {
                endPos = targetTr.transform.position;
                rigid.velocity = (endPos - startPos).normalized * spd;
            }
            if ((transform.position - startPos).magnitude >= (endPos - startPos).magnitude)
            {
                // 도착으로 본다
                if (targetTr.gameObject.activeSelf)
                {
                    actionEnd?.Invoke(targetTr);
                }
                ReturnToPool();
            }
            int idxT = trail.positionCount - 1;
            if (idxT >= 0)
            {
                trail.SetPosition(idxT, trail.GetPosition(idxT) + new Vector3(Random.Range(-distort, distort), Random.Range(-distort, distort), Random.Range(-distort, distort)));
            }
        }

        public override void Clear()
        {
            rigid.velocity = Vector3.zero;
        }

        protected override bool InitExtra(ProjectileInfo _info)
        {
            switch (_info.executeType)
            {
                case ProjectileExecuteType.Bullet:
                    color = _info.color;
                    startPos = _info.StartPos;
                    endPos = _info.EndPos;
                    actionEnd = _info.ActionEnd;
                    transform.position = startPos;
                    targetTr = _info.targetTr;
                    spd = _info.Spd;
                    trailType = _info.TrailType;
                    executeType = _info.executeType;
                    gameObject.SetActive(true);
                    if (_info.targetTr == null)
                    {
                        // 추적 아님
                        rigid.velocity = (endPos - startPos).normalized * spd;
                    }
                    return true;
                case ProjectileExecuteType.Instant:
                    // 즉발 = 바로 효과 적용하고 투사체 파기
                    _info.ActionEnd?.Invoke(_info.targetTr);
                    return false;
                case ProjectileExecuteType.Aura:
                    // 아우라 = 바로 아우라 켜고 투사체 파기
                    break;
            }
            return false;
        }
    }
}
