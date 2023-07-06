using Assets.Scripts.Common;
using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Assets.Scripts.Server
{
    /// <summary>
    /// 서버 연결 관리용 매니저
    /// </summary>
    public class ServerManager : SingletonObject<ServerManager>
    {
        [SerializeField]
        private string[] testDeck;
        [SerializeField]
        private MapInfo mapInfo;
        [SerializeField]
        private bool isSingle;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform);
            Application.targetFrameRate = 1000;
            GlobalStatus.MapInfo = mapInfo;
            GlobalStatus.IsSingle = isSingle;
            DataManager.Instance.LoadLocalDatas();
            CallUserInfo();
            LoadDungeonInfo(mapInfo);
        }

        /// <summary>
        /// 던전 정보 받아오기 함수
        /// </summary>
        /// <param name="dungeonName"></param>
        private void LoadDungeonInfo(MapInfo _mapInfo)
        {
            ServerData.Dungeon.Info = _mapInfo;
            string basePath = $"Datas/Maps/{ServerData.Dungeon.Info.radius}/{ServerData.Dungeon.Info.Code}";
            ServerData.Dungeon.TilesInfo = Resources.LoadAll<HexCoordinate>($"{basePath}/installable");
            ServerData.Dungeon.MonsterInfo = new UnitToken[ServerData.Dungeon.Info.rounds][];
            for (int i = 0; i < ServerData.Dungeon.Info.rounds; i++)
            {
                ServerData.Dungeon.MonsterInfo[i] = Resources.LoadAll<UnitToken>($"{basePath}/single/rounds/{i + 1}");
            }
            // 덱 정보 받아오기
            ServerData.User.Deck = new UnitInfo[testDeck.Length];
            for (int i = 0; i < testDeck.Length; i++)
            {
                if (testDeck[i] == null) continue;
                ServerData.User.Deck[i] = ServerData.Unit.data[testDeck[i]];
                ServerData.User.Deck[i].AccuDamage = 0;
                ServerData.User.Deck[i].CountSummon = 0;
            }
        }

        /// <summary>
        /// 광고 보고 두배 수령하고 메인메뉴로 나가기
        /// </summary>
        public void ExitDouble()
        {
            ServerData.User.Base.AmountArtifact += GlobalStatus.InGame.AccuArtifact * 2;
            ServerData.User.Base.AmountGold += GlobalStatus.InGame.AccuGold * 2;
            GlobalStatus.NextScene = "MainMenu";
            SceneManager.LoadScene("Loading");
        }

        /// <summary>
        /// 그냥 수령하고 메인메뉴로 나가기
        /// </summary>
        public void ExitNormal()
        {
            ServerData.User.Base.AmountArtifact += GlobalStatus.InGame.AccuArtifact;
            ServerData.User.Base.AmountGold += GlobalStatus.InGame.AccuGold;
            GlobalStatus.NextScene = "Main";
            SceneManager.LoadScene("Loading");
        }


        /// <summary>
        /// 유저 정보 불러오기
        /// </summary>
        public void CallUserInfo()
        {
            // 기본 정보 불러오기
            ServerData.User.Base = Resources.Load<UserBasicInfo>("Datas/Server/User Basic Info");
            ServerData.User.Storages = new UnitInfo[ServerData.User.Base.unitPossessList.Length];
            int idx = 0;
            // 각 기물 실제 정보 채우기
            ServerData.User.Base.unitPossessList.All((upInfo) =>
            {
                ServerData.User.Storages[idx++] = ServerData.Unit.data[upInfo.Code].Clone();
                return true;
            });
        }
    }
}
