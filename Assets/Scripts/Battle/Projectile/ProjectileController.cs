﻿using Assets.Scripts.Audio;
using Assets.Scripts.Battle.Area;
using Assets.Scripts.Monster;
using UnityEngine;

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
        private AfterHitInfo afterHitInfo;
        private bool IsArrived;

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
                if (!targetTr.gameObject.activeSelf)
                {
                    // 죽음 = 투사체 그냥 파기
                    ReturnToPool();
                }
                endPos = targetTr.transform.position;
                rigid.velocity = (endPos - startPos).normalized * spd;
            }
            if ((transform.position - startPos).magnitude >= (endPos - startPos).magnitude)
            {
                // 도착으로 본다
                if (targetTr.gameObject.activeSelf)
                {
                    ExecuteArrival();
                    return;
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
            IsArrived = false;
            afterHitInfo = (_info.afterHitInfo == null || _info.afterHitInfo.afterHitType.Equals(AfterHitType.None)) ? null : _info.afterHitInfo;
            startPos = _info.StartPos;
            targetTr = _info.targetTr;
            transform.position = startPos;
            color = _info.color;
            actionEnd = _info.ActionEnd;
            switch (_info.executeType)
            {
                case ProjectileExecuteType.Bullet:
                    endPos = _info.EndPos;
                    spd = _info.Spd;
                    trailType = _info.TrailType;
                    gameObject.SetActive(true);
                    if (_info.targetTr == null)
                    {
                        // 추적 아님
                        rigid.velocity = (endPos - startPos).normalized * spd;
                    }
                    return true;
                case ProjectileExecuteType.Instant:
                    // 즉발 = 바로 효과 적용하고 투사체 파기
                    transform.position = targetTr.transform.position;
                    ExecuteArrival();
                    return false;
                case ProjectileExecuteType.Aura:
                    break;
            }
            return false;
        }

        /// <summary>
        /// 도착 후 효과 실행
        /// </summary>
        private void ExecuteArrival()
        {
            if (IsArrived) return;
            IsArrived = true;
            actionEnd?.Invoke(targetTr);
            if (afterHitInfo != null)
            {
                // 도착 후 효과가 있다 = 실행
                AreaInfo areaInfo = afterHitInfo.GetAreaInfo();
                areaInfo.targetPos = transform.position;
                if (areaInfo.SoundFire != null)
                {
                    AudioManager.Instance.GetNewContent(new AudioInfo()
                    {
                        Clip = areaInfo.SoundFire,
                        Pos = areaInfo.targetPos,
                    });
                }
                AreaManager.Instance.GetNewContent(areaInfo);
            }
            ReturnToPool();
        }
    }
}
