using Assets.Scripts.Battle.Monster;
using Assets.Scripts.UI.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : SingletonObject<CommonInGameManager>
    {
        public IngameStageType CurrentStageType;
        public bool IsStageDone;
        [SerializeField]
        private string dungeonCodeForTest;
        /// <summary>
        /// 인게임 재화
        /// </summary>
        private int amountStone, amountSteel;

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
                amountSteel = value;
                UIInGameManager.Instance.AmountSteel = value;
            }
            get { return amountSteel; }
        }
        private void Start()
        {
            // 만약 테스트다 -> 테스트 던전 연결
            if (ServerData.InGame.DungeonInfo == null)
            {
                ServerData.InGame.DungeonInfo = ServerData.Dungeon.data[dungeonCodeForTest];
            }

            // 매니저들 초기화
            UIInGameManager.Instance.Init(() =>
            {
                IsStageDone = true;
            });
            ServerData.InGame.MiningLevel = 1;
            AmountStone = 30;
            AmountSteel = 0;

            ServerData.InGame.CurrentRound = 1;
            CurrentStageType = IngameStageType.Summon;
            IsStageDone = true;
            // 게임 시작
            // 현재 스테이지 체크 코루틴 실행 = 1초마다
            // 추가 기능: 철광석 채굴 = 1초마다 1씩
            ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
                // 채굴
                AmountSteel += ServerData.InGame.MiningLevel;
                // 각 스테이지 돌입 체크
                if (!IsStageDone) return;
                IsStageDone = false;
                switch (CurrentStageType)
                {
                    case IngameStageType.Summon:
                        InitStageSummon();
                        break;
                }
            }, () =>
            {
                return false;
            }, () =>
            {
            }, 1f);
        }

        /// <summary>
        /// 다음 라운드 시작 = 몬스터 소환
        /// </summary>
        private void InitStageSummon()
        {
            IsStageDone = false;

            // UI 띄우기
            Queue<Action> temp = new Queue<Action>();
            temp.Enqueue(() =>
            {
                UIInGameManager.Instance.TextCenter = $"라운드 {ServerData.InGame.CurrentRound} 시작";
            });
            temp.Enqueue(() =>
            {
                UIInGameManager.Instance.TextCenter = $"";
                MonsterManager.Instance.SummonMonster(ServerData.InGame.MonsterInfo[ServerData.InGame.CurrentRound++ - 1]);
                UIInGameManager.Instance.StartRound();
            });
            ServerManager.Instance.ExecuteCrInSequnce(temp, 1f);
        }
    }
}
