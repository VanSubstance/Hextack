using UnityEngine;

namespace Assets.Scripts.Path
{
    public class PathManager : SingletonObject<PathManager>
    {
        [SerializeField]
        public Path[] PathList;

        [System.Serializable]
        public class Path
        {
            public Transform[] TargetTr;
        }
    }
}
