using Assets.Scripts.Battle;
using Assets.Scripts.Map;
using Assets.Scripts.UI;
using Assets.Scripts.Unit;
using System.Collections.Generic;

/// <summary>
/// 현재 인게임에서 사용중인 데이터 관리 클래스
/// </summary>
public static class GlobalStatus
{

    private static MapInfo mapInfo;
    public static MapInfo MapInfo
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
    public static int CntUnit
    {
        get
        {
            return mapInfo.cntUnit;
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
        /// 투사체 속도
        /// </summary>
        public static float SpdProjectile = 7f;
        /// <summary>
        /// 0: 진행중; 1: 아군 승; 2: 적군 승; 3: 무승부
        /// </summary>
        public static int BattleStatus = 0;
        /// <summary>
        /// 이번에 설치한 기물 수, 현재 라운드
        /// </summary>
        public static int CntInstalled, Round;
    }

    /// <summary>
    /// 인게임에 들고 들어간 덱
    /// </summary>
    public static UnitInfo[] Deck;
    public static HexTileController[][] Map;
    public static UnitController[][] Units;
    /// <summary>
    /// 기물 오브젝트 풀
    /// </summary>
    public static Queue<UnitController> UnitPool;
    public static List<UnitController> UnitsActive;
    public static IngameStageType CurrentStage;
    public static bool IsInStage;

    /// <summary>
    /// 체력게이지 풀
    /// </summary>
    public static Queue<GageController> HpGagePool = new Queue<GageController>();

    /// <summary>
    /// 투사체 풀
    /// </summary>
    public static Queue<ProjectileController> ProjectilePool = new Queue<ProjectileController>();

    /// <summary>
    /// 텍스트 풀
    /// </summary>
    public static Queue<TextController> textPoll = new Queue<TextController>();

    /// <summary>
    /// 이펙트 풀
    /// </summary>
    public static Dictionary<string, Queue<EffectController>> effectPool = new Dictionary<string, Queue<EffectController>>();

    /// <summary>
    /// 기물이 풀에 존재할 경우 -> 꺼내서 줌
    /// </summary>
    /// <returns></returns>
    public static ProjectileController GetProjectile()
    {
        if (ProjectilePool.Count > 0)
            return ProjectilePool.Dequeue();
        else
            return null;
    }

    /// <summary>
    /// 기물이 풀에 존재할 경우 -> 꺼내서 줌
    /// </summary>
    /// <returns></returns>
    public static UnitController GetUnit()
    {
        if (UnitPool.Count > 0)
            return UnitPool.Dequeue();
        else
            return null;
    }

    /// <summary>
    /// 체력 게이지가 풀에 존재 -> 꺼내줌
    /// </summary>
    /// <returns></returns>
    public static GageController GetHpGageController()
    {
        if (HpGagePool.Count > 0)
            return HpGagePool.Dequeue();
        else
            return null;
    }

    /// <summary>
    /// 텍스트가 풀에 존재 -> 꺼내줌
    /// </summary>
    /// <returns></returns>
    public static TextController GetTextController()
    {
        if (textPoll.Count > 0)
            return textPoll.Dequeue();
        else
            return null;
    }
}