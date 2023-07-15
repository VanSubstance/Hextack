using UnityEngine;

namespace Assets.Scripts.Path
{
    public class PathPointer : MonoBehaviour
    {
        private Transform[] pathList;
        private int curIdx;
        private float prevDist;

        /// <summary>
        /// 해당 포인터 목적지 도달했는지
        /// </summary>
        private bool IsArrived
        {
            get
            {
                float curDist = (transform.position - pathList[curIdx].position).magnitude;
                if (prevDist < curDist)
                {
                    prevDist = float.MaxValue;
                    return true;
                    // 도착
                }
                else
                {
                    prevDist = curDist;
                    return false;
                }
            }
        }

        /// <summary>
        /// 패쓰 따라가기
        /// </summary>
        public PathPointer StartTracking(Transform[] _pathList)
        {
            pathList = _pathList;
            curIdx = 0;
            prevDist = float.MaxValue;
            transform.position = _pathList[0].position + (Vector3.up * .1f);
            gameObject.SetActive(true);
            Next();
            ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                if (IsArrived)
                {
                    Next();
                }
            }, () =>
            {
                return !gameObject.activeSelf;
            }, null, Time.deltaTime);
            return this;
        }

        private void Next()
        {
            if (curIdx == pathList.Length - 1)
            {
                // 다음 없음 = 2초 대기 후 파기
                GetComponent<Rigidbody>().velocity = Vector3.zero;
                if (PathManager.Instance.IsInVisual)
                {
                    // 꼬리물기 on
                    PathManager.Instance.SendPath();
                }
                ServerManager.Instance.ExecuteWithDelay(() =>
                {
                    gameObject.SetActive(false);
                    PathManager.Instance.QDeactive.Enqueue(this);
                }, 2f);
                return;
            }
            Vector3 nextVelocity = (pathList[curIdx + 1].position - transform.position).normalized * 20f;
            nextVelocity.y = 0;
            GetComponent<Rigidbody>().velocity = nextVelocity;
            curIdx++;
        }
    }
}
