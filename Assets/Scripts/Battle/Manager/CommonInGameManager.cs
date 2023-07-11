using Assets.Scripts.Monster;
using Assets.Scripts.UI.Manager;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : SingletonObject<CommonInGameManager>
    {
        [SerializeField]
        private string dungeonCodeForTest;

        /// <summary>
        /// 인게임 재화
        /// </summary>
        [HideInInspector]
        public int amountStone, MiningLevel;
        private Coroutine crMining;

        public int AmountStone
        {
            set
            {
                amountStone = value;
                UIInGameManager.Instance.AmountStone = value;
            }
            get { return amountStone; }
        }

        public int AmountSteel
        {
            set
            {
                ServerData.InGame.AmountSteel = value;
                UIInGameManager.Instance.AmountSteel = value;
            }
            get { return ServerData.InGame.AmountSteel; }
        }
        private void Start()
        {
            // 만약 테스트다 -> 테스트 던전 연결
            if (ServerData.InGame.DungeonInfo == null)
            {
                ServerData.InGame.DungeonInfo = ServerData.Dungeon.data[dungeonCodeForTest];
            }
            ServerData.InGame.MiningLevel = 1;
            AmountStone = 30;
            AmountSteel = 0;

            // 매니저들 초기화
            UIInGameManager.Instance.Init(() =>
            {
                ExecuteNextRound();
            });

            ServerData.InGame.CurrentRound = 1;
            // 게임 시작
            // 철광석 채굴 = 1초마다 1씩
            crMining = ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                // 채굴
                AmountSteel += ServerData.InGame.MiningLevel;
            }, () =>
            {
                return false;
            }, () =>
            {
            }, 1f);
            ExecuteNextRound();
        }

        /// <summary>
        /// 다음 라운드 시작 = 몬스터 소환
        /// </summary>
        private void ExecuteNextRound()
        {
            // 다음 라운드 시작
            UIInGameManager.Instance.TextCenter = $"라운드 {ServerData.InGame.CurrentRound} 시작";
            MonsterManager.Instance.SummonMonster(ServerData.InGame.MonsterInfo[ServerData.InGame.CurrentRound - 1]);
            UIInGameManager.Instance.StartRound();
            ServerData.InGame.CurrentRound++;
        }

        /// <summary>
        /// 채굴 레벨 업그레이드
        /// </summary>
        public void MiningLevelUp()
        {
            if (AmountStone < ServerData.InGame.PriceMiningLvUp)
            {
                UIInGameManager.Instance.TextWarning = $"석재가 부족합니다!";
                return;
            }
            AmountStone -= ServerData.InGame.PriceMiningLvUp;
            UIInGameManager.Instance.MiningLv = ++ServerData.InGame.MiningLevel;
        }

        /// <summary>
        /// 게임속도 변경(x1 -> x2 -> x3 -> x1)
        /// </summary>
        public void AccelarateSpeed()
        {
            ServerData.InGame.GameSpeed += 1;
            ServerData.InGame.GameSpeed %= 3;
            if (ServerData.InGame.GameSpeed == 0)
            {
                ServerData.InGame.GameSpeed = 3;
            }
            UIInGameManager.Instance.TextSpeed = ServerData.InGame.GameSpeed;
            Time.timeScale = ServerData.InGame.GameSpeed;
        }
    }
}
