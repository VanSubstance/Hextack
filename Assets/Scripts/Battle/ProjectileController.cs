using Assets.Scripts.Unit;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    /// <summary>
    /// 투사체 컨트롤러
    /// 시작 -> 도착 위치가 정해져있다
    /// 도착 시 기능 실행
    /// </summary>
    public class ProjectileController : MonoBehaviour
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
                Clear();
            }
        }

        /// <summary>
        /// 사용 시작 함수
        /// </summary>
        /// <param name="_color"></param>
        /// <param name="startPos">시작 좌표 (절대)</param>
        /// <param name="endPos">종료 좌표 (절대)</param>
        /// <returns></returns>
        public ProjectileController Init(Color _color, Vector3 _startPos, Vector3 _endPos, System.Action _actionEnd)
        {
            color = _color;
            startPos = _startPos;
            endPos = _endPos;
            actionEnd = _actionEnd;
            transform.position = startPos;
            gameObject.SetActive(true);
            rigid.AddForce((endPos - startPos).normalized * GlobalStatus.InGame.SpdProjectile, ForceMode.Impulse);
            return this;
        }

        /// <summary>
        /// 풀에 반납
        /// </summary>
        public void Clear()
        {
            gameObject.SetActive(false);
            rigid.velocity = Vector3.zero;
            GlobalStatus.ProjectilePool.Enqueue(this);
        }
    }
}
