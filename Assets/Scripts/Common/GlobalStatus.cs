﻿using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using Assets.Scripts.Unit;
using System.Collections.Generic;

/// <summary>
/// 현재 인게임에서 사용중인 데이터 관리 클래스
/// </summary>
public static class GlobalStatus
{

    private static DungeonInfo mapInfo;
    public static DungeonInfo MapInfo
    {
        get
        {
            return mapInfo;
        }
        set
        {
            mapInfo = value;
        }
    }
    public static int Radius
    {
        get
        {
            return mapInfo.radius;
        }
    }
    public static int CntTileBan
    {
        get
        {
            return mapInfo.cntTileBan;
        }
    }
    public static int CntUnitBan
    {
        get
        {
            return mapInfo.cntUnitBan;
        }
    }
    public static int CntUnitSummonAtOnce
    {
        get
        {
            return mapInfo.cntUnitSummonAtOnce;
        }
    }
    private static bool isSingle;
    public static bool IsSingle
    {
        get
        {
            return isSingle;
        }
        set
        {
            isSingle = value;
        }
    }

    /// <summary>
    /// 인게임 상수들
    /// </summary>
    public static class InGame
    {
        /// <summary>
        /// 0: 진행중; 1: 아군 승; 2: 적군 승; 3: 무승부
        /// </summary>
        public static int BattleStatus = 0;
        /// <summary>
        /// 이번에 설치한 기물 수, 현재 라운드, 승리한 라운드 카운트
        /// </summary>
        public static int CntInstalled, Round, WinCount;
        /// <summary>
        /// 기본 크리티컬 확률
        /// </summary>
        public static float RateCritical = .1f;
        /// <summary>
        /// 획득한 누적 골드량
        /// </summary>
        public static int AccuGold = 0;
        /// <summary>
        /// 획득한 누적 다이어량
        /// </summary>
        public static int AccuArtifact = 0;
    }

    /// <summary>
    /// 현재 씬 이름, 다음 씬 이름
    /// </summary>
    public static string CurScene, NextScene;

    /// <summary>
    /// 체력게이지 풀
    /// </summary>
    public static Queue<GageController> HpGagePool = new Queue<GageController>();

    /// <summary>
    /// 이펙트 풀
    /// </summary>
    public static Dictionary<string, Queue<EffectController>> effectPool = new Dictionary<string, Queue<EffectController>>();

    /// <summary>
    /// 체력 게이지가 풀에 존재 -> 꺼내줌
    /// </summary>
    /// <returns></returns>
    public static GageController GetHpGageController()
    {
        if (HpGagePool.TryDequeue(out GageController res))
            return res;
        else
            return null;
    }
}