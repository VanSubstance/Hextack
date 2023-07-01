using UnityEngine;
using Assets.Scripts.Map;

namespace Assets.Scripts.Server
{
    public class ServerManager : SingletonObject<ServerManager>
    {
        [SerializeField]
        private MapInfo mapInfo;
        public MapInfo MapInfo
        {
            get
            {
                return mapInfo;
            }
        }
        [SerializeField]
        private bool isSingle;
        public bool IsSingle
        {
            get
            {
                return isSingle;
            }
        }
        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform);
        }

        private void Start()
        {
            MapManager.Instance.InitMap(MapInfo);
        }
    }
}
