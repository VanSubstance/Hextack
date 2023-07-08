﻿using Assets.Scripts.Battle.Monster;
using Assets.Scripts.UI.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : SingletonObject<CommonInGameManager>
    {
        public IngameStageType CurrentStageType;
        [SerializeField]
        private string dungeonCodeForTest;
        /// <summary>
        /// 인게임 재화
        /// </summary>
        private int amountStone, amountSteel;
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
                ExecuteNextRound();
            });
            ServerData.InGame.MiningLevel = 1;
            AmountStone = 30;
            AmountSteel = 0;

            ServerData.InGame.CurrentRound = 1;
            CurrentStageType = IngameStageType.Summon;
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
    }
}