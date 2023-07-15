using Assets.Scripts.Monster;
using Assets.Scripts.UI.Manager;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : SingletonObject<CommonInGameManager>
    {
        [SerializeField]
        private string dungeonCodeForTest;

        private Coroutine crMining, crGameOver;

        public int AmountStone
        {
            set
            {
                ServerData.InGame.AmountStone = value;
                UIInGameManager.Instance.AmountStone = value;
            }
            get { return ServerData.InGame.AmountStone; }
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

            // 초기 기어 업그레이드 적용
            ServerData.InGame.MiningLevel = ServerData.OutGame.GearUpgradeLevel[UI.Fragment.Section.GearUpgrade.GearUpgradeType.Mining];
            ServerData.InGame.AmountStone = 20 + (ServerData.OutGame.GearUpgradeLevel[UI.Fragment.Section.GearUpgrade.GearUpgradeType.Stone] * 10);

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

            // 라운드 시작 전 5초 유예
            int t = 5;
            ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                UIInGameManager.Instance.TextCenter = $"게임 시작 {t--}초 전";
            }, () => t == 0, () =>
            {
                ExecuteNextRound();
                Path.PathManager.Instance.DeactivateVisualization();
            }, 1f);

            // 트랙 가시화
            Path.PathManager.Instance.ActivateVisualization();
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
            if (crGameOver == null)
            {
                // 게임 종료 조건 체커
                crGameOver = ServerManager.Instance.ExecuteCrInRepeat(null, () =>
                {
                    // 마지막 라운드가 아니다 and 라이브 몬스터 수 > 0
                    return ServerData.InGame.CurrentRound > ServerData.InGame.MaxRound && ServerData.InGame.CountMonsterLive == 0;
                }, () =>
                {
                    UIInGameManager.Instance.ExitGame();
                }, 1f);
            }
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
