using Assets.Scripts.Battle.Monster;
using Assets.Scripts.UI.Manager;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Battle
{
    public class CommonInGameManager : MonoBehaviour
    {
        public IngameStageType CurrentStageType;
        public bool IsStageDone;

        private void Start()
        {
            // 매니저들 초기화
            UIInGameManager.Instance.Init(() =>
            {
                IsStageDone = true;
            });

            CurrentStageType = IngameStageType.Summon;
            IsStageDone = true;
            // 게임 시작
            // 현재 스테이지 체크 코루틴 실행
            ServerManager.Instance.ExecuteCrInRepeat(() =>
            {
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
                UIInGameManager.Instance.TextCenter = $"라운드 {GlobalStatus.InGame.Round} 시작";
            });
            temp.Enqueue(() =>
            {
                UIInGameManager.Instance.TextCenter = $"";
                MonsterManager.Instance.SummonMonster(ServerData.InGame.MonsterInfo[GlobalStatus.InGame.Round++ - 1]);
                UIInGameManager.Instance.StartRound();
            });
            ServerManager.Instance.ExecuteCrInSequnce(temp, 1f);
        }
    }
}
