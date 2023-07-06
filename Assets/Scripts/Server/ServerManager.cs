using Assets.Scripts.Common;
using Assets.Scripts.Common.MainManager;
using Assets.Scripts.Map;
using Assets.Scripts.Unit;
using System.Linq;
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
        private bool isSingle;

        private new void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(transform);
            Application.targetFrameRate = 1000;
            GlobalStatus.MapInfo = ServerData.InGame.DungeonInfo;
            GlobalStatus.IsSingle = isSingle;
            DataManager.Instance.LoadLocalDatas();
            CallUserInfo();
        }

        /// <summary>
        /// 던전 정보 받아오기 함수
        /// </summary>
        /// <param name="dungeonName"></param>
        public void LoadDungeonInfo()
        {
            string basePath = $"Datas/Maps/{ServerData.InGame.DungeonInfo.radius}/{ServerData.InGame.DungeonInfo.Code}";
            ServerData.InGame.TilesInfo = Resources.LoadAll<HexCoordinate>($"{basePath}/installable");
            ServerData.InGame.MonsterInfo = new UnitToken[ServerData.InGame.DungeonInfo.rounds][];
            for (int i = 0; i < ServerData.InGame.DungeonInfo.rounds; i++)
            {
                ServerData.InGame.MonsterInfo[i] = Resources.LoadAll<UnitToken>($"{basePath}/single/rounds/{i + 1}");
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
        /// 신규 던전 입장
        /// </summary>
        /// <param name="dungeonCode"></param>
        public void EnterDungeon(string dungeonCode)
        {
            ServerData.InGame.DungeonInfo = ServerData.Dungeon.DungeonList[dungeonCode];
            ServerData.InGame.DeckAlly = ServerData.User.Decks[MainMainManager.Instance.CurrentDeckIdx];
            GlobalStatus.NextScene = "InGame";
            SceneManager.LoadScene("Loading");
        }

        /// <summary>
        /// 기존 던전 이어서 진행
        /// </summary>
        public void ContinueDungeon()
        {
            Debug.Log($"이어서 드가자 ㅡ ! ");
        }


        /// <summary>
        /// 유저 정보 불러오기
        /// </summary>
        public void CallUserInfo()
        {
            // 기본 정보 불러오기
            ServerData.User.Base = Resources.Load<UserBasicInfo>("Datas/Server/User Basic Info");
            ServerData.User.Storages = new UnitInfo[ServerData.User.Base.UnitStorageList.Length];
            int idx = 0;
            // 각 기물 실제 정보 채우기
            ServerData.User.Base.UnitStorageList.All((upInfo) =>
            {
                ServerData.User.Storages[idx++] = ServerData.Unit.data[upInfo.Code].Clone();
                return true;
            });

            // 덱 정보 받아오기
            ServerData.User.Decks = new UnitInfo[4][];
            idx = 0;
            while (idx < 4)
            {
                ServerData.User.Decks[idx++] = new UnitInfo[6];
            }
            idx = 0;
            int idxx;
            foreach (UserBasicInfo.DeckCodeList codeList in ServerData.User.Base.DeckList)
            {
                idxx = 0;
                foreach (string code in codeList.Codes)
                {
                    ServerData.User.Decks[idx][idxx++] = ServerData.Unit.data[code].Clone();
                }
                idx++;
            }
        }
    }
}
