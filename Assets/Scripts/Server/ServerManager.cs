using UnityEngine;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;

namespace Assets.Scripts.Server
{
    /// <summary>
    /// 서버 연결 관리용 매니저
    /// </summary>
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
        public int Radius
        {
            get
            {
                return mapInfo.radius;
            }
        }
        public int CntTileBan
        {
            get
            {
                return mapInfo.cntTileBan;
            }
        }
        public int CntUnitBan
        {
            get
            {
                return mapInfo.cntUnitBan;
            }
        }
        public int CntUnit
        {
            get
            {
                return mapInfo.cntUnit;
            }
        }
        public HexCoordinate[] tilesInfo;
        public UnitLiveInfo[][] monstersInfo;
        private int curRound;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform);
            string basePath = $"Datas/Maps/{mapInfo.radius}/{mapInfo.mapTitle}";
            tilesInfo = Resources.LoadAll<HexCoordinate>($"{basePath}/installable");
            monstersInfo = new UnitLiveInfo[mapInfo.rounds][];
            for (int i = 0; i < mapInfo.rounds; i++)
            {
                monstersInfo[i] = Resources.LoadAll<UnitLiveInfo>($"{basePath}/single/rounds/{i + 1}");
            }
            curRound = 1;
        }

        private void Start()
        {
            // 타일맵 생성
            MapManager.Instance.Init(tilesInfo);
            // 유닛 메니저 초기화
            UnitManager.Instance.Init();
            
            // 1라운드 강제 시작
            InitUnits(curRound - 1);
        }

        /// <summary>
        /// 적 유닛 생성
        /// </summary>
        /// <param name="idxRound"></param>
        private void InitUnits(int idxRound)
        {
            UnitManager.Instance.InitUnits(monstersInfo[idxRound], true);
        }
    }
}
