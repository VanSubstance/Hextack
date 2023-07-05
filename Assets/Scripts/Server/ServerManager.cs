using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            LoadDungeonInfo(mapInfo);
            MainInGameManager.Instance.NextStage = IngameStageType.Prepare;
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
            ServerData.User.AmountArtifact += GlobalStatus.InGame.AccuArtifact * 2;
            ServerData.User.AmountGold += GlobalStatus.InGame.AccuGold * 2;
            GlobalStatus.NextScene = "MainMenu";
            SceneManager.LoadScene("Loading");
        }

        /// <summary>
        /// 그냥 수령하고 메인메뉴로 나가기
        /// </summary>
        public void ExitNormal()
        {
            ServerData.User.AmountArtifact += GlobalStatus.InGame.AccuArtifact;
            ServerData.User.AmountGold += GlobalStatus.InGame.AccuGold;
            GlobalStatus.NextScene = "Main";
            SceneManager.LoadScene("Loading");
        }

    }
}
