﻿using Assets.Scripts.Common.Pooling;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    /// <summary>
    /// 투사체 컨트롤러
    /// 시작 -> 도착 위치가 정해져있다
    /// 도착 시 기능 실행
    /// </summary>
    public class ProjectileController : AbsPoolingContent
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
        private System.Action actionEnd;
        private Vector3 endPos, startPos;
        private float distort = .01f;

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
            if ((transform.position - startPos).magnitude >= (endPos - startPos).magnitude)
            {
                // 도착으로 본다
                actionEnd?.Invoke();
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

        protected override bool InitExtra(AbsPoolingContent.Info _info)
        {
            if (_info is not Info info)
                return false;
            color = info.color;
            startPos = info.StartPos;
            endPos = info.EndPos;
            actionEnd = info.ActionEnd;
            transform.position = startPos;
            gameObject.SetActive(true);
            rigid.AddForce((endPos - startPos).normalized * GlobalStatus.InGame.SpdProjectile, ForceMode.Impulse);
            return true;
        }

        public new class Info : AbsPoolingContent.Info
        {
            public Color color;
            public Vector3 StartPos, EndPos;
            public System.Action ActionEnd;
        }
    }
}