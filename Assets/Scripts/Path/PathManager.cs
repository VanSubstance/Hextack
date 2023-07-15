using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Path
{
    public class PathManager : SingletonObject<PathManager>
    {
        [SerializeField]
        public Path[] PathList;
        [SerializeField]
        private PathPointer PointerPrefab;
        [HideInInspector]
        public Queue<PathPointer> QActive, QDeactive;
        public bool IsInVisual;

        [System.Serializable]
        public class Path
        {
            public Transform[] TargetTr;
        }

        private new void Awake()
        {
            base.Awake();
            QDeactive = new Queue<PathPointer>();
            QActive = new Queue<PathPointer>();
            for (int i = 0; i < PathList.Length * 3; i++)
            {
                PathPointer newPointer = Instantiate(PointerPrefab, transform);
                newPointer.gameObject.SetActive(false);
                QDeactive.Enqueue(newPointer);
            }
            Destroy(PointerPrefab);
            IsInVisual = false;
        }

        /// <summary>
        /// 경로 시각화 켜기
        /// </summary>
        public void ActivateVisualization()
        {
            // 경로 하나당 도달 기준 간격으로 하나씩 보내기
            IsInVisual = true;
            SendPath();
        }

        /// <summary>
        /// 경로 시각화 끄기
        /// </summary>
        public void DeactivateVisualization()
        {
            IsInVisual = false;
        }

        /// <summary>
        /// 포인터 하나 보내기
        /// </summary>
        public void SendPath()
        {
            int idx = 0;
            while (PathList.Length > idx)
            {
                QActive.Enqueue(QDeactive.Dequeue().StartTracking(PathList[idx].TargetTr));
                idx++;
            }
        }
    }
}
